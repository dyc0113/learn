# author: baozhou(周龄)
#   desc: api/share/comm Makefile框架

######################## Makefile固定部分开始 ########################
patterns = "|/base_class|/innerlib|/application|/makeinclude|/outerlib"
project_home = $(shell pwd | awk -F$(patterns) '{print $$1}')

include $(project_home)/makeinclude/macro_base.mk
######################## Makefile固定部分结束 ########################


########################### 自定义部分开始 ###########################
#
# BASED变量定义需要依赖的底层文件, 下面的名字加上INC_或者LIB_前缀
# 之后对应macro_plib.mk, macro_base.mk里面的头文件跟库文件的宏定义

# -O3优化效果比较明显
C_FLAGS_EX = -O3

BASED = HYDRA_CORE

# 定义目标的名字
LIB_TARGET = libjsoncpp.a
#
########################### 自定义部分结束 ###########################


######################## Makefile固定部分开始 ########################
include $(project_home)/makeinclude/rules_api.mk
######################## Makefile固定部分结束 ########################
