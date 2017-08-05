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
    f = open("out.txt", "r")
    for line in f:
        rs = line.replace('\n', '')

        arr = rs.split(',')
        uin = arr[0]
        uid = arr[1]
        tm  = int(arr[2])
        time_local = time.localtime(tm)
        day = time.strftime("%Y-%m-%d", time_local)
        if dTimeCount.has_key(day):
            dTimeCount[day]=dTimeCount[day]+1
        else:
            dTimeCount[day]=0
        print uin, uid, tm
    f.close();

    ls = [];
    for i in dTimeCount.items():
        ls.append(i)
    ls.sort(key = lambda x:x[0])
    print ls;

    fout2=open("out2.txt", "w")
    for i in ls:
        fout2.write(i[0] + "\t" + str(i[1]) +"\n")
    fout2.close();




   


    
 
