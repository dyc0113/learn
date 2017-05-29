#ifndef __QZONE_JSON_WRITE_SHAWXIAO_2010_04_23__
#define __QZONE_JSON_WRITE_SHAWXIAO_2010_04_23__

#include "json.h"

class CFasterJsonWriter
{
public:
	static CFasterJsonWriter* Instance( int size=10240 );
	const char* write( const Json::Value& value, int& len );

protected:
	CFasterJsonWriter();
	int Initialize( int size );

protected:
	int Escape( const char* src, char* dst, int dstsize );
	int WriteValue( const Json::Value& value );

protected:
	int PushNull();
	int PushInt( int );
	int PushUInt( unsigned );
	int PushDouble( double );
	int PushBool( bool );
	int PushCString( const char* );
	int PushCStringNoEscape( const char* );
	int PushArray( const Json::Value& );
	int PushObject( const Json::Value& );

protected:
	char* _p;
	int _size;
	int _pos;

protected:
	static CFasterJsonWriter* _h;
	static std::string _escapemap[256];
};

#endif

