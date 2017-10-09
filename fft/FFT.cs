using System;
using static System.Math;

namespace FrequencyAnalysis
{
    public partial class FFT
    {
        public static void four2(double[] data, long nn, int isign)
        {
            long n, mmax, m, j, istep, i;
            double wr, wi, dtheta, theta;
            double wxoddr, wxoddi;

            n = 2 * nn;
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

                        data[idx] = dir + wxoddr;
                        data[idx + 1] = dii + wxoddi;

                        data[jdx] = dir - wxoddr;
                        data[jdx + 1] = dii - wxoddi;
                    }
                }
                mmax *= 2;
            }
        }
    }
}