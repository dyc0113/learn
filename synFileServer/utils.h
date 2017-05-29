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
void debug_epoll_event(epoll_event ev){
        printf("fd(%d), ev.events:", ev.data.fd);

        if(ev.events & EPOLLIN)
                printf(" EPOLLIN ");
        if(ev.events & EPOLLOUT)
                printf(" EPOLLOUT ");
        if(ev.events & EPOLLET)
                printf(" EPOLLET ");
        if(ev.events & EPOLLPRI)
                printf(" EPOLLPRI ");
        if(ev.events & EPOLLRDNORM)
                printf(" EPOLLRDNORM ");
        if(ev.events & EPOLLRDBAND)
                printf(" EPOLLRDBAND ");
        if(ev.events & EPOLLWRNORM)
                printf(" EPOLLRDNORM ");
        if(ev.events & EPOLLWRBAND)
                printf(" EPOLLWRBAND ");
        if(ev.events & EPOLLMSG)
                printf(" EPOLLMSG ");
        if(ev.events & EPOLLERR)
                printf(" EPOLLERR ");
        if(ev.events & EPOLLHUP)
                printf(" EPOLLHUP ");
        if(ev.events & EPOLLONESHOT)
                printf(" EPOLLONESHOT ");

        printf("\n");

}
// Setup nonblocking socket
int setnonblocking(int sockfd)
{
	CHK(fcntl(sockfd, F_SETFL, fcntl(sockfd, F_GETFD, 0)|O_NONBLOCK));// 获取本身状态，然后再非阻塞
	return 0;
}

int saf_tcp_recv(int sockfd, void *buf, int total)
{
    int recv_bytes, cur_len;
    for (recv_bytes = 0; recv_bytes < total; recv_bytes += cur_len)
    {
        cur_len = recv(sockfd, (char *)buf + recv_bytes, total - recv_bytes, 0);
        if (cur_len == 0)
        {
            cerr << "connection closed by peer"<<endl;
            return -1;
        }
        else if (cur_len < 0)
        {
            if (errno == EINTR || errno == EAGAIN)
            {
                cur_len = 0;  // 被中断了，接着读取
            }
            else
            {
                cerr<<"recv tcp packet error fd:"<<sockfd << " total:"<< total<<" cur_len:"<<cur_len<<" errno:"<< errno <<endl;
                return -2;
            }
        }
    }
    return recv_bytes == total ? 0 : -3;
}

int safe_tcp_send (int sockfd, void *buf, int total) {
    int send_bytes, cur_len;

    for (int i = 0; i < total; ++i)
    {
        printf("%x ", ((char*)buf)[i]);
    }
    for (send_bytes = 0; send_bytes < total; send_bytes += cur_len) {
        cur_len = send (sockfd, (char *)buf + send_bytes, total - send_bytes, 0);
        //closed by client
        if (cur_len == 0) {
            printf("send tcp error: fd=%d ",sockfd);
            return -1;
        } else if (cur_len < 0) {
            if (errno == EINTR)
                cur_len = 0;
            else 
            {
                cerr<<"send tcp error: fd=%d "<<sockfd<<endl;
                return -2;
            }
        }
    }

    return send_bytes == total ? 0 : -3;
}

int writeFile(const std::string fileName, char *buf, size_t len)
{
    FILE* f = fopen(fileName.c_str(), "w");
	int iRet = 0;
    if (f == NULL) 
    {
        cout<<"open file for write failed"<<endl;
        return -1;
    } 
    else 
    {
        size_t r = fwrite(buf, len, 1, f);
        if (r != 1)
        {
            cout<<"write file failed"<<endl;
			iRet = -1;
        }
    }
    if(fclose(f) != 0)
    {
         cerr<<"close file failed."<<endl;
         return -1;
    }
    return iRet;
}

#endif
