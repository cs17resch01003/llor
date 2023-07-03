#!/bin/bash

cd /devel/llov/build/bin

./clang -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone $1.c -I/usr/lib/gcc/x86_64-linux-gnu/11/include -o $1.ll
./opt -mem2reg -loop-simplify -simplifycfg $1.ll -S -o $1.ssa.ll
./opt -load ../lib/OpenMPVerify.so -openmp-resetbounds $1.ssa.ll -S -o $1.rb.ll
./opt -load ../lib/OpenMPVerify.so -openmp-forceinline -inline $1.rb.ll -S -o $1.in.ll
./opt -load ../lib/OpenMPVerify.so -openmp-split-basicblock $1.in.ll -S -o $1.sb.ll
./opt -load ../lib/OpenMPVerify.so -openmp-verify-mhp $1.sb.ll -S -o $1.debug.ll --strip-debug

rm $1.ll
rm $1.ssa.ll
rm $1.rb.ll
rm $1.in.ll
