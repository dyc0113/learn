#ifndef __EFFICIENT_JSON_WRITE_SHAWXIAO_2010_10_27__
#define __EFFICIENT_JSON_WRITE_SHAWXIAO_2010_10_27__

#include <string>
#include "efficient_json_value.h"

class CEfficientWriter
{
public:
	CEfficientWriter();

protected:
	int ToInt( int n );
	int ToUint( unsigned n );
	int ToDouble( double f );
	int ToCStringNoEscape( const char* s );
	int ToCString( const char* src, std::string* escapemap );

protected:
	char* _p;
	unsigned _size;
	unsigned _pos;
};

class CEfficientJsonWriter : public CEfficientWriter
{
public:
	CEfficientJsonWriter();
	const char* write( const CEfficientJsonValue& value, int& len, char* buf, const unsigned& bufsize );

protected:
	int WriteValue( const CEfficientJsonValue& value );

protected:
	int PushNull();
	int PushInt( int n );
	int PushUInt( unsigned n );
	int PushDouble( double n );
	int PushBool( bool b );
	int PushCString( const char* s );
	int PushArray( const CEfficientJsonValue& v );
	int PushObject( const CEfficientJsonValue& v );
};

class CEfficientXMLWriter : public CEfficientWriter
{
public:
	CEfficientXMLWriter();
	const char* write( const CEfficientJsonValue& value, int& len, char* buf, const unsigned& bufsize, const char* tag="root" );

protected:
	int WriteValue( const CEfficientJsonValue& value, const char* tag );

protected:
	int PushNull( const char* tag );
	int PushInt( const char* tag, int n );
	int PushUInt( const char* tag, unsigned n );
	int PushDouble( const char* tag, double n );
	int PushBool( const char* tag, bool b );
	int PushCString( const char* tag, const char* s );
	int PushArray( const char* tag, const CEfficientJsonValue& v );
	int PushObject( const char* tag, const CEfficientJsonValue& v );

protected:
	void TagBegin( const char* tag );
	void TagEnd( const char* tag );
	void TagNull( const char* tag );
};

#endif

