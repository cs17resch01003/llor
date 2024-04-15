<h1 align="center">LLOR Documentation</h1>

<h1>Table of Contents</h1>

- [A. Introduction](#a-introduction)
- [B. Developer Guide](#b-developer-guide)
    + [Building from Source on Ubuntu 22.04](#building-from-source-on-ubuntu-2204)
- [C. Getting Started with LLOR](#c-getting-started-with-llor)

# A. Introduction

LLOR (LLVM OpenMP Repairer) is our tool that can repair data race errors in parallel programs written in C/C++ and Fortran using the OpenMP API. It uses LLOV <a href="#1">[1]</a> as the verification oracle.

# B. Developer Guide

### Building from Source on Ubuntu 22.04

1. Install the [Git](https://git-scm.com/) client to download the source code

2. Install [build-essential](https://packages.ubuntu.com/jammy/build-essential), which is a meta-package that includes the tools that are essential to build C/C++ programs. Install [cmake](https://cmake.org/) and [ninja](https://ninja-build.org/) for building the tools from source. Install the [.NET SDK](https://dotnet.microsoft.com/en-us/download) to compile the C# code

```bash
sudo apt update

sudo apt install git
sudo apt install build-essential
sudo apt install cmake
sudo apt install ninja-build
sudo apt install dotnet-sdk-6.0
sudo apt install libopenmpi-dev
```

3. Download the source code of LLOV from the [cs17resch01003/llov](https://github.com/cs17resch01003/llov) repository and compile it

```bash
export BUILD_ROOT=/path/to/build

# download the source code
cd ${BUILD_ROOT}
git clone https://github.com/cs17resch01003/llov.git

# generate the build files
mkdir ${BUILD_ROOT}/llov/build
cd ${BUILD_ROOT}/llov/build
cmake -G Ninja -DCMAKE_BUILD_TYPE=Release -DLLVM_TARGETS_TO_BUILD=X86 -DLLVM_ENABLE_PROJECTS="clang;openmp;polly" ../llvm

# start the build (recommended value of N is the number of cores)
cmake --build . -jN
sudo cmake --build . --target install
```

4. Download the source code of classic-flang from the [classic-flang](https://github.com/flang-compiler/classic-flang-llvm-project) repository and flang from the [flang](https://github.com/flang-compiler/flang) repository

```bash
# download the source code
cd ${BUILD_ROOT}
git clone -b release_12x https://github.com/flang-compiler/classic-flang-llvm-project.git
git clone -b legacy https://github.com/flang-compiler/flang.git
```

5. Compile the source code of classic-flang

```bash
# generate the build files
mkdir ${BUILD_ROOT}/classic-flang-llvm-project/build
cd ${BUILD_ROOT}/classic-flang-llvm-project/build
cmake -G Ninja -DCMAKE_BUILD_TYPE=Release -DCMAKE_C_COMPILER=/usr/bin/gcc -DCMAKE_CXX_COMPILER=/usr/bin/g++ -DCMAKE_INSTALL_PREFIX=${BUILD_ROOT}/flang/install -DLLVM_TARGETS_TO_BUILD=X86 -DLLVM_ENABLE_CLASSIC_FLANG=ON -DLLVM_ENABLE_PROJECTS="clang;openmp" ../llvm

# start the build (recommended value of N is the number of cores)
cmake --build . -jN
cmake --build . --target install
```

6. Compile the source code of the Fortran runtime math library from the [flang](https://github.com/flang-compiler/flang) repository

```bash
# generate the build files
mkdir ${BUILD_ROOT}/flang/runtime/libpgmath/build
cd ${BUILD_ROOT}/flang/runtime/libpgmath/build
cmake -G Ninja -DCMAKE_BUILD_TYPE=Release -DCMAKE_C_COMPILER=${BUILD_ROOT}/flang/install/bin/clang -DCMAKE_CXX_COMPILER=${BUILD_ROOT}/flang/install/bin/clang++ -DCMAKE_INSTALL_PREFIX=${BUILD_ROOT}/flang/install ..

# start the build (recommended value of N is the number of cores)
cmake --build . -jN
cmake --build . --target install
```

7. Compile the source code of flang

```bash
# generate the build files
mkdir ${BUILD_ROOT}/flang/build
cd ${BUILD_ROOT}/flang/build
cmake -DCMAKE_BUILD_TYPE=Release -DCMAKE_C_COMPILER=${BUILD_ROOT}/flang/install/bin/clang -DCMAKE_CXX_COMPILER=${BUILD_ROOT}/flang/install/bin/clang++ -DCMAKE_Fortran_COMPILER=${BUILD_ROOT}/flang/install/bin/flang -DCMAKE_Fortran_COMPILER_ID=Flang -DLLVM_CONFIG=${BUILD_ROOT}/flang/install/bin/llvm-config -DCMAKE_INSTALL_PREFIX=${BUILD_ROOT}/flang/install -DLLVM_TARGETS_TO_BUILD=X86 -DFLANG_LLVM_EXTENSIONS=ON ..

# start the build (recommended value of N is the number of cores)
cmake --build . -jN
cmake --build . --target install
```

8. Add the install directory of flang to the PATH environment variable. To make this permanent, include this line in ~/.profile

```bash
export PATH="$PATH:/datadrive/flang/install/bin"
```

9. Download the source code of LLOR from the [llor](https://github.com/cs17resch01003/llor) repository and compile it

```bash
# download the source code
cd ${BUILD_ROOT}
git clone https://github.com/cs17resch01003/llor.git

# compile the code
cd ${BUILD_ROOT}/llor
dotnet build --configuration Release ./src/Repair/Repair.csproj
dotnet build --configuration Release ./src/Transform/Transform.csproj
dotnet build --configuration Release ./src/TestRunner/TestRunner.csproj

# create symbolic links
sudo ln -s ${BUILD_ROOT}/llor/src/Repair/bin/Release/net6.0/Repair /usr/bin/llor
sudo ln -s ${BUILD_ROOT}/llor/src/Transform/bin/Release/net6.0/Transform /usr/bin/llor_transform
sudo ln -s ${BUILD_ROOT}/llor/src/TestRunner/bin/Release/net6.0/TestRunner /usr/bin/llor_testrunner
sudo ln -s ${BUILD_ROOT}/llor/scripts/llov.sh /usr/bin/llov
sudo ln -s ${BUILD_ROOT}/llor/scripts/llov_transform.sh /usr/bin/llov_transform
sudo ln -s ${BUILD_ROOT}/llor/scripts/llov_compile.sh /usr/bin/llov_compile

# test the build
llor_testrunner --folder ${BUILD_ROOT}/llor/testsuite
```

# C. Getting Started with LLOR

LLOR has a simple execution syntax which uses the symbolic links created in the previous step. Below is a C program that has a data race. This program is located at *${BUILD_ROOT}/llor/testsuite/baseline/B01_simple_race.c*

```c
#include "omp.h"
#define NUM_THREADS 10

int main()
{
    int data[NUM_THREADS+1];
    omp_set_num_threads(NUM_THREADS);

    for (int i = 0; i < NUM_THREADS+1; i++)
        data[i] = i;

    #pragma omp parallel
    {
        int id = omp_get_thread_num();

        int temp = data[id+1];
        data[id] = temp;
    }
}
```

To repair this program, run the following command.

```bash
cd ${BUILD_ROOT}/llor
llor --file ./testsuite/baseline/B01_simple_race.c
```

This command repairs the program and prints the number of changes that are required to correct this program. The changes that are needed are written to a summary file at the same location where the program is.

```bash
cat ./testsuite/baseline/B01_simple_race.summary
# prints the line number where the barrier needs to be inserted to fix the data race
```

The changes in the summary file specify the file and line where a barrier needs to be added or removed. The fixed program is also available in its LLVM Intermediate Representation (IR) at the same location with the extension **.inst.ll**

The repaired program will look like below.

```c
#include "omp.h"
#define NUM_THREADS 10

int main()
{
    int data[NUM_THREADS+1];
    omp_set_num_threads(NUM_THREADS);

    for (int i = 0; i < NUM_THREADS+1; i++)
        data[i] = i;

    #pragma omp parallel
    {
        int id = omp_get_thread_num();

        int temp = data[id+1];
        #pragma omp barrier
        data[id] = temp;
    }
}
```

# References

<a id="1">[1]</a>
Utpal Bora, Santanu Das, Pankaj Kukreja, Saurabh Joshi, Ramakrishna Upadrasta, and Sanjay Rajopadhye.
LLOV: A Fast Static Data-Race Checker for OpenMP Programs.
In ACM Transactions on Architecture and Code Optimization 17(4): 35:1-35:26.
