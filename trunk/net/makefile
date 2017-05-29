# TINYXML_USE_STL can be used to turn on STL support. NO, then STL
# will not be used. YES will include the STL files.
TINYXML_USE_STL := YES

#****************************************************************************

COMM = ../comm
TBASE = ../comm/tbase
TSOCKCOMMU = ../comm/tbase/tsockcommu
MONITOR = ../comm/attr_api
LIB_PATH = ../../bin/lib/
TINYXML = ./tinyxml
TCONFBASE = ./tconfbase
STATMGR = ./stat_mgr
BUILD_DIR=../comm

ifeq ($(ARCH),32)
	CFLAGS +=  -march=pentium4 -m32 -DSUS_LINUX -pthread
else
	CFLAGS +=  -m64 -DSUS_LINUX -pthread
endif

CFLAGS += -O2 -fno-inline -g -ggdb -Wall -D_GNU_SOURCE -D_MP_MODE -Wno-write-strings #-Werror 
CFLAGS += -pipe -fno-ident -fPIC -shared -z defs
ifeq (YES, ${TINYXML_USE_STL})
	CFLAGS += -DTIXML_USE_STL
endif

INC += -I./ -I$(COMM) -I$(TBASE) -I$(TSOCKCOMMU) -I$(TINYXML) -I$(MONITOR)/inner_inc
LIB += -lpthread -ldl
TARGET=libnet.a
all : $(TARGET)
OBJ = poller.o \

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

$(TARGET):$(OBJ)
	@echo -e  Linking $(CYAN)$@$(RESET) ...$(RED) 
	@ar crs $@ $^ $(CRESET)
	@chmod +x $@

%.o: %.cpp
	@echo -e Compiling $(GREEN)$<$(RESET) ...$(RED)
	@$(CXX) $(CFLAGS) -c -o $@ $< $(INC) $(CRESET)   
%.o: %.c
	@echo -e Compiling $(GREEN)$<$(RESET) ...$(RED)
	@$(CC) $(CFLAGS) -c -o $@ $< $(INC) $(CRESET)    
clean:
	@rm -f $(OBJ) $(TARGET)

