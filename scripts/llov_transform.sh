#!/bin/bash

input=$1
filepath=${input%.*}
extension=${input##*.}

opt -mem2reg -loop-simplify -simplifycfg $filepath.ll -S -o $filepath.ssa.ll
opt -load OpenMPVerify.so -openmp-resetbounds $filepath.ssa.ll -S -o $filepath.rb.ll
opt -load OpenMPVerify.so -openmp-forceinline -inline $filepath.rb.ll -S -o $filepath.in.ll
opt -load OpenMPVerify.so -openmp-split-basicblock $filepath.in.ll -S -o $filepath.sb.ll

rm $filepath.ll
rm $filepath.ssa.ll
rm $filepath.rb.ll
rm $filepath.in.ll
