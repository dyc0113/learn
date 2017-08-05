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

class CTrunKCgiMk:
    def __init__(self):
        self.input_file = "input.txt"
        self.out_file = "out.txt"
        pass;
    def run(self):
        
        fr = open(self.input_file, "r")
        content = fr.readline();
        fr.close()
        
        print content;

        ls_replace = [
                      ["-isystem", "-I"],
                      ["-I.",""],
                      ["I/usr/local/mysql/include/mysql", ""],

                      ["/boost", "/boost-1.61.0"],
                      ["/rocketmq", "/rocketmq-client4cpp-1.0.4"],
                      ["/VipMQAgentApi", "/VipMQAgentApi-1.0.0"],
                      ["/grocery_api", "/gro_client_cpp_api_tlinux_mt_spp3"],
                      ["/boost", "/boost-1.61.0"],
                      ["/srf", "/srf-2.1.10"],
                      ["/spp", "/spp3.0"],

                      ["-I/data/home/dashdong/coroutinue/branch_online", ";\nE:\\dashdong_work\\手游平台\\协程-分支\\mt_branch_xr\\mt_branch_xr\\branch_online"],
                      
                      ["/", "\\"]
                    ]
        
        for r in ls_replace:
            content = content.replace(r[0], r[1])
            pass;
        
        add_data =["D:\Program Files (x86)\VC\include;", "E:\COMMON\include;", "E:\COMMON"]
        
        for a in add_data:
            content += ";" + a;
        
        fw = open(self.out_file, "w")
        fw.write(content)
        fw.close()
        
        print content;
        
        print "makefiel over"

mk = CTrunKCgiMk();
mk.run()