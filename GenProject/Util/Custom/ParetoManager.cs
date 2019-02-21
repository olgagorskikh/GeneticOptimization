using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmegaMathEngine;
using ParetoSeparator;

namespace GenProject.Util.Custom
{
    public class ParetoManager
    {

        public static List<Vector> funcOpt = new List<Vector>();
        public static double[,] functOptim;
        public static double[,] koordOpt;
        public static int sizeParet;

        public static int kolvo = 0;//final size of Pareto Frontier
        public static double[,] Param;//final coordinates
        public static double[,] Koordinates;//final parameters values


        public static void GetParetoFront(int KolFunc, int n, double[,] f, functional[] funct, int m, double[,] koord)
        {
            // Find List<Vector> funcOpt - Pareto-set
            SeparatePareto(KolFunc,n,f,funct);

            //Transform result to functOptim and koordOpt (both double[,])
            Result(KolFunc, m, koord, f, n);

            //Get rid of duplicate items
            SeparateArrays(m, KolFunc, koordOpt);
        }

        public static void SeparateArrays(int m, int KolFunc, double[,] koordOpt)
        {
            double[] k = new double[m];//vector of coordinates
            double[] fu = new double[KolFunc];//vactor of parameters
            int h = 0;

            Param = new double[sizeParet, KolFunc];//final coordinates
            Koordinates = new double[sizeParet, m];//final parameters values


            for (int i = 0; i < sizeParet; i++)
            {
                for (int j = 0; j < KolFunc; j++)//selected a current vector
                    fu[j] = functOptim[i, j];


                if (!SearchArray(fu, KolFunc, Param, h))//whether it is in a frontier already
                {
                    for (int j = 0; j < KolFunc; j++)
                        Param[h, j] = fu[j];

                    for (int j = 0; j < m; j++)
                        Koordinates[h, j] = koordOpt[i, j];

                    h++;
                }

            }

            kolvo = h;//final size of Pareto Frontier

        }

        //whether the solution is in a frontier already
        public static bool SearchArray(double[] g, int gr, double[,] w, int h)
        {
            bool pr = false;

            for (int i = 0; i < h; i++)
            {
                pr = true;
                for (int j = 0; j < gr; j++)
                    if (g[j] != w[i, j]) pr = false;
                if (pr == true) return pr;
            }


            return pr;
        }

        public static void searchX(double[,] x, int r, int KolFunc, double[,] functOptim, double[,] f, int n, int m, double[,] koord)
        {
            bool pr;
            for (int i = 0; i < r; i++)
            {

                int k;

                for (k = 0; k < n; k++)
                {
                    pr = true;
                    for (int j = 0; j < KolFunc; j++)
                        if (functOptim[i, j] != f[k, j]) pr = false;
                    if (pr)//k - needed number
                        goto M1;
                }

            M1:
                for (int j = 0; j < m; j++)
                    x[i, j] = koord[k, j];
            }
        }

        //Processing Pareto Separation results
        public static void Result(int KolFunc, int m, double[,] koord, double[,] f, int n)
        {
            int razmer = funcOpt.Count;//teh size of Pareto set

            functOptim = new double[razmer, KolFunc];
            koordOpt = new double[razmer, m];

            ListVectorToDouble(functOptim, funcOpt, razmer,KolFunc);//wrote 
            searchX(koordOpt, razmer,KolFunc,functOptim,f,n,m,koord);
            sizeParet = razmer;

        }

        public static void DoubleToListVector(double[,] a, List<Vector> t, int r, int KolFunc)
        {
            for (int i = 0; i < r; i++)
                for (int j = 0; j < KolFunc; j++)
                    t[i].Store[j] = a[i, j];
        }

        public static void ListVectorToDouble(double[,] a, List<Vector> t, int r, int KolFunc)
        {
            for (int i = 0; i < r; i++)
                for (int j = 0; j < KolFunc; j++)
                    a[i, j] = t[i].Store[j];
        }

        //selection of Pareto-effective points
        public static void SeparatePareto(int KolFunc, int n, double[,] f, functional[] funct)
        {
            List<Vector> popul = new List<Vector>();//Whole population
            for (int i = 0; i < n; i++)             //
                popul.Add(new Vector(KolFunc));     //

            DoubleToListVector(f, popul, n, KolFunc);//transformed double to List<Vector>

            //Allocating aims
            ParetoAim[] Aims = new ParetoAim[KolFunc];

            for (int i = 0; i < KolFunc; i++)
                if (funct[i].mima == 1)
                    Aims[i] = ParetoAim.Max;
                else
                    Aims[i] = ParetoAim.Min;

            Separator t = new Separator(popul) { Aims = Aims };
            funcOpt = t.Separate();
        }
    }
}
