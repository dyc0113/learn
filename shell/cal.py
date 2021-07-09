#!/usr/bin/python
# -*- coding: UTF-8 -*-
import sys
import os
import json
from urllib import urlencode
from urllib import quote
import urllib
import re
from difflib import *  
import difflib
import httplib, sys
from Queue import Queue
from threading import Thread
import httplib, sys
from Queue import Queue
import sys
import re
import urllib2
import json
import math
from datetime import datetime
import commands
import time
import sys
import pdb
reload(sys)
sys.setdefaultencoding('utf8')



zh =  263550 
zh_l = 7174 + 6113


zfb = 275000 
zfb_l = zfb - 57685 - 11000 - 746 

wx = 338893 

wx_l = wx - 111705 - 4563 - 70827 

gjj = 53344

xg = 126000 

all = zh + zfb + wx #+ gjj #+ xg

tz = zh_l + wx_l + wx_l #+ xg 


print "all=", all , " touzi=", tz,  " proportion=", tz * 1.0 / all

print all * 0.3


licai = wx_l + 286416 - 65872 - 8000 - 42488 

