export LLOV_BUILD = /datadrive/llov/build
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${LLOV_BUILD}/projects/openmp/runtime/src

# Example
# target: dependencies
	# command 1
	# command 2
          # .
          # .
          # .
	# command n

llov: 	main.c \
				define.c \
				graphics.c
	$(CC) $(CC_FLAGS)	main.c \
			-c -O3 -fopenmp

# link objects(binaries) together
a.out:	main.o
	gcc	main.o \
			-lm -fopenmp -o srad

# compile main function file into object (binary)
main.o: 	main.c \
				define.c \
				graphics.c
	gcc	main.c \
			-c -O3 -fopenmp

# delete all object files
clean:
	rm *.o srad
