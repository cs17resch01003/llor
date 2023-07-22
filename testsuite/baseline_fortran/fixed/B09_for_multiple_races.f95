!; Pass
!; Create an ordered region covering lines 18 to 22.

program B09_for_multiple_races
    use omp_lib

    implicit none
    integer :: i, temp, threads = 10
    integer :: data_a(11), data_b(11)

    do i = 1, threads+1
        data_a(i) = i
        data_b(i) = i
    end do

    !$omp parallel do private(temp) ordered
    do i = 1, threads
        !$omp ordered
            temp = data_a(i+1) + 1
            data_a(i) = temp

            temp = data_b(i+1) + 1
            data_b(i) = temp
        !$omp end ordered
    end do
    !$omp end parallel do
end program
