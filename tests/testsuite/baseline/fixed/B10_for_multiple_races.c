//; Pass
//; Create an ordered region covering lines 17 to 19.

#define NUM_THREADS 10

int main()
{
	int data[NUM_THREADS+1];

	for (int i = 0; i < NUM_THREADS+1; i++)
		data[i] = i;

	#pragma omp parallel for ordered
	for (int i = 0; i < NUM_THREADS; i++)
	{
		int temp = data[i+1];
		#pragma omp ordered
		{
			data[i] = temp;
			temp = data[i+1];			
		}
		data[i] = temp;
	}

	return 0;
}