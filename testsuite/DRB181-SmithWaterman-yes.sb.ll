; ModuleID = '/datadrive/llor/testsuite/dataracebench/DRB181-SmithWaterman-yes.in.ll'
source_filename = "/datadrive/llor/testsuite/dataracebench/DRB181-SmithWaterman-yes.c"
target datalayout = "e-m:e-p270:32:32-p271:32:32-p272:64:64-i64:64-f80:128-n8:16:32:64-S128"
target triple = "x86_64-unknown-linux-gnu"

%struct.ident_t = type { i32, i32, i32, i32, i8* }

@m = dso_local global i64 0, align 8, !dbg !0
@n = dso_local global i64 0, align 8, !dbg !6
@a = dso_local global i8* null, align 8, !dbg !10
@b = dso_local global i8* null, align 8, !dbg !14
@matchScore = dso_local global i32 5, align 4, !dbg !16
@missmatchScore = dso_local global i32 -3, align 4, !dbg !19
@gapScore = dso_local global i32 -4, align 4, !dbg !21
@0 = private unnamed_addr constant [92 x i8] c";/datadrive/llor/testsuite/dataracebench/DRB181-SmithWaterman-yes.c;similarityScore;181;5;;\00", align 1
@1 = private unnamed_addr constant %struct.ident_t { i32 0, i32 2, i32 0, i32 0, i8* getelementptr inbounds ([92 x i8], [92 x i8]* @0, i32 0, i32 0) }, align 8
@.gomp_critical_user_.var = common global [8 x i32] zeroinitializer
@2 = private unnamed_addr constant [82 x i8] c";/datadrive/llor/testsuite/dataracebench/DRB181-SmithWaterman-yes.c;main;253;13;;\00", align 1
@3 = private unnamed_addr constant %struct.ident_t { i32 0, i32 514, i32 0, i32 0, i8* getelementptr inbounds ([82 x i8], [82 x i8]* @2, i32 0, i32 0) }, align 8
@4 = private unnamed_addr constant [82 x i8] c";/datadrive/llor/testsuite/dataracebench/DRB181-SmithWaterman-yes.c;main;253;28;;\00", align 1
@5 = private unnamed_addr constant %struct.ident_t { i32 0, i32 514, i32 0, i32 0, i8* getelementptr inbounds ([82 x i8], [82 x i8]* @4, i32 0, i32 0) }, align 8
@6 = private unnamed_addr constant %struct.ident_t { i32 0, i32 66, i32 0, i32 0, i8* getelementptr inbounds ([82 x i8], [82 x i8]* @2, i32 0, i32 0) }, align 8
@7 = private unnamed_addr constant [81 x i8] c";/datadrive/llor/testsuite/dataracebench/DRB181-SmithWaterman-yes.c;main;246;5;;\00", align 1
@8 = private unnamed_addr constant %struct.ident_t { i32 0, i32 2, i32 0, i32 0, i8* getelementptr inbounds ([81 x i8], [81 x i8]* @7, i32 0, i32 0) }, align 8

; Function Attrs: noinline nounwind uwtable
define dso_local void @generate() #0 !dbg !27 {
  %1 = call i64 @time(i64* null) #5, !dbg !30
  %2 = trunc i64 %1 to i32, !dbg !30
  call void @srand(i32 %2) #5, !dbg !31
  call void @llvm.dbg.value(metadata i64 0, metadata !32, metadata !DIExpression()), !dbg !33
  br label %3, !dbg !34

3:                                                ; preds = %21, %0
  %.0 = phi i64 [ 0, %0 ], [ %22, %21 ], !dbg !36
  call void @llvm.dbg.value(metadata i64 %.0, metadata !32, metadata !DIExpression()), !dbg !33
  %4 = load i64, i64* @m, align 8, !dbg !37
  %5 = icmp slt i64 %.0, %4, !dbg !39
  br i1 %5, label %6, label %23, !dbg !40

6:                                                ; preds = %3
  %7 = call i32 @rand() #5, !dbg !41
  %8 = srem i32 %7, 4, !dbg !43
  call void @llvm.dbg.value(metadata i32 %8, metadata !44, metadata !DIExpression()), !dbg !45
  switch i32 %8, label %18 [
    i32 0, label %9
    i32 2, label %12
    i32 3, label %15
  ], !dbg !46

9:                                                ; preds = %6
  %10 = load i8*, i8** @a, align 8, !dbg !47
  %11 = getelementptr inbounds i8, i8* %10, i64 %.0, !dbg !47
  store i8 65, i8* %11, align 1, !dbg !49
  br label %21, !dbg !47

12:                                               ; preds = %6
  %13 = load i8*, i8** @a, align 8, !dbg !50
  %14 = getelementptr inbounds i8, i8* %13, i64 %.0, !dbg !50
  store i8 67, i8* %14, align 1, !dbg !52
  br label %21, !dbg !50

15:                                               ; preds = %6
  %16 = load i8*, i8** @a, align 8, !dbg !53
  %17 = getelementptr inbounds i8, i8* %16, i64 %.0, !dbg !53
  store i8 71, i8* %17, align 1, !dbg !55
  br label %21, !dbg !53

18:                                               ; preds = %6
  %19 = load i8*, i8** @a, align 8, !dbg !56
  %20 = getelementptr inbounds i8, i8* %19, i64 %.0, !dbg !56
  store i8 84, i8* %20, align 1, !dbg !57
  br label %21

21:                                               ; preds = %18, %15, %12, %9
  %22 = add nsw i64 %.0, 1, !dbg !58
  call void @llvm.dbg.value(metadata i64 %22, metadata !32, metadata !DIExpression()), !dbg !33
  br label %3, !dbg !59, !llvm.loop !60

23:                                               ; preds = %41, %3
  %.1 = phi i64 [ %42, %41 ], [ 0, %3 ], !dbg !63
  call void @llvm.dbg.value(metadata i64 %.1, metadata !32, metadata !DIExpression()), !dbg !33
  %24 = load i64, i64* @n, align 8, !dbg !65
  %25 = icmp slt i64 %.1, %24, !dbg !67
  br i1 %25, label %26, label %43, !dbg !68

26:                                               ; preds = %23
  %27 = call i32 @rand() #5, !dbg !69
  %28 = srem i32 %27, 4, !dbg !71
  call void @llvm.dbg.value(metadata i32 %28, metadata !72, metadata !DIExpression()), !dbg !73
  switch i32 %28, label %38 [
    i32 0, label %29
    i32 2, label %32
    i32 3, label %35
  ], !dbg !74

29:                                               ; preds = %26
  %30 = load i8*, i8** @b, align 8, !dbg !75
  %31 = getelementptr inbounds i8, i8* %30, i64 %.1, !dbg !75
  store i8 65, i8* %31, align 1, !dbg !77
  br label %41, !dbg !75

32:                                               ; preds = %26
  %33 = load i8*, i8** @b, align 8, !dbg !78
  %34 = getelementptr inbounds i8, i8* %33, i64 %.1, !dbg !78
  store i8 67, i8* %34, align 1, !dbg !80
  br label %41, !dbg !78

35:                                               ; preds = %26
  %36 = load i8*, i8** @b, align 8, !dbg !81
  %37 = getelementptr inbounds i8, i8* %36, i64 %.1, !dbg !81
  store i8 71, i8* %37, align 1, !dbg !83
  br label %41, !dbg !81

38:                                               ; preds = %26
  %39 = load i8*, i8** @b, align 8, !dbg !84
  %40 = getelementptr inbounds i8, i8* %39, i64 %.1, !dbg !84
  store i8 84, i8* %40, align 1, !dbg !85
  br label %41

41:                                               ; preds = %38, %35, %32, %29
  %42 = add nsw i64 %.1, 1, !dbg !86
  call void @llvm.dbg.value(metadata i64 %42, metadata !32, metadata !DIExpression()), !dbg !33
  br label %23, !dbg !87, !llvm.loop !88

43:                                               ; preds = %23
  ret void, !dbg !90
}

; Function Attrs: nounwind
declare dso_local void @srand(i32) #1

; Function Attrs: nounwind
declare dso_local i64 @time(i64*) #1

; Function Attrs: nofree nosync nounwind readnone speculatable willreturn
declare void @llvm.dbg.declare(metadata, metadata, metadata) #2

; Function Attrs: nounwind
declare dso_local i32 @rand() #1

; Function Attrs: alwaysinline nounwind uwtable
define dso_local i64 @nElement(i64 %0) #3 !dbg !91 {
  call void @llvm.dbg.value(metadata i64 %0, metadata !94, metadata !DIExpression()), !dbg !95
  %2 = load i64, i64* @m, align 8, !dbg !96
  %3 = icmp slt i64 %0, %2, !dbg !98
  %4 = load i64, i64* @n, align 8
  %5 = icmp slt i64 %0, %4
  %or.cond = and i1 %3, %5, !dbg !99
  br i1 %or.cond, label %37, label %6, !dbg !99

6:                                                ; preds = %1
  %7 = load i64, i64* @m, align 8, !dbg !100
  %8 = load i64, i64* @n, align 8, !dbg !100
  %9 = icmp sgt i64 %7, %8, !dbg !100
  %10 = load i64, i64* @m, align 8, !dbg !100
  %11 = load i64, i64* @n, align 8, !dbg !100
  %12 = select i1 %9, i64 %10, i64 %11, !dbg !100
  %13 = icmp slt i64 %0, %12, !dbg !102
  br i1 %13, label %14, label %22, !dbg !103

14:                                               ; preds = %6
  %15 = load i64, i64* @m, align 8, !dbg !104
  %16 = load i64, i64* @n, align 8, !dbg !104
  %17 = icmp slt i64 %15, %16, !dbg !104
  %18 = load i64, i64* @m, align 8, !dbg !104
  %19 = load i64, i64* @n, align 8, !dbg !104
  %20 = select i1 %17, i64 %18, i64 %19, !dbg !104
  call void @llvm.dbg.value(metadata i64 %20, metadata !106, metadata !DIExpression()), !dbg !107
  %21 = sub nsw i64 %20, 1, !dbg !108
  br label %37, !dbg !109

22:                                               ; preds = %6
  %23 = load i64, i64* @m, align 8, !dbg !110
  %24 = load i64, i64* @n, align 8, !dbg !110
  %25 = icmp slt i64 %23, %24, !dbg !110
  %26 = load i64, i64* @m, align 8, !dbg !110
  %27 = load i64, i64* @n, align 8, !dbg !110
  %28 = select i1 %25, i64 %26, i64 %27, !dbg !110
  call void @llvm.dbg.value(metadata i64 %28, metadata !112, metadata !DIExpression()), !dbg !113
  %29 = mul nsw i64 2, %28, !dbg !114
  %30 = sub nsw i64 %29, %0, !dbg !115
  %31 = load i64, i64* @m, align 8, !dbg !116
  %32 = load i64, i64* @n, align 8, !dbg !117
  %33 = sub nsw i64 %31, %32, !dbg !118
  %34 = call i64 @labs(i64 %33) #11, !dbg !119
  %35 = add nsw i64 %30, %34, !dbg !120
  %36 = sub nsw i64 %35, 2, !dbg !121
  br label %37, !dbg !122

37:                                               ; preds = %22, %14, %1
  %.0 = phi i64 [ %21, %14 ], [ %36, %22 ], [ %0, %1 ], !dbg !123
  ret i64 %.0, !dbg !124
}

; Function Attrs: nounwind readnone
declare dso_local i64 @labs(i64) #4

; Function Attrs: noinline nounwind uwtable
define dso_local i32 @matchMissmatchScore(i64 %0, i64 %1) #0 !dbg !125 {
  call void @llvm.dbg.value(metadata i64 %0, metadata !128, metadata !DIExpression()), !dbg !129
  call void @llvm.dbg.value(metadata i64 %1, metadata !130, metadata !DIExpression()), !dbg !129
  %3 = load i8*, i8** @a, align 8, !dbg !131
  %4 = sub nsw i64 %1, 1, !dbg !133
  %5 = getelementptr inbounds i8, i8* %3, i64 %4, !dbg !131
  %6 = load i8, i8* %5, align 1, !dbg !131
  %7 = sext i8 %6 to i32, !dbg !131
  %8 = load i8*, i8** @b, align 8, !dbg !134
  %9 = sub nsw i64 %0, 1, !dbg !135
  %10 = getelementptr inbounds i8, i8* %8, i64 %9, !dbg !134
  %11 = load i8, i8* %10, align 1, !dbg !134
  %12 = sext i8 %11 to i32, !dbg !134
  %13 = icmp eq i32 %7, %12, !dbg !136
  %14 = load i32, i32* @matchScore, align 4, !dbg !137
  %15 = load i32, i32* @missmatchScore, align 4, !dbg !137
  %.0 = select i1 %13, i32 %14, i32 %15, !dbg !137
  ret i32 %.0, !dbg !138
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @similarityScore(i64 %0, i64 %1, i32* %2, i32* %3, i64* %4) #3 !dbg !139 {
  %6 = call i32 @__kmpc_global_thread_num(%struct.ident_t* @1)
  call void @llvm.dbg.value(metadata i64 %0, metadata !144, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i64 %1, metadata !146, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i32* %2, metadata !147, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i32* %3, metadata !148, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i64* %4, metadata !149, metadata !DIExpression()), !dbg !145
  %7 = load i64, i64* @m, align 8, !dbg !150
  %8 = mul nsw i64 %7, %0, !dbg !151
  %9 = add nsw i64 %8, %1, !dbg !152
  call void @llvm.dbg.value(metadata i64 %9, metadata !153, metadata !DIExpression()), !dbg !145
  %10 = load i64, i64* @m, align 8, !dbg !154
  %11 = sub nsw i64 %9, %10, !dbg !155
  %12 = getelementptr inbounds i32, i32* %2, i64 %11, !dbg !156
  %13 = load i32, i32* %12, align 4, !dbg !156
  %14 = load i32, i32* @gapScore, align 4, !dbg !157
  %15 = add nsw i32 %13, %14, !dbg !158
  call void @llvm.dbg.value(metadata i32 %15, metadata !159, metadata !DIExpression()), !dbg !145
  %16 = sub nsw i64 %9, 1, !dbg !160
  %17 = getelementptr inbounds i32, i32* %2, i64 %16, !dbg !161
  %18 = load i32, i32* %17, align 4, !dbg !161
  %19 = load i32, i32* @gapScore, align 4, !dbg !162
  %20 = add nsw i32 %18, %19, !dbg !163
  call void @llvm.dbg.value(metadata i32 %20, metadata !164, metadata !DIExpression()), !dbg !145
  %21 = load i64, i64* @m, align 8, !dbg !165
  %22 = sub nsw i64 %9, %21, !dbg !166
  %23 = sub nsw i64 %22, 1, !dbg !167
  %24 = getelementptr inbounds i32, i32* %2, i64 %23, !dbg !168
  %25 = load i32, i32* %24, align 4, !dbg !168
  %26 = call i32 @matchMissmatchScore(i64 %0, i64 %1), !dbg !169
  %27 = add nsw i32 %25, %26, !dbg !170
  call void @llvm.dbg.value(metadata i32 %27, metadata !171, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i32 0, metadata !172, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i32 0, metadata !173, metadata !DIExpression()), !dbg !145
  %28 = icmp sgt i32 %27, 0, !dbg !174
  %spec.select = select i1 %28, i32 %27, i32 0, !dbg !176
  %spec.select4 = select i1 %28, i32 3, i32 0, !dbg !176
  call void @llvm.dbg.value(metadata i32 %spec.select4, metadata !173, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i32 %spec.select, metadata !172, metadata !DIExpression()), !dbg !145
  %29 = icmp sgt i32 %15, %spec.select, !dbg !177
  %.12 = select i1 %29, i32 %15, i32 %spec.select, !dbg !179
  %.1 = select i1 %29, i32 1, i32 %spec.select4, !dbg !179
  call void @llvm.dbg.value(metadata i32 %.1, metadata !173, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i32 %.12, metadata !172, metadata !DIExpression()), !dbg !145
  %30 = icmp sgt i32 %20, %.12, !dbg !180
  %spec.select5 = select i1 %30, i32 %20, i32 %.12, !dbg !182
  %spec.select6 = select i1 %30, i32 2, i32 %.1, !dbg !182
  call void @llvm.dbg.value(metadata i32 %spec.select6, metadata !173, metadata !DIExpression()), !dbg !145
  call void @llvm.dbg.value(metadata i32 %spec.select5, metadata !172, metadata !DIExpression()), !dbg !145
  %31 = getelementptr inbounds i32, i32* %2, i64 %9, !dbg !183
  store i32 %spec.select5, i32* %31, align 4, !dbg !184
  %32 = getelementptr inbounds i32, i32* %3, i64 %9, !dbg !185
  store i32 %spec.select6, i32* %32, align 4, !dbg !186
  %33 = load i64, i64* %4, align 8, !dbg !187
  %34 = getelementptr inbounds i32, i32* %2, i64 %33, !dbg !189
  %35 = load i32, i32* %34, align 4, !dbg !189
  %36 = icmp sgt i32 %spec.select5, %35, !dbg !190
  br i1 %36, label %37, label %38, !dbg !191

37:                                               ; preds = %5
  call void @__kmpc_critical(%struct.ident_t* @1, i32 %6, [8 x i32]* @.gomp_critical_user_.var), !dbg !192
  store i64 %9, i64* %4, align 8, !dbg !195
  call void @__kmpc_end_critical(%struct.ident_t* @1, i32 %6, [8 x i32]* @.gomp_critical_user_.var), !dbg !196
  br label %38, !dbg !197

38:                                               ; preds = %37, %5
  ret void, !dbg !198
}

; Function Attrs: nounwind
declare i32 @__kmpc_global_thread_num(%struct.ident_t*) #5

; Function Attrs: convergent nounwind
declare void @__kmpc_end_critical(%struct.ident_t*, i32, [8 x i32]*) #6

; Function Attrs: convergent nounwind
declare void @__kmpc_critical(%struct.ident_t*, i32, [8 x i32]*) #6

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @calcFirstDiagElement(i64* %0, i64* %1, i64* %2) #3 !dbg !199 {
  call void @llvm.dbg.value(metadata i64* %0, metadata !202, metadata !DIExpression()), !dbg !203
  call void @llvm.dbg.value(metadata i64* %1, metadata !204, metadata !DIExpression()), !dbg !203
  call void @llvm.dbg.value(metadata i64* %2, metadata !205, metadata !DIExpression()), !dbg !203
  %4 = load i64, i64* %0, align 8, !dbg !206
  %5 = load i64, i64* @n, align 8, !dbg !208
  %6 = icmp slt i64 %4, %5, !dbg !209
  br i1 %6, label %7, label %9, !dbg !210

7:                                                ; preds = %3
  %8 = load i64, i64* %0, align 8, !dbg !211
  store i64 %8, i64* %1, align 8, !dbg !213
  store i64 1, i64* %2, align 8, !dbg !214
  br label %16, !dbg !215

9:                                                ; preds = %3
  %10 = load i64, i64* @n, align 8, !dbg !216
  %11 = sub nsw i64 %10, 1, !dbg !218
  store i64 %11, i64* %1, align 8, !dbg !219
  %12 = load i64, i64* %0, align 8, !dbg !220
  %13 = load i64, i64* @n, align 8, !dbg !221
  %14 = sub nsw i64 %12, %13, !dbg !222
  %15 = add nsw i64 %14, 2, !dbg !223
  store i64 %15, i64* %2, align 8, !dbg !224
  br label %16

16:                                               ; preds = %9, %7
  ret void, !dbg !225
}

; Function Attrs: noinline nounwind uwtable
define dso_local i32 @main(i32 %0, i8** %1) #0 !dbg !226 {
  %3 = alloca i32*, align 8
  %4 = alloca i32*, align 8
  %5 = alloca i64, align 8
  %6 = alloca i64, align 8
  call void @llvm.dbg.value(metadata i32 %0, metadata !230, metadata !DIExpression()), !dbg !231
  call void @llvm.dbg.value(metadata i8** %1, metadata !232, metadata !DIExpression()), !dbg !231
  store i64 2048, i64* @m, align 8, !dbg !233
  store i64 2048, i64* @n, align 8, !dbg !234
  %7 = load i64, i64* @m, align 8, !dbg !235
  %8 = mul i64 %7, 1, !dbg !236
  %9 = call noalias i8* @malloc(i64 %8) #5, !dbg !237
  store i8* %9, i8** @a, align 8, !dbg !238
  %10 = load i64, i64* @n, align 8, !dbg !239
  %11 = mul i64 %10, 1, !dbg !240
  %12 = call noalias i8* @malloc(i64 %11) #5, !dbg !241
  store i8* %12, i8** @b, align 8, !dbg !242
  %13 = load i64, i64* @m, align 8, !dbg !243
  %14 = add nsw i64 %13, 1, !dbg !243
  store i64 %14, i64* @m, align 8, !dbg !243
  %15 = load i64, i64* @n, align 8, !dbg !244
  %16 = add nsw i64 %15, 1, !dbg !244
  store i64 %16, i64* @n, align 8, !dbg !244
  call void @llvm.dbg.declare(metadata i32** %3, metadata !245, metadata !DIExpression()), !dbg !246
  %17 = load i64, i64* @m, align 8, !dbg !247
  %18 = load i64, i64* @n, align 8, !dbg !248
  %19 = mul nsw i64 %17, %18, !dbg !249
  %20 = call noalias i8* @calloc(i64 %19, i64 4) #5, !dbg !250
  %21 = bitcast i8* %20 to i32*, !dbg !250
  store i32* %21, i32** %3, align 8, !dbg !251
  call void @llvm.dbg.declare(metadata i32** %4, metadata !252, metadata !DIExpression()), !dbg !253
  %22 = load i64, i64* @m, align 8, !dbg !254
  %23 = load i64, i64* @n, align 8, !dbg !255
  %24 = mul nsw i64 %22, %23, !dbg !256
  %25 = call noalias i8* @calloc(i64 %24, i64 4) #5, !dbg !257
  %26 = bitcast i8* %25 to i32*, !dbg !257
  store i32* %26, i32** %4, align 8, !dbg !258
  call void @generate(), !dbg !259
  call void @llvm.dbg.declare(metadata i64* %5, metadata !260, metadata !DIExpression()), !dbg !261
  store i64 0, i64* %5, align 8, !dbg !261
  call void @llvm.dbg.declare(metadata i64* undef, metadata !262, metadata !DIExpression()), !dbg !263
  call void @llvm.dbg.declare(metadata i64* undef, metadata !264, metadata !DIExpression()), !dbg !265
  %27 = call double @omp_get_wtime(), !dbg !266
  call void @llvm.dbg.value(metadata double %27, metadata !267, metadata !DIExpression()), !dbg !231
  call void @llvm.dbg.declare(metadata i64* undef, metadata !269, metadata !DIExpression()), !dbg !270
  call void @llvm.dbg.declare(metadata i64* undef, metadata !271, metadata !DIExpression()), !dbg !272
  call void @llvm.dbg.declare(metadata i64* undef, metadata !273, metadata !DIExpression()), !dbg !274
  call void @llvm.dbg.declare(metadata i64* undef, metadata !275, metadata !DIExpression()), !dbg !276
  call void @llvm.dbg.declare(metadata i64* %6, metadata !277, metadata !DIExpression()), !dbg !278
  %28 = load i64, i64* @m, align 8, !dbg !279
  %29 = load i64, i64* @n, align 8, !dbg !280
  %30 = add nsw i64 %28, %29, !dbg !281
  %31 = sub nsw i64 %30, 3, !dbg !282
  store i64 %31, i64* %6, align 8, !dbg !278
  call void @llvm.dbg.declare(metadata i64* undef, metadata !283, metadata !DIExpression()), !dbg !284
  call void (%struct.ident_t*, i32, void (i32*, i32*, ...)*, ...) @__kmpc_fork_call(%struct.ident_t* @8, i32 4, void (i32*, i32*, ...)* bitcast (void (i32*, i32*, i64*, i32**, i32**, i64*)* @.omp_outlined. to void (i32*, i32*, ...)*), i64* %6, i32** %3, i32** %4, i64* %5), !dbg !285
  ret i32 0, !dbg !286
}

; Function Attrs: nounwind
declare dso_local noalias i8* @malloc(i64) #1

; Function Attrs: nounwind
declare dso_local noalias i8* @calloc(i64, i64) #1

declare dso_local double @omp_get_wtime() #7

declare dso_local void @__kmpc_for_static_init_8(%struct.ident_t*, i32, i32, i32*, i64*, i64*, i64*, i64, i64)

; Function Attrs: nounwind
declare void @__kmpc_for_static_fini(%struct.ident_t*, i32) #5

; Function Attrs: convergent nounwind
declare void @__kmpc_barrier(%struct.ident_t*, i32) #6

; Function Attrs: noinline norecurse nounwind uwtable
define internal void @.omp_outlined.(i32* noalias %0, i32* noalias %1, i64* nonnull align 8 dereferenceable(8) %2, i32** nonnull align 8 dereferenceable(8) %3, i32** nonnull align 8 dereferenceable(8) %4, i64* nonnull align 8 dereferenceable(8) %5) #8 !dbg !287 {
  %7 = alloca i64, align 8
  %8 = alloca i64, align 8
  %9 = alloca i64, align 8
  %10 = alloca i64, align 8
  %11 = alloca i64, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  call void @llvm.dbg.value(metadata i32* %0, metadata !296, metadata !DIExpression()), !dbg !297
  call void @llvm.dbg.value(metadata i32* %1, metadata !298, metadata !DIExpression()), !dbg !297
  call void @llvm.dbg.value(metadata i64* %2, metadata !299, metadata !DIExpression()), !dbg !297
  call void @llvm.dbg.value(metadata i32** %3, metadata !300, metadata !DIExpression()), !dbg !297
  call void @llvm.dbg.value(metadata i32** %4, metadata !301, metadata !DIExpression()), !dbg !297
  call void @llvm.dbg.value(metadata i64* %5, metadata !302, metadata !DIExpression()), !dbg !297
  call void @llvm.experimental.noalias.scope.decl(metadata !303), !dbg !306
  %14 = bitcast i64* %7 to i8*, !dbg !307
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %14), !dbg !307
  %15 = bitcast i64* %8 to i8*, !dbg !307
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %15), !dbg !307
  %16 = bitcast i64* %9 to i8*, !dbg !307
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %16), !dbg !307
  %17 = bitcast i64* %10 to i8*, !dbg !307
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %17), !dbg !307
  %18 = bitcast i64* %11 to i8*, !dbg !307
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %18), !dbg !307
  %19 = bitcast i64* %12 to i8*, !dbg !307
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %19), !dbg !307
  %20 = bitcast i32* %13 to i8*, !dbg !307
  call void @llvm.lifetime.start.p0i8(i64 4, i8* %20), !dbg !307
  call void @llvm.dbg.value(metadata i32* %0, metadata !310, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.value(metadata i32* %1, metadata !311, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.value(metadata i64* %2, metadata !312, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.value(metadata i32** %3, metadata !313, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.value(metadata i32** %4, metadata !314, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.value(metadata i64* %5, metadata !315, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.declare(metadata i64* %7, metadata !316, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.declare(metadata i64* %8, metadata !317, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.declare(metadata i64* %9, metadata !318, metadata !DIExpression()) #5, !dbg !307
  store i64 1, i64* %7, align 8, !dbg !319, !noalias !303
  br label %21, !dbg !322

21:                                               ; preds = %.aftr.bar, %6
  %22 = load i64, i64* %7, align 8, !dbg !323, !noalias !303
  %23 = load i64, i64* %2, align 8, !dbg !325, !noalias !303
  %24 = icmp sle i64 %22, %23, !dbg !326
  br i1 %24, label %25, label %.omp_outlined._debug__.exit, !dbg !327

25:                                               ; preds = %21
  %26 = load i64, i64* %7, align 8, !dbg !328, !noalias !303
  call void @llvm.dbg.value(metadata i64 %26, metadata !94, metadata !DIExpression()) #5, !dbg !330
  %27 = load i64, i64* @m, align 8, !dbg !332, !noalias !303
  %28 = icmp slt i64 %26, %27, !dbg !333
  %29 = load i64, i64* @n, align 8, !noalias !303
  %30 = icmp slt i64 %26, %29
  %or.cond.i.i = and i1 %28, %30, !dbg !334
  br i1 %or.cond.i.i, label %nElement.exit.i, label %31, !dbg !334

31:                                               ; preds = %25
  %32 = load i64, i64* @m, align 8, !dbg !335, !noalias !303
  %33 = load i64, i64* @n, align 8, !dbg !335, !noalias !303
  %34 = icmp sgt i64 %32, %33, !dbg !335
  %35 = load i64, i64* @m, align 8, !dbg !335, !noalias !303
  %36 = load i64, i64* @n, align 8, !dbg !335, !noalias !303
  %37 = select i1 %34, i64 %35, i64 %36, !dbg !335
  %38 = icmp slt i64 %26, %37, !dbg !336
  br i1 %38, label %39, label %47, !dbg !337

39:                                               ; preds = %31
  %40 = load i64, i64* @m, align 8, !dbg !338, !noalias !303
  %41 = load i64, i64* @n, align 8, !dbg !338, !noalias !303
  %42 = icmp slt i64 %40, %41, !dbg !338
  %43 = load i64, i64* @m, align 8, !dbg !338, !noalias !303
  %44 = load i64, i64* @n, align 8, !dbg !338, !noalias !303
  %45 = select i1 %42, i64 %43, i64 %44, !dbg !338
  call void @llvm.dbg.value(metadata i64 %45, metadata !106, metadata !DIExpression()) #5, !dbg !339
  %46 = sub nsw i64 %45, 1, !dbg !340
  br label %nElement.exit.i, !dbg !341

47:                                               ; preds = %31
  %48 = load i64, i64* @m, align 8, !dbg !342, !noalias !303
  %49 = load i64, i64* @n, align 8, !dbg !342, !noalias !303
  %50 = icmp slt i64 %48, %49, !dbg !342
  %51 = load i64, i64* @m, align 8, !dbg !342, !noalias !303
  %52 = load i64, i64* @n, align 8, !dbg !342, !noalias !303
  %53 = select i1 %50, i64 %51, i64 %52, !dbg !342
  call void @llvm.dbg.value(metadata i64 %53, metadata !112, metadata !DIExpression()) #5, !dbg !343
  %54 = mul nsw i64 2, %53, !dbg !344
  %55 = sub nsw i64 %54, %26, !dbg !345
  %56 = load i64, i64* @m, align 8, !dbg !346, !noalias !303
  %57 = load i64, i64* @n, align 8, !dbg !347, !noalias !303
  %58 = sub nsw i64 %56, %57, !dbg !348
  %59 = call i64 @labs(i64 %58) #11, !dbg !349
  %60 = add nsw i64 %55, %59, !dbg !350
  %61 = sub nsw i64 %60, 2, !dbg !351
  br label %nElement.exit.i, !dbg !352

nElement.exit.i:                                  ; preds = %47, %39, %25
  %.0.i.i = phi i64 [ %46, %39 ], [ %61, %47 ], [ %26, %25 ], !dbg !353
  call void @llvm.dbg.value(metadata i64 %.0.i.i, metadata !354, metadata !DIExpression()) #5, !dbg !307
  call void @llvm.dbg.value(metadata i64* %7, metadata !202, metadata !DIExpression()) #5, !dbg !355
  call void @llvm.dbg.value(metadata i64* %8, metadata !204, metadata !DIExpression()) #5, !dbg !355
  call void @llvm.dbg.value(metadata i64* %9, metadata !205, metadata !DIExpression()) #5, !dbg !355
  %62 = load i64, i64* %7, align 8, !dbg !357, !noalias !303
  %63 = load i64, i64* @n, align 8, !dbg !358, !noalias !303
  %64 = icmp slt i64 %62, %63, !dbg !359
  br i1 %64, label %65, label %67, !dbg !360

65:                                               ; preds = %nElement.exit.i
  %66 = load i64, i64* %7, align 8, !dbg !361, !noalias !303
  store i64 %66, i64* %8, align 8, !dbg !362, !noalias !303
  store i64 1, i64* %9, align 8, !dbg !363, !noalias !303
  br label %calcFirstDiagElement.exit.i, !dbg !364

67:                                               ; preds = %nElement.exit.i
  %68 = load i64, i64* @n, align 8, !dbg !365, !noalias !303
  %69 = sub nsw i64 %68, 1, !dbg !366
  store i64 %69, i64* %8, align 8, !dbg !367, !noalias !303
  %70 = load i64, i64* %7, align 8, !dbg !368, !noalias !303
  %71 = load i64, i64* @n, align 8, !dbg !369, !noalias !303
  %72 = sub nsw i64 %70, %71, !dbg !370
  %73 = add nsw i64 %72, 2, !dbg !371
  store i64 %73, i64* %9, align 8, !dbg !372, !noalias !303
  br label %calcFirstDiagElement.exit.i

calcFirstDiagElement.exit.i:                      ; preds = %67, %65
  call void @llvm.dbg.value(metadata i64 %.0.i.i, metadata !373, metadata !DIExpression()) #5, !dbg !375
  %74 = sub nsw i64 %.0.i.i, 1, !dbg !376
  call void @llvm.dbg.value(metadata i64 %74, metadata !373, metadata !DIExpression()) #5, !dbg !375
  call void @llvm.dbg.value(metadata i64 1, metadata !377, metadata !DIExpression()) #5, !dbg !375
  %75 = icmp sle i64 1, %.0.i.i, !dbg !376
  br i1 %75, label %76, label %131, !dbg !378

76:                                               ; preds = %calcFirstDiagElement.exit.i
  call void @llvm.dbg.declare(metadata i64* %10, metadata !379, metadata !DIExpression()) #5, !dbg !375
  store i64 0, i64* %10, align 8, !dbg !380, !noalias !303
  call void @llvm.dbg.declare(metadata i64* %11, metadata !381, metadata !DIExpression()) #5, !dbg !375
  store i64 %74, i64* %11, align 8, !dbg !380, !noalias !303
  call void @llvm.dbg.declare(metadata i64* %12, metadata !382, metadata !DIExpression()) #5, !dbg !375
  store i64 1, i64* %12, align 8, !dbg !380, !noalias !303
  call void @llvm.dbg.declare(metadata i32* %13, metadata !383, metadata !DIExpression()) #5, !dbg !375
  store i32 0, i32* %13, align 4, !dbg !380, !noalias !303
  %77 = load i32, i32* %0, align 4, !dbg !378, !alias.scope !303
  call void @__kmpc_for_static_init_8(%struct.ident_t* @3, i32 %77, i32 34, i32* %13, i64* %10, i64* %11, i64* %12, i64 1, i64 1) #5, !dbg !384, !noalias !303
  %78 = load i64, i64* %11, align 8, !dbg !380, !noalias !303
  %79 = icmp sgt i64 %78, %74, !dbg !380
  %80 = load i64, i64* %11, align 8, !dbg !380, !noalias !303
  %81 = select i1 %79, i64 %74, i64 %80, !dbg !380
  store i64 %81, i64* %11, align 8, !dbg !380, !noalias !303
  %82 = load i64, i64* %10, align 8, !dbg !380, !noalias !303
  call void @llvm.dbg.value(metadata i64 %82, metadata !385, metadata !DIExpression()) #5, !dbg !375
  br label %83, !dbg !378

83:                                               ; preds = %similarityScore.exit.i, %76
  %.0.i = phi i64 [ %82, %76 ], [ %128, %similarityScore.exit.i ], !dbg !375
  call void @llvm.dbg.value(metadata i64 %.0.i, metadata !385, metadata !DIExpression()) #5, !dbg !375
  %84 = load i64, i64* %11, align 8, !dbg !380, !noalias !303
  %85 = icmp sle i64 %.0.i, %84, !dbg !376
  br i1 %85, label %86, label %129, !dbg !378

86:                                               ; preds = %83
  %87 = add nsw i64 1, %.0.i, !dbg !386
  call void @llvm.dbg.value(metadata i64 %87, metadata !377, metadata !DIExpression()) #5, !dbg !375
  %88 = load i64, i64* %8, align 8, !dbg !387, !noalias !303
  %89 = sub nsw i64 %88, %87, !dbg !389
  %90 = add nsw i64 %89, 1, !dbg !390
  call void @llvm.dbg.value(metadata i64 %90, metadata !391, metadata !DIExpression()) #5, !dbg !307
  %91 = load i64, i64* %9, align 8, !dbg !392, !noalias !303
  %92 = add nsw i64 %91, %87, !dbg !393
  %93 = sub nsw i64 %92, 1, !dbg !394
  call void @llvm.dbg.value(metadata i64 %93, metadata !395, metadata !DIExpression()) #5, !dbg !307
  %94 = load i32*, i32** %3, align 8, !dbg !396, !noalias !303
  %95 = load i32*, i32** %4, align 8, !dbg !397, !noalias !303
  %96 = call i32 @__kmpc_global_thread_num(%struct.ident_t* @1) #5, !noalias !303
  call void @llvm.dbg.value(metadata i64 %90, metadata !144, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i64 %93, metadata !146, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i32* %94, metadata !147, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i32* %95, metadata !148, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i64* %5, metadata !149, metadata !DIExpression()) #5, !dbg !398
  %97 = load i64, i64* @m, align 8, !dbg !400, !noalias !303
  %98 = mul nsw i64 %97, %90, !dbg !401
  %99 = add nsw i64 %98, %93, !dbg !402
  call void @llvm.dbg.value(metadata i64 %99, metadata !153, metadata !DIExpression()) #5, !dbg !398
  %100 = load i64, i64* @m, align 8, !dbg !403, !noalias !303
  %101 = sub nsw i64 %99, %100, !dbg !404
  %102 = getelementptr inbounds i32, i32* %94, i64 %101, !dbg !405
  %103 = load i32, i32* %102, align 4, !dbg !405, !noalias !303
  %104 = load i32, i32* @gapScore, align 4, !dbg !406, !noalias !303
  %105 = add nsw i32 %103, %104, !dbg !407
  call void @llvm.dbg.value(metadata i32 %105, metadata !159, metadata !DIExpression()) #5, !dbg !398
  %106 = sub nsw i64 %99, 1, !dbg !408
  %107 = getelementptr inbounds i32, i32* %94, i64 %106, !dbg !409
  %108 = load i32, i32* %107, align 4, !dbg !409, !noalias !303
  %109 = load i32, i32* @gapScore, align 4, !dbg !410, !noalias !303
  %110 = add nsw i32 %108, %109, !dbg !411
  call void @llvm.dbg.value(metadata i32 %110, metadata !164, metadata !DIExpression()) #5, !dbg !398
  %111 = load i64, i64* @m, align 8, !dbg !412, !noalias !303
  %112 = sub nsw i64 %99, %111, !dbg !413
  %113 = sub nsw i64 %112, 1, !dbg !414
  %114 = getelementptr inbounds i32, i32* %94, i64 %113, !dbg !415
  %115 = load i32, i32* %114, align 4, !dbg !415, !noalias !303
  %116 = call i32 @matchMissmatchScore(i64 %90, i64 %93) #5, !dbg !416, !noalias !303
  %117 = add nsw i32 %115, %116, !dbg !417
  call void @llvm.dbg.value(metadata i32 %117, metadata !171, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i32 0, metadata !172, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i32 0, metadata !173, metadata !DIExpression()) #5, !dbg !398
  %118 = icmp sgt i32 %117, 0, !dbg !418
  %spec.select.i.i = select i1 %118, i32 %117, i32 0, !dbg !419
  %spec.select4.i.i = select i1 %118, i32 3, i32 0, !dbg !419
  call void @llvm.dbg.value(metadata i32 %spec.select4.i.i, metadata !173, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i32 %spec.select.i.i, metadata !172, metadata !DIExpression()) #5, !dbg !398
  %119 = icmp sgt i32 %105, %spec.select.i.i, !dbg !420
  %.12.i.i = select i1 %119, i32 %105, i32 %spec.select.i.i, !dbg !421
  %.1.i.i = select i1 %119, i32 1, i32 %spec.select4.i.i, !dbg !421
  call void @llvm.dbg.value(metadata i32 %.1.i.i, metadata !173, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i32 %.12.i.i, metadata !172, metadata !DIExpression()) #5, !dbg !398
  %120 = icmp sgt i32 %110, %.12.i.i, !dbg !422
  %spec.select5.i.i = select i1 %120, i32 %110, i32 %.12.i.i, !dbg !423
  %spec.select6.i.i = select i1 %120, i32 2, i32 %.1.i.i, !dbg !423
  call void @llvm.dbg.value(metadata i32 %spec.select6.i.i, metadata !173, metadata !DIExpression()) #5, !dbg !398
  call void @llvm.dbg.value(metadata i32 %spec.select5.i.i, metadata !172, metadata !DIExpression()) #5, !dbg !398
  %121 = getelementptr inbounds i32, i32* %94, i64 %99, !dbg !424
  store i32 %spec.select5.i.i, i32* %121, align 4, !dbg !425, !noalias !303
  %122 = getelementptr inbounds i32, i32* %95, i64 %99, !dbg !426
  store i32 %spec.select6.i.i, i32* %122, align 4, !dbg !427, !noalias !303
  %123 = load i64, i64* %5, align 8, !dbg !428, !noalias !303
  %124 = getelementptr inbounds i32, i32* %94, i64 %123, !dbg !429
  %125 = load i32, i32* %124, align 4, !dbg !429, !noalias !303
  %126 = icmp sgt i32 %spec.select5.i.i, %125, !dbg !430
  br i1 %126, label %127, label %similarityScore.exit.i, !dbg !431

127:                                              ; preds = %86
  br label %.crt.strt, !dbg !432

.crt.strt:                                        ; preds = %127
  call void @__kmpc_critical(%struct.ident_t* @1, i32 %96, [8 x i32]* @.gomp_critical_user_.var) #5, !dbg !432, !noalias !303
  store i64 %99, i64* %5, align 8, !dbg !433, !noalias !303
  call void @__kmpc_end_critical(%struct.ident_t* @1, i32 %96, [8 x i32]* @.gomp_critical_user_.var) #5, !dbg !434, !noalias !303
  br label %.crt.strt.crt.end, !dbg !435

.crt.strt.crt.end:                                ; preds = %.crt.strt
  br label %similarityScore.exit.i, !dbg !435

similarityScore.exit.i:                           ; preds = %.crt.strt.crt.end, %86
  %128 = add nsw i64 %.0.i, 1, !dbg !376
  call void @llvm.dbg.value(metadata i64 %128, metadata !385, metadata !DIExpression()) #5, !dbg !375
  br label %83, !dbg !384, !llvm.loop !436

129:                                              ; preds = %83
  %130 = load i32, i32* %0, align 4, !dbg !384, !alias.scope !303
  call void @__kmpc_for_static_fini(%struct.ident_t* @5, i32 %130) #5, !dbg !437, !noalias !303
  br label %131, !dbg !384

131:                                              ; preds = %129, %calcFirstDiagElement.exit.i
  %132 = load i32, i32* %0, align 4, !dbg !437, !alias.scope !303
  br label %.bar, !dbg !437

.bar:                                             ; preds = %131
  call void @__kmpc_barrier(%struct.ident_t* @6, i32 %132) #5, !dbg !437, !noalias !303
  br label %.aftr.bar, !dbg !438

.aftr.bar:                                        ; preds = %.bar
  %133 = load i64, i64* %7, align 8, !dbg !438, !noalias !303
  %134 = add nsw i64 %133, 1, !dbg !438
  store i64 %134, i64* %7, align 8, !dbg !438, !noalias !303
  br label %21, !dbg !439, !llvm.loop !440

.omp_outlined._debug__.exit:                      ; preds = %21
  %135 = bitcast i64* %7 to i8*, !dbg !442
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %135), !dbg !442
  %136 = bitcast i64* %8 to i8*, !dbg !442
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %136), !dbg !442
  %137 = bitcast i64* %9 to i8*, !dbg !442
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %137), !dbg !442
  %138 = bitcast i64* %10 to i8*, !dbg !442
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %138), !dbg !442
  %139 = bitcast i64* %11 to i8*, !dbg !442
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %139), !dbg !442
  %140 = bitcast i64* %12 to i8*, !dbg !442
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %140), !dbg !442
  %141 = bitcast i32* %13 to i8*, !dbg !442
  call void @llvm.lifetime.end.p0i8(i64 4, i8* %141), !dbg !442
  br label %.omp_outlined._debug__.exit.par.end, !dbg !306

.omp_outlined._debug__.exit.par.end:              ; preds = %.omp_outlined._debug__.exit
  ret void, !dbg !306
}

; Function Attrs: nounwind
declare !callback !443 void @__kmpc_fork_call(%struct.ident_t*, i32, void (i32*, i32*, ...)*, ...) #5

; Function Attrs: nofree nosync nounwind readnone speculatable willreturn
declare void @llvm.dbg.value(metadata, metadata, metadata) #2

; Function Attrs: inaccessiblememonly nofree nosync nounwind willreturn
declare void @llvm.experimental.noalias.scope.decl(metadata) #9

; Function Attrs: argmemonly nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0i8(i64 immarg, i8* nocapture) #10

; Function Attrs: argmemonly nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0i8(i64 immarg, i8* nocapture) #10

attributes #0 = { noinline nounwind uwtable "disable-tail-calls"="false" "frame-pointer"="all" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #1 = { nounwind "disable-tail-calls"="false" "frame-pointer"="all" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #2 = { nofree nosync nounwind readnone speculatable willreturn }
attributes #3 = { alwaysinline nounwind uwtable "disable-tail-calls"="false" "frame-pointer"="all" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #4 = { nounwind readnone "disable-tail-calls"="false" "frame-pointer"="all" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #5 = { nounwind }
attributes #6 = { convergent nounwind }
attributes #7 = { "disable-tail-calls"="false" "frame-pointer"="all" "less-precise-fpmad"="false" "no-infs-fp-math"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #8 = { noinline norecurse nounwind uwtable "disable-tail-calls"="false" "frame-pointer"="all" "less-precise-fpmad"="false" "min-legal-vector-width"="0" "no-infs-fp-math"="false" "no-jump-tables"="false" "no-nans-fp-math"="false" "no-signed-zeros-fp-math"="false" "no-trapping-math"="true" "stack-protector-buffer-size"="8" "target-cpu"="x86-64" "target-features"="+cx8,+fxsr,+mmx,+sse,+sse2,+x87" "tune-cpu"="generic" "unsafe-fp-math"="false" "use-soft-float"="false" }
attributes #9 = { inaccessiblememonly nofree nosync nounwind willreturn }
attributes #10 = { argmemonly nofree nosync nounwind willreturn }
attributes #11 = { nounwind readnone }

!llvm.dbg.cu = !{!2}
!llvm.module.flags = !{!23, !24, !25}
!llvm.ident = !{!26}

!0 = !DIGlobalVariableExpression(var: !1, expr: !DIExpression())
!1 = distinct !DIGlobalVariable(name: "m", scope: !2, file: !8, line: 49, type: !9, isLocal: false, isDefinition: true)
!2 = distinct !DICompileUnit(language: DW_LANG_C99, file: !3, producer: "clang version 12.0.0 (https://github.com/cs17resch01003/llov 661eb82c8f6162ba4730375b672b9fd42a09b6f6)", isOptimized: false, runtimeVersion: 0, emissionKind: FullDebug, enums: !4, globals: !5, splitDebugInlining: false, nameTableKind: None)
!3 = !DIFile(filename: "/datadrive/llor/testsuite/dataracebench/DRB181-SmithWaterman-yes.c", directory: "/datadrive/llor/src/Repair")
!4 = !{}
!5 = !{!0, !6, !10, !14, !16, !19, !21}
!6 = !DIGlobalVariableExpression(var: !7, expr: !DIExpression())
!7 = distinct !DIGlobalVariable(name: "n", scope: !2, file: !8, line: 50, type: !9, isLocal: false, isDefinition: true)
!8 = !DIFile(filename: "testsuite/dataracebench/DRB181-SmithWaterman-yes.c", directory: "/datadrive/llor")
!9 = !DIBasicType(name: "long long int", size: 64, encoding: DW_ATE_signed)
!10 = !DIGlobalVariableExpression(var: !11, expr: !DIExpression())
!11 = distinct !DIGlobalVariable(name: "a", scope: !2, file: !8, line: 58, type: !12, isLocal: false, isDefinition: true)
!12 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !13, size: 64)
!13 = !DIBasicType(name: "char", size: 8, encoding: DW_ATE_signed_char)
!14 = !DIGlobalVariableExpression(var: !15, expr: !DIExpression())
!15 = distinct !DIGlobalVariable(name: "b", scope: !2, file: !8, line: 58, type: !12, isLocal: false, isDefinition: true)
!16 = !DIGlobalVariableExpression(var: !17, expr: !DIExpression())
!17 = distinct !DIGlobalVariable(name: "matchScore", scope: !2, file: !8, line: 53, type: !18, isLocal: false, isDefinition: true)
!18 = !DIBasicType(name: "int", size: 32, encoding: DW_ATE_signed)
!19 = !DIGlobalVariableExpression(var: !20, expr: !DIExpression())
!20 = distinct !DIGlobalVariable(name: "missmatchScore", scope: !2, file: !8, line: 54, type: !18, isLocal: false, isDefinition: true)
!21 = !DIGlobalVariableExpression(var: !22, expr: !DIExpression())
!22 = distinct !DIGlobalVariable(name: "gapScore", scope: !2, file: !8, line: 55, type: !18, isLocal: false, isDefinition: true)
!23 = !{i32 7, !"Dwarf Version", i32 4}
!24 = !{i32 2, !"Debug Info Version", i32 3}
!25 = !{i32 1, !"wchar_size", i32 4}
!26 = !{!"clang version 12.0.0 (https://github.com/cs17resch01003/llov 661eb82c8f6162ba4730375b672b9fd42a09b6f6)"}
!27 = distinct !DISubprogram(name: "generate", scope: !8, file: !8, line: 61, type: !28, scopeLine: 61, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!28 = !DISubroutineType(types: !29)
!29 = !{null}
!30 = !DILocation(line: 63, column: 11, scope: !27)
!31 = !DILocation(line: 63, column: 5, scope: !27)
!32 = !DILocalVariable(name: "i", scope: !27, file: !8, line: 66, type: !9)
!33 = !DILocation(line: 0, scope: !27)
!34 = !DILocation(line: 67, column: 10, scope: !35)
!35 = distinct !DILexicalBlock(scope: !27, file: !8, line: 67, column: 5)
!36 = !DILocation(line: 0, scope: !35)
!37 = !DILocation(line: 67, column: 21, scope: !38)
!38 = distinct !DILexicalBlock(scope: !35, file: !8, line: 67, column: 5)
!39 = !DILocation(line: 67, column: 19, scope: !38)
!40 = !DILocation(line: 67, column: 5, scope: !35)
!41 = !DILocation(line: 68, column: 19, scope: !42)
!42 = distinct !DILexicalBlock(scope: !38, file: !8, line: 67, column: 29)
!43 = !DILocation(line: 68, column: 26, scope: !42)
!44 = !DILocalVariable(name: "aux", scope: !42, file: !8, line: 68, type: !18)
!45 = !DILocation(line: 0, scope: !42)
!46 = !DILocation(line: 69, column: 13, scope: !42)
!47 = !DILocation(line: 70, column: 13, scope: !48)
!48 = distinct !DILexicalBlock(scope: !42, file: !8, line: 69, column: 13)
!49 = !DILocation(line: 70, column: 18, scope: !48)
!50 = !DILocation(line: 72, column: 13, scope: !51)
!51 = distinct !DILexicalBlock(scope: !48, file: !8, line: 71, column: 18)
!52 = !DILocation(line: 72, column: 18, scope: !51)
!53 = !DILocation(line: 74, column: 13, scope: !54)
!54 = distinct !DILexicalBlock(scope: !51, file: !8, line: 73, column: 18)
!55 = !DILocation(line: 74, column: 18, scope: !54)
!56 = !DILocation(line: 76, column: 13, scope: !54)
!57 = !DILocation(line: 76, column: 18, scope: !54)
!58 = !DILocation(line: 67, column: 25, scope: !38)
!59 = !DILocation(line: 67, column: 5, scope: !38)
!60 = distinct !{!60, !40, !61, !62}
!61 = !DILocation(line: 77, column: 5, scope: !35)
!62 = !{!"llvm.loop.mustprogress"}
!63 = !DILocation(line: 0, scope: !64)
!64 = distinct !DILexicalBlock(scope: !27, file: !8, line: 80, column: 5)
!65 = !DILocation(line: 80, column: 21, scope: !66)
!66 = distinct !DILexicalBlock(scope: !64, file: !8, line: 80, column: 5)
!67 = !DILocation(line: 80, column: 19, scope: !66)
!68 = !DILocation(line: 80, column: 5, scope: !64)
!69 = !DILocation(line: 81, column: 19, scope: !70)
!70 = distinct !DILexicalBlock(scope: !66, file: !8, line: 80, column: 29)
!71 = !DILocation(line: 81, column: 26, scope: !70)
!72 = !DILocalVariable(name: "aux", scope: !70, file: !8, line: 81, type: !18)
!73 = !DILocation(line: 0, scope: !70)
!74 = !DILocation(line: 82, column: 13, scope: !70)
!75 = !DILocation(line: 83, column: 13, scope: !76)
!76 = distinct !DILexicalBlock(scope: !70, file: !8, line: 82, column: 13)
!77 = !DILocation(line: 83, column: 18, scope: !76)
!78 = !DILocation(line: 85, column: 13, scope: !79)
!79 = distinct !DILexicalBlock(scope: !76, file: !8, line: 84, column: 18)
!80 = !DILocation(line: 85, column: 18, scope: !79)
!81 = !DILocation(line: 87, column: 13, scope: !82)
!82 = distinct !DILexicalBlock(scope: !79, file: !8, line: 86, column: 18)
!83 = !DILocation(line: 87, column: 18, scope: !82)
!84 = !DILocation(line: 89, column: 13, scope: !82)
!85 = !DILocation(line: 89, column: 18, scope: !82)
!86 = !DILocation(line: 80, column: 25, scope: !66)
!87 = !DILocation(line: 80, column: 5, scope: !66)
!88 = distinct !{!88, !68, !89, !62}
!89 = !DILocation(line: 90, column: 5, scope: !64)
!90 = !DILocation(line: 91, column: 1, scope: !27)
!91 = distinct !DISubprogram(name: "nElement", scope: !8, file: !8, line: 98, type: !92, scopeLine: 98, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!92 = !DISubroutineType(types: !93)
!93 = !{!9, !9}
!94 = !DILocalVariable(name: "i", arg: 1, scope: !91, file: !8, line: 98, type: !9)
!95 = !DILocation(line: 0, scope: !91)
!96 = !DILocation(line: 99, column: 13, scope: !97)
!97 = distinct !DILexicalBlock(scope: !91, file: !8, line: 99, column: 9)
!98 = !DILocation(line: 99, column: 11, scope: !97)
!99 = !DILocation(line: 99, column: 15, scope: !97)
!100 = !DILocation(line: 103, column: 18, scope: !101)
!101 = distinct !DILexicalBlock(scope: !97, file: !8, line: 103, column: 14)
!102 = !DILocation(line: 103, column: 16, scope: !101)
!103 = !DILocation(line: 103, column: 14, scope: !97)
!104 = !DILocation(line: 105, column: 29, scope: !105)
!105 = distinct !DILexicalBlock(scope: !101, file: !8, line: 103, column: 29)
!106 = !DILocalVariable(name: "min", scope: !105, file: !8, line: 105, type: !9)
!107 = !DILocation(line: 0, scope: !105)
!108 = !DILocation(line: 106, column: 20, scope: !105)
!109 = !DILocation(line: 106, column: 9, scope: !105)
!110 = !DILocation(line: 110, column: 29, scope: !111)
!111 = distinct !DILexicalBlock(scope: !101, file: !8, line: 108, column: 10)
!112 = !DILocalVariable(name: "min", scope: !111, file: !8, line: 110, type: !9)
!113 = !DILocation(line: 0, scope: !111)
!114 = !DILocation(line: 111, column: 18, scope: !111)
!115 = !DILocation(line: 111, column: 24, scope: !111)
!116 = !DILocation(line: 111, column: 35, scope: !111)
!117 = !DILocation(line: 111, column: 39, scope: !111)
!118 = !DILocation(line: 111, column: 37, scope: !111)
!119 = !DILocation(line: 111, column: 30, scope: !111)
!120 = !DILocation(line: 111, column: 28, scope: !111)
!121 = !DILocation(line: 111, column: 42, scope: !111)
!122 = !DILocation(line: 111, column: 9, scope: !111)
!123 = !DILocation(line: 0, scope: !97)
!124 = !DILocation(line: 113, column: 1, scope: !91)
!125 = distinct !DISubprogram(name: "matchMissmatchScore", scope: !8, file: !8, line: 119, type: !126, scopeLine: 119, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!126 = !DISubroutineType(types: !127)
!127 = !{!18, !9, !9}
!128 = !DILocalVariable(name: "i", arg: 1, scope: !125, file: !8, line: 119, type: !9)
!129 = !DILocation(line: 0, scope: !125)
!130 = !DILocalVariable(name: "j", arg: 2, scope: !125, file: !8, line: 119, type: !9)
!131 = !DILocation(line: 120, column: 9, scope: !132)
!132 = distinct !DILexicalBlock(scope: !125, file: !8, line: 120, column: 9)
!133 = !DILocation(line: 120, column: 13, scope: !132)
!134 = !DILocation(line: 120, column: 21, scope: !132)
!135 = !DILocation(line: 120, column: 25, scope: !132)
!136 = !DILocation(line: 120, column: 18, scope: !132)
!137 = !DILocation(line: 120, column: 9, scope: !125)
!138 = !DILocation(line: 124, column: 1, scope: !125)
!139 = distinct !DISubprogram(name: "similarityScore", scope: !8, file: !8, line: 127, type: !140, scopeLine: 127, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!140 = !DISubroutineType(types: !141)
!141 = !{null, !9, !9, !142, !142, !143}
!142 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !18, size: 64)
!143 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !9, size: 64)
!144 = !DILocalVariable(name: "i", arg: 1, scope: !139, file: !8, line: 127, type: !9)
!145 = !DILocation(line: 0, scope: !139)
!146 = !DILocalVariable(name: "j", arg: 2, scope: !139, file: !8, line: 127, type: !9)
!147 = !DILocalVariable(name: "H", arg: 3, scope: !139, file: !8, line: 127, type: !142)
!148 = !DILocalVariable(name: "P", arg: 4, scope: !139, file: !8, line: 127, type: !142)
!149 = !DILocalVariable(name: "maxPos", arg: 5, scope: !139, file: !8, line: 127, type: !143)
!150 = !DILocation(line: 132, column: 27, scope: !139)
!151 = !DILocation(line: 132, column: 29, scope: !139)
!152 = !DILocation(line: 132, column: 33, scope: !139)
!153 = !DILocalVariable(name: "index", scope: !139, file: !8, line: 132, type: !9)
!154 = !DILocation(line: 135, column: 20, scope: !139)
!155 = !DILocation(line: 135, column: 18, scope: !139)
!156 = !DILocation(line: 135, column: 10, scope: !139)
!157 = !DILocation(line: 135, column: 25, scope: !139)
!158 = !DILocation(line: 135, column: 23, scope: !139)
!159 = !DILocalVariable(name: "up", scope: !139, file: !8, line: 129, type: !18)
!160 = !DILocation(line: 138, column: 20, scope: !139)
!161 = !DILocation(line: 138, column: 12, scope: !139)
!162 = !DILocation(line: 138, column: 27, scope: !139)
!163 = !DILocation(line: 138, column: 25, scope: !139)
!164 = !DILocalVariable(name: "left", scope: !139, file: !8, line: 129, type: !18)
!165 = !DILocation(line: 141, column: 22, scope: !139)
!166 = !DILocation(line: 141, column: 20, scope: !139)
!167 = !DILocation(line: 141, column: 24, scope: !139)
!168 = !DILocation(line: 141, column: 12, scope: !139)
!169 = !DILocation(line: 141, column: 31, scope: !139)
!170 = !DILocation(line: 141, column: 29, scope: !139)
!171 = !DILocalVariable(name: "diag", scope: !139, file: !8, line: 129, type: !18)
!172 = !DILocalVariable(name: "max", scope: !139, file: !8, line: 144, type: !18)
!173 = !DILocalVariable(name: "pred", scope: !139, file: !8, line: 145, type: !18)
!174 = !DILocation(line: 161, column: 14, scope: !175)
!175 = distinct !DILexicalBlock(scope: !139, file: !8, line: 161, column: 9)
!176 = !DILocation(line: 161, column: 9, scope: !139)
!177 = !DILocation(line: 166, column: 12, scope: !178)
!178 = distinct !DILexicalBlock(scope: !139, file: !8, line: 166, column: 9)
!179 = !DILocation(line: 166, column: 9, scope: !139)
!180 = !DILocation(line: 171, column: 14, scope: !181)
!181 = distinct !DILexicalBlock(scope: !139, file: !8, line: 171, column: 9)
!182 = !DILocation(line: 171, column: 9, scope: !139)
!183 = !DILocation(line: 176, column: 5, scope: !139)
!184 = !DILocation(line: 176, column: 14, scope: !139)
!185 = !DILocation(line: 177, column: 5, scope: !139)
!186 = !DILocation(line: 177, column: 14, scope: !139)
!187 = !DILocation(line: 180, column: 17, scope: !188)
!188 = distinct !DILexicalBlock(scope: !139, file: !8, line: 180, column: 9)
!189 = !DILocation(line: 180, column: 15, scope: !188)
!190 = !DILocation(line: 180, column: 13, scope: !188)
!191 = !DILocation(line: 180, column: 9, scope: !139)
!192 = !DILocation(line: 181, column: 5, scope: !193)
!193 = distinct !DILexicalBlock(scope: !194, file: !8, line: 181, column: 5)
!194 = distinct !DILexicalBlock(scope: !188, file: !8, line: 180, column: 27)
!195 = !DILocation(line: 182, column: 17, scope: !193)
!196 = !DILocation(line: 182, column: 9, scope: !193)
!197 = !DILocation(line: 183, column: 5, scope: !194)
!198 = !DILocation(line: 185, column: 1, scope: !139)
!199 = distinct !DISubprogram(name: "calcFirstDiagElement", scope: !8, file: !8, line: 191, type: !200, scopeLine: 191, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!200 = !DISubroutineType(types: !201)
!201 = !{null, !143, !143, !143}
!202 = !DILocalVariable(name: "i", arg: 1, scope: !199, file: !8, line: 191, type: !143)
!203 = !DILocation(line: 0, scope: !199)
!204 = !DILocalVariable(name: "si", arg: 2, scope: !199, file: !8, line: 191, type: !143)
!205 = !DILocalVariable(name: "sj", arg: 3, scope: !199, file: !8, line: 191, type: !143)
!206 = !DILocation(line: 193, column: 9, scope: !207)
!207 = distinct !DILexicalBlock(scope: !199, file: !8, line: 193, column: 9)
!208 = !DILocation(line: 193, column: 14, scope: !207)
!209 = !DILocation(line: 193, column: 12, scope: !207)
!210 = !DILocation(line: 193, column: 9, scope: !199)
!211 = !DILocation(line: 194, column: 15, scope: !212)
!212 = distinct !DILexicalBlock(scope: !207, file: !8, line: 193, column: 17)
!213 = !DILocation(line: 194, column: 13, scope: !212)
!214 = !DILocation(line: 195, column: 13, scope: !212)
!215 = !DILocation(line: 196, column: 5, scope: !212)
!216 = !DILocation(line: 197, column: 15, scope: !217)
!217 = distinct !DILexicalBlock(scope: !207, file: !8, line: 196, column: 12)
!218 = !DILocation(line: 197, column: 17, scope: !217)
!219 = !DILocation(line: 197, column: 13, scope: !217)
!220 = !DILocation(line: 198, column: 15, scope: !217)
!221 = !DILocation(line: 198, column: 20, scope: !217)
!222 = !DILocation(line: 198, column: 18, scope: !217)
!223 = !DILocation(line: 198, column: 22, scope: !217)
!224 = !DILocation(line: 198, column: 13, scope: !217)
!225 = !DILocation(line: 200, column: 1, scope: !199)
!226 = distinct !DISubprogram(name: "main", scope: !8, file: !8, line: 203, type: !227, scopeLine: 203, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!227 = !DISubroutineType(types: !228)
!228 = !{!18, !18, !229}
!229 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !12, size: 64)
!230 = !DILocalVariable(name: "argc", arg: 1, scope: !226, file: !8, line: 203, type: !18)
!231 = !DILocation(line: 0, scope: !226)
!232 = !DILocalVariable(name: "argv", arg: 2, scope: !226, file: !8, line: 203, type: !229)
!233 = !DILocation(line: 204, column: 7, scope: !226)
!234 = !DILocation(line: 205, column: 7, scope: !226)
!235 = !DILocation(line: 212, column: 16, scope: !226)
!236 = !DILocation(line: 212, column: 18, scope: !226)
!237 = !DILocation(line: 212, column: 9, scope: !226)
!238 = !DILocation(line: 212, column: 7, scope: !226)
!239 = !DILocation(line: 213, column: 16, scope: !226)
!240 = !DILocation(line: 213, column: 18, scope: !226)
!241 = !DILocation(line: 213, column: 9, scope: !226)
!242 = !DILocation(line: 213, column: 7, scope: !226)
!243 = !DILocation(line: 216, column: 6, scope: !226)
!244 = !DILocation(line: 217, column: 6, scope: !226)
!245 = !DILocalVariable(name: "H", scope: !226, file: !8, line: 220, type: !142)
!246 = !DILocation(line: 220, column: 10, scope: !226)
!247 = !DILocation(line: 221, column: 16, scope: !226)
!248 = !DILocation(line: 221, column: 20, scope: !226)
!249 = !DILocation(line: 221, column: 18, scope: !226)
!250 = !DILocation(line: 221, column: 9, scope: !226)
!251 = !DILocation(line: 221, column: 7, scope: !226)
!252 = !DILocalVariable(name: "P", scope: !226, file: !8, line: 224, type: !142)
!253 = !DILocation(line: 224, column: 10, scope: !226)
!254 = !DILocation(line: 225, column: 16, scope: !226)
!255 = !DILocation(line: 225, column: 20, scope: !226)
!256 = !DILocation(line: 225, column: 18, scope: !226)
!257 = !DILocation(line: 225, column: 9, scope: !226)
!258 = !DILocation(line: 225, column: 7, scope: !226)
!259 = !DILocation(line: 229, column: 5, scope: !226)
!260 = !DILocalVariable(name: "maxPos", scope: !226, file: !8, line: 233, type: !9)
!261 = !DILocation(line: 233, column: 19, scope: !226)
!262 = !DILocalVariable(name: "i", scope: !226, file: !8, line: 235, type: !9)
!263 = !DILocation(line: 235, column: 19, scope: !226)
!264 = !DILocalVariable(name: "j", scope: !226, file: !8, line: 235, type: !9)
!265 = !DILocation(line: 235, column: 22, scope: !226)
!266 = !DILocation(line: 238, column: 26, scope: !226)
!267 = !DILocalVariable(name: "initialTime", scope: !226, file: !8, line: 238, type: !268)
!268 = !DIBasicType(name: "double", size: 64, encoding: DW_ATE_float)
!269 = !DILocalVariable(name: "si", scope: !226, file: !8, line: 240, type: !9)
!270 = !DILocation(line: 240, column: 19, scope: !226)
!271 = !DILocalVariable(name: "sj", scope: !226, file: !8, line: 240, type: !9)
!272 = !DILocation(line: 240, column: 23, scope: !226)
!273 = !DILocalVariable(name: "ai", scope: !226, file: !8, line: 240, type: !9)
!274 = !DILocation(line: 240, column: 27, scope: !226)
!275 = !DILocalVariable(name: "aj", scope: !226, file: !8, line: 240, type: !9)
!276 = !DILocation(line: 240, column: 31, scope: !226)
!277 = !DILocalVariable(name: "nDiag", scope: !226, file: !8, line: 243, type: !9)
!278 = !DILocation(line: 243, column: 19, scope: !226)
!279 = !DILocation(line: 243, column: 27, scope: !226)
!280 = !DILocation(line: 243, column: 31, scope: !226)
!281 = !DILocation(line: 243, column: 29, scope: !226)
!282 = !DILocation(line: 243, column: 33, scope: !226)
!283 = !DILocalVariable(name: "nEle", scope: !226, file: !8, line: 244, type: !9)
!284 = !DILocation(line: 244, column: 19, scope: !226)
!285 = !DILocation(line: 246, column: 5, scope: !226)
!286 = !DILocation(line: 262, column: 1, scope: !226)
!287 = distinct !DISubprogram(name: ".omp_outlined.", scope: !8, file: !8, line: 246, type: !288, scopeLine: 246, flags: DIFlagPrototyped, spFlags: DISPFlagLocalToUnit | DISPFlagDefinition, unit: !2, retainedNodes: !4)
!288 = !DISubroutineType(types: !289)
!289 = !{null, !290, !290, !294, !295, !295, !294}
!290 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !291)
!291 = !DIDerivedType(tag: DW_TAG_restrict_type, baseType: !292)
!292 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !293, size: 64)
!293 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !18)
!294 = !DIDerivedType(tag: DW_TAG_reference_type, baseType: !9, size: 64)
!295 = !DIDerivedType(tag: DW_TAG_reference_type, baseType: !142, size: 64)
!296 = !DILocalVariable(name: ".global_tid.", arg: 1, scope: !287, type: !290, flags: DIFlagArtificial)
!297 = !DILocation(line: 0, scope: !287)
!298 = !DILocalVariable(name: ".bound_tid.", arg: 2, scope: !287, type: !290, flags: DIFlagArtificial)
!299 = !DILocalVariable(name: "nDiag", arg: 3, scope: !287, type: !294, flags: DIFlagArtificial)
!300 = !DILocalVariable(name: "H", arg: 4, scope: !287, type: !295, flags: DIFlagArtificial)
!301 = !DILocalVariable(name: "P", arg: 5, scope: !287, type: !295, flags: DIFlagArtificial)
!302 = !DILocalVariable(name: "maxPos", arg: 6, scope: !287, type: !294, flags: DIFlagArtificial)
!303 = !{!304}
!304 = distinct !{!304, !305, !".omp_outlined._debug__: argument 0"}
!305 = distinct !{!305, !".omp_outlined._debug__"}
!306 = !DILocation(line: 246, column: 5, scope: !287)
!307 = !DILocation(line: 0, scope: !308, inlinedAt: !309)
!308 = distinct !DISubprogram(name: ".omp_outlined._debug__", scope: !8, file: !8, line: 248, type: !288, scopeLine: 248, flags: DIFlagPrototyped, spFlags: DISPFlagLocalToUnit | DISPFlagDefinition, unit: !2, retainedNodes: !4)
!309 = distinct !DILocation(line: 246, column: 5, scope: !287)
!310 = !DILocalVariable(name: ".global_tid.", arg: 1, scope: !308, type: !290, flags: DIFlagArtificial)
!311 = !DILocalVariable(name: ".bound_tid.", arg: 2, scope: !308, type: !290, flags: DIFlagArtificial)
!312 = !DILocalVariable(name: "nDiag", arg: 3, scope: !308, file: !8, line: 243, type: !294)
!313 = !DILocalVariable(name: "H", arg: 4, scope: !308, file: !8, line: 220, type: !295)
!314 = !DILocalVariable(name: "P", arg: 5, scope: !308, file: !8, line: 224, type: !295)
!315 = !DILocalVariable(name: "maxPos", arg: 6, scope: !308, file: !8, line: 233, type: !294)
!316 = !DILocalVariable(name: "i", scope: !308, type: !9, flags: DIFlagArtificial)
!317 = !DILocalVariable(name: "si", scope: !308, type: !9, flags: DIFlagArtificial)
!318 = !DILocalVariable(name: "sj", scope: !308, type: !9, flags: DIFlagArtificial)
!319 = !DILocation(line: 249, column: 16, scope: !320, inlinedAt: !309)
!320 = distinct !DILexicalBlock(scope: !321, file: !8, line: 249, column: 9)
!321 = distinct !DILexicalBlock(scope: !308, file: !8, line: 248, column: 5)
!322 = !DILocation(line: 249, column: 14, scope: !320, inlinedAt: !309)
!323 = !DILocation(line: 249, column: 21, scope: !324, inlinedAt: !309)
!324 = distinct !DILexicalBlock(scope: !320, file: !8, line: 249, column: 9)
!325 = !DILocation(line: 249, column: 26, scope: !324, inlinedAt: !309)
!326 = !DILocation(line: 249, column: 23, scope: !324, inlinedAt: !309)
!327 = !DILocation(line: 249, column: 9, scope: !320, inlinedAt: !309)
!328 = !DILocation(line: 251, column: 29, scope: !329, inlinedAt: !309)
!329 = distinct !DILexicalBlock(scope: !324, file: !8, line: 250, column: 9)
!330 = !DILocation(line: 0, scope: !91, inlinedAt: !331)
!331 = distinct !DILocation(line: 251, column: 20, scope: !329, inlinedAt: !309)
!332 = !DILocation(line: 99, column: 13, scope: !97, inlinedAt: !331)
!333 = !DILocation(line: 99, column: 11, scope: !97, inlinedAt: !331)
!334 = !DILocation(line: 99, column: 15, scope: !97, inlinedAt: !331)
!335 = !DILocation(line: 103, column: 18, scope: !101, inlinedAt: !331)
!336 = !DILocation(line: 103, column: 16, scope: !101, inlinedAt: !331)
!337 = !DILocation(line: 103, column: 14, scope: !97, inlinedAt: !331)
!338 = !DILocation(line: 105, column: 29, scope: !105, inlinedAt: !331)
!339 = !DILocation(line: 0, scope: !105, inlinedAt: !331)
!340 = !DILocation(line: 106, column: 20, scope: !105, inlinedAt: !331)
!341 = !DILocation(line: 106, column: 9, scope: !105, inlinedAt: !331)
!342 = !DILocation(line: 110, column: 29, scope: !111, inlinedAt: !331)
!343 = !DILocation(line: 0, scope: !111, inlinedAt: !331)
!344 = !DILocation(line: 111, column: 18, scope: !111, inlinedAt: !331)
!345 = !DILocation(line: 111, column: 24, scope: !111, inlinedAt: !331)
!346 = !DILocation(line: 111, column: 35, scope: !111, inlinedAt: !331)
!347 = !DILocation(line: 111, column: 39, scope: !111, inlinedAt: !331)
!348 = !DILocation(line: 111, column: 37, scope: !111, inlinedAt: !331)
!349 = !DILocation(line: 111, column: 30, scope: !111, inlinedAt: !331)
!350 = !DILocation(line: 111, column: 28, scope: !111, inlinedAt: !331)
!351 = !DILocation(line: 111, column: 42, scope: !111, inlinedAt: !331)
!352 = !DILocation(line: 111, column: 9, scope: !111, inlinedAt: !331)
!353 = !DILocation(line: 0, scope: !97, inlinedAt: !331)
!354 = !DILocalVariable(name: "nEle", scope: !308, type: !9, flags: DIFlagArtificial)
!355 = !DILocation(line: 0, scope: !199, inlinedAt: !356)
!356 = distinct !DILocation(line: 252, column: 13, scope: !329, inlinedAt: !309)
!357 = !DILocation(line: 193, column: 9, scope: !207, inlinedAt: !356)
!358 = !DILocation(line: 193, column: 14, scope: !207, inlinedAt: !356)
!359 = !DILocation(line: 193, column: 12, scope: !207, inlinedAt: !356)
!360 = !DILocation(line: 193, column: 9, scope: !199, inlinedAt: !356)
!361 = !DILocation(line: 194, column: 15, scope: !212, inlinedAt: !356)
!362 = !DILocation(line: 194, column: 13, scope: !212, inlinedAt: !356)
!363 = !DILocation(line: 195, column: 13, scope: !212, inlinedAt: !356)
!364 = !DILocation(line: 196, column: 5, scope: !212, inlinedAt: !356)
!365 = !DILocation(line: 197, column: 15, scope: !217, inlinedAt: !356)
!366 = !DILocation(line: 197, column: 17, scope: !217, inlinedAt: !356)
!367 = !DILocation(line: 197, column: 13, scope: !217, inlinedAt: !356)
!368 = !DILocation(line: 198, column: 15, scope: !217, inlinedAt: !356)
!369 = !DILocation(line: 198, column: 20, scope: !217, inlinedAt: !356)
!370 = !DILocation(line: 198, column: 18, scope: !217, inlinedAt: !356)
!371 = !DILocation(line: 198, column: 22, scope: !217, inlinedAt: !356)
!372 = !DILocation(line: 198, column: 13, scope: !217, inlinedAt: !356)
!373 = !DILocalVariable(name: ".capture_expr.", scope: !374, type: !9, flags: DIFlagArtificial)
!374 = distinct !DILexicalBlock(scope: !329, file: !8, line: 253, column: 13)
!375 = !DILocation(line: 0, scope: !374, inlinedAt: !309)
!376 = !DILocation(line: 254, column: 13, scope: !374, inlinedAt: !309)
!377 = !DILocalVariable(name: "j", scope: !374, type: !9, flags: DIFlagArtificial)
!378 = !DILocation(line: 253, column: 13, scope: !329, inlinedAt: !309)
!379 = !DILocalVariable(name: ".omp.lb", scope: !374, type: !9, flags: DIFlagArtificial)
!380 = !DILocation(line: 254, column: 18, scope: !374, inlinedAt: !309)
!381 = !DILocalVariable(name: ".omp.ub", scope: !374, type: !9, flags: DIFlagArtificial)
!382 = !DILocalVariable(name: ".omp.stride", scope: !374, type: !9, flags: DIFlagArtificial)
!383 = !DILocalVariable(name: ".omp.is_last", scope: !374, type: !18, flags: DIFlagArtificial)
!384 = !DILocation(line: 253, column: 13, scope: !374, inlinedAt: !309)
!385 = !DILocalVariable(name: ".omp.iv", scope: !374, type: !9, flags: DIFlagArtificial)
!386 = !DILocation(line: 254, column: 36, scope: !374, inlinedAt: !309)
!387 = !DILocation(line: 256, column: 22, scope: !388, inlinedAt: !309)
!388 = distinct !DILexicalBlock(scope: !374, file: !8, line: 255, column: 13)
!389 = !DILocation(line: 256, column: 25, scope: !388, inlinedAt: !309)
!390 = !DILocation(line: 256, column: 29, scope: !388, inlinedAt: !309)
!391 = !DILocalVariable(name: "ai", scope: !308, type: !9, flags: DIFlagArtificial)
!392 = !DILocation(line: 257, column: 22, scope: !388, inlinedAt: !309)
!393 = !DILocation(line: 257, column: 25, scope: !388, inlinedAt: !309)
!394 = !DILocation(line: 257, column: 29, scope: !388, inlinedAt: !309)
!395 = !DILocalVariable(name: "aj", scope: !308, type: !9, flags: DIFlagArtificial)
!396 = !DILocation(line: 258, column: 41, scope: !388, inlinedAt: !309)
!397 = !DILocation(line: 258, column: 44, scope: !388, inlinedAt: !309)
!398 = !DILocation(line: 0, scope: !139, inlinedAt: !399)
!399 = distinct !DILocation(line: 258, column: 17, scope: !388, inlinedAt: !309)
!400 = !DILocation(line: 132, column: 27, scope: !139, inlinedAt: !399)
!401 = !DILocation(line: 132, column: 29, scope: !139, inlinedAt: !399)
!402 = !DILocation(line: 132, column: 33, scope: !139, inlinedAt: !399)
!403 = !DILocation(line: 135, column: 20, scope: !139, inlinedAt: !399)
!404 = !DILocation(line: 135, column: 18, scope: !139, inlinedAt: !399)
!405 = !DILocation(line: 135, column: 10, scope: !139, inlinedAt: !399)
!406 = !DILocation(line: 135, column: 25, scope: !139, inlinedAt: !399)
!407 = !DILocation(line: 135, column: 23, scope: !139, inlinedAt: !399)
!408 = !DILocation(line: 138, column: 20, scope: !139, inlinedAt: !399)
!409 = !DILocation(line: 138, column: 12, scope: !139, inlinedAt: !399)
!410 = !DILocation(line: 138, column: 27, scope: !139, inlinedAt: !399)
!411 = !DILocation(line: 138, column: 25, scope: !139, inlinedAt: !399)
!412 = !DILocation(line: 141, column: 22, scope: !139, inlinedAt: !399)
!413 = !DILocation(line: 141, column: 20, scope: !139, inlinedAt: !399)
!414 = !DILocation(line: 141, column: 24, scope: !139, inlinedAt: !399)
!415 = !DILocation(line: 141, column: 12, scope: !139, inlinedAt: !399)
!416 = !DILocation(line: 141, column: 31, scope: !139, inlinedAt: !399)
!417 = !DILocation(line: 141, column: 29, scope: !139, inlinedAt: !399)
!418 = !DILocation(line: 161, column: 14, scope: !175, inlinedAt: !399)
!419 = !DILocation(line: 161, column: 9, scope: !139, inlinedAt: !399)
!420 = !DILocation(line: 166, column: 12, scope: !178, inlinedAt: !399)
!421 = !DILocation(line: 166, column: 9, scope: !139, inlinedAt: !399)
!422 = !DILocation(line: 171, column: 14, scope: !181, inlinedAt: !399)
!423 = !DILocation(line: 171, column: 9, scope: !139, inlinedAt: !399)
!424 = !DILocation(line: 176, column: 5, scope: !139, inlinedAt: !399)
!425 = !DILocation(line: 176, column: 14, scope: !139, inlinedAt: !399)
!426 = !DILocation(line: 177, column: 5, scope: !139, inlinedAt: !399)
!427 = !DILocation(line: 177, column: 14, scope: !139, inlinedAt: !399)
!428 = !DILocation(line: 180, column: 17, scope: !188, inlinedAt: !399)
!429 = !DILocation(line: 180, column: 15, scope: !188, inlinedAt: !399)
!430 = !DILocation(line: 180, column: 13, scope: !188, inlinedAt: !399)
!431 = !DILocation(line: 180, column: 9, scope: !139, inlinedAt: !399)
!432 = !DILocation(line: 181, column: 5, scope: !193, inlinedAt: !399)
!433 = !DILocation(line: 182, column: 17, scope: !193, inlinedAt: !399)
!434 = !DILocation(line: 182, column: 9, scope: !193, inlinedAt: !399)
!435 = !DILocation(line: 183, column: 5, scope: !194, inlinedAt: !399)
!436 = distinct !{!436, !384, !437}
!437 = !DILocation(line: 253, column: 28, scope: !374, inlinedAt: !309)
!438 = !DILocation(line: 249, column: 33, scope: !324, inlinedAt: !309)
!439 = !DILocation(line: 249, column: 9, scope: !324, inlinedAt: !309)
!440 = distinct !{!440, !327, !441, !62}
!441 = !DILocation(line: 260, column: 9, scope: !320, inlinedAt: !309)
!442 = !DILocation(line: 261, column: 5, scope: !308, inlinedAt: !309)
!443 = !{!444}
!444 = !{i64 2, i64 -1, i64 -1, i1 true}
