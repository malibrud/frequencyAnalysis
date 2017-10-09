using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using static FrequencyAnalysis.FFT;

namespace fft
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 4096;
            double f = 2 * Math.PI / N;
            double[] data1 = new double[2 * N];
            double[] data2 = new double[2 * N]; ;

            for (int i = 0; i < N; i++)
            {
                data1[2*i] = 1 + Math.Cos(f * i) + Math.Sin(3*f * i);
                data1[2*i + 1] = 0;
            }
            Array.Copy(data1, data2, 2 * N);


            var sw = new Stopwatch();
            var M = 1_000_000;
            var freq = Stopwatch.Frequency;
            sw.Start();
            four1(data1, N, -1);
            sw.Stop();
            Console.WriteLine("four1 = {0} us", M*sw.ElapsedTicks / freq);
            sw.Reset();

            sw.Start();
            four2(data2, N, -1);
            sw.Stop();
            Console.WriteLine("four2 = {0} us", M*sw.ElapsedTicks / freq);


            double sum = 0;
            for (int i = 0; i < 2*N; i++)
            {
                sum += Abs(data1[i] - data2[i]);
            }
            Console.WriteLine("Abs Difference = {0}", sum);
            //Console.WriteLine("");
            //for (int i = 0; i < 2*N; i+=2)
            //{
            //    if (Math.Abs(data1[i]) < 1e-6) data1[i] = 0;
            //    if (Math.Abs(data1[i+1]) < 1e-6) data1[i+1] = 0;
            //    Console.WriteLine("data1[{2}] = {0} +{1} i", data1[i], data1[i + 1], i/2);
            //}
            //Console.WriteLine("");
            //for (int i = 0; i < 2 * N; i += 2)
            //{
            //    if (Math.Abs(data2[i]) < 1e-6) data2[i] = 0;
            //    if (Math.Abs(data2[i + 1]) < 1e-6) data2[i + 1] = 0;
            //    Console.WriteLine("data2[{2}] = {0} +{1} i", data2[i], data2[i + 1], i / 2);
            //}
            Console.WriteLine("Done");
        }
    }
}
