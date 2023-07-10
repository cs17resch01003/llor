/*
!!!~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~!!!
!!! Copyright (c) 2017-20, Lawrence Livermore National Security, LLC
!!! and DataRaceBench project contributors. See the DataRaceBench/COPYRIGHT file for details.
!!!
!!! SPDX-License-Identifier: (BSD-3-Clause)
!!!~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~!!!
*/

/*
Concurrent access of var@28:5 in an intra region. Lock ensures that there is no data race.
*/

#include <stdio.h>
#include <omp.h>
#define N 100

int main(){
  omp_lock_t lck;
  int var=0;
  omp_init_lock(&lck);

  #pragma omp parallel for ordered
  for (int i=0; i<N; i++){
    #pragma omp ordered
    var++;
  }

  omp_destroy_lock(&lck);
  printf("%d\n",var);
  return 0;
}
