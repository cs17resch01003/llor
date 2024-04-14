#! /bin/bash

export LLOV_BUILD = /datadrive/llov/build

export LD_LIBRARY_PATH=${LLOV_BUILD}/lib:$LD_LIBRARY_PATH
export C_INCLUDE_PATH=${LLOV_BUILD}/projects/openmp/runtime/src:$C_INCLUDE_PATH
export CPP_INCLUDE_PATH=${LLOV_BUILD}/projects/openmp/runtime/src:$CPP_INCLUDE_PATH

echo "Compiling DualSPHysics..."
( cd DualSPHysics/src/source && mkdir -p build && cd build && \
   cmake -DCMAKE_CXX_COMPILER=${LLOV_BUILD}/bin/clang++ -DCMAKE_CXX_FLAGS="-Xclang -disable-O0-optnone -g -Xclang -load -Xclang ${LLOV_BUILD}/lib/OpenMPVerify.so" -DCMAKE_BUILD_TYPE=DEBUG ../ && make clean && make -k ) ;
