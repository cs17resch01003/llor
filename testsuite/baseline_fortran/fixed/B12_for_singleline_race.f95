!; Pass
!; Create an ordered region covering line 17.

program B12_for_singleline_race
    use omp_lib

    implicit none
    integer :: i, threads = 10
    integer :: data(11)

    do i = 1, threads+1
        data(i) = i
    end do

    !$omp parallel do ordered
    do i = 1, threads
        !$omp ordered
            data(i) = data(i+1) + 1
        !$omp end ordered
    end do
    !$omp end parallel do
end program
