#include "json.h"
#include "efficient_json_write.h"

using namespace Json;

static std::string _escapemap[256];
static std::string _xml_escapemap[256];

const char STRING_FMT_BOOL_TRUE[] = "true";
const char STRING_FMT_BOOL_FALSE[] = "false";

CEfficientWriter::CEfficientWriter()
{
	_p = NULL;
	_size = 0;
	_pos = 0;
}

int CEfficientWriter::ToInt( int n )
{
	if ( n < 0 )
	{
		ToCStringNoEscape( "-" );
		ToUint( -n );
	}
	else
	{
		ToUint( n );
	}
	return 0;
}

int CEfficientWriter::ToUint( unsigned n )
{
	unsigned tmpn = n;
	char tmp[32];
	char *current = tmp + sizeof(tmp);

	*--current = 0;
	do
	{
		*--current = (tmpn % 10) + '0';
		tmpn /= 10;
	} while( tmpn != 0 );
	
	unsigned len = sizeof(tmp)-(current-tmp) - 1;
	if ( _size-_pos > len )
	{
		char* cur_p = _p + _pos;
		while ( ( *cur_p++ = *current++ ) );
		_pos += len;
	}
	return 0;
}

int CEfficientWriter::ToDouble( double f )
{
	char* cur_p = _p + _pos;
	int cur_size =  _size - _pos;
	int cur_move = 0;
	cur_move = snprintf( cur_p, cur_size, "%f", f );
	if ( cur_move >= cur_size )
	{
		*cur_p = 0;
	}
	else
	{
		_pos += cur_move;
	}
	return 0;
}

int CEfficientWriter::ToCStringNoEscape( const char* s )
{
	const char* tmps = s;
	char* cur_p = _p + _pos;
	while ( _pos < _size && *tmps )
	{
		*cur_p++ = *tmps++;
		_pos++;
	}
	*cur_p = 0;
	return 0;
}

int CEfficientWriter::ToCString( const char* src, std::string* escapemap )
{
	char* dst = _p + _pos;
	int dstsize =  _size - _pos;
	char* bak = dst;
	unsigned char c;
	while ( 0 != (c=*src) )
	{
		std::string& to = escapemap[c];
		if ( to.empty() )
		{/* 无须转义，直接copy */
			if ( dstsize <= 1 )
			{
				break;
			}
			*dst++ = *src;
			dstsize -= 1;
		}
		else
		{/* 需要转义 */
			if ( unsigned(dstsize) <= to.size() )
			{
				break;
			}
			memcpy( dst, to.c_str(), to.size() );
			dst += to.size();
			dstsize -= to.size();
		}
		src++;
	}

	*dst = 0;
	_pos += ( dst-bak );
	return 0;
}


////////////////////////////////////////////////////////////////////////////////////////////////////
// JSON格式
////////////////////////////////////////////////////////////////////////////////////////////////////
CEfficientJsonWriter::CEfficientJsonWriter()
{
	// 初始化转移表
	static bool isEscapemapInit = false;
	if ( !isEscapemapInit )
	{
		unsigned char escape_char[] = { '\"', '\\', '\b', '\f', '\n', '\r', '\t' };
		const char* escape_to[] = { "\\\"", "\\\\", "\\b", "\\f", "\\n", "\\r", "\\t" };
		for ( unsigned i=0; i<sizeof(escape_char)/sizeof(escape_char[0]); i++ )
		{
			_escapemap[ unsigned(escape_char[i]) ] = escape_to[i];
		}
		isEscapemapInit = true;
	}
}

const char* CEfficientJsonWriter::write( const CEfficientJsonValue& value, int& len, char* buf, const unsigned& bufsize )
{
	_p = buf;
	_size = bufsize;
	_p[0] = 0;
	_pos = 0;

	WriteValue( value );
	len = _pos;
	return _p;
}

int CEfficientJsonWriter::WriteValue( const CEfficientJsonValue& value )
{
	switch ( value.type() )
	{
	case nullValue:
		PushNull();
		break;

	case intValue:
		PushInt( value.asInt() );
		break;

	case uintValue:
		PushUInt( value.asUInt() );
		break;

	case realValue:
		PushDouble( value.asDouble() );
		break;

	case stringValue:
		PushCString( value.asCString() );
		break;

	case booleanValue:
		PushBool( value.asBool() );
		break;

	case arrayValue:
		PushArray( value );
		break;

	case objectValue:
		PushObject( value );
		break;
	}
	return 0;
}

int CEfficientJsonWriter::PushNull()
{
	ToCStringNoEscape( "\"\"" );
	return 0;
}

int CEfficientJsonWriter::PushInt( int n )
{
	return ToInt( n );
}

int CEfficientJsonWriter::PushUInt( unsigned n )
{
	return ToUint( n );
}

int CEfficientJsonWriter::PushDouble( double f )
{
	return ToDouble( f );
}

int CEfficientJsonWriter::PushBool( bool b )
{
	ToCStringNoEscape( b ? STRING_FMT_BOOL_TRUE : STRING_FMT_BOOL_FALSE );
	return 0;
}

int CEfficientJsonWriter::PushCString( const char* s  )
{
	ToCStringNoEscape( "\"" );	
	ToCString( s, _escapemap );
	ToCStringNoEscape( "\"" );
	return 0;
}

int CEfficientJsonWriter::PushArray( const CEfficientJsonValue& value )
{
	ToCStringNoEscape( "[" );

	if ( value.size() > 0 )
	{
		CEfficientJsonValue elem;
		value.GetArrayElem( 0, elem );
		WriteValue( elem );
		for ( unsigned i=1; i<value.size(); i++ )
		{
			ToCStringNoEscape( "," );
			value.GetArrayElem( i, elem );
			WriteValue( elem );
		}
	}
	
	ToCStringNoEscape( "]" );
	return 0;
}

int CEfficientJsonWriter::PushObject( const CEfficientJsonValue& value )
{
	ToCStringNoEscape( "{" );

	if ( value.size() > 0 )
	{
		const char* tag = NULL;
		CEfficientJsonValue elem;
		value.GetObjectElem( 0, tag, elem );
		ToCStringNoEscape( "\"" );
		ToCStringNoEscape( tag );
		ToCStringNoEscape( "\":" );
		WriteValue( elem );
		for ( unsigned i=1; i<value.size(); i++ )
		{
			value.GetObjectElem( i, tag, elem );
			ToCStringNoEscape( ",\"" );
			ToCStringNoEscape( tag );
			ToCStringNoEscape( "\":" );
			WriteValue( elem );
		}
	}
	ToCStringNoEscape( "}" );
	return 0;
}

///////////////////////////////////////////////////////////////////////////////////////////
// XML格式
///////////////////////////////////////////////////////////////////////////////////////////
CEfficientXMLWriter::CEfficientXMLWriter()
{
	// 初始化转移表
	static bool isEscapemapInit = false;
	if ( !isEscapemapInit )
	{
		unsigned char escape_char[] = { '&', '<', '>', '\"', '\'' };
		const char* escape_to[] = { "&amp;", "&lt;", "&gt;", "&quot;", "&apos;" };
		for ( unsigned i=0; i<sizeof(escape_char)/sizeof(escape_char[0]); i++ )
		{
			_xml_escapemap[ unsigned(escape_char[i]) ] = escape_to[i];
		}
		isEscapemapInit = true;
	}
}

const char* CEfficientXMLWriter::write( const CEfficientJsonValue& value, int& len, char* buf, const unsigned& bufsize, const char* tag/*="root"*/ )
{
	_p = buf;
	_size = bufsize;
	_p[0] = 0;
	_pos = 0;

	WriteValue( value, tag );
	len = _pos;
	return _p;
}

int CEfficientXMLWriter::WriteValue( const CEfficientJsonValue& value, const char* tag )
{
	switch ( value.type() )
	{
	case nullValue:
		PushNull( tag );
		break;

	case intValue:
		PushInt( tag, value.asInt() );
		break;

	case uintValue:
		PushUInt( tag, value.asUInt() );
		break;

	case realValue:
		PushDouble( tag, value.asDouble() );
		break;

	case stringValue:
		PushCString( tag, value.asCString() );
		break;

	case booleanValue:
		PushBool( tag, value.asBool() );
		break;

	case arrayValue:
		PushArray( tag, value );
		break;

	case objectValue:
		PushObject( tag, value );
		break;
	}
	return 0;
}

int CEfficientXMLWriter::PushNull( const char* tag )
{
	TagNull( tag );
	return 0;
}

int CEfficientXMLWriter::PushInt( const char* tag, int n )
{
	TagBegin( tag );
	ToInt( n );
	TagEnd( tag );
	return 0;
}

int CEfficientXMLWriter::PushUInt( const char* tag, unsigned n )
{
	TagBegin( tag );
	ToUint( n );
	TagEnd( tag );
	return 0;
}

int CEfficientXMLWriter::PushDouble( const char* tag, double f )
{
	TagBegin( tag );
	ToDouble( f );
	TagEnd( tag );
	return 0;
}

int CEfficientXMLWriter::PushBool( const char* tag, bool b )
{
	TagBegin( tag );
	ToCStringNoEscape( b ? STRING_FMT_BOOL_TRUE : STRING_FMT_BOOL_FALSE );
	TagEnd( tag );
	return 0;	
}

int CEfficientXMLWriter::PushCString( const char* tag, const char* s )
{
	TagBegin( tag );
	ToCString( s, _xml_escapemap );
	TagEnd( tag );
	return 0;
}

int CEfficientXMLWriter::PushArray( const char* tag, const CEfficientJsonValue& value )
{
	if ( 0 == value.size() )
	{
		PushNull( tag );
	}
	else
	{
		CEfficientJsonValue elem;
		for ( unsigned i=0; i<value.size(); i++ )
		{
			value.GetArrayElem( i, elem );
			WriteValue( elem, tag );
		}
	}
	return 0;
}

int CEfficientXMLWriter::PushObject( const char* tag, const CEfficientJsonValue& value )
{
	if ( 0 == value.size() )
	{
		PushNull( tag );
	}
	else
	{
		TagBegin( tag );
	
		const char* subtag = NULL;
		CEfficientJsonValue elem;
		for ( unsigned i=0; i<value.size(); i++ )
		{
			value.GetObjectElem( i, subtag, elem );
			WriteValue( elem, subtag );
		}

		TagEnd( tag );
	}
	return 0;
}

void CEfficientXMLWriter::TagBegin( const char* tag )
{
	if ( tag == NULL || *tag == '\0' )
	{
		return;
	}

	ToCStringNoEscape( "<" );
	ToCStringNoEscape( tag );
	ToCStringNoEscape( ">" );
}

void CEfficientXMLWriter::TagEnd( const char* tag )
{
	if ( tag == NULL || *tag == '\0' )
	{
		return;
	}

	ToCStringNoEscape( "</" );
	ToCStringNoEscape( tag );
	ToCStringNoEscape( ">" );
}

void CEfficientXMLWriter::TagNull( const char* tag )
{
	if ( tag == NULL || *tag == '\0' )
	{
		return;
	}

	ToCStringNoEscape( "<" );
	ToCStringNoEscape( tag );
	ToCStringNoEscape( "/>" );
}

