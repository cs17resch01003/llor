!; Pass
!; Create an ordered region covering lines 20 to 21.
!; Create an ordered region covering lines 27 to 28.

program B11_for_multiple_blocks
    use omp_lib

    implicit none
    integer :: i, id, temp, threads = 10
    integer :: data_a(11), data_b(11)

    call omp_set_num_threads(threads)
    do i = 1, threads+1
        data_a(i) = i
        data_b(i) = i
    end do

    !$omp parallel do private(temp) ordered
    do i = 1, threads
        !$omp ordered
            temp = data_a(i+1) + 1
            data_a(i) = temp
        !$omp end ordered
    end do
    !$omp end parallel do

    !$omp parallel do private(temp) ordered
    do i = 1, threads
        !$omp ordered
            temp = data_b(i+1) + 1
            data_b(i) = temp
        !$omp end ordered
    end do
    !$omp end parallel do
end program
