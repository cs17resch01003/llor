#!/bin/bash

cd /devel/llov/build/bin
./clang -Xclang -disable-O0-optnone -Xclang -load -Xclang ../lib/OpenMPVerify.so -I/usr/lib/gcc/x86_64-linux-gnu/11/include/ -fopenmp -g $1
