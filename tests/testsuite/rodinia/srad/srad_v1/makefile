CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH}

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
