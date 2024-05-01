export LLOV_BUILD = /datadrive/llov/build
export OPENMP_INCLUDE_PATH = ${LLOV_BUILD}/projects/openmp/runtime/src
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH}

llov: OPENMP/kmeans

all: OPENMP/kmeans SERIAL/kmeans

OPENMP/kmeans:
	cd kmeans_openmp; make llov
	
SERIAL/kmeans:
	cd kmeans_serial; make

clean:
	cd kmeans_serial; make clean
	cd kmeans_openmp; make clean
	
