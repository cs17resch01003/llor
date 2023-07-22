//; Pass
//; Create an ordered region covering lines 20 to 23.

#define NUM_THREADS 10

int main()
{
	int data_a[NUM_THREADS+1], data_b[NUM_THREADS+1];

	for (int i = 0; i < NUM_THREADS+1; i++)
	{
		data_a[i] = i;
		data_b[i] = i;
	}

	#pragma omp parallel for ordered
	for (int i = 0; i < NUM_THREADS; i++)
	{
		int temp = data_a[i+1];
		#pragma omp ordered
		{
			data_a[i] = temp;

			temp = data_b[i+1];
			data_b[i] = temp;
		}
	}

	return 0;
}
