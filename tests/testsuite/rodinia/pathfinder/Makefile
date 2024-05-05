SRC = pathfinder.cpp
EXE = pathfinder
FLAGS = -fopenmp

CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH}

llov:
	$(CC) $(CC_FLAGS) $(SRC)

release:
	$(CC) $(SRC) $(FLAGS) -o $(EXE)

debug:
	$(CC) $(SRC) -g -Wall -o $(EXE)

clean:
	rm -f pathfinder


