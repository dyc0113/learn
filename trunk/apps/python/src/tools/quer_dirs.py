#!/usr/local/bin/python2.7
# encoding: utf-8
'''
Created on 2017��4��20��
@author: dashdong
'''
import sys
import os
import os.path
import os
import time

class CQueryDirs:
    def __init__(self):
        self.save_file="all_path"
        pass;
    def run(self):
        fw = open(self.save_file, "r")    
        content = fw.readline()
        fw.close();
        print content;
        print "over"


t = 1296000 / (24 * 3600)
print t
query = CQueryDirs();
query.run()