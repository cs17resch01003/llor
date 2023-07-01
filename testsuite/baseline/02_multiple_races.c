//; Pass
//; Add a barrier at line number 25.
//; Add a barrier at line number 24.
//; Add a barrier at line number 22.

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

		temp = data[id+2];
		data[id] = temp;
	}
}
