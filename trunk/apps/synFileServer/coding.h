/// @file pgg_common.h
/// @brief 通用功能
/// @author dashdong dashdong@tencent.com
/// @version 1.0
/// @date 2016-5-15
/// @copyright Copyright (c) 2014 Tencent Inc. All Rights Reserved.

#pragma once

#include <string>
#include <ctime>
#include <cstring>
#include <cstdio>
#include <vector>
#include <arpa/inet.h>

using namespace std;

namespace SynServer 

{
     class CCoding
    {
    public:
        // 使用本机的字节序，存储64位整数
        static void SetLocalInt64(char *buff, long long  value);
        // 根据本机字节序获取64 整数
        static void GetLocalInt64(char *buff, long long  &value);
        
        static int SetNetInt32(char *buff, int32_t len);
       
        static int GetNetInt32(char *buff, int32_t &len);
       
        static void EncodeFixed32(char* buf, int32_t value);
    
        static void DecodeFixed32(char *buf, int32_t &value);
      
        static void GetString(char *buf, int32_t len, std::string &value);
       
    };

};
