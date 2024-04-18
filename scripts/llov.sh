#!/bin/bash

input=$1
filepath=${input%.*}
extension=${input##*.}

llov_compile $input

if [[ $extension == $input ]]
then
    cd $input
    for file in `find . -name '*.ll' -type f`; do
        llov_transform $file
        filepath=${file%.*}

        if [[ $2 == "debug" ]]
        then
            opt -load OpenMPVerify.so -openmp-verify-mhp $filepath.sb.ll -S -o $filepath.debug.ll --strip-debug
            exitCode=`echo $?`
            if [[ $exitCode != 0 ]]; then
                exit $exitCode
            fi
        else
            opt -load OpenMPVerify.so -disable-output -openmp-verify-mhp $filepath.sb.ll
            exitCode=`echo $?`
            if [[ $exitCode != 0 ]]; then
                exit $exitCode
            fi

            rm $filepath.sb.ll
        fi
    done
else
    llov_transform $input
    if [[ $2 == "debug" ]]
    then
        opt -load OpenMPVerify.so -openmp-verify-mhp $filepath.sb.ll -S -o $filepath.debug.ll --strip-debug
        exitCode=`echo $?`
        if [[ $exitCode != 0 ]]; then
            exit $exitCode
        fi
    else
        opt -load OpenMPVerify.so -disable-output -openmp-verify-mhp $filepath.sb.ll
        exitCode=`echo $?`
        if [[ $exitCode != 0 ]]; then
            exit $exitCode
        fi

        rm $filepath.sb.ll
    fi
fi
