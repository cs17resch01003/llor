!; Pass
!; Add a barrier at line number 18.

program B03_singleline_race
    use omp_lib

    implicit none
    integer :: i, id, threads = 10
    integer :: data(11)

    call omp_set_num_threads(threads)
    do i = 1, threads+1
        data(i) = i
    end do

    !$omp parallel private(id)
        id = omp_get_thread_num()
        data(id) = data(id+1)
    !$omp end parallel
end program
