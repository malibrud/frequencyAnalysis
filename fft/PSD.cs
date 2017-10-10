using System;
using System.Diagnostics;
using static System.Math;

namespace FrequencyAnalysis
{
    public partial class PSD
    {
        public static void psd(double[] data, int Nfft, double dt)
        {
            Debug.Assert(isPowerOf2(Nfft));
            int Nolap = Nfft / 2;
            double[] window = makeHann(Nfft);

        }

        private static double[] makeHann(int nfft)
        {
            double[] w = new double[nfft];
            double sum = 0;
            for (int i = 0; i < nfft; i++)
            {
                w[i] = (1 - Cos(i * PI / (nfft - 1))) / 2;
                sum += w[i];
            }
            sum /= nfft;
            for (int i = 0; i < nfft; i++)
            {
                w[i] /= sum;
            }
            return w;
        }

        private static bool isPowerOf2(int nfft)
        {
            int mask = 2;
            for (int i = 0; i < 31; i++)
            {
                if ((nfft & ~mask) == 0) return true;
            }
            return false;
        }
    }
}