#pragma once

#include <iostream>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <sys/epoll.h>
#include <fcntl.h>
#include <errno.h>
#include <unistd.h>
#include <stdio.h>
#include <list>
#include <time.h>
#include <stdlib.h>
#include <string.h>
#include <sstream>
#include <fstream>

using namespace std;
#pragma pack(push)
#pragma pack(1)
// 文件同步报文格式
struct SHead
{
    int32_t len;        // 长度
    int32_t flag;       // 标识字段，暂时不用 0 文件传输 1 json命令
};

struct SCommandReq
{
    int32_t len;        // 长度
    int32_t flag;       // 标识字段，暂时不用
    char dir[512];      // 要同步的目录，绝对路径
    char fileName[512]; // 要同步的文件名称
    uint64_t create_tm; // 创建的时间
    char data[0];       // 文件的数据部分
public:
    SCommandReq():len(sizeof(struct SCommandReq)),create_tm(0)
    {
        memset(dir, 0 , sizeof(dir));
        memset(fileName, 0, sizeof(fileName));
    }
    bool IsHaveData()
    {
        int headLen = sizeof(struct SCommandReq);
        if (len == headLen)
        {
            return false;
        }
        else if ( len > headLen)
        {
            return true;
        }
        else
        {
            perror("SCommandReq len unvalid");
            return false;
        }
        
    }
    std::string ToString()
    {
        std::stringstream ss;
        ss<< len<<"|"<<dir<<"|"<<fileName<<"|"<<create_tm<<"|";
        return ss.str();
    }
};

struct SCommandRsp
{
    int32_t len;
    int32_t ret;          //0 不需要同步  1 需要同步  其它 无效
    char msg[200];    // 返回给前端的消息
    char data[0];
public:
    SCommandRsp():len(sizeof(struct SCommandRsp)), ret(0)
    {
        memset(msg, 0 , sizeof(msg));
    }
    bool IsHaveData()
    {
        int headLen = sizeof(struct SCommandRsp);
        if (len == headLen)
        {
            return false;
        }
        else if ( len > headLen)
        {
            return true;
        }
        else
        {
            perror("SCommandReq len unvalid");
        }
	    return false;	
    }
    std::string ToString()
    {
        std::stringstream ss;
        ss<< len<<"|"<<ret<<"|";
        return ss.str();
    }
	
};

struct SCommonReq
{
    int32_t len;        // 长度
    int32_t flag;       // 标识字段，暂时不用
    int32_t extLen;     // 扩展数据长度
    char data[0];       // 扩展数据，可以使用json来传输数据
    
    SCommonReq():
        len(0),flag(0),extLen(0){}
    std::string ToString()
    {
        std::stringstream ss;
        ss<<"len:"<< len<<" flag:"<<flag;
        return ss.str();
    }
};

struct SCommonRsp
{
    int32_t len;        // 长度
    int32_t extLen;     // 扩展数据长度
    char data[0];       // 扩展数据，可以使用json来传输数据
    SCommonRsp():len(0),extLen(0){}
};



#pragma pack(pop)

// Default buffer size
#define BUF_SIZE  1024*4*1024 

// Default port
#define SERVER_PORT 44445

#define SERVER_HOST  "10.121.234.47"

// Default timeout - http://linux.die.net/man/2/epoll_wait
#define EPOLL_RUN_TIMEOUT -1

// Count of connections that we are planning to handle (just hint to kernel)
#define EPOLL_SIZE 10000

// First welcome message from server
#define STR_WELCOME "Welcome to seChat! You ID is: Client #%d"

// Format of message population
#define STR_MESSAGE "Client #%d>> %s"

// Warning message if you alone in server
#define STR_NOONE_CONNECTED "Noone connected to server except you!"

// Commad to exit
#define CMD_EXIT "EXIT"

// Macros - exit in any error (eval < 0) case
#define CHK(eval) if(eval < 0){perror("eval"); exit(-1);}

// Macros - same as above, but save the result(res) of expression(eval) 
#define CHK2(res, eval) if((res = eval) < 0){perror("eval"); exit(-1);}

// Preliminary declaration of functions
int setnonblocking(int sockfd);
void debug_epoll_event(epoll_event ev);
int handle_message(int new_fd);
int print_incoming(int fd);



