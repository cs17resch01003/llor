//; Pass
//; Create an ordered region covering lines 76 to 77.

/*
Copyright (c) 2017, Lawrence Livermore National Security, LLC.
Produced at the Lawrence Livermore National Laboratory
Written by Chunhua Liao, Pei-Hung Lin, Joshua Asplund,
Markus Schordan, and Ian Karlin
(email: liao6@llnl.gov, lin32@llnl.gov, asplund1@llnl.gov,
schordan1@llnl.gov, karlin1@llnl.gov)
LLNL-CODE-732144
All rights reserved.

This file is part of DataRaceBench. For details, see
https://github.com/LLNL/dataracebench. Please also see the LICENSE file
for our additional BSD notice.

Redistribution and use in source and binary forms, with
or without modification, are permitted provided that the following
conditions are met:

* Redistributions of source code must retain the above copyright
  notice, this list of conditions and the disclaimer below.

* Redistributions in binary form must reproduce the above copyright
  notice, this list of conditions and the disclaimer (as noted below)
  in the documentation and/or other materials provided with the
  distribution.

* Neither the name of the LLNS/LLNL nor the names of its contributors
  may be used to endorse or promote products derived from this
  software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND
CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF
MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL LAWRENCE LIVERMORE NATIONAL
SECURITY, LLC, THE U.S. DEPARTMENT OF ENERGY OR CONTRIBUTORS BE
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY,
OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
THE POSSIBILITY OF SUCH DAMAGE.
*/

/*
For a variable declared in a scope inside an OpenMP construct:
* private if the variable has an automatic storage duration
* shared if the variable has a static storage duration. 

Dependence pairs: 
   tmp@73:7:W vs. tmp@73:7:W
   tmp@73:7:W vs. tmp@74:14:R
*/
#include<stdio.h>

int main(int argc, char* argv[])
{
  int i;
  int len=100;
  int a[len], b[len];

  for (i=0;i<len;i++)
  {  a[i]=i; b[i]=i;} 
/* static storage for a local variable */
#pragma omp parallel 
  {
    static int tmp;
#pragma omp for
    for (i=0;i<len;i++)
    {
      tmp = a[i]+i;
      a[i] = tmp;
    }
  }

/* automatic storage for a local variable */
#pragma omp parallel 
  {
    int tmp;
#pragma omp for
    for (i=0;i<len;i++)
    {
      tmp = b[i]+i;
      b[i] = tmp;
    }
  }

  printf("a[50]=%d b[50]=%d\n", a[50], b[50]);
 
  return 0;
}
