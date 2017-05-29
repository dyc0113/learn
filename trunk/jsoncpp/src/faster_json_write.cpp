#include "faster_json_write.h"

#define QZONE_JSON_PUSHVALUE( fmt, args... ) \
	{ \
		char* cur_p = _p + _pos; \
		int cur_size =  _size - _pos; \
		int cur_move = 0; \
		cur_move = snprintf( cur_p, cur_size, fmt, ##args ); \
		if ( cur_move >= cur_size ) \
		{ \
			*cur_p = 0; \
		} \
		else \
		{ \
			_pos += cur_move; \
		} \
	}

CFasterJsonWriter* CFasterJsonWriter::_h = NULL;
std::string CFasterJsonWriter::_escapemap[256];

CFasterJsonWriter* CFasterJsonWriter::Instance( int size )
{
	if ( NULL == _h )
	{
		_h = new CFasterJsonWriter;
		_h->Initialize( size );
	}
	return _h;
}

const char* CFasterJsonWriter::write( const Json::Value& value, int& len )
{
	_p[0] = 0;
	_pos = 0;
	WriteValue( value );
	len = _pos;
	return _p;
}

CFasterJsonWriter::CFasterJsonWriter()
{
	_p = NULL;
	_size = 0;
	_pos = 0;
}

int CFasterJsonWriter::Initialize( int size )
{
	_p = new char[size+1];
	_size = size;
	_pos = 0;

	unsigned char escape_char[] = { '\"', '\\', '\b', '\f', '\n', '\r', '\t' };
	const char* escape_to[] = { "\\\"", "\\\\", "\\b", "\\f", "\\n", "\\r", "\\t" };
	for ( unsigned i=0; i<sizeof(escape_char)/sizeof(escape_char[0]); i++ )
	{
		_escapemap[ unsigned(escape_char[i]) ] = escape_to[i];
	}
	return 0;
}

int CFasterJsonWriter::Escape( const char* src, char* dst, int dstsize )
{
	char* bak = dst;
	unsigned char c;
	while ( 0 != (c=*src) )
	{
		std::string& to = _escapemap[c];
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
	return dst-bak;
}

int CFasterJsonWriter::PushNull()
{
	PushCStringNoEscape( "\"\"" );
	return 0;
}

int CFasterJsonWriter::PushInt( int n )
{
	if ( n < 0 )
	{
		PushCStringNoEscape( "-" );
		PushUInt( -n );
	}
	else
	{
		PushUInt( n );
	}
	return 0;
}

int CFasterJsonWriter::PushUInt( unsigned n )
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
	
	int len = sizeof(tmp)-(current-tmp) - 1;
	if ( _size-_pos > len )
	{
		char* cur_p = _p + _pos;
		while ( ( *cur_p++ = *current++ ) );
		_pos += len; \
	}
	return 0;
}

int CFasterJsonWriter::PushDouble( double f )
{
	QZONE_JSON_PUSHVALUE( "%f", f );
	return 0;
}

int CFasterJsonWriter::PushBool( bool b )
{
	PushCStringNoEscape( b ? "true" : "false" );
	return 0;
}

int CFasterJsonWriter::PushCStringNoEscape( const char* s )
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

int CFasterJsonWriter::PushCString( const char* s  )
{
	PushCStringNoEscape( "\"" );
	
	char* cur_p = _p + _pos;
	int cur_size =  _size - _pos;
	int cur_move = Escape( s, cur_p, cur_size );
	_pos += cur_move;

	PushCStringNoEscape( "\"" );
	return 0;
}

int CFasterJsonWriter::PushArray( const Json::Value& value )
{
	PushCStringNoEscape( "[" );

	Json::Value::ObjectValues::const_iterator itr_begin = value.Begin();
	Json::Value::ObjectValues::const_iterator itr_end = value.End();
	if ( itr_begin != itr_end )
	{
		WriteValue( itr_begin->second );
		for ( itr_begin++; itr_begin!=itr_end; itr_begin++ )
		{
			PushCStringNoEscape( "," );
			WriteValue( itr_begin->second );
		}
	}
	
	PushCStringNoEscape( "]" );
	return 0;
}

int CFasterJsonWriter::PushObject( const Json::Value& value )
{
	PushCStringNoEscape( "{" );

	Json::Value::ObjectValues::const_iterator itr_begin = value.Begin();
	Json::Value::ObjectValues::const_iterator itr_end = value.End();
	if ( itr_begin != itr_end )
	{
		PushCStringNoEscape( "\"" );
		PushCStringNoEscape( itr_begin->first.c_str() );
		PushCStringNoEscape( "\":" );
		WriteValue( itr_begin->second );

		for ( itr_begin++; itr_begin!=itr_end; itr_begin++ )
		{
			PushCStringNoEscape( ",\"" );
			PushCStringNoEscape( itr_begin->first.c_str() );
			PushCStringNoEscape( "\":" );
			WriteValue( itr_begin->second );
		}
	}
	PushCStringNoEscape( "}" );
	return 0;
}

int CFasterJsonWriter::WriteValue( const Json::Value& value )
{
	switch ( value.type() )
	{
	case Json::nullValue:
		PushNull();
		break;

	case Json::intValue:
		PushInt( value.asInt() );
		break;

	case Json::uintValue:
		PushInt( value.asUInt() );
		break;

	case Json::int64Value:
		PushInt( value.asInt64() );
		break;

	case Json::uint64Value:
		PushInt( value.asUInt64() );
		break;

	case Json::realValue:
		PushDouble( value.asDouble() );
		break;

	case Json::stringValue:
		PushCString( value.asCString() );
		break;

	case Json::booleanValue:
		PushBool( value.asBool() );
		break;

	case Json::arrayValue:
		PushArray( value );
		break;

	case Json::objectValue:
		PushObject( value );
		break;
	}
	return 0;
}

