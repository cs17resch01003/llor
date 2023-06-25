#define NUM_THREADS 10

int main()
{
	int data[NUM_THREADS+1];

	for (int i = 0; i < NUM_THREADS+1; i++)
		data[i] = i;

	#pragma omp parallel for
	for (int i = 0; i < NUM_THREADS; i++)
		data[i] = data[i+1];

	return 0;
}
