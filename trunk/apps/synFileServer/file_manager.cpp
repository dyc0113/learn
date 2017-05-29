
#include "file_manager.h"


CPidFilesMana::CPidFilesMana()
{
}

CPidFilesMana::~CPidFilesMana()
{
}

void CPidFilesMana::SetDBRootPath(const std::string& path)
{
    _root_path = path;
    CreateDir(_root_path);
}

int CPidFilesMana::CreateFile(const std::string &fileName)
{
    std::string fullPath = _root_path + "/" + fileName;
    FILE* f = fopen(fullPath.c_str(), "w");
    if (f == NULL)
    {
        printf("create file faile. errno:%d", errno);
        return -1; 
    }
    fclose(f);
    return 0;
}

///Ll
int CPidFilesMana::DelFile(const std::string &fileName)
{
    std::string fullPath = _root_path + "/" + fileName;
    if (unlink(fullPath.c_str()) != 0) {
      printf("del file faile. errno:%d", errno);
      return -1;
    }
    return 0;
}

int CPidFilesMana::GetFileList(vector<std::string> &vecFilesName)
{
    vecFilesName.clear();
    DIR* d = opendir(_root_path.c_str());
    if (d == NULL)
    {
        printf("opendir failed. erron:%d", errno);
        return -1;
    }
    struct dirent* entry;
    while ((entry = readdir(d)) != NULL)
    {
        if(strncmp(entry->d_name, ".", 2)==0 || strncmp(entry->d_name, "..", 3)==0)
        {
            continue;
        }
        vecFilesName.push_back(entry->d_name);
    }
    closedir(d);
    return 0;
}
int CPidFilesMana::CreateDir(const std::string& path)
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
int CPidFilesMana::CreateFilePath(const std::string& path)
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
  
    return 0;
}
 std::string CPidFilesMana::GetRootDir()
 {
     return _root_path;
 }

