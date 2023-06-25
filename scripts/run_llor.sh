#!/bin/bash

cd /devel/llor/src/Repair/bin/Debug/net6.0
./Repair --llov /devel/llov/build --include /usr/lib/gcc/x86_64-linux-gnu/11/include/ --file $1
