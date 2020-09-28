// thread example  
#include <iostream>       // std::cout  
#include <thread>         // std::thread  

void thr_function1()  
{  
    for (int i = 0; i != 10; ++i)  
    {  
        std::cout << "thread 1 print " << i << std::endl;  
    }  
}  

void thr_function2(int n)  
{  
    std::cout << "thread 1 print " << n << std::endl;  
}  

int main()  
{  
    std::thread t1(thr_function1);     // spawn new thread that calls foo()  
    std::thread t2(thr_function2, 111);  // spawn new thread that calls bar(0)  

    std::cout << "main, foo and bar now execute concurrently...\n";  

    // synchronize threads:  
    t1.join();                // pauses until first finishes  
    t2.join();               // pauses until second finishes  

    std::cout << "thread 1 and htread 2 completed.\n";  

    return 0;  
}  