!; Pass
!; Create an ordered region covering lines 17 to 21.

program B10_for_multiple_races
    use omp_lib

    implicit none
    integer :: i, temp, threads = 10
    integer :: data(11)

    do i = 1, threads+1
        data(i) = i
    end do

    !$omp parallel do private(temp)
    do i = 1, threads
        temp = data(i+1) + 1
        data(i) = temp

        temp = data(i+1) + 1
        data(i) = temp
    end do
    !$omp end parallel do
end program
