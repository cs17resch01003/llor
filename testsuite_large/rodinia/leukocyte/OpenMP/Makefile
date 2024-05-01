export LLOV_BUILD = /datadrive/llov/build
export OPENMP_INCLUDE_PATH = ${LLOV_BUILD}/projects/openmp/runtime/src
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

# Matrix library path
MATRIX_DIR = ../meschach_lib

CC   = mpicc
LD   = mpicc
CC_FLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${OPENMP_INCLUDE_PATH} -I$(MATRIX_DIR)

ifdef OUTPUT
override OUTPUT = -DOUTPUT
endif

llov: detect_main.o avilib.o find_ellipse.o track_ellipse.o misc_math.o

leukocyte: detect_main.o avilib.o find_ellipse.o track_ellipse.o misc_math.o $(MATRIX_DIR)/meschach.a
	$(CC) $(CC_FLAGS) -lm avilib.o find_ellipse.o track_ellipse.o misc_math.o detect_main.o -o leukocyte -lm $(MATRIX_DIR)/meschach.a -lpthread

%.o: %.[ch]
	$(CC) $(OUTPUT) $(CC_FLAGS) $< -c

detect_main.o: find_ellipse.h track_ellipse.h avilib.h
find_ellipse.o: avilib.h
track_ellipse.o: find_ellipse.h track_ellipse.h avilib.h

$(MATRIX_DIR)/meschach.a:
	cd $(MATRIX_DIR); ./configure --with-all; make all; make clean

clean:
	rm -f *.o leukocyte
