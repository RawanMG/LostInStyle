#!/bin/sh
echo "Hello World";
rnum=( $(awk 'BEGIN{srand(); r=rand(); print r}') );
arr="$rnum";
for i in {1..9};
do
	rnum=( $(awk 'BEGIN{srand(); r=rand(); print r}') );
	arr="$arr,$rnum";
done
C:\\Users\\Rawan\\Anaconda2\\envs\\tensorflow_gpu\\python get_stress.py $arr