using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

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


        static void four2(double[] data, long nn, int isign)
        {
            long n, mmax, m, j, istep, i;
            double wr, wi, dtheta, theta;
            double wxoddr, wxoddi;

            n = 2*nn;
            j = 1;
            for (i = 1; i < n; i += 2)
            {
                if (j > i)
                {
                    SWAP(ref data[j - 1], ref data[i - 1]);
                    SWAP(ref data[j], ref data[i]);
                }
                m = n >> 1;
                while (m >= 2 && j > m)
                {
                    j -= m;
                    m /= 2;
                }
                j += m;
            }

            mmax = 1;
            while (mmax < nn)
            {
                //Console.WriteLine("O - mmax = {0}", mmax);
                istep = 2 * mmax;
                dtheta = isign * (Math.PI / mmax);
                for (m = 0; m < mmax; m++)
                {
                    //Console.WriteLine("  I - m = {0}", m);
                    theta = dtheta * m;
                    wr = Cos(theta);
                    wi = Sin(theta);
                    for (i = m; i < nn; i += istep)
                    {
                        j = i + mmax;
                        //Console.WriteLine("    B - i,j = {0}, {1}", i, j);
                        var idx = 2 * i;
                        var jdx = 2 * j;

                        var dir = data[idx];
                        var dii = data[idx + 1];
                        var djr = data[jdx];
                        var dji = data[jdx + 1];

                        wxoddr = wr * djr - wi * dji;
                        wxoddi = wr * dji + wi * djr;

                        data[idx]   = dir + wxoddr;
                        data[idx + 1] = dii + wxoddi;

                        data[jdx]   = dir - wxoddr;
                        data[jdx + 1] = dii - wxoddi;
                    }
                }
                mmax *= 2;
            }
        }

        static void four1(double[] data,  long nn, int isign)
        {
            long n, mmax, m, j, istep, i;
            double wtemp, wr, wpr, wpi, wi, theta;
            double tempr, tempi;

            n = nn << 1;
            j = 1;
            for (i = 1; i < n; i += 2)
            {
                if (j > i)
                {
                    SWAP(ref data[j-1], ref data[i-1]);
                    SWAP(ref data[j], ref data[i]);
                }
                m = n >> 1;
                while (m >= 2 && j > m)
                {
                    j -= m;
                    m >>= 1;
                }
                j += m;
            }
            mmax = 2;
            while (n > mmax)
            {
                istep = mmax << 1;
                theta = isign * (6.28318530717959 / mmax);
                wtemp = Math.Sin(0.5 * theta);
                wpr = -2.0 * wtemp * wtemp;
                wpi = Math.Sin(theta);
                wr = 1.0;
                wi = 0.0;
                for (m = 1; m < mmax; m += 2)
                {
                    for (i = m; i <= n; i += istep)
                    {
                        j = i + mmax;
                        tempr = wr * data[j-1] - wi * data[j];
                        tempi = wr * data[j] + wi * data[j-1];
                        data[j-1] = data[i-1] - tempr;
                        data[j] = data[i] - tempi;
                        data[i-1] += tempr;
                        data[i] += tempi;
                    }
                    wr = (wtemp = wr) * wpr - wi * wpi + wr;
                    wi = wi * wpr + wtemp * wpi + wi;
                }
                mmax = istep;
            }
        }
        static void SWAP(ref double a, ref double b)
        {
            double t = a;
            a = b;
            b = t;
        }
    }
}
