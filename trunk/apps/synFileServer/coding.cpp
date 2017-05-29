#include "coding.h"

using namespace std;

namespace SynServer 
{
 // 本机字节序64位
 void CCoding::SetLocalInt64(char *buff, const  long long value)
{
    memcpy(buff, &value, sizeof(value)); 
}
 // 网络字节序64位
 void CCoding::GetLocalInt64(char *buff, long long  &value)
{
    memcpy((char*)&value, buff, sizeof(value)); 
}
 int CCoding::SetNetInt32(char *buff, int32_t len)
{
    int tmpLen = htonl(len);
    EncodeFixed32(buff, tmpLen);
    return 0;
}
int CCoding::GetNetInt32(char *buff, int32_t &len)
{
    int tmpValue;
    DecodeFixed32(buff, tmpValue);
    len = ntohl(tmpValue);
    return 0;
}
void CCoding::EncodeFixed32(char* buf, int32_t value)
{
    memcpy(buf, &value, sizeof(value));
}
void CCoding::DecodeFixed32(char *buf, int32_t &value)
{
    memcpy((char*)&value, buf, sizeof(value));
}
void CCoding::GetString(char *buf, int32_t len, std::string &value)
{
    value.append(buf);
}

};


