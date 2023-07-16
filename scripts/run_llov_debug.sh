#!/bin/bash

filepath=$1
filepath=${filepath%.*}

clang -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone $1 -I/usr/lib/gcc/x86_64-linux-gnu/11/include -o $filepath.ll
opt -mem2reg -loop-simplify -simplifycfg $filepath.ll -S -o $filepath.ssa.ll
opt -load OpenMPVerify.so -openmp-resetbounds $filepath.ssa.ll -S -o $filepath.rb.ll
opt -load OpenMPVerify.so -openmp-forceinline -inline $filepath.rb.ll -S -o $filepath.in.ll
opt -load OpenMPVerify.so -openmp-split-basicblock $filepath.in.ll -S -o $filepath.sb.ll
opt -load OpenMPVerify.so -openmp-verify-mhp $filepath.sb.ll -S -o $filepath.debug.ll --strip-debug

rm $filepath.ll
rm $filepath.ssa.ll
rm $filepath.rb.ll
rm $filepath.in.ll
