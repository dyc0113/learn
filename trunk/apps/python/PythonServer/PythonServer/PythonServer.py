# -*- coding:utf-8 -*-
from socket import *
import struct
import json
#文件传输的例子 
#http://www.cnblogs.com/dreamer-fish/p/5501924.html

def printWindow(contentData):
    print contentData.decode("UTF-8").encode("GBK")

#获取请求的内容json串
def GetContentData(connection, dataLen):
     #接收报文内容信息
    try:
        contentData=""
        recvd_size = 0 #定义接收了的文件大小
        print 'stat receiving...'
        while not recvd_size == dataLen:
            if dataLen - recvd_size > 1024:
                rdata = connection.recv(1024)
                contentData = contentData + rdata
                recvd_size += len(rdata)
            else:
                rdata = connection.recv(dataLen - recvd_size) 
                contentData = contentData + rdata
                recvd_size = dataLen

        print 'receive done'
    except Exception,ex:
        print ex
    return contentData

def SocketServer():
    try:
        Colon = ServerUrl.find(':')
        IP = ServerUrl[0:Colon]
        Port = int(ServerUrl[Colon+1:])
 
        print 'Server start:%s'%ServerUrl
        sockobj = socket(AF_INET, SOCK_STREAM)
        sockobj.setsockopt(SOL_SOCKET,SO_REUSEADDR, 1)
 
        sockobj.bind((IP, Port))
        sockobj.listen(5)
 
        while True:
            connection, address = sockobj.accept( )
            print 'Server connected by client:', address

            fileinfo_size=struct.calcsize('128sl') #定义文件信息。128s 命令字 ，l表示一个int或log文件类型，在此为文件大小
            data_buf = connection.recv(fileinfo_size) #接受报文头信息
            #输出报文头信息
            cmd,dataLen =struct.unpack('128sl', data_buf)
            print "cmd:",cmd,"  dataLen:", dataLen;

            contentData = GetContentData(connection, dataLen);

            printWindow("receive data:"  + contentData) 

            #data = connection.recv(1024)
            #if not data: break
            #RES='200 OK'
            #connection.send(RES)
            #print 'Receive MSG:%s'%data.strip()
            #print 'Send RES:%s\r\n'%RES
            connection.close( )

    except Exception,ex:
        print ex
 
ServerUrl = "127.0.0.1:9999"
SocketServer()