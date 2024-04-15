export LLOV_BUILD = /datadrive/llov/build
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

CC   = mpicc
LD   = mpicc
CFLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${LLOV_BUILD}/projects/openmp/runtime/src

OBJS = omp_smithW.o serial_smithW.o

llov: $(OBJS)
