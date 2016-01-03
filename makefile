CFLAG=-g
main:main.o a.o
	g++ ${CFLAG} -o main main.o a.o
main.o:main.cpp
	g++ ${CFLAG} -c main.o main.cpp
a.o:a.cpp a.h
	g++ ${CFLAG} -c a.h a.cpp
clean:
	rm *.o main
