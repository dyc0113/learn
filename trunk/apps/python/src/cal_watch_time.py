#coding=utf-8
#!/usr/bin/python

#-*-coding:utf-8-*- 

import os
import uuid
import urllib2
import re
import sys, os
import glob
import logging
import logging.handlers
import re
import string
import tempfile
import time

if __name__ == '__main__':

    dTimeCount = {}
    dUser = {}
    f = open("in_watch.txt", "r")
    for line in f:
        rs = line.replace('\n', '')
        arr = rs.split(',')
        uin = arr[1]
        time = arr[2]
        tm  = int(arr[0])
        time_local = time.localtime(int(tm) / 1000)
        day = time.strftime("%Y-%m-%d", time_local)
        if dTimeCount.has_key(day):
            key=day+str(uin)        # 每一天，每个用户统计一次
            if not dUser.has_key(key):  # 当前用户没有被统计过
                dTimeCount[day]=dTimeCount[day]+1
                dUser[uin] = 1
        else:
            dTimeCount[day]=0
        print uin,tm,time
    f.close();

    ls = [];
    for i in dTimeCount.items():
        ls.append(i)
    ls.sort(key = lambda x:x[0])
    print ls;

    fout2=open("out_watch.txt", "w")
    for i in ls:
        fout2.write(i[0] + "\t" + str(i[1]) +"\n")
    fout2.close();




   


    
 
