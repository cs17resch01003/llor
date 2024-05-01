export LLOV_BUILD = /datadrive/llov/build
export OPENMP_INCLUDE_PATH = ${LLOV_BUILD}/projects/openmp/runtime/src
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH}

llov: ex_particle_OPENMP_seq.c
	$(CC) $(CC_FLAGS) -O3 -ffast-math -fopenmp ex_particle_OPENMP_seq.c -lm 

openmp: ex_particle_OPENMP_seq.c
	gcc -O3 -ffast-math -fopenmp ex_particle_OPENMP_seq.c -o particle_filter -lm 


icc_openmp: ex_particle_OPENMP_seq.c
	icc -O3 -fast -openmp ex_particle_OPENMP_seq.c -o particle_filter


clean:
	rm particle_filter
