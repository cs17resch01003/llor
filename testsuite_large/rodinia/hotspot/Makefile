export LLOV_BUILD = /datadrive/llov/build
export OPENMP_INCLUDE_PATH = ${LLOV_BUILD}/projects/openmp/runtime/src
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH}

llov: hotspot_openmp.cpp Makefile 
	$(CC) $(CC_FLAGS) hotspot_openmp.cpp -o hotspot.o

all: hotspot hotspot_offload 

hotspot: hotspot_openmp.cpp Makefile 
	$(CC) $(CC_FLAGS) hotspot_openmp.cpp -o hotspot 

hotspot_offload: hotspot_openmp.cpp Makefile
	$(ICC) $(CC_FLAGS) $(OFFLOAD_CC_FLAGS) -DOMP_OFFLOAD hotspot_openmp.cpp -o hotspot_offload

clean:
	rm -f hotspot hotspot_offload
