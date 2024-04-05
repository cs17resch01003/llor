#!/bin/bash

input=$1
filepath=${input%.*}
extension=${input##*.}

llov_compile $input
llov_transform $input

if [[ $2 == "debug" ]]
then
    opt -load OpenMPVerify.so -openmp-verify-mhp $filepath.sb.ll -S -o $filepath.debug.ll --strip-debug
else
    opt -load OpenMPVerify.so -disable-output -openmp-verify-mhp $filepath.sb.ll
    rm $filepath.sb.ll
fi
