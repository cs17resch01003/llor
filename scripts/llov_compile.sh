#!/bin/bash

input=$1
filepath=${input%.*}
extension=${input##*.}

if [[ $extension == "f95" ]]
then
    flang -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone $1 -o $filepath.ll
    llor_transform --file $filepath.ll
elif [[ $extension == $input ]]
then
    cd $input
    make llov
    for file in `find . -name '*.o' -type f`; do
        mv -- "$file" "${file%.o}.ll"
    done
else
    clang -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone $1 -o $filepath.ll
fi
