CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH}

ifdef OUTPUT
override OUTPUT = -DOUTPUT
endif

llov: main.c define.c kernel.c
	${CC} ${CC_FLAGS} $(OUTPUT) main.c -I./AVI -c -O3 -fopenmp

# link objects(binaries) together
heartwall: main.o ./AVI/avilib.o ./AVI/avimod.o
	gcc main.o ./AVI/avilib.o ./AVI/avimod.o -lm -fopenmp -o heartwall

# compile main function file into object (binary)
main.o: main.c define.c kernel.c
	gcc $(OUTPUT) main.c -I./AVI -c -O3 -fopenmp

./AVI/avilib.o ./AVI/avimod.o:
	cd AVI; make;

# delete all object files
clean:
	rm -f *.o AVI/*.o heartwall
