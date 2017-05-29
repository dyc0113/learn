#include "command.h"

int CProcessCommand::process_client_command(std::string jsonCommand,
                                            std::string &rspJsonstring)
{
    int iRet = 0;
     // 解析json
    Json::Reader reader;
    Json::Value value;

    Json::Value jsRsp;
    Json::FastWriter writer;

    if (reader.parse(jsonCommand, value))
    {
        Json::Value param = value["param"];   // 参数
        std::string method = value["method"].asString();
        cout<<"method: "<<method<<endl;
        if (method == "create_dir")  
        {
            std::string linux_dir = param["dir"].asString();
            create_dir(linux_dir);
            jsRsp["msg"] = "创建目录成功!";
        }
        else if (method == "rename_dir")
        {
            std::string old_name = param["old_name"].asString();
            std::string new_name = param["new_name"].asString();
            if (rename(old_name.c_str(), new_name.c_str()) == 0)
            {
                cout<<"rename success"<<old_name.c_str()<<":"<<new_name.c_str()<<endl;
                jsRsp["msg"] = "重名名目录成功!";
            }
            else
            {
                cout<<"rename failed"<<old_name.c_str()<<":"<<new_name.c_str()<<endl;
                jsRsp["msg"] = "重名名目录失败!";
            }
        }
        else if (method == "del_dir")
        {
            std::string linux_dir = param["dir"].asString();
            std::string command = std::string("rm -f -r ") + linux_dir;
            system(command.c_str());
            jsRsp["msg"] = "删除目录成功!";
            
        }
        else if (method == "del_file")  // 删除文件
        {
            std::string file_name = param["file_name"].asString();
            std::string command = std::string("rm -f '") + file_name +"'";
            system(command.c_str()); 
            jsRsp["msg"] = "删除成功!";
        }
        else if (method == "rename_file")  // 重名问文件
        {
            std::string old_name = param["old_name"].asString();
            std::string new_name = param["new_name"].asString();
            if (rename(old_name.c_str(), new_name.c_str()) == 0)
            {


                cout<<"rename success"<<old_name.c_str()<<":"<<new_name.c_str()<<endl;
                jsRsp["msg"] = "重命名成功!";
            }
            else
            {
                cout<<"rename failed"<<old_name.c_str()<<":"<<new_name.c_str()<<endl;
                jsRsp["msg"] = "重命名失败!";
            }
            
        }
        rspJsonstring = writer.write(jsRsp);
    }
    return iRet;
}


int CProcessCommand::handle_file_message(SCommandReq *pReq, SCommandRsp &stRsp)
{
    int iRet = 0;
    cout<<"header:"<<pReq->ToString().c_str()<<endl;
    do
    {
        cout<<"get dir stat"<<endl;
        // 判断文件夹是否存在
        CPidFilesMana::CreateDir(pReq->dir); // 创建目录层次
        struct stat stStat;
        iRet = lstat(pReq->dir, &stStat);
        if (iRet != 0)
        {
            cerr<<"获取文件状态失败!"<<endl;
            stRsp.ret = CONSTANT_GET_FILE_STAT_FAILED;
            break;
        }
        cout<<"判断是否是文件夹?"<<endl;
        if (!S_ISDIR(stStat.st_mode))// 判断是否是文件夹
        {
            cout<<"不是文件夹"<<endl;
            stRsp.ret = CONSTANT_NOT_FOLDER;  // 不是文件夹
            break;
        }
        cout<<"是文件夹"<<endl;
        std::string filePath = std::string(pReq->dir) + std::string("/")+ std::string(pReq->fileName);
        cout<<"filePath:"<<filePath.c_str()<<endl;
        if( access(filePath.c_str(), 0) == 0)// 文件存在
        {
            cout<<"file exist"<<endl;
            iRet = lstat(filePath.c_str(), &stStat);
            if (0 != iRet)
            {
                cerr<<"获取文件状态失败!"<<endl;
                stRsp.ret = CONSTANT_GET_FILE_STAT_FAILED;
                break;
            }
            cout<<"wTime:"<<pReq->create_tm<<" Ltime:"<<stStat.st_atim.tv_sec<<endl;
            if (pReq->create_tm >= (uint64_t)stStat.st_mtim.tv_sec)// 修改时间较早，覆盖
            {
                if (pReq->IsHaveData()) // 如果有数据
                {
                    // 进行数据写入
                    // 从socket当中读入数据
                    cout<<"pReq->len - sizeof(SCommandReq):"<<pReq->len - sizeof(SCommandReq)<<endl;
                    iRet =  writeFile(filePath, pReq->data , pReq->len - sizeof(SCommandReq));
                    if (0 != iRet)
                    {
                        cerr<<"write file failed"<<endl;
                        stRsp.ret = CONSTANT_WRITE_FILE_FAILED;
                        break;
                    }
                    stRsp.ret = CONSTANT_SUCESS;// 写入成功
                    strcpy(stRsp.msg, "同步成功");

                }
                else//
                {
                    stRsp.ret = CONSTANT_NEED_SYN;  // 需要覆盖
                }
            }
            else
            {
                stRsp.ret = CONSTANT_NOT_NEED_SYN;  // 不需要同步
                strcpy(stRsp.msg, "不需要同步");
            }
        }
        else // 文件不存在
        {
            cout<<"file not exists"<<endl;
            if (pReq->IsHaveData() || pReq->fileName[0]!='\0') // 如果有数据,或者有文件名
            {
                // 进行数据写入
                // 从socket当中读入数据
                iRet =  writeFile(filePath, pReq->data , pReq->len - sizeof(SCommandReq));
                if (0 != iRet)
                {
                    cerr<<"write file failed"<<endl;
                    stRsp.ret = CONSTANT_WRITE_FILE_FAILED;
                    break;
                }
                stRsp.ret = CONSTANT_SUCESS;// 写入成功

            }
            else//
            {
                cout<<"file now exists...";
                stRsp.ret = CONSTANT_NEED_SYN;  // 需要覆盖
            }
        }
        // 判断文件是否存在
    }while(0);
    return iRet;
}