export LLOV_BUILD = /datadrive/llov/build
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${LLOV_BUILD}/projects/openmp/runtime/src

llov:
	$(CC) $(CC_FLAGS) 3D.c -o 3D.o

3D:
	$(CC) -o 3D $(CCFLAGS) 3D.c -lm 

clean:
	rm -f 3D $(OUTPUT)
