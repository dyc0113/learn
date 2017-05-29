/**
 * @author  baozhou(周龄)
 */
#ifndef __TIME_COUNT_H__
#define __TIME_COUNT_H__

#include <sys/time.h>
#include <time.h>

class CTimeCount
{
    public:
        CTimeCount();
        ~CTimeCount();

    public:
        int Cost() const;

    private:
        struct timeval m_tvBegin;
};

#endif

