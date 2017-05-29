#ifndef UTILS_H
#define UTILS_H
#include "local.h"

//一些返回值常量.
#define CONSTANT_SUCESS      0
#define	CONSTANT_NEED_SYN    1       // 需要同步
#define CONSTANT_NOT_NEED_SYN 2      // 不需要同步
#define CONSTANT_GET_FILE_STAT_FAILED 3  // 获取文件状态失败
#define CONSTANT_NOT_FOLDER 4        // 不是一个文件夹
#define CONSTANT_WRITE_FILE_FAILED 5
// Debug epoll_event
void debug_epoll_event(epoll_event ev);

// Setup nonblocking socket
int setnonblocking(int sockfd);

int saf_tcp_recv(int sockfd, void *buf, int total);


int safe_tcp_send (int sockfd, void *buf, int total) ;

int writeFile(const std::string fileName, char *buf, size_t len);


#endif
