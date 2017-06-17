# -*- coding:utf-8 -*-
from socket import *
import struct
 
def SocketClient():
    try:
        #????socket????
        s=socket(AF_INET,SOCK_STREAM,0)
 
        Colon = ServerUrl.find(':')
        IP = ServerUrl[0:Colon]
        Port = ServerUrl[Colon+1:]
 
        #????????
        s.connect((IP,int(Port)))
        sdata='GET /Test HTTP/1.1\r\n\
Host: %s\r\n\r\n'%ServerUrl
 
        print "Request:\r\n%s\r\n"%sdata
        s.send(sdata)
        sresult=s.recv(1024)
 
        print "Response:\r\n%s\r\n" %sresult
        #?ر?Socket


        values = (1, 'ab', 2.7)
        packer = struct.Struct('I 2s f')
        packed_data = packer.pack(*values)
        sock.sendall(packed_data)

        s.close()





    except Exception,ex:
        print ex
 
ServerUrl = "127.0.0.1:9999"
SocketClient()