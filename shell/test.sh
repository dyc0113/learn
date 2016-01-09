case $1 in
"") echo 50;;
*[!0-9]*) echo "Usage:`basename $0`file-to-cleanup";;
*) echo '*'
esac
echo $$  #进程ID
