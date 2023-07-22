/*
!!!~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~!!!
!!! Copyright (c) 2017-20, Lawrence Livermore National Security, LLC
!!! and DataRaceBench project contributors. See the DataRaceBench/COPYRIGHT file for details.
!!!
!!! SPDX-License-Identifier: (BSD-3-Clause)
!!!~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~!!!
*/

/*This example is referred from DRACC by Adrian Schmitz et al.
Vector addition followed by multiplication involving the same var should have a barrier in between.
omp distribute directive does not have implicit barrier. This will cause data race.
Data Race Pair: b[i]@42:19:R vs. b[i]@47:9:W
*/

#include <stdio.h>
#include <omp.h>
#include <stdlib.h>
#define N 100
#define C 16


int a;
int b[C];
int c[C];
int temp[C];

int main(){
  for(int i=0; i<C; i++){
    b[i]=0;
    c[i]=2;
    temp[i]=0;
  }
  a=2;

    #pragma omp parallel
    for(int i=0; i<N ;i++){
      #pragma omp for ordered
      for(int i=0; i<C; i++){
        #pragma omp ordered
        temp[i] = b[i] + c[i];
      }

      #pragma omp for ordered
      for(int i=C-1; i>=0; i--){
        #pragma omp ordered
        b[i] = temp[i] * a;
      }
    }

  int val = 0;

  for(int i=0; i<N; i++){
    val = val + 2;
    val = val * 2;
  }

  for(int i=0; i<C; i++){
    if(b[i]!=val){
      printf("index: %d val: %d\n",i, b[i]);
    }
  }

  return 0;
}