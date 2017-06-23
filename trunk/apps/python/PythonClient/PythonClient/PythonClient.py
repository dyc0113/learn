# -*- coding:utf-8 -*-
from socket import *
import struct
import struct
import json
import time



def SocketClient(cmd, contentData):
    try:
        #监听地址
        s=socket(AF_INET,SOCK_STREAM,0)
 
        Colon = ServerUrl.find(':')
        IP = ServerUrl[0:Colon]
        Port = ServerUrl[Colon+1:]
        #连接地址
        s.connect((IP,int(Port)))

        fileinfo_size=struct.calcsize('128sl') #定义打包规则
        #定义文件头信息，包含文件名和文件大小
        fhead = struct.pack('128sl',cmd , len(contentData))
        print "send head"
        s.send(fhead) 
        print "send data content", contentData.decode("UTF-8").encode("GBK")
        s.send(contentData)
        print 'send over...'

    except Exception,ex:
        print ex
 
ServerUrl = "127.0.0.1:9999"
while True:
    SocketClient("test", "宋鑫茹妹子好漂亮呀")
    time.sleep(1)