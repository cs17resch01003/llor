; ModuleID = '/datadrive/llor/testsuite/DRB181-SmithWaterman-yes.sb.ll'
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
  %4 = load i64, i64* @m, align 8, !dbg !37, !repair !39
  %5 = icmp slt i64 %.0, %4, !dbg !40
  br i1 %5, label %6, label %23, !dbg !41

6:                                                ; preds = %3
  %7 = call i32 @rand() #5, !dbg !42
  %8 = srem i32 %7, 4, !dbg !44
  call void @llvm.dbg.value(metadata i32 %8, metadata !45, metadata !DIExpression()), !dbg !46
  switch i32 %8, label %18 [
    i32 0, label %9
    i32 2, label %12
    i32 3, label %15
  ], !dbg !47

9:                                                ; preds = %6
  %10 = load i8*, i8** @a, align 8, !dbg !48, !repair !50
  %11 = getelementptr inbounds i8, i8* %10, i64 %.0, !dbg !48
  store i8 65, i8* %11, align 1, !dbg !51
  br label %21, !dbg !48

12:                                               ; preds = %6
  %13 = load i8*, i8** @a, align 8, !dbg !52, !repair !54
  %14 = getelementptr inbounds i8, i8* %13, i64 %.0, !dbg !52
  store i8 67, i8* %14, align 1, !dbg !55
  br label %21, !dbg !52

15:                                               ; preds = %6
  %16 = load i8*, i8** @a, align 8, !dbg !56, !repair !58
  %17 = getelementptr inbounds i8, i8* %16, i64 %.0, !dbg !56
  store i8 71, i8* %17, align 1, !dbg !59
  br label %21, !dbg !56

18:                                               ; preds = %6
  %19 = load i8*, i8** @a, align 8, !dbg !60, !repair !61
  %20 = getelementptr inbounds i8, i8* %19, i64 %.0, !dbg !60
  store i8 84, i8* %20, align 1, !dbg !62
  br label %21

21:                                               ; preds = %18, %15, %12, %9
  %22 = add nsw i64 %.0, 1, !dbg !63
  call void @llvm.dbg.value(metadata i64 %22, metadata !32, metadata !DIExpression()), !dbg !33
  br label %3, !dbg !64, !llvm.loop !65

23:                                               ; preds = %41, %3
  %.1 = phi i64 [ %42, %41 ], [ 0, %3 ], !dbg !68
  call void @llvm.dbg.value(metadata i64 %.1, metadata !32, metadata !DIExpression()), !dbg !33
  %24 = load i64, i64* @n, align 8, !dbg !70, !repair !72
  %25 = icmp slt i64 %.1, %24, !dbg !73
  br i1 %25, label %26, label %43, !dbg !74

26:                                               ; preds = %23
  %27 = call i32 @rand() #5, !dbg !75
  %28 = srem i32 %27, 4, !dbg !77
  call void @llvm.dbg.value(metadata i32 %28, metadata !78, metadata !DIExpression()), !dbg !79
  switch i32 %28, label %38 [
    i32 0, label %29
    i32 2, label %32
    i32 3, label %35
  ], !dbg !80

29:                                               ; preds = %26
  %30 = load i8*, i8** @b, align 8, !dbg !81, !repair !83
  %31 = getelementptr inbounds i8, i8* %30, i64 %.1, !dbg !81
  store i8 65, i8* %31, align 1, !dbg !84
  br label %41, !dbg !81

32:                                               ; preds = %26
  %33 = load i8*, i8** @b, align 8, !dbg !85, !repair !87
  %34 = getelementptr inbounds i8, i8* %33, i64 %.1, !dbg !85
  store i8 67, i8* %34, align 1, !dbg !88
  br label %41, !dbg !85

35:                                               ; preds = %26
  %36 = load i8*, i8** @b, align 8, !dbg !89, !repair !91
  %37 = getelementptr inbounds i8, i8* %36, i64 %.1, !dbg !89
  store i8 71, i8* %37, align 1, !dbg !92
  br label %41, !dbg !89

38:                                               ; preds = %26
  %39 = load i8*, i8** @b, align 8, !dbg !93, !repair !94
  %40 = getelementptr inbounds i8, i8* %39, i64 %.1, !dbg !93
  store i8 84, i8* %40, align 1, !dbg !95
  br label %41

41:                                               ; preds = %38, %35, %32, %29
  %42 = add nsw i64 %.1, 1, !dbg !96
  call void @llvm.dbg.value(metadata i64 %42, metadata !32, metadata !DIExpression()), !dbg !33
  br label %23, !dbg !97, !llvm.loop !98

43:                                               ; preds = %23
  ret void, !dbg !100
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
define dso_local i64 @nElement(i64 %0) #3 !dbg !101 {
  call void @llvm.dbg.value(metadata i64 %0, metadata !104, metadata !DIExpression()), !dbg !105
  %2 = load i64, i64* @m, align 8, !dbg !106
  %3 = icmp slt i64 %0, %2, !dbg !108
  %4 = load i64, i64* @n, align 8
  %5 = icmp slt i64 %0, %4
  %or.cond = and i1 %3, %5, !dbg !109
  br i1 %or.cond, label %37, label %6, !dbg !109

6:                                                ; preds = %1
  %7 = load i64, i64* @m, align 8, !dbg !110
  %8 = load i64, i64* @n, align 8, !dbg !110
  %9 = icmp sgt i64 %7, %8, !dbg !110
  %10 = load i64, i64* @m, align 8, !dbg !110
  %11 = load i64, i64* @n, align 8, !dbg !110
  %12 = select i1 %9, i64 %10, i64 %11, !dbg !110
  %13 = icmp slt i64 %0, %12, !dbg !112
  br i1 %13, label %14, label %22, !dbg !113

14:                                               ; preds = %6
  %15 = load i64, i64* @m, align 8, !dbg !114
  %16 = load i64, i64* @n, align 8, !dbg !114
  %17 = icmp slt i64 %15, %16, !dbg !114
  %18 = load i64, i64* @m, align 8, !dbg !114
  %19 = load i64, i64* @n, align 8, !dbg !114
  %20 = select i1 %17, i64 %18, i64 %19, !dbg !114
  call void @llvm.dbg.value(metadata i64 %20, metadata !116, metadata !DIExpression()), !dbg !117
  %21 = sub nsw i64 %20, 1, !dbg !118
  br label %37, !dbg !119

22:                                               ; preds = %6
  %23 = load i64, i64* @m, align 8, !dbg !120
  %24 = load i64, i64* @n, align 8, !dbg !120
  %25 = icmp slt i64 %23, %24, !dbg !120
  %26 = load i64, i64* @m, align 8, !dbg !120
  %27 = load i64, i64* @n, align 8, !dbg !120
  %28 = select i1 %25, i64 %26, i64 %27, !dbg !120
  call void @llvm.dbg.value(metadata i64 %28, metadata !122, metadata !DIExpression()), !dbg !123
  %29 = mul nsw i64 2, %28, !dbg !124
  %30 = sub nsw i64 %29, %0, !dbg !125
  %31 = load i64, i64* @m, align 8, !dbg !126
  %32 = load i64, i64* @n, align 8, !dbg !127
  %33 = sub nsw i64 %31, %32, !dbg !128
  %34 = call i64 @labs(i64 %33) #11, !dbg !129
  %35 = add nsw i64 %30, %34, !dbg !130
  %36 = sub nsw i64 %35, 2, !dbg !131
  br label %37, !dbg !132

37:                                               ; preds = %22, %14, %1
  %.0 = phi i64 [ %21, %14 ], [ %36, %22 ], [ %0, %1 ], !dbg !133
  ret i64 %.0, !dbg !134
}

; Function Attrs: nounwind readnone
declare dso_local i64 @labs(i64) #4

; Function Attrs: noinline nounwind uwtable
define dso_local i32 @matchMissmatchScore(i64 %0, i64 %1) #0 !dbg !135 {
  call void @llvm.dbg.value(metadata i64 %0, metadata !138, metadata !DIExpression()), !dbg !139
  call void @llvm.dbg.value(metadata i64 %1, metadata !140, metadata !DIExpression()), !dbg !139
  %3 = load i8*, i8** @a, align 8, !dbg !141, !repair !143
  %4 = sub nsw i64 %1, 1, !dbg !144
  %5 = getelementptr inbounds i8, i8* %3, i64 %4, !dbg !141
  %6 = load i8, i8* %5, align 1, !dbg !141
  %7 = sext i8 %6 to i32, !dbg !141
  %8 = load i8*, i8** @b, align 8, !dbg !145, !repair !146
  %9 = sub nsw i64 %0, 1, !dbg !147
  %10 = getelementptr inbounds i8, i8* %8, i64 %9, !dbg !145
  %11 = load i8, i8* %10, align 1, !dbg !145
  %12 = sext i8 %11 to i32, !dbg !145
  %13 = icmp eq i32 %7, %12, !dbg !148
  %14 = load i32, i32* @matchScore, align 4, !dbg !149, !repair !150
  %15 = load i32, i32* @missmatchScore, align 4, !dbg !149, !repair !151
  %.0 = select i1 %13, i32 %14, i32 %15, !dbg !149
  ret i32 %.0, !dbg !152
}

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @similarityScore(i64 %0, i64 %1, i32* %2, i32* %3, i64* %4) #3 !dbg !153 {
  %6 = call i32 @__kmpc_global_thread_num(%struct.ident_t* @1)
  call void @llvm.dbg.value(metadata i64 %0, metadata !158, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i64 %1, metadata !160, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i32* %2, metadata !161, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i32* %3, metadata !162, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i64* %4, metadata !163, metadata !DIExpression()), !dbg !159
  %7 = load i64, i64* @m, align 8, !dbg !164
  %8 = mul nsw i64 %7, %0, !dbg !165
  %9 = add nsw i64 %8, %1, !dbg !166
  call void @llvm.dbg.value(metadata i64 %9, metadata !167, metadata !DIExpression()), !dbg !159
  %10 = load i64, i64* @m, align 8, !dbg !168
  %11 = sub nsw i64 %9, %10, !dbg !169
  %12 = getelementptr inbounds i32, i32* %2, i64 %11, !dbg !170
  %13 = load i32, i32* %12, align 4, !dbg !170
  %14 = load i32, i32* @gapScore, align 4, !dbg !171
  %15 = add nsw i32 %13, %14, !dbg !172
  call void @llvm.dbg.value(metadata i32 %15, metadata !173, metadata !DIExpression()), !dbg !159
  %16 = sub nsw i64 %9, 1, !dbg !174
  %17 = getelementptr inbounds i32, i32* %2, i64 %16, !dbg !175
  %18 = load i32, i32* %17, align 4, !dbg !175
  %19 = load i32, i32* @gapScore, align 4, !dbg !176
  %20 = add nsw i32 %18, %19, !dbg !177
  call void @llvm.dbg.value(metadata i32 %20, metadata !178, metadata !DIExpression()), !dbg !159
  %21 = load i64, i64* @m, align 8, !dbg !179
  %22 = sub nsw i64 %9, %21, !dbg !180
  %23 = sub nsw i64 %22, 1, !dbg !181
  %24 = getelementptr inbounds i32, i32* %2, i64 %23, !dbg !182
  %25 = load i32, i32* %24, align 4, !dbg !182
  %26 = call i32 @matchMissmatchScore(i64 %0, i64 %1), !dbg !183
  %27 = add nsw i32 %25, %26, !dbg !184
  call void @llvm.dbg.value(metadata i32 %27, metadata !185, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i32 0, metadata !186, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i32 0, metadata !187, metadata !DIExpression()), !dbg !159
  %28 = icmp sgt i32 %27, 0, !dbg !188
  %spec.select = select i1 %28, i32 %27, i32 0, !dbg !190
  %spec.select4 = select i1 %28, i32 3, i32 0, !dbg !190
  call void @llvm.dbg.value(metadata i32 %spec.select4, metadata !187, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i32 %spec.select, metadata !186, metadata !DIExpression()), !dbg !159
  %29 = icmp sgt i32 %15, %spec.select, !dbg !191
  %.12 = select i1 %29, i32 %15, i32 %spec.select, !dbg !193
  %.1 = select i1 %29, i32 1, i32 %spec.select4, !dbg !193
  call void @llvm.dbg.value(metadata i32 %.1, metadata !187, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i32 %.12, metadata !186, metadata !DIExpression()), !dbg !159
  %30 = icmp sgt i32 %20, %.12, !dbg !194
  %spec.select5 = select i1 %30, i32 %20, i32 %.12, !dbg !196
  %spec.select6 = select i1 %30, i32 2, i32 %.1, !dbg !196
  call void @llvm.dbg.value(metadata i32 %spec.select6, metadata !187, metadata !DIExpression()), !dbg !159
  call void @llvm.dbg.value(metadata i32 %spec.select5, metadata !186, metadata !DIExpression()), !dbg !159
  %31 = getelementptr inbounds i32, i32* %2, i64 %9, !dbg !197
  store i32 %spec.select5, i32* %31, align 4, !dbg !198
  %32 = getelementptr inbounds i32, i32* %3, i64 %9, !dbg !199
  store i32 %spec.select6, i32* %32, align 4, !dbg !200
  %33 = load i64, i64* %4, align 8, !dbg !201
  %34 = getelementptr inbounds i32, i32* %2, i64 %33, !dbg !203
  %35 = load i32, i32* %34, align 4, !dbg !203
  %36 = icmp sgt i32 %spec.select5, %35, !dbg !204
  br i1 %36, label %37, label %38, !dbg !205

37:                                               ; preds = %5
  call void @__kmpc_critical(%struct.ident_t* @1, i32 %6, [8 x i32]* @.gomp_critical_user_.var), !dbg !206
  store i64 %9, i64* %4, align 8, !dbg !209
  call void @__kmpc_end_critical(%struct.ident_t* @1, i32 %6, [8 x i32]* @.gomp_critical_user_.var), !dbg !210
  br label %38, !dbg !211

38:                                               ; preds = %37, %5
  ret void, !dbg !212
}

; Function Attrs: nounwind
declare i32 @__kmpc_global_thread_num(%struct.ident_t*) #5

; Function Attrs: convergent nounwind
declare void @__kmpc_end_critical(%struct.ident_t*, i32, [8 x i32]*) #6

; Function Attrs: convergent nounwind
declare void @__kmpc_critical(%struct.ident_t*, i32, [8 x i32]*) #6

; Function Attrs: alwaysinline nounwind uwtable
define dso_local void @calcFirstDiagElement(i64* %0, i64* %1, i64* %2) #3 !dbg !213 {
  call void @llvm.dbg.value(metadata i64* %0, metadata !216, metadata !DIExpression()), !dbg !217
  call void @llvm.dbg.value(metadata i64* %1, metadata !218, metadata !DIExpression()), !dbg !217
  call void @llvm.dbg.value(metadata i64* %2, metadata !219, metadata !DIExpression()), !dbg !217
  %4 = load i64, i64* %0, align 8, !dbg !220
  %5 = load i64, i64* @n, align 8, !dbg !222
  %6 = icmp slt i64 %4, %5, !dbg !223
  br i1 %6, label %7, label %9, !dbg !224

7:                                                ; preds = %3
  %8 = load i64, i64* %0, align 8, !dbg !225
  store i64 %8, i64* %1, align 8, !dbg !227
  store i64 1, i64* %2, align 8, !dbg !228
  br label %16, !dbg !229

9:                                                ; preds = %3
  %10 = load i64, i64* @n, align 8, !dbg !230
  %11 = sub nsw i64 %10, 1, !dbg !232
  store i64 %11, i64* %1, align 8, !dbg !233
  %12 = load i64, i64* %0, align 8, !dbg !234
  %13 = load i64, i64* @n, align 8, !dbg !235
  %14 = sub nsw i64 %12, %13, !dbg !236
  %15 = add nsw i64 %14, 2, !dbg !237
  store i64 %15, i64* %2, align 8, !dbg !238
  br label %16

16:                                               ; preds = %9, %7
  ret void, !dbg !239
}

; Function Attrs: noinline nounwind uwtable
define dso_local i32 @main(i32 %0, i8** %1) #0 !dbg !240 {
  %3 = alloca i32*, align 8
  %4 = alloca i32*, align 8
  %5 = alloca i64, align 8
  %6 = alloca i64, align 8
  call void @llvm.dbg.value(metadata i32 %0, metadata !244, metadata !DIExpression()), !dbg !245
  call void @llvm.dbg.value(metadata i8** %1, metadata !246, metadata !DIExpression()), !dbg !245
  store i64 2048, i64* @m, align 8, !dbg !247
  store i64 2048, i64* @n, align 8, !dbg !248
  %7 = load i64, i64* @m, align 8, !dbg !249
  %8 = mul i64 %7, 1, !dbg !250
  %9 = call noalias i8* @malloc(i64 %8) #5, !dbg !251
  store i8* %9, i8** @a, align 8, !dbg !252
  %10 = load i64, i64* @n, align 8, !dbg !253
  %11 = mul i64 %10, 1, !dbg !254
  %12 = call noalias i8* @malloc(i64 %11) #5, !dbg !255
  store i8* %12, i8** @b, align 8, !dbg !256
  %13 = load i64, i64* @m, align 8, !dbg !257
  %14 = add nsw i64 %13, 1, !dbg !257
  store i64 %14, i64* @m, align 8, !dbg !257
  %15 = load i64, i64* @n, align 8, !dbg !258
  %16 = add nsw i64 %15, 1, !dbg !258
  store i64 %16, i64* @n, align 8, !dbg !258
  call void @llvm.dbg.declare(metadata i32** %3, metadata !259, metadata !DIExpression()), !dbg !260
  %17 = load i64, i64* @m, align 8, !dbg !261
  %18 = load i64, i64* @n, align 8, !dbg !262
  %19 = mul nsw i64 %17, %18, !dbg !263
  %20 = call noalias i8* @calloc(i64 %19, i64 4) #5, !dbg !264
  %21 = bitcast i8* %20 to i32*, !dbg !264
  store i32* %21, i32** %3, align 8, !dbg !265
  call void @llvm.dbg.declare(metadata i32** %4, metadata !266, metadata !DIExpression()), !dbg !267
  %22 = load i64, i64* @m, align 8, !dbg !268
  %23 = load i64, i64* @n, align 8, !dbg !269
  %24 = mul nsw i64 %22, %23, !dbg !270
  %25 = call noalias i8* @calloc(i64 %24, i64 4) #5, !dbg !271
  %26 = bitcast i8* %25 to i32*, !dbg !271
  store i32* %26, i32** %4, align 8, !dbg !272
  call void @generate(), !dbg !273
  call void @llvm.dbg.declare(metadata i64* %5, metadata !274, metadata !DIExpression()), !dbg !275
  store i64 0, i64* %5, align 8, !dbg !275
  call void @llvm.dbg.declare(metadata i64* undef, metadata !276, metadata !DIExpression()), !dbg !277
  call void @llvm.dbg.declare(metadata i64* undef, metadata !278, metadata !DIExpression()), !dbg !279
  %27 = call double @omp_get_wtime(), !dbg !280
  call void @llvm.dbg.value(metadata double %27, metadata !281, metadata !DIExpression()), !dbg !245
  call void @llvm.dbg.declare(metadata i64* undef, metadata !283, metadata !DIExpression()), !dbg !284
  call void @llvm.dbg.declare(metadata i64* undef, metadata !285, metadata !DIExpression()), !dbg !286
  call void @llvm.dbg.declare(metadata i64* undef, metadata !287, metadata !DIExpression()), !dbg !288
  call void @llvm.dbg.declare(metadata i64* undef, metadata !289, metadata !DIExpression()), !dbg !290
  call void @llvm.dbg.declare(metadata i64* %6, metadata !291, metadata !DIExpression()), !dbg !292
  %28 = load i64, i64* @m, align 8, !dbg !293
  %29 = load i64, i64* @n, align 8, !dbg !294
  %30 = add nsw i64 %28, %29, !dbg !295
  %31 = sub nsw i64 %30, 3, !dbg !296
  store i64 %31, i64* %6, align 8, !dbg !292
  call void @llvm.dbg.declare(metadata i64* undef, metadata !297, metadata !DIExpression()), !dbg !298
  call void (%struct.ident_t*, i32, void (i32*, i32*, ...)*, ...) @__kmpc_fork_call(%struct.ident_t* @8, i32 4, void (i32*, i32*, ...)* bitcast (void (i32*, i32*, i64*, i32**, i32**, i64*)* @.omp_outlined. to void (i32*, i32*, ...)*), i64* %6, i32** %3, i32** %4, i64* %5), !dbg !299
  ret i32 0, !dbg !300
}

; Function Attrs: nounwind
declare dso_local noalias i8* @malloc(i64) #1

; Function Attrs: nounwind
declare dso_local noalias i8* @calloc(i64, i64) #1

declare dso_local double @omp_get_wtime() #7

declare dso_local void @__kmpc_for_static_init_8(%struct.ident_t*, i32, i32, i32*, i64*, i64*, i64*, i64, i64)

; Function Attrs: nounwind
declare void @__kmpc_for_static_fini(%struct.ident_t*, i32) #5

; Function Attrs: noinline norecurse nounwind uwtable
define internal void @.omp_outlined.(i32* noalias %0, i32* noalias %1, i64* nonnull align 8 dereferenceable(8) %2, i32** nonnull align 8 dereferenceable(8) %3, i32** nonnull align 8 dereferenceable(8) %4, i64* nonnull align 8 dereferenceable(8) %5) #8 !dbg !301 {
  %7 = alloca i64, align 8
  %8 = alloca i64, align 8
  %9 = alloca i64, align 8
  %10 = alloca i64, align 8
  %11 = alloca i64, align 8
  %12 = alloca i64, align 8
  %13 = alloca i32, align 4
  call void @llvm.dbg.value(metadata i32* %0, metadata !310, metadata !DIExpression()), !dbg !311
  call void @llvm.dbg.value(metadata i32* %1, metadata !312, metadata !DIExpression()), !dbg !311
  call void @llvm.dbg.value(metadata i64* %2, metadata !313, metadata !DIExpression()), !dbg !311
  call void @llvm.dbg.value(metadata i32** %3, metadata !314, metadata !DIExpression()), !dbg !311
  call void @llvm.dbg.value(metadata i32** %4, metadata !315, metadata !DIExpression()), !dbg !311
  call void @llvm.dbg.value(metadata i64* %5, metadata !316, metadata !DIExpression()), !dbg !311
  call void @llvm.experimental.noalias.scope.decl(metadata !317), !dbg !320
  %14 = bitcast i64* %7 to i8*, !dbg !321
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %14), !dbg !321
  %15 = bitcast i64* %8 to i8*, !dbg !321
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %15), !dbg !321
  %16 = bitcast i64* %9 to i8*, !dbg !321
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %16), !dbg !321
  %17 = bitcast i64* %10 to i8*, !dbg !321
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %17), !dbg !321
  %18 = bitcast i64* %11 to i8*, !dbg !321
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %18), !dbg !321
  %19 = bitcast i64* %12 to i8*, !dbg !321
  call void @llvm.lifetime.start.p0i8(i64 8, i8* %19), !dbg !321
  %20 = bitcast i32* %13 to i8*, !dbg !321
  call void @llvm.lifetime.start.p0i8(i64 4, i8* %20), !dbg !321
  call void @llvm.dbg.value(metadata i32* %0, metadata !324, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.value(metadata i32* %1, metadata !325, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.value(metadata i64* %2, metadata !326, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.value(metadata i32** %3, metadata !327, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.value(metadata i32** %4, metadata !328, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.value(metadata i64* %5, metadata !329, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.declare(metadata i64* %7, metadata !330, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.declare(metadata i64* %8, metadata !331, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.declare(metadata i64* %9, metadata !332, metadata !DIExpression()) #5, !dbg !321
  store i64 1, i64* %7, align 8, !dbg !333, !noalias !317
  br label %21, !dbg !336

21:                                               ; preds = %.aftr.bar, %6
  %22 = load i64, i64* %7, align 8, !dbg !337, !noalias !317
  %23 = load i64, i64* %2, align 8, !dbg !339, !noalias !317, !repair !340
  %24 = icmp sle i64 %22, %23, !dbg !341
  br i1 %24, label %25, label %.omp_outlined._debug__.exit, !dbg !342

25:                                               ; preds = %21
  %26 = load i64, i64* %7, align 8, !dbg !343, !noalias !317
  call void @llvm.dbg.value(metadata i64 %26, metadata !104, metadata !DIExpression()) #5, !dbg !345
  %27 = load i64, i64* @m, align 8, !dbg !347, !noalias !317, !repair !348
  %28 = icmp slt i64 %26, %27, !dbg !349
  %29 = load i64, i64* @n, align 8, !noalias !317, !repair !72
  %30 = icmp slt i64 %26, %29
  %or.cond.i.i = and i1 %28, %30, !dbg !350
  br i1 %or.cond.i.i, label %nElement.exit.i, label %31, !dbg !350

31:                                               ; preds = %25
  %32 = load i64, i64* @m, align 8, !dbg !351, !noalias !317, !repair !352
  %33 = load i64, i64* @n, align 8, !dbg !351, !noalias !317, !repair !353
  %34 = icmp sgt i64 %32, %33, !dbg !351
  %35 = load i64, i64* @m, align 8, !dbg !351, !noalias !317, !repair !354
  %36 = load i64, i64* @n, align 8, !dbg !351, !noalias !317, !repair !355
  %37 = select i1 %34, i64 %35, i64 %36, !dbg !351
  %38 = icmp slt i64 %26, %37, !dbg !356
  br i1 %38, label %39, label %47, !dbg !357

39:                                               ; preds = %31
  %40 = load i64, i64* @m, align 8, !dbg !358, !noalias !317, !repair !359
  %41 = load i64, i64* @n, align 8, !dbg !358, !noalias !317, !repair !360
  %42 = icmp slt i64 %40, %41, !dbg !358
  %43 = load i64, i64* @m, align 8, !dbg !358, !noalias !317, !repair !361
  %44 = load i64, i64* @n, align 8, !dbg !358, !noalias !317, !repair !362
  %45 = select i1 %42, i64 %43, i64 %44, !dbg !358
  call void @llvm.dbg.value(metadata i64 %45, metadata !116, metadata !DIExpression()) #5, !dbg !363
  %46 = sub nsw i64 %45, 1, !dbg !364
  br label %nElement.exit.i, !dbg !365

47:                                               ; preds = %31
  %48 = load i64, i64* @m, align 8, !dbg !366, !noalias !317, !repair !367
  %49 = load i64, i64* @n, align 8, !dbg !366, !noalias !317, !repair !368
  %50 = icmp slt i64 %48, %49, !dbg !366
  %51 = load i64, i64* @m, align 8, !dbg !366, !noalias !317, !repair !369
  %52 = load i64, i64* @n, align 8, !dbg !366, !noalias !317, !repair !370
  %53 = select i1 %50, i64 %51, i64 %52, !dbg !366
  call void @llvm.dbg.value(metadata i64 %53, metadata !122, metadata !DIExpression()) #5, !dbg !371
  %54 = mul nsw i64 2, %53, !dbg !372
  %55 = sub nsw i64 %54, %26, !dbg !373
  %56 = load i64, i64* @m, align 8, !dbg !374, !noalias !317, !repair !375
  %57 = load i64, i64* @n, align 8, !dbg !376, !noalias !317, !repair !377
  %58 = sub nsw i64 %56, %57, !dbg !378
  %59 = call i64 @labs(i64 %58) #11, !dbg !379
  %60 = add nsw i64 %55, %59, !dbg !380
  %61 = sub nsw i64 %60, 2, !dbg !381
  br label %nElement.exit.i, !dbg !382

nElement.exit.i:                                  ; preds = %47, %39, %25
  %.0.i.i = phi i64 [ %46, %39 ], [ %61, %47 ], [ %26, %25 ], !dbg !383
  call void @llvm.dbg.value(metadata i64 %.0.i.i, metadata !384, metadata !DIExpression()) #5, !dbg !321
  call void @llvm.dbg.value(metadata i64* %7, metadata !216, metadata !DIExpression()) #5, !dbg !385
  call void @llvm.dbg.value(metadata i64* %8, metadata !218, metadata !DIExpression()) #5, !dbg !385
  call void @llvm.dbg.value(metadata i64* %9, metadata !219, metadata !DIExpression()) #5, !dbg !385
  %62 = load i64, i64* %7, align 8, !dbg !387, !noalias !317
  %63 = load i64, i64* @n, align 8, !dbg !388, !noalias !317, !repair !389
  %64 = icmp slt i64 %62, %63, !dbg !390
  br i1 %64, label %65, label %67, !dbg !391

65:                                               ; preds = %nElement.exit.i
  %66 = load i64, i64* %7, align 8, !dbg !392, !noalias !317
  store i64 %66, i64* %8, align 8, !dbg !393, !noalias !317
  store i64 1, i64* %9, align 8, !dbg !394, !noalias !317
  br label %calcFirstDiagElement.exit.i, !dbg !395

67:                                               ; preds = %nElement.exit.i
  %68 = load i64, i64* @n, align 8, !dbg !396, !noalias !317, !repair !397
  %69 = sub nsw i64 %68, 1, !dbg !398
  store i64 %69, i64* %8, align 8, !dbg !399, !noalias !317
  %70 = load i64, i64* %7, align 8, !dbg !400, !noalias !317
  %71 = load i64, i64* @n, align 8, !dbg !401, !noalias !317, !repair !402
  %72 = sub nsw i64 %70, %71, !dbg !403
  %73 = add nsw i64 %72, 2, !dbg !404
  store i64 %73, i64* %9, align 8, !dbg !405, !noalias !317
  br label %calcFirstDiagElement.exit.i

calcFirstDiagElement.exit.i:                      ; preds = %67, %65
  call void @llvm.dbg.value(metadata i64 %.0.i.i, metadata !406, metadata !DIExpression()) #5, !dbg !408
  %74 = sub nsw i64 %.0.i.i, 1, !dbg !409
  call void @llvm.dbg.value(metadata i64 %74, metadata !406, metadata !DIExpression()) #5, !dbg !408
  call void @llvm.dbg.value(metadata i64 1, metadata !410, metadata !DIExpression()) #5, !dbg !408
  %75 = icmp sle i64 1, %.0.i.i, !dbg !409
  br i1 %75, label %76, label %131, !dbg !411

76:                                               ; preds = %calcFirstDiagElement.exit.i
  call void @llvm.dbg.declare(metadata i64* %10, metadata !412, metadata !DIExpression()) #5, !dbg !408
  store i64 0, i64* %10, align 8, !dbg !413, !noalias !317
  call void @llvm.dbg.declare(metadata i64* %11, metadata !414, metadata !DIExpression()) #5, !dbg !408
  store i64 %74, i64* %11, align 8, !dbg !413, !noalias !317
  call void @llvm.dbg.declare(metadata i64* %12, metadata !415, metadata !DIExpression()) #5, !dbg !408
  store i64 1, i64* %12, align 8, !dbg !413, !noalias !317
  call void @llvm.dbg.declare(metadata i32* %13, metadata !416, metadata !DIExpression()) #5, !dbg !408
  store i32 0, i32* %13, align 4, !dbg !413, !noalias !317
  %77 = load i32, i32* %0, align 4, !dbg !411, !alias.scope !317
  call void @__kmpc_for_static_init_8(%struct.ident_t* @3, i32 %77, i32 34, i32* %13, i64* %10, i64* %11, i64* %12, i64 1, i64 1) #5, !dbg !417, !noalias !317
  %78 = load i64, i64* %11, align 8, !dbg !413, !noalias !317
  %79 = icmp sgt i64 %78, %74, !dbg !413
  %80 = load i64, i64* %11, align 8, !dbg !413, !noalias !317
  %81 = select i1 %79, i64 %74, i64 %80, !dbg !413
  store i64 %81, i64* %11, align 8, !dbg !413, !noalias !317
  %82 = load i64, i64* %10, align 8, !dbg !413, !noalias !317
  call void @llvm.dbg.value(metadata i64 %82, metadata !418, metadata !DIExpression()) #5, !dbg !408
  br label %83, !dbg !411

83:                                               ; preds = %similarityScore.exit.i, %76
  %.0.i = phi i64 [ %82, %76 ], [ %128, %similarityScore.exit.i ], !dbg !408
  call void @llvm.dbg.value(metadata i64 %.0.i, metadata !418, metadata !DIExpression()) #5, !dbg !408
  %84 = load i64, i64* %11, align 8, !dbg !413, !noalias !317
  %85 = icmp sle i64 %.0.i, %84, !dbg !409
  br i1 %85, label %86, label %129, !dbg !411

86:                                               ; preds = %83
  %87 = add nsw i64 1, %.0.i, !dbg !419
  call void @llvm.dbg.value(metadata i64 %87, metadata !410, metadata !DIExpression()) #5, !dbg !408
  %88 = load i64, i64* %8, align 8, !dbg !420, !noalias !317
  %89 = sub nsw i64 %88, %87, !dbg !422
  %90 = add nsw i64 %89, 1, !dbg !423
  call void @llvm.dbg.value(metadata i64 %90, metadata !424, metadata !DIExpression()) #5, !dbg !321
  %91 = load i64, i64* %9, align 8, !dbg !425, !noalias !317
  %92 = add nsw i64 %91, %87, !dbg !426
  %93 = sub nsw i64 %92, 1, !dbg !427
  call void @llvm.dbg.value(metadata i64 %93, metadata !428, metadata !DIExpression()) #5, !dbg !321
  %94 = load i32*, i32** %3, align 8, !dbg !429, !noalias !317, !repair !430
  %95 = load i32*, i32** %4, align 8, !dbg !431, !noalias !317, !repair !432
  %96 = call i32 @__kmpc_global_thread_num(%struct.ident_t* @1) #5, !noalias !317
  call void @llvm.dbg.value(metadata i64 %90, metadata !158, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i64 %93, metadata !160, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i32* %94, metadata !161, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i32* %95, metadata !162, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i64* %5, metadata !163, metadata !DIExpression()) #5, !dbg !433
  %97 = load i64, i64* @m, align 8, !dbg !435, !noalias !317, !repair !436
  %98 = mul nsw i64 %97, %90, !dbg !437
  %99 = add nsw i64 %98, %93, !dbg !438
  call void @llvm.dbg.value(metadata i64 %99, metadata !167, metadata !DIExpression()) #5, !dbg !433
  %100 = load i64, i64* @m, align 8, !dbg !439, !noalias !317, !repair !440
  %101 = sub nsw i64 %99, %100, !dbg !441
  %102 = getelementptr inbounds i32, i32* %94, i64 %101, !dbg !442
  %103 = load i32, i32* %102, align 4, !dbg !442, !noalias !317
  %104 = load i32, i32* @gapScore, align 4, !dbg !443, !noalias !317, !repair !444
  %105 = add nsw i32 %103, %104, !dbg !445
  call void @llvm.dbg.value(metadata i32 %105, metadata !173, metadata !DIExpression()) #5, !dbg !433
  %106 = sub nsw i64 %99, 1, !dbg !446
  %107 = getelementptr inbounds i32, i32* %94, i64 %106, !dbg !447
  %108 = load i32, i32* %107, align 4, !dbg !447, !noalias !317
  %109 = load i32, i32* @gapScore, align 4, !dbg !448, !noalias !317, !repair !449
  %110 = add nsw i32 %108, %109, !dbg !450
  call void @llvm.dbg.value(metadata i32 %110, metadata !178, metadata !DIExpression()) #5, !dbg !433
  %111 = load i64, i64* @m, align 8, !dbg !451, !noalias !317, !repair !452
  %112 = sub nsw i64 %99, %111, !dbg !453
  %113 = sub nsw i64 %112, 1, !dbg !454
  %114 = getelementptr inbounds i32, i32* %94, i64 %113, !dbg !455
  %115 = load i32, i32* %114, align 4, !dbg !455, !noalias !317
  %116 = call i32 @matchMissmatchScore(i64 %90, i64 %93) #5, !dbg !456, !noalias !317
  %117 = add nsw i32 %115, %116, !dbg !457
  call void @llvm.dbg.value(metadata i32 %117, metadata !185, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i32 0, metadata !186, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i32 0, metadata !187, metadata !DIExpression()) #5, !dbg !433
  %118 = icmp sgt i32 %117, 0, !dbg !458
  %spec.select.i.i = select i1 %118, i32 %117, i32 0, !dbg !459
  %spec.select4.i.i = select i1 %118, i32 3, i32 0, !dbg !459
  call void @llvm.dbg.value(metadata i32 %spec.select4.i.i, metadata !187, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i32 %spec.select.i.i, metadata !186, metadata !DIExpression()) #5, !dbg !433
  %119 = icmp sgt i32 %105, %spec.select.i.i, !dbg !460
  %.12.i.i = select i1 %119, i32 %105, i32 %spec.select.i.i, !dbg !461
  %.1.i.i = select i1 %119, i32 1, i32 %spec.select4.i.i, !dbg !461
  call void @llvm.dbg.value(metadata i32 %.1.i.i, metadata !187, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i32 %.12.i.i, metadata !186, metadata !DIExpression()) #5, !dbg !433
  %120 = icmp sgt i32 %110, %.12.i.i, !dbg !462
  %spec.select5.i.i = select i1 %120, i32 %110, i32 %.12.i.i, !dbg !463
  %spec.select6.i.i = select i1 %120, i32 2, i32 %.1.i.i, !dbg !463
  call void @llvm.dbg.value(metadata i32 %spec.select6.i.i, metadata !187, metadata !DIExpression()) #5, !dbg !433
  call void @llvm.dbg.value(metadata i32 %spec.select5.i.i, metadata !186, metadata !DIExpression()) #5, !dbg !433
  %121 = getelementptr inbounds i32, i32* %94, i64 %99, !dbg !464
  store i32 %spec.select5.i.i, i32* %121, align 4, !dbg !465, !noalias !317
  %122 = getelementptr inbounds i32, i32* %95, i64 %99, !dbg !466
  store i32 %spec.select6.i.i, i32* %122, align 4, !dbg !467, !noalias !317
  %123 = load i64, i64* %5, align 8, !dbg !468, !noalias !317, !repair !469
  %124 = getelementptr inbounds i32, i32* %94, i64 %123, !dbg !470
  %125 = load i32, i32* %124, align 4, !dbg !470, !noalias !317
  %126 = icmp sgt i32 %spec.select5.i.i, %125, !dbg !471
  br i1 %126, label %127, label %similarityScore.exit.i, !dbg !472

127:                                              ; preds = %86
  br label %.crt.strt, !dbg !473

.crt.strt:                                        ; preds = %127
  call void @__kmpc_critical(%struct.ident_t* @1, i32 %96, [8 x i32]* @.gomp_critical_user_.var) #5, !dbg !473, !noalias !317
  store i64 %99, i64* %5, align 8, !dbg !474, !noalias !317, !repair !475
  call void @__kmpc_end_critical(%struct.ident_t* @1, i32 %96, [8 x i32]* @.gomp_critical_user_.var) #5, !dbg !476, !noalias !317
  br label %.crt.strt.crt.end, !dbg !477

.crt.strt.crt.end:                                ; preds = %.crt.strt
  br label %similarityScore.exit.i, !dbg !477

similarityScore.exit.i:                           ; preds = %.crt.strt.crt.end, %86
  %128 = add nsw i64 %.0.i, 1, !dbg !409
  call void @llvm.dbg.value(metadata i64 %128, metadata !418, metadata !DIExpression()) #5, !dbg !408
  br label %83, !dbg !417, !llvm.loop !478

129:                                              ; preds = %83
  %130 = load i32, i32* %0, align 4, !dbg !417, !alias.scope !317
  call void @__kmpc_for_static_fini(%struct.ident_t* @5, i32 %130) #5, !dbg !479, !noalias !317
  br label %131, !dbg !417

131:                                              ; preds = %129, %calcFirstDiagElement.exit.i
  %132 = load i32, i32* %0, align 4, !dbg !479, !alias.scope !317
  br label %.bar, !dbg !479

.bar:                                             ; preds = %131
  br label %.aftr.bar, !dbg !480

.aftr.bar:                                        ; preds = %.bar
  %133 = load i64, i64* %7, align 8, !dbg !480, !noalias !317
  %134 = add nsw i64 %133, 1, !dbg !480
  store i64 %134, i64* %7, align 8, !dbg !480, !noalias !317
  br label %21, !dbg !481, !llvm.loop !482

.omp_outlined._debug__.exit:                      ; preds = %21
  %135 = bitcast i64* %7 to i8*, !dbg !484
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %135), !dbg !484
  %136 = bitcast i64* %8 to i8*, !dbg !484
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %136), !dbg !484
  %137 = bitcast i64* %9 to i8*, !dbg !484
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %137), !dbg !484
  %138 = bitcast i64* %10 to i8*, !dbg !484
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %138), !dbg !484
  %139 = bitcast i64* %11 to i8*, !dbg !484
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %139), !dbg !484
  %140 = bitcast i64* %12 to i8*, !dbg !484
  call void @llvm.lifetime.end.p0i8(i64 8, i8* %140), !dbg !484
  %141 = bitcast i32* %13 to i8*, !dbg !484
  call void @llvm.lifetime.end.p0i8(i64 4, i8* %141), !dbg !484
  br label %.omp_outlined._debug__.exit.par.end, !dbg !320

.omp_outlined._debug__.exit.par.end:              ; preds = %.omp_outlined._debug__.exit
  ret void, !dbg !320
}

; Function Attrs: nounwind
declare !callback !485 void @__kmpc_fork_call(%struct.ident_t*, i32, void (i32*, i32*, ...)*, ...) #5

; Function Attrs: nofree nosync nounwind readnone speculatable willreturn
declare void @llvm.dbg.value(metadata, metadata, metadata) #2

; Function Attrs: inaccessiblememonly nofree nosync nounwind willreturn
declare void @llvm.experimental.noalias.scope.decl(metadata) #9

; Function Attrs: argmemonly nofree nosync nounwind willreturn
declare void @llvm.lifetime.start.p0i8(i64 immarg, i8* nocapture) #10

; Function Attrs: argmemonly nofree nosync nounwind willreturn
declare void @llvm.lifetime.end.p0i8(i64 immarg, i8* nocapture) #10

declare void @__kmpc_barrier()

declare void @__kmpc_ordered()

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
!39 = !{!"b17.ordered.false"}
!40 = !DILocation(line: 67, column: 19, scope: !38)
!41 = !DILocation(line: 67, column: 5, scope: !35)
!42 = !DILocation(line: 68, column: 19, scope: !43)
!43 = distinct !DILexicalBlock(scope: !38, file: !8, line: 67, column: 29)
!44 = !DILocation(line: 68, column: 26, scope: !43)
!45 = !DILocalVariable(name: "aux", scope: !43, file: !8, line: 68, type: !18)
!46 = !DILocation(line: 0, scope: !43)
!47 = !DILocation(line: 69, column: 13, scope: !43)
!48 = !DILocation(line: 70, column: 13, scope: !49)
!49 = distinct !DILexicalBlock(scope: !43, file: !8, line: 69, column: 13)
!50 = !{!"b33.ordered.false"}
!51 = !DILocation(line: 70, column: 18, scope: !49)
!52 = !DILocation(line: 72, column: 13, scope: !53)
!53 = distinct !DILexicalBlock(scope: !49, file: !8, line: 71, column: 18)
!54 = !{!"b32.ordered.false"}
!55 = !DILocation(line: 72, column: 18, scope: !53)
!56 = !DILocation(line: 74, column: 13, scope: !57)
!57 = distinct !DILexicalBlock(scope: !53, file: !8, line: 73, column: 18)
!58 = !{!"b31.ordered.false"}
!59 = !DILocation(line: 74, column: 18, scope: !57)
!60 = !DILocation(line: 76, column: 13, scope: !57)
!61 = !{!"b30.ordered.false"}
!62 = !DILocation(line: 76, column: 18, scope: !57)
!63 = !DILocation(line: 67, column: 25, scope: !38)
!64 = !DILocation(line: 67, column: 5, scope: !38)
!65 = distinct !{!65, !41, !66, !67}
!66 = !DILocation(line: 77, column: 5, scope: !35)
!67 = !{!"llvm.loop.mustprogress"}
!68 = !DILocation(line: 0, scope: !69)
!69 = distinct !DILexicalBlock(scope: !27, file: !8, line: 80, column: 5)
!70 = !DILocation(line: 80, column: 21, scope: !71)
!71 = distinct !DILexicalBlock(scope: !69, file: !8, line: 80, column: 5)
!72 = !{!"b28.ordered.false"}
!73 = !DILocation(line: 80, column: 19, scope: !71)
!74 = !DILocation(line: 80, column: 5, scope: !69)
!75 = !DILocation(line: 81, column: 19, scope: !76)
!76 = distinct !DILexicalBlock(scope: !71, file: !8, line: 80, column: 29)
!77 = !DILocation(line: 81, column: 26, scope: !76)
!78 = !DILocalVariable(name: "aux", scope: !76, file: !8, line: 81, type: !18)
!79 = !DILocation(line: 0, scope: !76)
!80 = !DILocation(line: 82, column: 13, scope: !76)
!81 = !DILocation(line: 83, column: 13, scope: !82)
!82 = distinct !DILexicalBlock(scope: !76, file: !8, line: 82, column: 13)
!83 = !{!"b38.ordered.false"}
!84 = !DILocation(line: 83, column: 18, scope: !82)
!85 = !DILocation(line: 85, column: 13, scope: !86)
!86 = distinct !DILexicalBlock(scope: !82, file: !8, line: 84, column: 18)
!87 = !{!"b37.ordered.false"}
!88 = !DILocation(line: 85, column: 18, scope: !86)
!89 = !DILocation(line: 87, column: 13, scope: !90)
!90 = distinct !DILexicalBlock(scope: !86, file: !8, line: 86, column: 18)
!91 = !{!"b36.ordered.false"}
!92 = !DILocation(line: 87, column: 18, scope: !90)
!93 = !DILocation(line: 89, column: 13, scope: !90)
!94 = !{!"b35.ordered.false"}
!95 = !DILocation(line: 89, column: 18, scope: !90)
!96 = !DILocation(line: 80, column: 25, scope: !71)
!97 = !DILocation(line: 80, column: 5, scope: !71)
!98 = distinct !{!98, !74, !99, !67}
!99 = !DILocation(line: 90, column: 5, scope: !69)
!100 = !DILocation(line: 91, column: 1, scope: !27)
!101 = distinct !DISubprogram(name: "nElement", scope: !8, file: !8, line: 98, type: !102, scopeLine: 98, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!102 = !DISubroutineType(types: !103)
!103 = !{!9, !9}
!104 = !DILocalVariable(name: "i", arg: 1, scope: !101, file: !8, line: 98, type: !9)
!105 = !DILocation(line: 0, scope: !101)
!106 = !DILocation(line: 99, column: 13, scope: !107)
!107 = distinct !DILexicalBlock(scope: !101, file: !8, line: 99, column: 9)
!108 = !DILocation(line: 99, column: 11, scope: !107)
!109 = !DILocation(line: 99, column: 15, scope: !107)
!110 = !DILocation(line: 103, column: 18, scope: !111)
!111 = distinct !DILexicalBlock(scope: !107, file: !8, line: 103, column: 14)
!112 = !DILocation(line: 103, column: 16, scope: !111)
!113 = !DILocation(line: 103, column: 14, scope: !107)
!114 = !DILocation(line: 105, column: 29, scope: !115)
!115 = distinct !DILexicalBlock(scope: !111, file: !8, line: 103, column: 29)
!116 = !DILocalVariable(name: "min", scope: !115, file: !8, line: 105, type: !9)
!117 = !DILocation(line: 0, scope: !115)
!118 = !DILocation(line: 106, column: 20, scope: !115)
!119 = !DILocation(line: 106, column: 9, scope: !115)
!120 = !DILocation(line: 110, column: 29, scope: !121)
!121 = distinct !DILexicalBlock(scope: !111, file: !8, line: 108, column: 10)
!122 = !DILocalVariable(name: "min", scope: !121, file: !8, line: 110, type: !9)
!123 = !DILocation(line: 0, scope: !121)
!124 = !DILocation(line: 111, column: 18, scope: !121)
!125 = !DILocation(line: 111, column: 24, scope: !121)
!126 = !DILocation(line: 111, column: 35, scope: !121)
!127 = !DILocation(line: 111, column: 39, scope: !121)
!128 = !DILocation(line: 111, column: 37, scope: !121)
!129 = !DILocation(line: 111, column: 30, scope: !121)
!130 = !DILocation(line: 111, column: 28, scope: !121)
!131 = !DILocation(line: 111, column: 42, scope: !121)
!132 = !DILocation(line: 111, column: 9, scope: !121)
!133 = !DILocation(line: 0, scope: !107)
!134 = !DILocation(line: 113, column: 1, scope: !101)
!135 = distinct !DISubprogram(name: "matchMissmatchScore", scope: !8, file: !8, line: 119, type: !136, scopeLine: 119, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!136 = !DISubroutineType(types: !137)
!137 = !{!18, !9, !9}
!138 = !DILocalVariable(name: "i", arg: 1, scope: !135, file: !8, line: 119, type: !9)
!139 = !DILocation(line: 0, scope: !135)
!140 = !DILocalVariable(name: "j", arg: 2, scope: !135, file: !8, line: 119, type: !9)
!141 = !DILocation(line: 120, column: 9, scope: !142)
!142 = distinct !DILexicalBlock(scope: !135, file: !8, line: 120, column: 9)
!143 = !{!"b29.ordered.false"}
!144 = !DILocation(line: 120, column: 13, scope: !142)
!145 = !DILocation(line: 120, column: 21, scope: !142)
!146 = !{!"b34.ordered.false"}
!147 = !DILocation(line: 120, column: 25, scope: !142)
!148 = !DILocation(line: 120, column: 18, scope: !142)
!149 = !DILocation(line: 120, column: 9, scope: !135)
!150 = !{!"b39.ordered.false"}
!151 = !{!"b40.ordered.false"}
!152 = !DILocation(line: 124, column: 1, scope: !135)
!153 = distinct !DISubprogram(name: "similarityScore", scope: !8, file: !8, line: 127, type: !154, scopeLine: 127, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!154 = !DISubroutineType(types: !155)
!155 = !{null, !9, !9, !156, !156, !157}
!156 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !18, size: 64)
!157 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !9, size: 64)
!158 = !DILocalVariable(name: "i", arg: 1, scope: !153, file: !8, line: 127, type: !9)
!159 = !DILocation(line: 0, scope: !153)
!160 = !DILocalVariable(name: "j", arg: 2, scope: !153, file: !8, line: 127, type: !9)
!161 = !DILocalVariable(name: "H", arg: 3, scope: !153, file: !8, line: 127, type: !156)
!162 = !DILocalVariable(name: "P", arg: 4, scope: !153, file: !8, line: 127, type: !156)
!163 = !DILocalVariable(name: "maxPos", arg: 5, scope: !153, file: !8, line: 127, type: !157)
!164 = !DILocation(line: 132, column: 27, scope: !153)
!165 = !DILocation(line: 132, column: 29, scope: !153)
!166 = !DILocation(line: 132, column: 33, scope: !153)
!167 = !DILocalVariable(name: "index", scope: !153, file: !8, line: 132, type: !9)
!168 = !DILocation(line: 135, column: 20, scope: !153)
!169 = !DILocation(line: 135, column: 18, scope: !153)
!170 = !DILocation(line: 135, column: 10, scope: !153)
!171 = !DILocation(line: 135, column: 25, scope: !153)
!172 = !DILocation(line: 135, column: 23, scope: !153)
!173 = !DILocalVariable(name: "up", scope: !153, file: !8, line: 129, type: !18)
!174 = !DILocation(line: 138, column: 20, scope: !153)
!175 = !DILocation(line: 138, column: 12, scope: !153)
!176 = !DILocation(line: 138, column: 27, scope: !153)
!177 = !DILocation(line: 138, column: 25, scope: !153)
!178 = !DILocalVariable(name: "left", scope: !153, file: !8, line: 129, type: !18)
!179 = !DILocation(line: 141, column: 22, scope: !153)
!180 = !DILocation(line: 141, column: 20, scope: !153)
!181 = !DILocation(line: 141, column: 24, scope: !153)
!182 = !DILocation(line: 141, column: 12, scope: !153)
!183 = !DILocation(line: 141, column: 31, scope: !153)
!184 = !DILocation(line: 141, column: 29, scope: !153)
!185 = !DILocalVariable(name: "diag", scope: !153, file: !8, line: 129, type: !18)
!186 = !DILocalVariable(name: "max", scope: !153, file: !8, line: 144, type: !18)
!187 = !DILocalVariable(name: "pred", scope: !153, file: !8, line: 145, type: !18)
!188 = !DILocation(line: 161, column: 14, scope: !189)
!189 = distinct !DILexicalBlock(scope: !153, file: !8, line: 161, column: 9)
!190 = !DILocation(line: 161, column: 9, scope: !153)
!191 = !DILocation(line: 166, column: 12, scope: !192)
!192 = distinct !DILexicalBlock(scope: !153, file: !8, line: 166, column: 9)
!193 = !DILocation(line: 166, column: 9, scope: !153)
!194 = !DILocation(line: 171, column: 14, scope: !195)
!195 = distinct !DILexicalBlock(scope: !153, file: !8, line: 171, column: 9)
!196 = !DILocation(line: 171, column: 9, scope: !153)
!197 = !DILocation(line: 176, column: 5, scope: !153)
!198 = !DILocation(line: 176, column: 14, scope: !153)
!199 = !DILocation(line: 177, column: 5, scope: !153)
!200 = !DILocation(line: 177, column: 14, scope: !153)
!201 = !DILocation(line: 180, column: 17, scope: !202)
!202 = distinct !DILexicalBlock(scope: !153, file: !8, line: 180, column: 9)
!203 = !DILocation(line: 180, column: 15, scope: !202)
!204 = !DILocation(line: 180, column: 13, scope: !202)
!205 = !DILocation(line: 180, column: 9, scope: !153)
!206 = !DILocation(line: 181, column: 5, scope: !207)
!207 = distinct !DILexicalBlock(scope: !208, file: !8, line: 181, column: 5)
!208 = distinct !DILexicalBlock(scope: !202, file: !8, line: 180, column: 27)
!209 = !DILocation(line: 182, column: 17, scope: !207)
!210 = !DILocation(line: 182, column: 9, scope: !207)
!211 = !DILocation(line: 183, column: 5, scope: !208)
!212 = !DILocation(line: 185, column: 1, scope: !153)
!213 = distinct !DISubprogram(name: "calcFirstDiagElement", scope: !8, file: !8, line: 191, type: !214, scopeLine: 191, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!214 = !DISubroutineType(types: !215)
!215 = !{null, !157, !157, !157}
!216 = !DILocalVariable(name: "i", arg: 1, scope: !213, file: !8, line: 191, type: !157)
!217 = !DILocation(line: 0, scope: !213)
!218 = !DILocalVariable(name: "si", arg: 2, scope: !213, file: !8, line: 191, type: !157)
!219 = !DILocalVariable(name: "sj", arg: 3, scope: !213, file: !8, line: 191, type: !157)
!220 = !DILocation(line: 193, column: 9, scope: !221)
!221 = distinct !DILexicalBlock(scope: !213, file: !8, line: 193, column: 9)
!222 = !DILocation(line: 193, column: 14, scope: !221)
!223 = !DILocation(line: 193, column: 12, scope: !221)
!224 = !DILocation(line: 193, column: 9, scope: !213)
!225 = !DILocation(line: 194, column: 15, scope: !226)
!226 = distinct !DILexicalBlock(scope: !221, file: !8, line: 193, column: 17)
!227 = !DILocation(line: 194, column: 13, scope: !226)
!228 = !DILocation(line: 195, column: 13, scope: !226)
!229 = !DILocation(line: 196, column: 5, scope: !226)
!230 = !DILocation(line: 197, column: 15, scope: !231)
!231 = distinct !DILexicalBlock(scope: !221, file: !8, line: 196, column: 12)
!232 = !DILocation(line: 197, column: 17, scope: !231)
!233 = !DILocation(line: 197, column: 13, scope: !231)
!234 = !DILocation(line: 198, column: 15, scope: !231)
!235 = !DILocation(line: 198, column: 20, scope: !231)
!236 = !DILocation(line: 198, column: 18, scope: !231)
!237 = !DILocation(line: 198, column: 22, scope: !231)
!238 = !DILocation(line: 198, column: 13, scope: !231)
!239 = !DILocation(line: 200, column: 1, scope: !213)
!240 = distinct !DISubprogram(name: "main", scope: !8, file: !8, line: 203, type: !241, scopeLine: 203, flags: DIFlagPrototyped, spFlags: DISPFlagDefinition, unit: !2, retainedNodes: !4)
!241 = !DISubroutineType(types: !242)
!242 = !{!18, !18, !243}
!243 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !12, size: 64)
!244 = !DILocalVariable(name: "argc", arg: 1, scope: !240, file: !8, line: 203, type: !18)
!245 = !DILocation(line: 0, scope: !240)
!246 = !DILocalVariable(name: "argv", arg: 2, scope: !240, file: !8, line: 203, type: !243)
!247 = !DILocation(line: 204, column: 7, scope: !240)
!248 = !DILocation(line: 205, column: 7, scope: !240)
!249 = !DILocation(line: 212, column: 16, scope: !240)
!250 = !DILocation(line: 212, column: 18, scope: !240)
!251 = !DILocation(line: 212, column: 9, scope: !240)
!252 = !DILocation(line: 212, column: 7, scope: !240)
!253 = !DILocation(line: 213, column: 16, scope: !240)
!254 = !DILocation(line: 213, column: 18, scope: !240)
!255 = !DILocation(line: 213, column: 9, scope: !240)
!256 = !DILocation(line: 213, column: 7, scope: !240)
!257 = !DILocation(line: 216, column: 6, scope: !240)
!258 = !DILocation(line: 217, column: 6, scope: !240)
!259 = !DILocalVariable(name: "H", scope: !240, file: !8, line: 220, type: !156)
!260 = !DILocation(line: 220, column: 10, scope: !240)
!261 = !DILocation(line: 221, column: 16, scope: !240)
!262 = !DILocation(line: 221, column: 20, scope: !240)
!263 = !DILocation(line: 221, column: 18, scope: !240)
!264 = !DILocation(line: 221, column: 9, scope: !240)
!265 = !DILocation(line: 221, column: 7, scope: !240)
!266 = !DILocalVariable(name: "P", scope: !240, file: !8, line: 224, type: !156)
!267 = !DILocation(line: 224, column: 10, scope: !240)
!268 = !DILocation(line: 225, column: 16, scope: !240)
!269 = !DILocation(line: 225, column: 20, scope: !240)
!270 = !DILocation(line: 225, column: 18, scope: !240)
!271 = !DILocation(line: 225, column: 9, scope: !240)
!272 = !DILocation(line: 225, column: 7, scope: !240)
!273 = !DILocation(line: 229, column: 5, scope: !240)
!274 = !DILocalVariable(name: "maxPos", scope: !240, file: !8, line: 233, type: !9)
!275 = !DILocation(line: 233, column: 19, scope: !240)
!276 = !DILocalVariable(name: "i", scope: !240, file: !8, line: 235, type: !9)
!277 = !DILocation(line: 235, column: 19, scope: !240)
!278 = !DILocalVariable(name: "j", scope: !240, file: !8, line: 235, type: !9)
!279 = !DILocation(line: 235, column: 22, scope: !240)
!280 = !DILocation(line: 238, column: 26, scope: !240)
!281 = !DILocalVariable(name: "initialTime", scope: !240, file: !8, line: 238, type: !282)
!282 = !DIBasicType(name: "double", size: 64, encoding: DW_ATE_float)
!283 = !DILocalVariable(name: "si", scope: !240, file: !8, line: 240, type: !9)
!284 = !DILocation(line: 240, column: 19, scope: !240)
!285 = !DILocalVariable(name: "sj", scope: !240, file: !8, line: 240, type: !9)
!286 = !DILocation(line: 240, column: 23, scope: !240)
!287 = !DILocalVariable(name: "ai", scope: !240, file: !8, line: 240, type: !9)
!288 = !DILocation(line: 240, column: 27, scope: !240)
!289 = !DILocalVariable(name: "aj", scope: !240, file: !8, line: 240, type: !9)
!290 = !DILocation(line: 240, column: 31, scope: !240)
!291 = !DILocalVariable(name: "nDiag", scope: !240, file: !8, line: 243, type: !9)
!292 = !DILocation(line: 243, column: 19, scope: !240)
!293 = !DILocation(line: 243, column: 27, scope: !240)
!294 = !DILocation(line: 243, column: 31, scope: !240)
!295 = !DILocation(line: 243, column: 29, scope: !240)
!296 = !DILocation(line: 243, column: 33, scope: !240)
!297 = !DILocalVariable(name: "nEle", scope: !240, file: !8, line: 244, type: !9)
!298 = !DILocation(line: 244, column: 19, scope: !240)
!299 = !DILocation(line: 246, column: 5, scope: !240)
!300 = !DILocation(line: 262, column: 1, scope: !240)
!301 = distinct !DISubprogram(name: ".omp_outlined.", scope: !8, file: !8, line: 246, type: !302, scopeLine: 246, flags: DIFlagPrototyped, spFlags: DISPFlagLocalToUnit | DISPFlagDefinition, unit: !2, retainedNodes: !4)
!302 = !DISubroutineType(types: !303)
!303 = !{null, !304, !304, !308, !309, !309, !308}
!304 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !305)
!305 = !DIDerivedType(tag: DW_TAG_restrict_type, baseType: !306)
!306 = !DIDerivedType(tag: DW_TAG_pointer_type, baseType: !307, size: 64)
!307 = !DIDerivedType(tag: DW_TAG_const_type, baseType: !18)
!308 = !DIDerivedType(tag: DW_TAG_reference_type, baseType: !9, size: 64)
!309 = !DIDerivedType(tag: DW_TAG_reference_type, baseType: !156, size: 64)
!310 = !DILocalVariable(name: ".global_tid.", arg: 1, scope: !301, type: !304, flags: DIFlagArtificial)
!311 = !DILocation(line: 0, scope: !301)
!312 = !DILocalVariable(name: ".bound_tid.", arg: 2, scope: !301, type: !304, flags: DIFlagArtificial)
!313 = !DILocalVariable(name: "nDiag", arg: 3, scope: !301, type: !308, flags: DIFlagArtificial)
!314 = !DILocalVariable(name: "H", arg: 4, scope: !301, type: !309, flags: DIFlagArtificial)
!315 = !DILocalVariable(name: "P", arg: 5, scope: !301, type: !309, flags: DIFlagArtificial)
!316 = !DILocalVariable(name: "maxPos", arg: 6, scope: !301, type: !308, flags: DIFlagArtificial)
!317 = !{!318}
!318 = distinct !{!318, !319, !".omp_outlined._debug__: argument 0"}
!319 = distinct !{!319, !".omp_outlined._debug__"}
!320 = !DILocation(line: 246, column: 5, scope: !301)
!321 = !DILocation(line: 0, scope: !322, inlinedAt: !323)
!322 = distinct !DISubprogram(name: ".omp_outlined._debug__", scope: !8, file: !8, line: 248, type: !302, scopeLine: 248, flags: DIFlagPrototyped, spFlags: DISPFlagLocalToUnit | DISPFlagDefinition, unit: !2, retainedNodes: !4)
!323 = distinct !DILocation(line: 246, column: 5, scope: !301)
!324 = !DILocalVariable(name: ".global_tid.", arg: 1, scope: !322, type: !304, flags: DIFlagArtificial)
!325 = !DILocalVariable(name: ".bound_tid.", arg: 2, scope: !322, type: !304, flags: DIFlagArtificial)
!326 = !DILocalVariable(name: "nDiag", arg: 3, scope: !322, file: !8, line: 243, type: !308)
!327 = !DILocalVariable(name: "H", arg: 4, scope: !322, file: !8, line: 220, type: !309)
!328 = !DILocalVariable(name: "P", arg: 5, scope: !322, file: !8, line: 224, type: !309)
!329 = !DILocalVariable(name: "maxPos", arg: 6, scope: !322, file: !8, line: 233, type: !308)
!330 = !DILocalVariable(name: "i", scope: !322, type: !9, flags: DIFlagArtificial)
!331 = !DILocalVariable(name: "si", scope: !322, type: !9, flags: DIFlagArtificial)
!332 = !DILocalVariable(name: "sj", scope: !322, type: !9, flags: DIFlagArtificial)
!333 = !DILocation(line: 249, column: 16, scope: !334, inlinedAt: !323)
!334 = distinct !DILexicalBlock(scope: !335, file: !8, line: 249, column: 9)
!335 = distinct !DILexicalBlock(scope: !322, file: !8, line: 248, column: 5)
!336 = !DILocation(line: 249, column: 14, scope: !334, inlinedAt: !323)
!337 = !DILocation(line: 249, column: 21, scope: !338, inlinedAt: !323)
!338 = distinct !DILexicalBlock(scope: !334, file: !8, line: 249, column: 9)
!339 = !DILocation(line: 249, column: 26, scope: !338, inlinedAt: !323)
!340 = !{!"b1.ordered.false"}
!341 = !DILocation(line: 249, column: 23, scope: !338, inlinedAt: !323)
!342 = !DILocation(line: 249, column: 9, scope: !334, inlinedAt: !323)
!343 = !DILocation(line: 251, column: 29, scope: !344, inlinedAt: !323)
!344 = distinct !DILexicalBlock(scope: !338, file: !8, line: 250, column: 9)
!345 = !DILocation(line: 0, scope: !101, inlinedAt: !346)
!346 = distinct !DILocation(line: 251, column: 20, scope: !344, inlinedAt: !323)
!347 = !DILocation(line: 99, column: 13, scope: !107, inlinedAt: !346)
!348 = !{!"b16.ordered.false"}
!349 = !DILocation(line: 99, column: 11, scope: !107, inlinedAt: !346)
!350 = !DILocation(line: 99, column: 15, scope: !107, inlinedAt: !346)
!351 = !DILocation(line: 103, column: 18, scope: !111, inlinedAt: !346)
!352 = !{!"b15.ordered.false"}
!353 = !{!"b27.ordered.false"}
!354 = !{!"b14.ordered.false"}
!355 = !{!"b26.ordered.false"}
!356 = !DILocation(line: 103, column: 16, scope: !111, inlinedAt: !346)
!357 = !DILocation(line: 103, column: 14, scope: !107, inlinedAt: !346)
!358 = !DILocation(line: 105, column: 29, scope: !115, inlinedAt: !346)
!359 = !{!"b13.ordered.false"}
!360 = !{!"b25.ordered.false"}
!361 = !{!"b12.ordered.false"}
!362 = !{!"b24.ordered.false"}
!363 = !DILocation(line: 0, scope: !115, inlinedAt: !346)
!364 = !DILocation(line: 106, column: 20, scope: !115, inlinedAt: !346)
!365 = !DILocation(line: 106, column: 9, scope: !115, inlinedAt: !346)
!366 = !DILocation(line: 110, column: 29, scope: !121, inlinedAt: !346)
!367 = !{!"b11.ordered.false"}
!368 = !{!"b23.ordered.false"}
!369 = !{!"b10.ordered.false"}
!370 = !{!"b22.ordered.false"}
!371 = !DILocation(line: 0, scope: !121, inlinedAt: !346)
!372 = !DILocation(line: 111, column: 18, scope: !121, inlinedAt: !346)
!373 = !DILocation(line: 111, column: 24, scope: !121, inlinedAt: !346)
!374 = !DILocation(line: 111, column: 35, scope: !121, inlinedAt: !346)
!375 = !{!"b9.ordered.false"}
!376 = !DILocation(line: 111, column: 39, scope: !121, inlinedAt: !346)
!377 = !{!"b21.ordered.false"}
!378 = !DILocation(line: 111, column: 37, scope: !121, inlinedAt: !346)
!379 = !DILocation(line: 111, column: 30, scope: !121, inlinedAt: !346)
!380 = !DILocation(line: 111, column: 28, scope: !121, inlinedAt: !346)
!381 = !DILocation(line: 111, column: 42, scope: !121, inlinedAt: !346)
!382 = !DILocation(line: 111, column: 9, scope: !121, inlinedAt: !346)
!383 = !DILocation(line: 0, scope: !107, inlinedAt: !346)
!384 = !DILocalVariable(name: "nEle", scope: !322, type: !9, flags: DIFlagArtificial)
!385 = !DILocation(line: 0, scope: !213, inlinedAt: !386)
!386 = distinct !DILocation(line: 252, column: 13, scope: !344, inlinedAt: !323)
!387 = !DILocation(line: 193, column: 9, scope: !221, inlinedAt: !386)
!388 = !DILocation(line: 193, column: 14, scope: !221, inlinedAt: !386)
!389 = !{!"b20.ordered.false"}
!390 = !DILocation(line: 193, column: 12, scope: !221, inlinedAt: !386)
!391 = !DILocation(line: 193, column: 9, scope: !213, inlinedAt: !386)
!392 = !DILocation(line: 194, column: 15, scope: !226, inlinedAt: !386)
!393 = !DILocation(line: 194, column: 13, scope: !226, inlinedAt: !386)
!394 = !DILocation(line: 195, column: 13, scope: !226, inlinedAt: !386)
!395 = !DILocation(line: 196, column: 5, scope: !226, inlinedAt: !386)
!396 = !DILocation(line: 197, column: 15, scope: !231, inlinedAt: !386)
!397 = !{!"b19.ordered.false"}
!398 = !DILocation(line: 197, column: 17, scope: !231, inlinedAt: !386)
!399 = !DILocation(line: 197, column: 13, scope: !231, inlinedAt: !386)
!400 = !DILocation(line: 198, column: 15, scope: !231, inlinedAt: !386)
!401 = !DILocation(line: 198, column: 20, scope: !231, inlinedAt: !386)
!402 = !{!"b18.ordered.false"}
!403 = !DILocation(line: 198, column: 18, scope: !231, inlinedAt: !386)
!404 = !DILocation(line: 198, column: 22, scope: !231, inlinedAt: !386)
!405 = !DILocation(line: 198, column: 13, scope: !231, inlinedAt: !386)
!406 = !DILocalVariable(name: ".capture_expr.", scope: !407, type: !9, flags: DIFlagArtificial)
!407 = distinct !DILexicalBlock(scope: !344, file: !8, line: 253, column: 13)
!408 = !DILocation(line: 0, scope: !407, inlinedAt: !323)
!409 = !DILocation(line: 254, column: 13, scope: !407, inlinedAt: !323)
!410 = !DILocalVariable(name: "j", scope: !407, type: !9, flags: DIFlagArtificial)
!411 = !DILocation(line: 253, column: 13, scope: !344, inlinedAt: !323)
!412 = !DILocalVariable(name: ".omp.lb", scope: !407, type: !9, flags: DIFlagArtificial)
!413 = !DILocation(line: 254, column: 18, scope: !407, inlinedAt: !323)
!414 = !DILocalVariable(name: ".omp.ub", scope: !407, type: !9, flags: DIFlagArtificial)
!415 = !DILocalVariable(name: ".omp.stride", scope: !407, type: !9, flags: DIFlagArtificial)
!416 = !DILocalVariable(name: ".omp.is_last", scope: !407, type: !18, flags: DIFlagArtificial)
!417 = !DILocation(line: 253, column: 13, scope: !407, inlinedAt: !323)
!418 = !DILocalVariable(name: ".omp.iv", scope: !407, type: !9, flags: DIFlagArtificial)
!419 = !DILocation(line: 254, column: 36, scope: !407, inlinedAt: !323)
!420 = !DILocation(line: 256, column: 22, scope: !421, inlinedAt: !323)
!421 = distinct !DILexicalBlock(scope: !407, file: !8, line: 255, column: 13)
!422 = !DILocation(line: 256, column: 25, scope: !421, inlinedAt: !323)
!423 = !DILocation(line: 256, column: 29, scope: !421, inlinedAt: !323)
!424 = !DILocalVariable(name: "ai", scope: !322, type: !9, flags: DIFlagArtificial)
!425 = !DILocation(line: 257, column: 22, scope: !421, inlinedAt: !323)
!426 = !DILocation(line: 257, column: 25, scope: !421, inlinedAt: !323)
!427 = !DILocation(line: 257, column: 29, scope: !421, inlinedAt: !323)
!428 = !DILocalVariable(name: "aj", scope: !322, type: !9, flags: DIFlagArtificial)
!429 = !DILocation(line: 258, column: 41, scope: !421, inlinedAt: !323)
!430 = !{!"b2.ordered.false"}
!431 = !DILocation(line: 258, column: 44, scope: !421, inlinedAt: !323)
!432 = !{!"b3.ordered.false"}
!433 = !DILocation(line: 0, scope: !153, inlinedAt: !434)
!434 = distinct !DILocation(line: 258, column: 17, scope: !421, inlinedAt: !323)
!435 = !DILocation(line: 132, column: 27, scope: !153, inlinedAt: !434)
!436 = !{!"b8.ordered.false"}
!437 = !DILocation(line: 132, column: 29, scope: !153, inlinedAt: !434)
!438 = !DILocation(line: 132, column: 33, scope: !153, inlinedAt: !434)
!439 = !DILocation(line: 135, column: 20, scope: !153, inlinedAt: !434)
!440 = !{!"b7.ordered.false"}
!441 = !DILocation(line: 135, column: 18, scope: !153, inlinedAt: !434)
!442 = !DILocation(line: 135, column: 10, scope: !153, inlinedAt: !434)
!443 = !DILocation(line: 135, column: 25, scope: !153, inlinedAt: !434)
!444 = !{!"b42.ordered.false"}
!445 = !DILocation(line: 135, column: 23, scope: !153, inlinedAt: !434)
!446 = !DILocation(line: 138, column: 20, scope: !153, inlinedAt: !434)
!447 = !DILocation(line: 138, column: 12, scope: !153, inlinedAt: !434)
!448 = !DILocation(line: 138, column: 27, scope: !153, inlinedAt: !434)
!449 = !{!"b41.ordered.false"}
!450 = !DILocation(line: 138, column: 25, scope: !153, inlinedAt: !434)
!451 = !DILocation(line: 141, column: 22, scope: !153, inlinedAt: !434)
!452 = !{!"b6.ordered.false"}
!453 = !DILocation(line: 141, column: 20, scope: !153, inlinedAt: !434)
!454 = !DILocation(line: 141, column: 24, scope: !153, inlinedAt: !434)
!455 = !DILocation(line: 141, column: 12, scope: !153, inlinedAt: !434)
!456 = !DILocation(line: 141, column: 31, scope: !153, inlinedAt: !434)
!457 = !DILocation(line: 141, column: 29, scope: !153, inlinedAt: !434)
!458 = !DILocation(line: 161, column: 14, scope: !189, inlinedAt: !434)
!459 = !DILocation(line: 161, column: 9, scope: !153, inlinedAt: !434)
!460 = !DILocation(line: 166, column: 12, scope: !192, inlinedAt: !434)
!461 = !DILocation(line: 166, column: 9, scope: !153, inlinedAt: !434)
!462 = !DILocation(line: 171, column: 14, scope: !195, inlinedAt: !434)
!463 = !DILocation(line: 171, column: 9, scope: !153, inlinedAt: !434)
!464 = !DILocation(line: 176, column: 5, scope: !153, inlinedAt: !434)
!465 = !DILocation(line: 176, column: 14, scope: !153, inlinedAt: !434)
!466 = !DILocation(line: 177, column: 5, scope: !153, inlinedAt: !434)
!467 = !DILocation(line: 177, column: 14, scope: !153, inlinedAt: !434)
!468 = !DILocation(line: 180, column: 17, scope: !202, inlinedAt: !434)
!469 = !{!"b5.ordered.false"}
!470 = !DILocation(line: 180, column: 15, scope: !202, inlinedAt: !434)
!471 = !DILocation(line: 180, column: 13, scope: !202, inlinedAt: !434)
!472 = !DILocation(line: 180, column: 9, scope: !153, inlinedAt: !434)
!473 = !DILocation(line: 181, column: 5, scope: !207, inlinedAt: !434)
!474 = !DILocation(line: 182, column: 17, scope: !207, inlinedAt: !434)
!475 = !{!"b4.ordered.false"}
!476 = !DILocation(line: 182, column: 9, scope: !207, inlinedAt: !434)
!477 = !DILocation(line: 183, column: 5, scope: !208, inlinedAt: !434)
!478 = distinct !{!478, !417, !479}
!479 = !DILocation(line: 253, column: 28, scope: !407, inlinedAt: !323)
!480 = !DILocation(line: 249, column: 33, scope: !338, inlinedAt: !323)
!481 = !DILocation(line: 249, column: 9, scope: !338, inlinedAt: !323)
!482 = distinct !{!482, !342, !483, !67}
!483 = !DILocation(line: 260, column: 9, scope: !334, inlinedAt: !323)
!484 = !DILocation(line: 261, column: 5, scope: !322, inlinedAt: !323)
!485 = !{!486}
!486 = !{i64 2, i64 -1, i64 -1, i1 true}
