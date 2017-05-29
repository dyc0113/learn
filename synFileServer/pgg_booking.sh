 #! /bin/sh
tool=$1
targetdir=/data/home/dashdong/dash/
if [ ! -d ${targetdir}success ]; then
   mkdir ${targetdir}success 
fi
if [ ! -d ${targetdir}failed ]; then
   mkdir ${targetdir}failed  
fi
for file in ${targetdir}*.cpp
do
    tool -f ${file} -a follow
    if [ "$?" -eq 0 ]
    then
       echo 'mv to success'
       mv ${file} ${targetdir}success;  
    else
       echo 'mv to error'
       mv ${file} ${targetdir}failed;  
        echo 'false'
    fi
    echo $file
done
