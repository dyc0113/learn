#pragma once
#include <map>
#include <string>
#include <vector>
#include <stdio.h>
#include <sys/stat.h>
#include<sys/types.h>
#include<dirent.h>
#include <unistd.h>
#include <errno.h>

using namespace std;


class CPidFilesMana
{
public:
    CPidFilesMana();
    ~CPidFilesMana();


    void SetDBRootPath(const std::string& path);
    int CreateFile(const std::string &fileName);

    ///删除节目文件 
    int DelFile(const std::string &fileName);

    int GetFileList(vector<std::string> &vecFilesName);

    static int CreateDir(const std::string& path);

    static int CreateFilePath(const std::string& filePath);
    
    std::string GetRootDir();
private:
    std::string _root_path;       //DB存储的根路径
};


