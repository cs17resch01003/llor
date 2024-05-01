CC   = mpicc
LD   = mpicc
CFLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH}

OBJS = omp_smithW.o serial_smithW.o

llov: $(OBJS)
