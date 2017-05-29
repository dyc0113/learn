/**
 * @author  baozhou(周龄)
 */
#include "timecount.h"


CTimeCount::CTimeCount()
{
    gettimeofday(&m_tvBegin, NULL);
}

CTimeCount::~CTimeCount()
{
}

int CTimeCount::Cost() const
{
    struct timeval tvCur;
    gettimeofday(&tvCur, NULL);

    int past = (tvCur.tv_sec - m_tvBegin.tv_sec) * 1000
                    + (tvCur.tv_usec - m_tvBegin.tv_usec) / 1000;
    return past;
}

