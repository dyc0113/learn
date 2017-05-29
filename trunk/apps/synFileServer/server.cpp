#include "command.h"
#include "utils.h"
#include "local.h"
#include "coding.h"
using namespace std;

namespace SynServer
{
// To store client's socket list
list<int> clients_list;

static int DEBUG_MODE = 1;
char *g_buf = new char[BUF_SIZE];
char *g_data = new char[BUF_SIZE];


int GetIp(std::string &strIp)
{
    //strIp = "192.168.174.137";
    //return 0;
	struct hostent *he;
	char hostname[20] = {0};
	gethostname(hostname,sizeof(hostname));
	he = gethostbyname(hostname);
	printf("hostname=%s\n",hostname);
	printf("%s\n",inet_ntoa(*(struct in_addr*)(he->h_addr)));
	strIp.assign(inet_ntoa(*(struct in_addr*)(he->h_addr)));
	return 0;
}

//  处理客户端命令。
int handle_client_command(int client)
{
    cout<<"----------------------------处理客户端请求---------------------"<<endl;
    int iRet = 0;

    bzero(g_buf, BUF_SIZE);
    bzero(g_data, BUF_SIZE);

    // try to get new raw message from client 
    if(DEBUG_MODE) printf("Try to read from fd(%d)\n", client); 
    iRet =saf_tcp_recv(client, g_buf, sizeof(SHead));
    if (iRet != 0)
    {
        cerr<<"reve len failed"<<endl;
        return -1;
    }
    // 开始对文件处理
    SHead *pHead = ((SHead *)g_buf);
    cout<<"data:len"<<pHead->len<<" flag:"<<pHead->flag<<endl;
    // 接受完整的请求包
    iRet =saf_tcp_recv(client, g_buf + sizeof(SHead), pHead->len - sizeof(SHead));
    if (iRet != 0)
    {
        cerr<<"reve len failed"<<endl;
        return -1;
    } 
    cout<<"revev all data"<<endl;
    CProcessCommand process;
    if (pHead->flag == 1)
    {
        SCommonReq *pReq = (SCommonReq*)g_buf; 
        // 解析出json字符串
        std::cout<<"header:"<<pReq->ToString().c_str()<<endl;
        std::string strJson(pReq->data);
        std::string strRsp;
        process.process_client_command(strJson, strRsp);
        std::cout<<"rsp:"<<strRsp.c_str()<<endl;
        // 组装回应报文
        static char rspBuff[1024 * 4] = {0};
        CCoding::EncodeFixed32(rspBuff, sizeof(SCommonRsp) + strRsp.size());
        CCoding::EncodeFixed32(rspBuff + 4, strRsp.size());
        memcpy(rspBuff + 8, strRsp.data(), strRsp.size());


        SCommonRsp *pRsp = (SCommonRsp*)rspBuff;

        cout<<"回应报文长度:"<<pRsp->len<<endl;
        iRet = safe_tcp_send(client, rspBuff , pRsp->len);
        if (0 != iRet)
        {
            cerr << "send data failed "<<endl;
        }

    }
    else if (pHead->flag == 0)  // 传输文件的的请求.
    {
        SCommandReq *pReq = (SCommandReq*)g_buf; 
        SCommandRsp  stRsp;
        iRet = process.handle_file_message(pReq, stRsp);
        stRsp.len = sizeof(SCommandRsp) ;
        if (0 != iRet)
        {
            cout<<"handle_file_message failed"<<endl;
            strcpy(stRsp.msg, "同步文件失败!");
        }
        cout<<"响应报文:"<<stRsp.ToString()<<endl;

        iRet = safe_tcp_send(client, (void*)&stRsp , sizeof(stRsp));
        if (0 != iRet)
        {
            cerr << "send data failed "<<endl;
        }
    }
    cout<<"--------------------------------处理客户端请求结束----------------------"<<endl;
    return iRet;
}

// *** Handle incoming message from clients
int handle_message(int client)
{
    // get row message from client(buf)
    //     and format message to populate(message)
    char buf[BUF_SIZE], message[BUF_SIZE];
    bzero(buf, BUF_SIZE);
    bzero(message, BUF_SIZE);

    // to keep different results
    int len;

    // try to get new raw message from client 
    if(DEBUG_MODE) printf("Try to read from fd(%d)\n", client); 
    CHK2(len,recv(client, buf, BUF_SIZE, 0));

    // zero size of len mean the client closed connection
    if(len == 0){
	CHK(close(client));
        clients_list.remove(client);
	if(DEBUG_MODE) printf("Client with fd: %d closed! And now clients_list.size = %lu\n", client, clients_list.size());
    // populate message around the world
    }else{ 

        if(clients_list.size() == 1) { // this means that noone connected to server except YOU!
		CHK(send(client, STR_NOONE_CONNECTED, strlen(STR_NOONE_CONNECTED), 0));
                return len;
        }
        
        // format message to populate
        sprintf(message, STR_MESSAGE, client, buf);

        // populate message around the world ;-)...
        list<int>::iterator it;
        for(it = clients_list.begin(); it != clients_list.end(); it++){
           if(*it != client){ // ... except youself of course
		CHK(send(*it, message, BUF_SIZE, 0));
                if(DEBUG_MODE) printf("Message '%s' send to client with fd(%d) \n", message, *it);
           }
        }
        if(DEBUG_MODE) printf("Client(%d) received message successfully:'%s', a total of %d bytes data...\n",
             client, 
             buf,
             len);
    }

    return len;
}
};

using namespace SynServer;
int main(int argc, char *argv[])
{

    {
        if (argc > 1 && std::string(argv[1]) == "test")
        {
            cout<<"test"<<endl;
            CProcessCommand process;
            process.test();
            return 0;
        }
    }
    if (argc > 1 )
    {
        std::string cmd = argv[1];
        cout<<cmd<<endl;
        if (cmd == "daemon")
            daemon(1, 1);
    }
    {
        int32_t t = 208;
        uint8_t *p = (uint8_t*)&t;
        for (int i =0; i<sizeof(t); ++i)
        {
            printf("%x \n", p[i]);
        }
     }

    std::string strIp;
	GetIp(strIp);
    cout<<"监听的IP:"<<strIp.c_str()<<" port:"<<SERVER_PORT<<endl;
    // cout<<sizeof(SCommandReq)<<endl;
    //return 0;
	// *** Define debug mode
	//     any additional parameres on startup
	//     i.e. like './server f' or './server debug'
	//     we will switch to switch to debug mode(very simple anmd useful)
	if(argc > 1) DEBUG_MODE = 1;

	if(DEBUG_MODE){
		printf("Debug mode is ON!\n");
		printf("MAIN: argc = %d\n", argc);
		for(int i=0; i<argc; i++)
			printf(" argv[%d] = %s\n", i, argv[i]);
	}else printf("Debug mode is OFF!\n");

	// *** Define values
	//     main server listener 
	int listener;

	// define ip & ports for server(addr)
	//     and incoming client ip & ports(their_addr)
	struct sockaddr_in addr, their_addr;
	//     configure ip & port for listen
	addr.sin_family = PF_INET;
	addr.sin_port = htons(SERVER_PORT);
	addr.sin_addr.s_addr = inet_addr(strIp.c_str());

	//     size of address
	socklen_t socklen;
	socklen = sizeof(struct sockaddr_in);

	//     event template for epoll_ctl(ev)
	//     storage array for incoming events from epoll_wait(events)
	//        and maximum events count could be EPOLL_SIZE
	static struct epoll_event ev, events[EPOLL_SIZE];
	//     watch just incoming(EPOLLIN) 
	//     and Edge Trigged(EPOLLET) events
	ev.events = EPOLLIN | EPOLLET;

	//     epoll descriptor to watch events
	int epfd;

	//     to calculate the execution time of a program
	clock_t tStart;

	// other values:
	//     new client descriptor(client)
	//     to keep the results of different functions(res)
	//     to keep incoming epoll_wait's events count(epoll_events_count)
	int client, epoll_events_count;


	// *** Setup server listener
	//     create listener with PF_INET(IPv4) and 
	//     SOCK_STREAM(sequenced, reliable, two-way, connection-based byte stream)
	CHK2(listener, socket(PF_INET, SOCK_STREAM, 0));
	printf("Main listener(fd=%d) created! \n",listener);

	//    setup nonblocking socket
	//setnonblocking(listener);

	//    bind listener to address(addr)
	CHK(bind(listener, (struct sockaddr *)&addr, sizeof(addr)));
	printf("Listener binded to: %s\n", strIp.c_str());

	//    start to listen connections
	CHK(listen(listener, 1));
	printf("Start to listen: %s!\n", strIp.c_str());

	// *** Setup epoll
	//     create epoll descriptor 
	//     and backup store for EPOLL_SIZE of socket events
	CHK2(epfd,epoll_create(EPOLL_SIZE));
	printf("Epoll(fd=%d) created!\n", epfd);

	//     set listener to event template 
	ev.data.fd = listener;

	//     add listener to epoll
	CHK(epoll_ctl(epfd, EPOLL_CTL_ADD, listener, &ev));
	printf("Main listener(%d) added to epoll!\n", epfd);

	// *** Main cycle(epoll_wait)
	while(1)
	{
		CHK2(epoll_events_count,epoll_wait(epfd, events, EPOLL_SIZE, EPOLL_RUN_TIMEOUT));
		if(DEBUG_MODE) printf("Epoll events count: %d\n", epoll_events_count);
		// setup tStart time
		tStart = clock();

		for(int i = 0; i < epoll_events_count ; i++)
		{
			if(DEBUG_MODE){
				printf("events[%d].data.fd = %d\n", i, events[i].data.fd); 
				debug_epoll_event(events[i]);

			}
			// EPOLLIN event for listener(new client connection)
			if(events[i].data.fd == listener)
			{
				CHK2(client,accept(listener, (struct sockaddr *) &their_addr, &socklen));
				if(DEBUG_MODE) printf("connection from:%s:%d, socket assigned to:%d \n", 
						inet_ntoa(their_addr.sin_addr), 
						ntohs(their_addr.sin_port), 
						client);
				// setup nonblocking socket
				setnonblocking(client);

				// set new client to event template
				ev.data.fd = client;

				// add new client to epoll
				CHK(epoll_ctl(epfd, EPOLL_CTL_ADD, client, &ev));

				// save new descriptor to further use
				clients_list.push_back(client); // add new connection to list of clients
				if(DEBUG_MODE) printf("Add new client(fd = %d) to epoll and now clients_list.size = %lu\n", 
						client, 
						clients_list.size());

				// send initial welcome message to client
			   /*bzero(message, BUF_SIZE);
				res = sprintf(message, STR_WELCOME, client);
				CHK2(res, send(client, message, BUF_SIZE, 0));*/

			}else { // EPOLLIN event for others(new incoming message from client)
				//CHK2(res,handle_message(events[i].data.fd));
                int iRet = handle_client_command(events[i].data.fd);
                if (iRet != 0)
                {
                    cerr<<"handle file failed"<<endl;
                }
			}
		}
		// print epoll events handling statistics
		printf("Statistics: %d events handled at: %.2f second(s)\n", 
				epoll_events_count, 
				(double)(clock() - tStart)/CLOCKS_PER_SEC);
	}

	close(listener);
	close(epfd);

	return 0;
}


