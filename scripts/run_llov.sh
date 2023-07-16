#!/bin/bash
clang -Xclang -disable-O0-optnone -Xclang -load -Xclang OpenMPVerify.so -fopenmp -g $1
