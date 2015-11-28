CFLAG=-g
main:main.o
	g++ ${CFLAG} -o main main.o
:wq
main.o:main.cpp
	g++ ${CFLAG} -c main.o main.cpp
