!; Pass

program B07_racefree
    use omp_lib

    implicit none
    integer, parameter :: threads = 10
    integer :: i, id, temp
    integer :: data(threads+1)

    call omp_set_num_threads(threads)
    do i = 1, threads+1
        data(i) = i
    end do

    !$omp parallel private(id, temp)
        id = omp_get_thread_num()

        temp = data(id+1)
        !$omp barrier
		data(id) = temp
    !$omp end parallel
end program
