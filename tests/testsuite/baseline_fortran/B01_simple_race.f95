!; Pass

program B01_simple_race
    use omp_lib

    implicit none
    integer :: i, id, temp, threads = 10
    integer :: data(11)

    call omp_set_num_threads(threads)
    do i = 1, threads+1
        data(i) = i
    end do

    !$omp parallel private(id,temp)
        id = omp_get_thread_num()

        temp = data(id+1)
		data(id) = temp
    !$omp end parallel
end program
