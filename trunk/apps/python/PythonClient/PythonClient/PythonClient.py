# -*- coding: utf-8 -*-
from socket import *
import struct
import struct
import json
import time
import os,sys

import logging
logging.basicConfig(level=logging.DEBUG,
                format='%(asctime)s %(filename)s[line:%(lineno)d] %(levelname)s %(message)s',
                datefmt='%a, %d %b %Y %H:%M:%S',
                filename='client.log',
                filemode='w')
    
#logging.debug('This is debug message')
#logging.info('This is info message')
#logging.warning('This is warning message')


ServerUrl = "127.0.0.1:9999"
def GetConnect():
    try:
        #监听地址
        s=socket(AF_INET,SOCK_STREAM,0)
 
        Colon = ServerUrl.find(':')
        IP = ServerUrl[0:Colon]
        Port = ServerUrl[Colon+1:]
        #连接地址
        s.connect((IP,int(Port)))
        return 0,s
    except Exception,ex:
        print "exceprion", ex
        return 1,s

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
        return 0;

    except Exception,ex:
        print "sendData exceprion", ex
        return 1


#获取请求的内容json串
def RecevData(connection, dataLen):
     #接收报文内容信息
    try:
        contentData=""
        recvd_size = 0 #定义接收了的文件大小
        logging.debug( 'stat receiving...')
        while not recvd_size == dataLen:
            if dataLen - recvd_size > 1024:
                rdata = connection.recv(1024)
                contentData = contentData + rdata
                recvd_size += len(rdata)
            else:
                rdata = connection.recv(dataLen - recvd_size) 
                contentData = contentData + rdata
                recvd_size = dataLen

        logging.debug( 'receive done')
    except Exception,ex:
        print  ex
    return contentData

#接受客户端数据，并根据协议解析出dict结构返回
def RecevDictData(conn):
    iRet = 0;
    fileinfo_size=struct.calcsize('128sl') #定义文件信息。128s 命令字 ，l表示一个int或log文件类型，在此为文件大小
    data_buf = conn.recv(fileinfo_size) #接受报文头信息
    #输出报文头信息
    cmd,dataLen =struct.unpack('128sl', data_buf)
    print "cmd:",cmd,"  dataLen:", dataLen;
    contentData = RecevData(conn, dataLen);
    logging.debug("receive data:"  + contentData) 
    dRspData = json.loads(contentData) #
    return iRet, dRspData;

#发送字典数据
def SendDictData(conn, dData, cmd="defalut"):
    jsonReq = json.dumps(mReq, ensure_ascii=False, sort_keys=True)
    logging.debug("send data:%s", jsonReq); 
    iRet = SendData(conn, cmd, jsonReq)
    iRet, dRspData = RecevDictData(conn)
    return iRet,dRspData


#获取目录下所有目录的名称
def GetDirs(parentDir):
    lsFile = [];
    list = os.listdir(parentDir)#列出目录下的所有文件和目录
    for line in list:
        print line.encode("gbk")
        lsFile.append(line.encode("utf8"));
    return lsFile;


while True:
    try:
        iRet,conn = GetConnect(); #获取连接
        if (iRet != 0):
            print "conect failed"
            time.sleep(1)
            continue;
          
        #组装报文
        lsDirs = GetDirs(u"H:\\电影");
        mReq = {}
        mReq["dirs"] = lsDirs;

        iRet, mRsp = SendDictData(conn, mReq, "test");
        logging.debug("mRsp:%s", str(mRsp).decode("utf8").encode("gbk"))
        logging.debug("send req success");

        time.sleep(3)
    except Exception,ex:
        print "exception", ex
        time.sleep(10)