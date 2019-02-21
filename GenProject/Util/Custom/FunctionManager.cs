using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenProject.Util.Custom
{
    public class FunctionManager
    {
        public static double MaxInArray(double[] f, int n)
        {
            double max = 0;
            for (int i = 0; i < n; i++)
                if (f[i] > max) max = f[i];
            return max;
        }

        public static double MinInArray(double[] f, int n)
        {
            double min = 100000000;
            for (int i = 0; i < n; i++)
                if (f[i] < min) min = f[i];
            return min;
        }

        public static void SortAscending(double[] h, int n)
        {
            int nm = h.Length;
            double p;
            for (int i = 1; i < n; i++)
                for (int j = 0; j < n - i; j++)
                    if (h[j] > h[j + 1])
                    {
                        p = h[j];
                        h[j] = h[j + 1];
                        h[j + 1] = p;
                    }

        }

        public static void SortDescending(double[] h, int n)
        {
            int nm = h.Length;
            double p;
            for (int i = 1; i < n; i++)
                for (int j = 0; j < n - i; j++)
                    if (h[j] < h[j + 1])
                    {
                        p = h[j];
                        h[j] = h[j + 1];
                        h[j + 1] = p;
                    }

        }

        public static int ToGrey(int a)
        {
            int y;
            y = a ^ (a >> 1);
            return y;

        }

        public static int FromGrey(int b, int kol)
        {
            int t = b;
            int result = b;
            for (int i = 0; i < kol; i++)
            {
                t = t >> 1;
                result = result ^ t;
            }
            return result;
        }

        //calculate function a
        public double Counter(String a, double[] X,int m)
        {

            double c = 0;
            a = ReplaceMathFunctions(a);
            a = GetFinalFunction(a, X, m);

            MathParser.Parser p = new MathParser.Parser();
            if (p.Evaluate(a)) c = p.Result;
            return c;

        }

        //adjust functions according to parser requirements
        public static String ReplaceMathFunctions(String a)
        {
            if (a.Contains("cos")) return a.Replace("cos", "Math.cos");
            return a;
        }

        //adjust array items according to parser requirements
        public static String GetFinalFunction(String func, double[] X, int m)
        {
            String result;
            for (int i = 0; i < m; i++)
            {
                func = func.Replace("X[" + i + "]", X[i].ToString());
            }

            result = func;
            return result;
        }

        //transform int a to binary d (array of int)
        private static void Zz_d(int a, int i, int j, int[, ,] A, int kol)
        {
            int y = 0;
            int k1 = 0;
            if (a == 0)
            {
                A[i, j, 0] = 0;
                k1 = 1;
            }

            while (a != 0)
            {
                if (a % 2 == 0) A[i, j, k1] = 0; else A[i, j, k1] = 1;
                k1++;
                a /= 2;
            }
            while (k1 % kol != 0)
            {
                A[i, j, k1] = 0;
                k1++;
            }
            for (int h = 0; h < (k1 / 2); h++)
            {
                y = A[i, j, h];
                A[i, j, h] = A[i, j, k1 - h - 1];
                A[i, j, k1 - h - 1] = y;
            }
        }

        private static int step(int a, int b) //= a^b
        {
            int pr = 1;
            for (int i = 0; i < b; i++)
                pr *= a;
            return pr;
        }

        // transformation of x[n][m] to an array of binary numbers A[n][m][kol]
        public static void Z_D(int[, ,] A, int kol, int n, int m, double[,] koord, double eps)
        {

            int total_min = 0;
            int total_len = n;

            MultiThread.RunThreads(total_len,
            (threadIndex, threadCount) =>
            {
                int len = total_len / threadCount;
                int lowBound = total_min + len * threadIndex;
                int hiBound = (threadIndex == threadCount - 1) ? total_len : lowBound + len;
                for (int i = lowBound; i < hiBound; i++)
                {
                    for (int j = 0; j < m; j++)
                    {
                        int b;
                        b = (int)(koord[i, j] / eps);
                        int b1 = ToGrey(b);//b1 - Grey version of b
                        Zz_d(b1, i, j, A, kol);//Grey->binary

                    }
                }
            });

        }

        //signle point transformation binary -> double
        public static void Dd_z(int u, int v, int kol, int[,,] A, double eps, double[,] koord, int m)
        {
            int a = 0, i;
            for (int j = 0; j < m; j++)
            {
                for (i = kol - 1; i >= 0; i--)
                    if (A[v, j, i] == 1) a += step(2, kol - 1 - i);
                //а - греевское число - порядковый номер интервала
                int a1 = FromGrey(a, kol);//а1 - нормальный порядковый номер интервала
                koord[u, j] = eps * a1;
                a = 0;
                a1 = 0;

            }
        }

        //all points transformation binary -> double
        public static void D_Z(int kol, int[, ,] A, double eps, double[,] koord, int m, int n)
        {

            for (int i = 0; i < n; i++)
                Dd_z(i, i,kol,A,eps,koord,m);

        }              
    }
}
