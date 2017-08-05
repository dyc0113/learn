#!/usr/local/bin/python2.7
# encoding: utf8
'''
module -- shortdesc

module is a description

It defines classes_and_methods

@author:     user_name

@copyright:  2017 organization_name. All rights reserved.

@license:    license

@contact:    user_email
@deffield    updated: Updated
'''
#获取目录下所有文件
import sys
import os
import os.path
import os
import time

#递归的获取根目录下所有的目录                             
def GetDirs(rootdir):
    dirs = []
    for parent,dirnames,filenames in os.walk(rootdir):   #三个参数：分别返回1.父目录 2.所有文件夹名字（不含路径） 3.所有文件名字
        for dirname in  dirnames:                           #输出文件夹信息
            print "parent is:" + parent
            print  "dirname is" + dirname
            fullDirName = parent + "/" + dirname;
            dirs.append(fullDirName)
            sub_dirs = GetDirs(fullDirName)
            dirs = dirs + sub_dirs;
        break;
        for filename in filenames:  #输出文件信息
            print "parent is:" + parent
            print "filename is:" + filename
            print "the full name of the file is:" + os.path.join(parent,filename) #输出文件路径信息
    return dirs;

class CModelDirs:
    def __init__(self):
        self.root_path="E:/open_source"
        self.save_file="tool_file_out.txt"
        pass;
    def run(self):
        root_dir=self.root_path;
        dirs = GetDirs(root_dir)
        print dirs;
        
        fw = open(self.save_file, "w")    
        fw.write(";\n".join(dirs));
        fw.close();
        print "over"
        
m = CModelDirs();
m.run();
#while True:
#    m.run()
#    time.sleep(30)
    
    

        
    

