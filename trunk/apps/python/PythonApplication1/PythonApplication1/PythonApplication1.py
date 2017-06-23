#作者：宝宝可乖了
#链接：https://www.zhihu.com/question/30444342/answer/154837690
#来源：知乎
#著作权归作者所有。商业转载请联系作者获得授权，非商业转载请注明出处。

#heart

from turtle import *
from time import sleep
import struct
import json
data1 = {'b':789,'c':456,'a':123}

data2 = {'a':123,'b':789,'c':456}

data2["data1"] = data1

d1 = json.dumps(data1,sort_keys=True)
d2 = json.dumps(data2)
d3 = json.dumps(data2,sort_keys=True)
print d1
print d2

d3 = d1 + d2

print d3


wx = 175155
jz = 22149
bd = 180000
zsyh=1717
zfb=71479


total = wx + jz + bd + zsyh + zfb

print "total", total;


L = [ x* x for x in range(10)]
dict = {}
i = 0;
for l in L:
    i+=1
    dict[i]=l;

print dict
dict["sxr"]= "dongchong";
for d in dict.items():
    print d


print "keys:"
for k in dict.keys():
    print k
    print dict[k]

s = eval("1+2")
print s

