#include <time.h>
#include <sys/time.h>
#include <map>
#include <string>
#include <fstream>
#include <iostream>
#include "json.h"
#include "timecount.inl"

using namespace std;
using namespace Json;

static int Parse(void)
{
    ifstream ifile("json_data.txt");
    if (!ifile) return -111;

    string sLine;
    string sResp;
    while (getline(ifile,sLine)) sResp += sLine + "\n";

    Json::Reader oReader;
    Json::Value stRoot;
    if (!oReader.parse(sResp, stRoot)) return -1;
    if (!stRoot.isMember("ret")) return -2;

    if (stRoot.isMember("data"))
    {
        Json::Value oData = stRoot["data"];
        if (!oData.isObject()) return -3;

        Json::Value::Members vMembers = oData.getMemberNames();
        for (std::vector<std::string>::const_iterator iter = vMembers.begin();
                iter != vMembers.end(); iter++)
        {
            //读取全局信息
            if (*iter == "144116309885310159")
                ;
            else if (*iter == "72058715847382223")
                ;
            else
                return -4;

            Json::Value stAdPos_Json = oData[*iter];
            if (!stAdPos_Json.isMember("ret") )
                return -5;
            if (!stAdPos_Json["ret"].isInt())
                return -6;
        }
    }
    return 0;
}

int main(int argc, const char* argv[])
{
    string strs[200];

    for (int i = 0; i < 200; ++i)
    {
        int len = rand() % 30;           
        strs[i].resize(len);
        for (int j = 0; j < len; ++j)
            strs[i][j] = rand() & 127;    
    }

    CTimeCount tc;
    for (int i = 0; i < 50000; ++i)
    {
        Value root;
        // map<string, map<string, string> > root;
        for (int j = 0; j < 20; ++j)
            root[strs[j]][strs[j]] = strs[j];
    }
    printf("map<string, map<string, string>> cost: %d\n", tc.Cost());

    CTimeCount tc2;
    for (int i = 0; i < 50000; ++i)
    {
        Value root;
        // map<string, vector<uint64> > root;
        for (int j = 0; j < 20; ++j)
        {
            root[strs[j]].resize(3);
            //root[strs[j]][0] = UINT_MAX;
            root[strs[j]][1] = LLONG_MAX;
            root[strs[j]][2] = ULLONG_MAX;
        }
    }
    printf("map<string, vector<uint64>> cost: %d\n", tc2.Cost());

    Value i64(LLONG_MAX);
    printf("i64 %jd\n", i64.asInt64());

    Value u64(ULLONG_MAX);
    printf("u64 %ju\n", u64.asUInt64());

    Value obj;
    FastWriter w;
    obj["x"] = "</script><script> window.location=\'http://att.isd.com/x.php\'"
            "</script><script>";
    printf("FastWriter: [[%s]]\n", w.write(obj).c_str());

    printf("Parse ret: %d\n", Parse());
    return 0;
}
