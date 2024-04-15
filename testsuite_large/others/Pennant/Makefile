export LLOV_BUILD = /datadrive/llov/build
export OMPI_CC = ${LLOV_BUILD}/bin/clang
export OMPI_CXX = ${LLOV_BUILD}/bin/clang++

BUILDDIR := build
PRODUCT := pennant

SRCDIR := src

HDRS := $(wildcard $(SRCDIR)/*.hh)
SRCS := $(wildcard $(SRCDIR)/*.cc)
OBJS := $(SRCS:$(SRCDIR)/%.cc=$(BUILDDIR)/%.o)
DEPS := $(SRCS:$(SRCDIR)/%.cc=$(BUILDDIR)/%.d)

BINARY := $(BUILDDIR)/$(PRODUCT)

# begin compiler-dependent flags
#
# gcc flags:
#CXX := g++
#CXXFLAGS_DEBUG := -g
#CXXFLAGS_OPT := -O3
#CXXFLAGS_OPENMP := -fopenmp

# intel flags:
CXX := icpc
CXXFLAGS_DEBUG := -g
CXXFLAGS_OPT := -O3 -fast -fno-alias
CXXFLAGS_OPENMP := -openmp

# pgi flags:
#CXX := pgCC
#CXXFLAGS_DEBUG := -g
#CXXFLAGS_OPT := -O3 -fastsse
#CXXFLAGS_OPENMP := -mp

# end compiler-dependent flags

# select optimized or debug
CXXFLAGS := $(CXXFLAGS_OPT)
#CXXFLAGS := $(CXXFLAGS_DEBUG)

# add mpi to compile (comment out for serial build)
# the following assumes the existence of an mpi compiler
# wrapper called mpicxx
CXX := mpicxx
CXXFLAGS += -DUSE_MPI

# add openmp flags (comment out for serial build)
CXXFLAGS += $(CXXFLAGS_OPENMP)
LDFLAGS += $(CXXFLAGS_OPENMP)

LD := $(CXX)

CXX   = mpicc
CXXFLAGS = -fopenmp -S -emit-llvm -g -Xclang -disable-O0-optnone -I${LLOV_BUILD}/projects/openmp/runtime/src

# begin rules
llov: $(OBJS)

all : $(BINARY)

-include $(DEPS)

$(BINARY) : $(OBJS)
	@echo linking $@
	$(maketargetdir)
	$(LD) -o $@ $^ $(LDFLAGS)

$(BUILDDIR)/%.o : $(SRCDIR)/%.cc
	@echo compiling $<
	$(maketargetdir)
	$(CXX) $(CXXFLAGS) $(CXXINCLUDES) -c -o $@ $<

$(BUILDDIR)/%.d : $(SRCDIR)/%.cc
	@echo making depends for $<
	$(maketargetdir)
	@$(CXX) $(CXXFLAGS) $(CXXINCLUDES) -MM $< | sed "1s![^ \t]\+\.o!$(@:.d=.o) $@!" >$@

define maketargetdir
	-@mkdir -p $(dir $@) >/dev/null 2>&1
endef

.PHONY : clean
clean :
	rm -f $(BINARY) $(OBJS) $(DEPS)
