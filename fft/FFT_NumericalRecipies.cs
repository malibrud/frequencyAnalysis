using System;

namespace FrequencyAnalysis
{
     public partial class FFT
     {
        public static void four1(double[] data, long nn, int isign)
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
                    SWAP(ref data[j - 1], ref data[i - 1]);
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
                        tempr = wr * data[j - 1] - wi * data[j];
                        tempi = wr * data[j] + wi * data[j - 1];
                        data[j - 1] = data[i - 1] - tempr;
                        data[j] = data[i] - tempi;
                        data[i - 1] += tempr;
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