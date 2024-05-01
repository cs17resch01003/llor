export LLOV_BUILD = /datadrive/llov/build
export OPENMP_INCLUDE_PATH = ${LLOV_BUILD}/projects/openmp/runtime/src
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

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
				ecc.c \
				cam.c \
				fin.c \
				master.c \
				embedded_fehlberg_7_8.c \
				solver.c \
				file.c \
				timer.c
	$(CC) $(CC_FLAGS)	main.c \
			-c -O3 -fopenmp

# link objects(binaries) together
myocyte.out:	main.o
	gcc	main.o \
			-lm -fopenmp \
	-o myocyte.out

# compile main function file into object (binary)
main.o: 	main.c \
				define.c \
				ecc.c \
				cam.c \
				fin.c \
				master.c \
				embedded_fehlberg_7_8.c \
				solver.c \
				file.c \
				timer.c
	gcc	main.c \
			-c -O3 -fopenmp

# delete all object files
clean:
	rm *.o myocyte.out output.txt
