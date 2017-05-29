
#pragma once
#include <string>
#include <sys/stat.h>
#include <netdb.h>
#include <iostream>
#include "json/json.h"
#include "utils.h"
#include "file_manager.h"

using namespace std;

// dashdong  test
class CProcessCommand
{
private:
public:
    // 处理命令
    int process_client_command(std::string jsonCommand,
        std::string &rspJsonstring);
    int  create_dir(std::string path)
    {
        size_t found = 0;

        if (access(path.data(), F_OK) == 0)
        {
            return 0;
        }
        std::string parent_path;
        while ((found = path.find('/', found)) != string::npos)
        {
            parent_path = path.substr(0, found);
            if (access(parent_path.data(), F_OK) != 0)
            {
                mkdir(parent_path.data(), 0755);
            }
            found += 1;
        }
        if (access(path.data(), F_OK) != 0)
        {
            mkdir(path.data(), 0755);
        }
        return 0;
    }
    //  处理文件同步的消息
    int handle_file_message(SCommandReq *pReq, SCommandRsp &stRsp);

    int test()
    {
        cout<<"创建目录"<<endl;
        Json::Value value;
        value["method"] = "create_dir";
        Json::Value param;
        param["dir"] = "/data/home/dashdong/dash/synFileServer/create_dir/";
        value["param"] = param;

        Json::FastWriter writer;
        std::string jsonCommand = writer.write(value);
        cout<<jsonCommand<<endl;
        std::string strJsonRsp;
        process_client_command(jsonCommand, strJsonRsp);

        cout<<"重命名目录"<<endl;
        value["method"] = "rename_dir"; 
        param["old_name"] = "/data/home/dashdong/dash/synFileServer/create_dir";
        param["new_name"] = "/data/home/dashdong/dash/synFileServer/rename_dir";
        value["param"] = param;
        jsonCommand = writer.write(value);
        process_client_command(jsonCommand, strJsonRsp);

        cout<<"删除目录"<<endl;
        value["method"] = "del_dir"; 
        param["dir"] = "/data/home/dashdong/dash/synFileServer/rename_dir";
        value["param"] = param;
        jsonCommand = writer.write(value);
        process_client_command(jsonCommand, strJsonRsp);

        return 0;

    }
  };

