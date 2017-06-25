# -*- coding:utf-8 -*-
from socket import *
import struct
import json
#文件传输的例子 
#http://www.cnblogs.com/dreamer-fish/p/5501924.html

import logging
logging.basicConfig(level=logging.DEBUG,
                format='%(asctime)s %(filename)s[line:%(lineno)d] %(levelname)s %(message)s',
                datefmt='%a, %d %b %Y %H:%M:%S',
                filename='server.log',
                filemode='w')

def printWindow(contentData):
    print contentData.decode("UTF-8").encode("GBK")

#获取请求的内容json串
def RecevData(connection, dataLen):
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

#接受客户端数据，并根据协议解析出dict结构返回
def RecevDictData(conn, address):
    iRet = 0
    fileinfo_size=struct.calcsize('128sl') #定义文件信息。128s 命令字 ，l表示一个int或log文件类型，在此为文件大小
    data_buf = conn.recv(fileinfo_size) #接受报文头信息
    #输出报文头信息
    cmd,dataLen =struct.unpack('128sl', data_buf)
    print "cmd:",cmd,"  dataLen:", dataLen;
    contentData = RecevData(conn, dataLen);
    printWindow("receive data:"  + contentData) 
    dReq = json.loads(contentData) #
    dReq["cmd"] = cmd;
    return iRet, dReq;

#想对端发送一个命令字
def SendData(s, cmd, contentData):
    try:

        fileinfo_size=struct.calcsize('128sl') #定义打包规则
        #定义文件头信息，包含文件名和文件大小
        fhead = struct.pack('128sl',cmd , len(contentData))
        print "send head"
        s.send(fhead) 
        print "send data content", contentData.decode("UTF-8").encode("GBK")
        s.send(contentData)
        print 'send over...'

    except Exception,ex:
        print "exceprion", e

#发送字典数据
def SendDictData(conn, dData):
    jsonReq = json.dumps(dData, ensure_ascii=False, sort_keys=True)
    print jsonReq.encode("gbk")
    SendData(conn, "rsp", jsonReq);


def processTestCmd(mReq):
    iRet = 0;
    mRsp ={}
    mRsp["rsp"] = "success" 
    return iRet, mRsp;

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
            try:
                connection, address = sockobj.accept( )
                print 'Server connected by client:', address

                iRet, dReq = RecevDictData(connection, address); #收包
                if (iRet != 0):
                    logging.error('RecevDictData faild. iRet:%d', iRet)
                    break
                mRsp = {} 
                logging.debug('dReq:%s', str(dReq));
                if "test" in dReq["cmd"]:
                    iRet,mRsp = processTestCmd(dReq);
          
                #回包
                logging.debug('begin SendDictData:%s', str(mRsp));
                SendDictData(connection, mRsp);
                connection.close( )
            except Exception,ex:
                print ex

    except Exception,ex:
        print ex
 
ServerUrl = "127.0.0.1:9999"
SocketServer()