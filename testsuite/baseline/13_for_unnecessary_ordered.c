#define NUM_THREADS 10

int main()
{
	int data[NUM_THREADS+1];

	for (int i = 0; i < NUM_THREADS+1; i++)
		data[i] = i;

	#pragma omp parallel for ordered
	for (int i = 0; i < NUM_THREADS; i++)
	{
		int temp;
		#pragma omp ordered
			temp = i+10;
		data[i] = temp;
	}

	return 0;
}
