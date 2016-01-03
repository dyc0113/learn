/*************************************************************************
	> File Name: main.cpp
	> Author: ma6174
	> Mail: ma6174@163.com 
	> Created Time: Sat 28 Nov 2015 07:34:06 AM PST
 ************************************************************************/

#include<iostream>
#include <vector>
#include <map>
#include <stdio.h>

using namespace std;
int fun()
{
	cout<<"Hello "<<endl;
	return 0;
}
int fun(int x)
{
	cout<<x<<endl;
	return 3;
}
int fun(char *p)
{
	return 34;
}
class A
{
	public:
		void print()
		{
			cout<<"abcdef"<<endl;
		}
};
int main()
{
	while(1)
	{
		cout<<"Hello "<<endl;
	}
	return 0;
}
