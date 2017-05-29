
CC ?= gcc
CC_FLAGS +=  -ggdb -Wall -march=x86-64 -Wno-deprecated -fno-omit-frame-pointer $(DEV_FLAG)
#常用函数
define exe_link_mult
	@echo -e "$(CXX) $(CC_FLAGS) $(CPP_OBJECTS) \e[36;1m -o $@ \e[0m"
    @(errmsg=$$( $(CXX) $(CC_FLAGS) $< $(LIB) -o $@ 2>&1); r=$$?; \
		if [[ ! -z $$errmsg ]]; \
		then \
			echo -ne "$(CXX) $(CC_FLAGS) $< $(LIB) -o $@\n\n"\
			echo -ne "\e[31;1m $$errmsg\n\e[0m";\
			exit $$r;\
		fi\
	)
endef

define obj_compile
	@echo -e "compiling: \e[36;1m $@ \e[0m"
	@(errmsg=$$( $(CXX) -c $(CC_FLAGS) $(INC) $< -o $@ 2>&1 ); r=$$?; \
		if [[ ! -z $$errmsg ]]; \
		then \
			echo -ne "$(CXX) -c $(CC_FLAGS) $(INC) $< -o  $@\n";\
			echo -ne "\e[31;1merrmsg:\n$$errmsg\e[0m\n"; \
			exit $$r;\
		fi\
	)
endef

define exe_link
	@echo -e "$(CXX) $(CC_FLAGS) $(CPP_OBJECTS) \e[36;1m -o $@ \e[0m"
    @(errmsg=$$( $(CXX) $(CC_FLAGS) $(CPP_OBJECTS) $(LIB) -o $@ 2>&1); r=$$?; \
		if [[ ! -z $$errmsg ]]; \
		then \
			echo -ne "$(CXX) $(CC_FLAGS) $(CPP_OBJECTS) $(LIB) -o $@\n\n"\
			echo -ne "\e[31;1m $$errmsg\n\e[0m";\
			exit $$r;\
		fi\
	)
endef

INC = -I./outlib/jsoncpp-src-0.5.0/include
LIB = -L./outlib/jsoncpp-src-0.5.0/libs/linux-gcc-4.1.2 -ljson_linux-gcc-4.1.2_libmt
#
CPP_OBJECTS = $(patsubst %.cpp, .obj/%.o, $(wildcard *.cpp))
TARGET = synServer 

RED = \\e[1m\\e[31m
DARKRED = \\e[31m
GREEN = \\e[1m\\e[32m
DARKGREEN = \\e[32m
BLUE = \\e[1m\\e[34m
DARKBLUE = \\e[34m
YELLOW = \\e[1m\\e[33m
DARKYELLOW = \\e[33m
MAGENTA = \\e[1m\\e[35m
DARKMAGENTA = \\e[35m
CYAN = \\e[1m\\e[36m
DARKCYAN = \\e[36m
RESET = \\e[m
CRESET =  ;echo -ne \\e[m; test -s $@

INC +=
LIB +=

all:$(CPP_OBJECTS) $(TARGET)
	@echo "make $(TARGET) sucessful!"
	@echo -e $(RED)....$(RESET)

$(TARGET):$(CPP_OBJECTS)
	$(call exe_link)

.obj/%.o:%.cpp
	-@if (! test -d .obj) ; then  mkdir .obj > /dev/null 2>&1; fi
	$(call obj_compile)

clean:
	-$(RM) -rf $(CPP_OBJECTS) $(TARGET) .obj

cleanall:clean
	-$(RM) .depend* .tmp.depend*
	-$(RM) server tester  client 
