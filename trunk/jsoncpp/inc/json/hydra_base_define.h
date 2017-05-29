/// hydra 基础定义, 对应的makefile宏定义：INC_HYDRA_CORE,LIB_HYDRA_CORE
/// @file
/// @date 2013-07-26 19:24:29
/// @version 1.0.0
/// @author baozhou(周龄), gorgeous_god@qq.com
/// @copyright Tencent
/// @namespace hydra

#ifndef __HYDRA_BASE_DEFINE_H__
#define __HYDRA_BASE_DEFINE_H__ \
        "$Id: hydra_base_define.h 8664 2015-03-03 11:50:08Z starhuang $"

#include <stdint.h>
#include <sys/types.h>
#include <unistd.h>
#include <sys/syscall.h>
#include <byteswap.h>

#undef uchar
#undef ushort
#undef uint
#undef ulong
#undef int64
#undef uint64
#undef qqnum_t
#undef QQ_VALID
#undef UIN_VALID
#undef HYDRA_SERVICE_INI
#undef QZONE_PROJECT_ROOT
#undef SET_ERR_MSG
#undef ntoh2
#undef hton2
#undef ntoh4
#undef hton4
#undef ntoh8
#undef hton8
#undef LIKELY
#undef UNLIKELY

#define LIKELY(x)    (__builtin_expect(!!(x), 1))
#define UNLIKELY(x)  (__builtin_expect(!!(x), 0))

/// 网络字节序转换接口，目前系统只提供: s(2 bytes)、l(4 bytes)两种类型
/// 的转换，而且默认是基于函数调用的，hydra定义更快、更统一的方式
# if __BYTE_ORDER == __BIG_ENDIAN
# define ntoh2(x)    (x)
# define hton2(x)    (x)
# define ntoh4(x)    (x)
# define hton4(x)    (x)
# define ntoh8(x)    (x)
# define hton8(x)    (x)
# else
#  if __BYTE_ORDER == __LITTLE_ENDIAN
#   define ntoh2(x)    __bswap_16(x)
#   define hton2(x)    __bswap_16(x)
#   define ntoh4(x)    __bswap_32(x)
#   define hton4(x)    __bswap_32(x)
#   define ntoh8(x)    __bswap_64(x)
#   define hton8(x)    __bswap_64(x)
#  endif
# endif

/// 将name或name对应的标识符转换成字符串，
/// 如#define name value，则HYDRA_NAME_TOSTR(name) => "value",
/// 如name不是一个宏，则HYDRA_NAME_TOSTR(name) => "name",
#define HYDRA_NAME_TOSTR(name) HYDRA_NAME_TOSTR2(name)
#define HYDRA_NAME_TOSTR2(name) #name

/// 将两个标识符进行拼接
/// 如#define a x, #define b y, 则HYDRA_NAME_APPEND(a, b) => xy
/// 如a, b不是一个宏，则HYDRA_NAME_APPEND(a, b) => ab
#define HYDRA_NAME_APPEND(a, b) HYDRA_NAME_APPEND2(a, b)
#define HYDRA_NAME_APPEND2(a, b) a##b

/// 判断一个串是否为空
#define HYDRA_NULLSTR(ptr) (NULL == (ptr) || 0 == *(ptr))

#define QQ_VALID(qq) (qq > 10000U)
#define UIN_VALID QQ_VALID
#define HYDRA_SERVICE_INI  \
    "/data/release/hydra/conf/base/hydra_service.ini"
#define QZONE_PROJECT_ROOT "/usr/local/qzone_v3.0/" // 应对qzone接口

/// gcc/g++的属性
#define _UNUSED_            __attribute__((__unused__))
#define _CONSTRUCTOR_       __attribute__((__constructor__))
#define _DESTRUCTOR_        __attribute__((__destructor__))
#define _WEAK_              __attribute__((__weak__))
#define _WEAKREF_           __attribute__((__weakref__))
#define _INLINE_     inline __attribute__((__always_inline__))
#define _DEPRECATED_        __attribute__((__deprecated__))
#define _NORETURN_          __attribute__((__noreturn__))
#define _FORMAT_(x, y)      __attribute__((__format__(__printf__, x, y)))
#if __WORDSIZE == 32
#define _FASTCALL_          __attribute__((__fastcall__))
#else
#define _FASTCALL_
#endif

#define _FILE_BASE_ basename(__FILE__)
#define SET_ERR_MSG(fmt, args...) \
        snprintf(_errmsg, sizeof(_errmsg), "[%s:%d][%s] " fmt, \
                _FILE_BASE_, __LINE__, __FUNCTION__, ##args)

/// 线程私有类型
#if 0
#if __PIC__ || __pic__
#define HYDRA_TLS __thread __attribute__((tls_model("initial-exec")))
#else
#define HYDRA_TLS __thread __attribute__((tls_model("local-exec")))
#endif
#endif

#define HYDRA_TLS __thread 

/// 整数类型定义，只是为了使类型写起来更简洁
typedef unsigned char  uchar;
typedef unsigned short ushort;
typedef unsigned int   uint;
typedef unsigned long  ulong;
typedef intmax_t       int64;   ///< 对应的printf格式串: %jd(man 3 printf)
typedef uintmax_t      uint64;  ///< 对应的printf格式串: %ju(man 3 printf)
typedef uint64         qqnum_t; ///< qq号统一类型定义

namespace hydra {

/// 文本编码统一定义
enum HydraEncoding
{
    HE_UNKNOWN      = -1,  ///< unknown
    HE_UTF8         = 0,   ///< utf8
    HE_GBK          = 1,   ///< gbk
    HE_GB18030      = 2,   ///< gb18030
};

/// @brief 字符串/二进制块 单元，最大容量为SIZE，实际长度为len
template <int SIZE, typename TLen>
struct HydraStringUnit
{
    union
    {
        char str[SIZE];
        char s[SIZE];   // alias name for str
    };

    union
    {
        TLen len;
        TLen l;   // alias name for len
    };
};

/// @brief 固定大小的 字符串/二进制块 单元, 长度为SIZE
template <int SIZE>
struct HydraFixedStrUnit
{
    union
    {
        char str[SIZE];
        char s[SIZE];   // alias name for str
    };
};


/// define function: pid_t gettid();
inline pid_t gettid()
{
     return syscall(SYS_gettid);
}

}

#endif
