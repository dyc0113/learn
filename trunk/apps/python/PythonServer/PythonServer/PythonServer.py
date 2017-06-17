# -*- coding:utf-8 -*-
from socket import *
 
def SocketServer():
    try:
        Colon = ServerUrl.find(':')
        IP = ServerUrl[0:Colon]
        Port = int(ServerUrl[Colon+1:])
 
        #????socket????
        print 'Server start:%s'%ServerUrl
        sockobj = socket(AF_INET, SOCK_STREAM)
        sockobj.setsockopt(SOL_SOCKET,SO_REUSEADDR, 1)
 
        #????IP?˿ں?
        sockobj.bind((IP, Port))
        #??????????5??????
        sockobj.listen(5)
 
        #ֱ?????̽???ʱ?Ž???ѭ??
        while True:
            #?ȴ?client????
            connection, address = sockobj.accept( )
            print 'Server connected by client:', address
            while True:
                #??ȡClient??Ϣ??????
                data = connection.recv(1024)
                #????û??data??????ѭ??
                if not data: break
                #???ͻظ???Client
                RES='200 OK'
                connection.send(RES)
                print 'Receive MSG:%s'%data.strip()
                print 'Send RES:%s\r\n'%RES
            #?ر?Socket
            connection.close( )
 
    except Exception,ex:
        print ex
 
ServerUrl = "127.0.0.1:9999"
SocketServer()