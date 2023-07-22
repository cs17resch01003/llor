!; Pass
!; Create an ordered region covering lines 17 to 18.

program B08_for_race
    use omp_lib

    implicit none
    integer :: i, temp, threads = 10
    integer :: data(11)

    do i = 1, threads+1
        data(i) = i
    end do

    !$omp parallel do private(temp) ordered
    do i = 1, threads
        !$omp ordered
            temp = data(i+1) + 1
            data(i) = temp
        !$omp end ordered
    end do
    !$omp end parallel do
end program
