﻿// See https://aka.ms/new-console-template for more information
using Benchmark;
using BenchmarkDotNet.Running;

Console.WriteLine("Hello, World!");
//BenchmarkRunner.Run<SimpleBenchmark>();
BenchmarkRunner.Run<ArrayEmpty>(); 