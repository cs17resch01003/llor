//; Pass
//; Create an ordered region covering line 15.

#define NUM_THREADS 10

int main()
{
	int data[NUM_THREADS+1];

	for (int i = 0; i < NUM_THREADS+1; i++)
		data[i] = i;

	#pragma omp parallel for ordered
	for (int i = 0; i < NUM_THREADS; i++)
		#pragma omp ordered
			data[i] = data[i+1];

	return 0;
}
