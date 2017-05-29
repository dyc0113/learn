#ifndef __EFFICIENT_JSON_VALUE_SHAWXIAO_2010_10_25__
#define __EFFICIENT_JSON_VALUE_SHAWXIAO_2010_10_25__

#include <stdexcept>
#include <vector>
#include "hydra_singleton.h"
#include "json.h"

extern unsigned POOL_SIZE_STRING;
extern unsigned POOL_SIZE_ARRAY;
extern unsigned POOL_SIZE_OBJECT;
extern unsigned MAX_LEN_STRING;
extern unsigned MAX_LEN_ARRAY;
extern unsigned MAX_LEN_OBJECT;

using std::vector;

#define EJV_ASSERT( conf )	if ( !(conf) ) { char err[256]; snprintf( err, sizeof(err), "exception in %s-%d-%s", __FILE__, __LINE__, __FUNCTION__ ); throw std::runtime_error( err ); }

#define EJV_POOL_STRING		( hydra::HSingleton<CStringPool>::Instance() )
#define EJV_POOL_ARRAY		( hydra::HSingleton<CArrayPool>::Instance() )
#define EJV_POOL_OBJECT		( hydra::HSingleton<CObjectPool>::Instance() )

#define EJV_POOL_DESTROY \
	hydra::HSingleton<CStringPool>::Destroy(); \
	hydra::HSingleton<CArrayPool>::Destroy(); \
	hydra::HSingleton<CObjectPool>::Destroy(); \

#define EJV_POOL_CLEAR \
	EJV_POOL_STRING->Clear(); \
	EJV_POOL_ARRAY->Clear(); \
	EJV_POOL_OBJECT->Clear();

class JsonArray;
class JsonObject;
class CStringPool;
class CArrayPool;
class CObjectPool;

class CEfficientJsonValue
{
public:
	CEfficientJsonValue();
	CEfficientJsonValue( const Json::ValueType& );
	CEfficientJsonValue( const int& );
	CEfficientJsonValue( const unsigned& );
	CEfficientJsonValue( const double& );
	CEfficientJsonValue( const bool& );
	CEfficientJsonValue( const char* );
	CEfficientJsonValue( const std::string& );
	CEfficientJsonValue( JsonArray* );
	CEfficientJsonValue( JsonObject* );

public:
	CEfficientJsonValue& operator=( const CEfficientJsonValue& );
	CEfficientJsonValue& operator=( const Json::ValueType& );
	CEfficientJsonValue& operator=( const int& );
	CEfficientJsonValue& operator=( const unsigned& );
	CEfficientJsonValue& operator=( const double& );
	CEfficientJsonValue& operator=( const bool& );
	CEfficientJsonValue& operator=( const char* );
	CEfficientJsonValue& operator=( const std::string& );

public:
	// 数组类型操作
	void resize( unsigned );
	CEfficientJsonValue& append( const CEfficientJsonValue& );
	CEfficientJsonValue& operator[]( const int& );
	CEfficientJsonValue& operator[]( const unsigned& );

public:
	// 对象类型操作, 注意不支持读操作，每次都插入新对象
	CEfficientJsonValue& operator[]( const char* );
	CEfficientJsonValue& operator[]( const std::string& );

public:
	int type() const{ return _type; };

public:
	  bool isNull() const;
	  bool isBool() const;
	  bool isInt() const;
	  bool isUInt() const;
	  bool isIntegral() const;
	  bool isDouble() const;
	  bool isNumeric() const;
	  bool isString() const;
	  bool isArray() const;
	  bool isObject() const;

public:
	  const char* asCString() const;
	  std::string asString() const;
	  int asInt() const;
	  unsigned asUInt() const;
	  double asDouble() const;
	  bool asBool() const;

public:
	unsigned size() const;
	int GetArrayElem( const unsigned& index, CEfficientJsonValue& v ) const;
	int GetObjectElem( const unsigned& index, const char*& tag, CEfficientJsonValue& v ) const;

protected:
	void Init();

protected:
	int _type;
	union Value
	{
		int _int;
		unsigned _uint;
		double _real;
		bool _bool;
		const char* _str;
		JsonObject* _obj;
		JsonArray* _arr;
	}_value;

protected:
	friend class CStringPool;
	friend class CArrayPool;
	friend class CObjectPool;
};

class JsonArray
{
public:
	JsonArray();
	~JsonArray();

public:
	void Init();
	CEfficientJsonValue& operator[]( const unsigned& );
	CEfficientJsonValue& append( const CEfficientJsonValue& v );
	unsigned Pos(){ return _pos+_idx*MAX_LEN_ARRAY; };

protected:
	unsigned _pos;
	unsigned _idx;
	CEfficientJsonValue* _data;
	vector<CEfficientJsonValue*> _vecdata;
};

class JsonObject
{
public:
	JsonObject();
	~JsonObject();

public:
	void Init();
	CEfficientJsonValue& operator[]( const char* );
	CEfficientJsonValue& Tag( const unsigned& );
	CEfficientJsonValue& Value( const unsigned& );
	unsigned Pos(){ return _pos+_idx*MAX_LEN_OBJECT; };

protected:
	unsigned _pos;
	unsigned _idx;
	CEfficientJsonValue* _tag;
	CEfficientJsonValue* _data;
	vector<CEfficientJsonValue*> _vectag;
	vector<CEfficientJsonValue*> _vecdata;
};

class CStringPool
{
public:
	CStringPool();
	~CStringPool();

public:
	CEfficientJsonValue Alloc( const char* );
	void Clear(){ _pos = 0; _idx=0; _data=_vecdata[0]; };

protected:
	unsigned _pos;			  /* 行偏移量 */
	unsigned _idx;			  /* 行号 */
	char** _data;			  /* 行首指针 */
	vector<char**> _vecdata; 	  /* 元素为每一行的行首指针*/
};

class CArrayPool
{
public:
	CArrayPool();
	~CArrayPool();

public:
	CEfficientJsonValue Alloc();
	void Clear(){ _pos = 0; _idx = 0; _data=_vecdata[0]; };

protected:
	unsigned _pos;				  /* 行偏移量 */
	unsigned _idx;				  /* 行号 */
	JsonArray* _data;			  /* 行首指针 */
	vector<JsonArray*> _vecdata;  /* 元素为每一行的行首指针*/
};

class CObjectPool
{
public:
	CObjectPool();
	~CObjectPool();

public:
	CEfficientJsonValue Alloc();
	void Clear(){ _pos = 0; _idx = 0; _data=_vecdata[0]; };

protected:
	unsigned _pos;				  /* 行偏移量 */
	unsigned _idx;				  /* 行号 */
	JsonObject* _data;			  /* 行首指针 */
	vector<JsonObject*> _vecdata;   /* 元素为每一行的行首指针 */
};

#endif

