CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH}

llov:
	$(CC) $(CC_FLAGS) 3D.c -o 3D.o

3D:
	$(CC) -o 3D $(CCFLAGS) 3D.c -lm 

clean:
	rm -f 3D $(OUTPUT)
