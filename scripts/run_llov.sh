#!/bin/bash

input=$1
filepath=${input%.*}
extension=${input##*.}

if [[ $extension == "f95" ]]
then
    flang -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone $1 -o $filepath.ll
    llor_transform --file $filepath.ll
else
    clang -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone $1 -o $filepath.ll
fi

opt -mem2reg -loop-simplify -simplifycfg $filepath.ll -S -o $filepath.ssa.ll
opt -load OpenMPVerify.so -openmp-resetbounds $filepath.ssa.ll -S -o $filepath.rb.ll
opt -load OpenMPVerify.so -openmp-forceinline -inline $filepath.rb.ll -S -o $filepath.in.ll
opt -load OpenMPVerify.so -openmp-split-basicblock $filepath.in.ll -S -o $filepath.sb.ll

rm $filepath.ll
rm $filepath.ssa.ll
rm $filepath.rb.ll
rm $filepath.in.ll

if [[ $2 == "debug" ]]
then
    opt -load OpenMPVerify.so -openmp-verify-mhp $filepath.sb.ll -S -o $filepath.debug.ll --strip-debug
else
    opt -load OpenMPVerify.so -disable-output -openmp-verify-mhp $filepath.sb.ll
    rm $filepath.sb.ll
fi
