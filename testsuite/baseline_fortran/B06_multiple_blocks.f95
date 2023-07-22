!; Pass
!; Add a barrier at line number 22.
!; Add a barrier at line number 29.

program B06_multiple_blocks
    use omp_lib

    implicit none
    integer :: i, id, temp, threads = 10
    integer :: data_a(11), data_b(11)

    call omp_set_num_threads(threads)
    do i = 1, threads+1
        data_a(i) = i
        data_b(i) = i
    end do

    !$omp parallel private(id,temp)
        id = omp_get_thread_num()

        temp = data_a(id+1)
		data_a(id) = temp
    !$omp end parallel

    !$omp parallel private(id,temp)
        id = omp_get_thread_num()

        temp = data_b(id+1)
		data_b(id) = temp
    !$omp end parallel
end program
