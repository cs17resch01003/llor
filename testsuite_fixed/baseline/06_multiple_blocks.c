//; pass
//; Add a barrier at line number 24.
//; Add a barrier at line number 32.

#include "omp.h"
#define NUM_THREADS 10

int main()
{
	int data_a[NUM_THREADS+1], data_b[NUM_THREADS+1];
	omp_set_num_threads(NUM_THREADS);

	for (int i = 0; i < NUM_THREADS+1; i++)
	{
		data_a[i] = i;
		data_b[i] = i;
	}

	#pragma omp parallel
	{
		int id = omp_get_thread_num();
		
		int temp = data_a[id+1];
		#pragma omp barrier
		data_a[id] = temp;
	}

	#pragma omp parallel
	{
		int id = omp_get_thread_num();
		
		int temp = data_b[id+1];
		#pragma omp barrier
		data_b[id] = temp;
	}
}
