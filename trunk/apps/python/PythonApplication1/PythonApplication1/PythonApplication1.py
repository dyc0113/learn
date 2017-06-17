#作者：宝宝可乖了
#链接：https://www.zhihu.com/question/30444342/answer/154837690
#来源：知乎
#著作权归作者所有。商业转载请联系作者获得授权，非商业转载请注明出处。

#heart

from turtle import *
from time import sleep
import struct

def go_to(x, y):
   up()
   goto(x, y)
   down()


def big_Circle(size):  #函数用于绘制心的大圆
   speed(10)
   for i in range(150):
       forward(size)
       right(0.3)

def small_Circle(size):  #函数用于绘制心的小圆
   speed(10)
   for i in range(210):
       forward(size)
       right(0.786)

def line(size):
   speed(1)
   forward(51*size)

def heart( x, y, size):
   go_to(x, y)
   left(150)
   begin_fill()
   line(size)
   big_Circle(size)
   small_Circle(size)
   left(120)
   small_Circle(size)
   big_Circle(size)
   line(size)
   end_fill()

def arrow():
   pensize(10)
   setheading(0)
   go_to(-400, 0)
   left(15)
   forward(150)
   go_to(339, 178)
   forward(150)

def arrowHead():
   pensize(1)
   speed(5)
   color('red', 'red')
   begin_fill()
   left(120)
   forward(20)
   right(150)
   forward(35)
   right(120)
   forward(35)
   right(150)
   forward(20)
   end_fill()


def main():
   pensize(2)
   color('red', 'pink')
   #getscreen().tracer(30, 0) #取消注释后，快速显示图案
   heart(200, 0, 1)          #画出第一颗心，前面两个参数控制心的位置，函数最后一个参数可控制心的大小
   setheading(0)             #使画笔的方向朝向x轴正方向
   heart(-80, -100, 1.5)     #画出第二颗心
   arrow()                   #画出穿过两颗心的直线
   arrowHead()               #画出箭的箭头
   go_to(400, -300)
   write("author：大冲哥哥", move=True, align="left", font=("宋体", 30, "normal"))
   done()


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



#编辑于 2017-04-02215 条评论分享收藏感谢收起更多回答匿名用户36 人赞同了该回答from turtle import *
def curvemove():
    for i in range(200):
        right(1)
        forward(1)