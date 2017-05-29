#include "json.h"
#include "efficient_json_value.h"

using namespace Json;

unsigned POOL_SIZE_STRING = 5120;
unsigned POOL_SIZE_ARRAY = 2048;
unsigned POOL_SIZE_OBJECT = 2048;
unsigned MAX_LEN_STRING = 512;
unsigned MAX_LEN_ARRAY = 100;
unsigned MAX_LEN_OBJECT = 100;

///////////////////////////////////////////////////////////////////////////////////////////
// 数组对象
///////////////////////////////////////////////////////////////////////////////////////////
JsonArray::JsonArray()
{
	_pos = 0;
	_idx = 0;
	_data = new CEfficientJsonValue[ MAX_LEN_ARRAY ];
	_vecdata.push_back(_data);
}

JsonArray::~JsonArray()
{
	for (vector<CEfficientJsonValue*>::iterator it = _vecdata.begin(); it != _vecdata.end(); it++)
	{
		delete[] *it;
	}
}

void JsonArray::Init()
{
	_pos = 0;
	_idx = 0;
	// _data使用到的时候再初始化
}

CEfficientJsonValue& JsonArray::operator[]( const unsigned& index )
{
	EJV_ASSERT( index < Pos());

	int idx = index / MAX_LEN_ARRAY;
	int pos = index % MAX_LEN_ARRAY;

	CEfficientJsonValue * data = _vecdata[idx];

	return data[pos];
}

CEfficientJsonValue& JsonArray::append( const CEfficientJsonValue& v )
{
	if (_pos >= MAX_LEN_ARRAY)
	{
		if (_idx+1 >= _vecdata.size())
		{
			_data = new CEfficientJsonValue[ MAX_LEN_ARRAY ];
			_vecdata.push_back(_data);
		}

		_idx++;
		_pos = 0;
		_data = _vecdata[_idx];
	}

	_data[_pos] = v;
	return _data[_pos++];
}

///////////////////////////////////////////////////////////////////////////////////////////
// 对象对象
///////////////////////////////////////////////////////////////////////////////////////////
JsonObject::JsonObject()
{
	_pos = 0;
	_idx = 0;
	_tag = new CEfficientJsonValue[MAX_LEN_OBJECT];
	_data = new CEfficientJsonValue[MAX_LEN_OBJECT];

	_vectag.push_back(_tag);
	_vecdata.push_back(_data);
}

JsonObject::~JsonObject()
{
	for (vector<CEfficientJsonValue*>::iterator it = _vectag.begin(); it != _vectag.end(); it++)
	{
		delete[] *it;
	}

	for (vector<CEfficientJsonValue*>::iterator it = _vecdata.begin(); it != _vecdata.end(); it++)
	{
		delete[] *it;
	}
}

void JsonObject::Init()
{
	_pos = 0;
	_idx = 0;
	// _tag 使用到的时候才初始化
	// _data使用到的时候再初始化
}

CEfficientJsonValue& JsonObject::operator[]( const char* tag )
{
	// 只往后分配，不支持查找
	if (_pos >= MAX_LEN_OBJECT)
	{
		if (_idx+1 >= _vecdata.size())
		{
			_data = new CEfficientJsonValue[ MAX_LEN_OBJECT ];
			_tag = new CEfficientJsonValue[ MAX_LEN_OBJECT ];
			_vecdata.push_back(_data);
			_vectag.push_back(_tag);
		}

		_idx++;
		_pos = 0;
		_data = _vecdata[_idx];
		_tag = _vectag[_idx];
	}
	_tag[_pos] = EJV_POOL_STRING->Alloc( tag );
	_data[_pos] = CEfficientJsonValue();
	return _data[_pos++];
}

CEfficientJsonValue& JsonObject::Tag( const unsigned& index )
{
	EJV_ASSERT( index < Pos());
	int idx = index / MAX_LEN_OBJECT;
	int pos = index % MAX_LEN_OBJECT;

	CEfficientJsonValue * tag = _vectag[idx];
	
	return tag[pos];
}

CEfficientJsonValue& JsonObject::Value( const unsigned& index )
{
	EJV_ASSERT( index < Pos());
	int idx = index / MAX_LEN_OBJECT;
	int pos = index % MAX_LEN_OBJECT;

	CEfficientJsonValue * data = _vecdata[idx];

	return data[pos];
}

///////////////////////////////////////////////////////////////////////////////////////////
// 字符串对象池
///////////////////////////////////////////////////////////////////////////////////////////
CStringPool::CStringPool()
{
	_pos = 0;
	_idx = 0;

	_data = new char*[POOL_SIZE_STRING];
	for ( unsigned i=0; i<POOL_SIZE_STRING; i++ )
	{
		_data[i] = new char[MAX_LEN_STRING];
	}

	_vecdata.push_back(_data);	
}

CStringPool::~CStringPool()
{
	for (vector<char**>::iterator it = _vecdata.begin(); it != _vecdata.end(); it++)
	{
		_data = *it;
		
		for ( unsigned i=0; i<POOL_SIZE_STRING; i++ )
		{
			delete[] _data[i];
		}
		delete[] _data;

	}
}

CEfficientJsonValue CStringPool::Alloc( const char* str )
{
	if (_pos >= POOL_SIZE_STRING)
	{
		if (_idx+1 >=  _vecdata.size())
		{
			_data = new char*[POOL_SIZE_STRING];
			for ( unsigned i=0; i<POOL_SIZE_STRING; i++ )
			{
				_data[i] = new char[MAX_LEN_STRING];
			}
			
			_vecdata.push_back(_data);
		}

		_idx++;
		_pos = 0;		
		_data = _vecdata[_idx];
	}
	

	int len = strlen(str);
	EJV_ASSERT( len < (int)MAX_LEN_STRING );

	char* s = _data[ _pos++ ];
	memcpy( s, str, len );
	s[ len ] = 0;

	// 设置数据
	CEfficientJsonValue o;
	o._type = stringValue;
	o._value._str = s;
	return o;
}

///////////////////////////////////////////////////////////////////////////////////////////
// 数组对象池
///////////////////////////////////////////////////////////////////////////////////////////
CArrayPool::CArrayPool()
{
	_pos = 0;
	_idx = 0;
	_data = new JsonArray[POOL_SIZE_ARRAY];
	_vecdata.push_back(_data);
}

CArrayPool::~CArrayPool()
{
	for (vector<JsonArray*>::iterator it = _vecdata.begin(); it != _vecdata.end(); it++)
	{
		delete[] *it;
	}
}

CEfficientJsonValue CArrayPool::Alloc()
{
	if (_pos >= POOL_SIZE_ARRAY)
	{
		if (_idx+1 >=  _vecdata.size())
		{
			_data = new JsonArray[POOL_SIZE_ARRAY];
			_vecdata.push_back(_data);
		}

		_idx++;
		_pos = 0;		
		_data = _vecdata[_idx];
	}

	// 需要初始化对象
	JsonArray& arr = _data[ _pos++ ];
	arr.Init();
	return CEfficientJsonValue( &arr );
}

///////////////////////////////////////////////////////////////////////////////////////////
// 对象对象
///////////////////////////////////////////////////////////////////////////////////////////
CObjectPool::CObjectPool()
{
	_pos = 0;
	_idx = 0;
	_data = new JsonObject[POOL_SIZE_OBJECT];
	_vecdata.push_back(_data);
}

CObjectPool::~CObjectPool()
{
	for (vector<JsonObject*>::iterator it = _vecdata.begin(); it != _vecdata.end(); it++)
	{
		delete[] *it;
	}
}

CEfficientJsonValue CObjectPool::Alloc()
{
	if (_pos >= POOL_SIZE_OBJECT)
	{/* 当前行已使用完*/
		if (_idx+1 >=  _vecdata.size())
		{/* 所有行都已用完，再分配一行*/
			_data = new JsonObject[POOL_SIZE_OBJECT];
			_vecdata.push_back(_data);
		}

		/* 指向下一行的开始*/
		_idx++;
		_pos = 0;		
		_data = _vecdata[_idx];
	}

	// 需要初始化对象
	JsonObject& jsonobj = _data[ _pos++ ];
	jsonobj.Init();
	return CEfficientJsonValue( &jsonobj );
}

///////////////////////////////////////////////////////////////////////////////////////////
// 高效JSON对象
///////////////////////////////////////////////////////////////////////////////////////////
void CEfficientJsonValue::Init()
{
	memset( &_value, 0, sizeof(_value) );
	_type = nullValue;
}

CEfficientJsonValue::CEfficientJsonValue()
{
	Init();
}

CEfficientJsonValue::CEfficientJsonValue( const Json::ValueType& type )
{
	Init();
	*this = type;
}

CEfficientJsonValue::CEfficientJsonValue( const int& n )
{
	Init();
	*this = n;
}

CEfficientJsonValue::CEfficientJsonValue( const unsigned& n )
{
	Init();
	*this = n;
}

CEfficientJsonValue::CEfficientJsonValue( const double& n )
{
	Init();
	*this = n;
}

CEfficientJsonValue::CEfficientJsonValue( const bool& b )
{
	Init();
	*this = b;
}

CEfficientJsonValue::CEfficientJsonValue( const char* s )
{
	Init();
	*this = s;
}

CEfficientJsonValue::CEfficientJsonValue( const std::string& s )
{
	Init();
	*this = s;
}

CEfficientJsonValue::CEfficientJsonValue( JsonArray* arr )
{
	_type = arrayValue;
	_value._arr = arr;
}

CEfficientJsonValue::CEfficientJsonValue( JsonObject* obj )
{
	_type = objectValue;
	_value._obj = obj;
}

CEfficientJsonValue& CEfficientJsonValue::operator=( const ValueType& type )
{
	switch ( type )
	{
	case intValue:
		*this = int( 0 );
		break;
	case uintValue:
		*this = unsigned( 0 );
		break;
	case realValue:
		*this = double( 0 );
		break;
	case stringValue:
		*this = "";
		break;
	case booleanValue:
		*this = false;
		break;
	case arrayValue:
		*this = EJV_POOL_ARRAY->Alloc();
		break;
	case objectValue:
		*this = EJV_POOL_OBJECT->Alloc();
		break;
	default:
		*this = CEfficientJsonValue();
		break;
	}
	return *this;
}

CEfficientJsonValue& CEfficientJsonValue::operator=( const CEfficientJsonValue& v )
{
	_type = v._type;
	_value = v._value;
	return *this;
}

CEfficientJsonValue& CEfficientJsonValue::operator=( const int& n )
{
	EJV_ASSERT( nullValue == _type || intValue == _type );

	_value._int = n;
	_type = intValue;
	return *this;
}

CEfficientJsonValue& CEfficientJsonValue::operator=( const unsigned& n )
{
	EJV_ASSERT( nullValue == _type || uintValue == _type );

	_value._uint = n;
	_type = uintValue;
	return *this;
}

CEfficientJsonValue& CEfficientJsonValue::operator=( const double& n )
{
	EJV_ASSERT( nullValue == _type || realValue == _type );

	_value._real = n;
	_type = realValue;
	return *this;
}

CEfficientJsonValue& CEfficientJsonValue::operator=( const bool& b )
{
	EJV_ASSERT( nullValue == _type || booleanValue == _type );

	_value._bool = b;
	_type = booleanValue;
	return *this;
}

CEfficientJsonValue& CEfficientJsonValue::operator=( const char* s )
{
	EJV_ASSERT( nullValue == _type || stringValue == _type );

	*this = EJV_POOL_STRING->Alloc( s );
	return *this;
}

CEfficientJsonValue& CEfficientJsonValue::operator=( const std::string& s )
{
	return ( (*this) = s.c_str() );
}

CEfficientJsonValue& CEfficientJsonValue::operator[]( const int& index )
{
	return (*this)[ (unsigned)index ];
}

CEfficientJsonValue& CEfficientJsonValue::operator[]( const unsigned& index )
{
	EJV_ASSERT( arrayValue == _type );
	EJV_ASSERT( NULL != _value._arr );
	
	return (*_value._arr)[ index ];
}

CEfficientJsonValue& CEfficientJsonValue::operator[]( const char* tag )
{
	EJV_ASSERT( nullValue == _type || objectValue == _type );
	if ( nullValue == _type )
	{
		*this = EJV_POOL_OBJECT->Alloc();
	}

	EJV_ASSERT( NULL != _value._obj );
	return (*_value._obj)[ tag ];
}

CEfficientJsonValue& CEfficientJsonValue::operator[]( const std::string& tag )
{
	return (*this)[ tag.c_str() ];
}

void CEfficientJsonValue::resize( unsigned size )
{
	EJV_ASSERT( nullValue == _type || arrayValue == _type );
	if ( nullValue == _type )
	{
		*this = EJV_POOL_ARRAY->Alloc();
	}

	CEfficientJsonValue null;
	EJV_ASSERT( NULL != _value._obj );
	_value._obj->Init();
	for ( unsigned i=0; i<size; i++ )
	{
		_value._arr->append( null );
	}
}

CEfficientJsonValue& CEfficientJsonValue::append( const CEfficientJsonValue& v )
{
	EJV_ASSERT( nullValue == _type || arrayValue == _type );
	if ( nullValue == _type )
	{
		*this = EJV_POOL_ARRAY->Alloc();
	}

	EJV_ASSERT( NULL != _value._obj );
	return _value._arr->append( v );
}

bool CEfficientJsonValue::isNull() const
{
	return nullValue == _type;
}

bool CEfficientJsonValue::isBool() const
{
	return booleanValue == _type;
}

bool CEfficientJsonValue::isInt() const
{
	return intValue == _type;
}

bool CEfficientJsonValue::isUInt() const
{
	return uintValue == _type;
}

bool CEfficientJsonValue::isIntegral() const
{
	return intValue == _type || uintValue == _type || booleanValue == _type;
}

bool CEfficientJsonValue::isDouble() const
{
	return realValue == _type;
}

bool CEfficientJsonValue::isNumeric() const
{
	return isIntegral() || isDouble();
}

bool CEfficientJsonValue::isString() const
{
	return stringValue == _type;
}

bool CEfficientJsonValue::isArray() const
{
	return isNull() || arrayValue == _type;
}

bool CEfficientJsonValue::isObject() const
{
	return isNull() || objectValue == _type;
}

const char* CEfficientJsonValue::asCString() const
{
	EJV_ASSERT( nullValue == _type || stringValue == _type );
	return NULL == _value._str ? "" : _value._str;
}

std::string CEfficientJsonValue::asString() const
{
	return asCString();
}

int CEfficientJsonValue::asInt() const
{
	EJV_ASSERT( nullValue == _type || intValue == _type );
	return _value._int;
}

unsigned CEfficientJsonValue::asUInt() const
{
	EJV_ASSERT( nullValue == _type || uintValue == _type );
	return _value._uint;
}

double CEfficientJsonValue::asDouble() const
{
	EJV_ASSERT( nullValue == _type || realValue == _type );
	return _value._real;
}

bool CEfficientJsonValue::asBool() const
{
	EJV_ASSERT( nullValue == _type || booleanValue == _type );
	return _value._bool;
}

unsigned CEfficientJsonValue::size() const
{
	EJV_ASSERT( arrayValue == _type || objectValue == _type );

	unsigned size = 0;
	switch ( _type )
	{
	case arrayValue:
		size = _value._arr->Pos();
		break;
	case objectValue:
		size = _value._obj->Pos();
		break;
	}
	return size;
}

int CEfficientJsonValue::GetArrayElem( const unsigned& index, CEfficientJsonValue& v ) const
{
	EJV_ASSERT( arrayValue == _type );
	EJV_ASSERT( index < _value._arr->Pos() );

	v = (*_value._arr)[ index ];
	return 0;
}

int CEfficientJsonValue::GetObjectElem( const unsigned& index, const char*& tag, CEfficientJsonValue& v ) const
{
	EJV_ASSERT( objectValue == _type );
	EJV_ASSERT( index < _value._obj->Pos() );

	tag = ( _value._obj->Tag( index ) ).asCString();
	v = _value._obj->Value( index );
	return 0;
}

