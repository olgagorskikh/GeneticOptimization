using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OmegaMathEngine;
using GenProject.Util.Custom;
using ParetoSeparator;

namespace GenProject
{
    public class GenAlgorythm
    {
        
        #region GenAlgorythm fields

            public double[,] koord; //double coordinates of all points in a population
            public int[, ,] A;//binary coordinates of all points in a population
            public double[,] f;//array of fitnesses of all points in a population      
            public double[] otvet;//coordinates of an optimal solution
            public functional[] funct;//array of target functions
            public uslov[] uslovia;//array of conditions

            public int KolPok;//number of generations
            public int KolFunc;//number of target functions
            public int KolOgr;//number of conditions
            public static int n;//number of points
            public int m;//number of coordinates
            public static int kol;//number of bits for a single number            

            public double eps;//precision
            public int pok = 1;//number of a current generation  
            public int nomerFunc = 0;//number of a current target function
            public int maxvalue;//maximum limit of coordinates

            public MainForm form;//main form

        # endregion

        //initialize all the arrays
        private void InitializeArrays()
        {            
            koord = new double[n, m];
            A = new int[n, m, kol];
            f = new double[n, KolFunc];
            otvet = new double[m];
        }

        //generate initial population (koord[n][m])
        private void GenerateStartPopulation()
        {
            int s;

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
                    Random rnd = new Random();
                    do
                    {
                        for (int j = 0; j < m; j++)
                        {
                            s = RNG.Next(9);
                            koord[i, j] = RNG.Next(maxvalue);
                            koord[i, j] += eps * s;
                        }

                    }

                    while (!Check(i));

                }
            });

        }

        //main run
        private void StartCalculations()
        {
            //generating initial populatoin
            GenerateStartPopulation();
            Random rnd = new Random();

            while (pok <= KolPok)//while number of generations is less than stated
            {
                nomerFunc = RNG.Next(KolFunc - 1);//number of a target function for the crossover
                FunctionManager.Z_D(A,kol,n,m,koord,eps);//decimal to binary transformation
                FillFunctionArray();//calculate array of fitnesses

                int total_min = 0;
                int total_len = n;
                MultiThread.RunThreads(total_len,
                (threadIndex, threadCount) =>
                {
                    int len = total_len / threadCount;
                    int lowBound = total_min + len * threadIndex;
                    int hiBound = (threadIndex == threadCount - 1) ? total_len : lowBound + len;
                    for (int k = lowBound; k < hiBound; k++)
                    {
                        int a = Choice();//number of the first point
                        int b = Choice();//number of the second point
                        Crossing(a, b, k);
                    }
                });

                FillFunctionArray();//calculate array of updated fitnesses
                pok++;
                form.pBar1.PerformStep();
            }

            ParetoManager.GetParetoFront(KolFunc,n,f,funct,m,koord);
        }

        //imitating random selection of points for crossover
        private int Choice()
        {
            int zn = funct[nomerFunc].mima;

            double[] u = new double[n];
            for (int i = 0; i < n; i++)//copying f[n] to u[n]
                u[i] = f[i, nomerFunc];

            //sort array of fitnesses in ascending order
            if (zn == 1) FunctionManager.SortDescending(u,n);
            else FunctionManager.SortAscending(u,n);

            //model a random selection
            Random rnd = new Random();
            double prisp = 0;
            int t = RNG.Next(n / 2);
            prisp = u[t];

            int nomer = 0;
            for (int i = 0; i < n; i++)
                if (prisp == f[i, nomerFunc]) { nomer = i; goto m1; }
            m1:
                return nomer;
        }

        //crossover of points with numbers a and b
        private void Crossing(int a, int b, int num)
        {
            int razriv;
            Random rnd = new Random();

            m2:

                razriv = RNG.Next(kol); //breakpoint of chromosome

                for (int j = 0; j < m; j++)
                {
                    for (int k = 0; k < razriv; k++)//coordinate-wise crossover
                    {
                        A[b, j, k] = A[a, j, k];
                    }

                }

                //INVERSION with probability 1%
                int t = rnd.Next(100);
                int nom = rnd.Next(m);
                if (t >= 50)
                    for (int j = 0; j < 12; j++)
                        if (A[b, nom, j] == 1) A[b, nom, j] = 0;
                        else A[b, nom, j] = 1;


                //Sacing the child number num to array x[n]:
                FunctionManager.Dd_z(num, b, kol, A, eps, koord, m);             


                if (!Check(num)) goto m2;

        }  

        //fill array of fitnesses f[n]
        private void FillFunctionArray()
        {
            int total_min = 0;
            int total_len = n;
            double[] A = new double[total_len];

            MultiThread.RunThreads(total_len,
            (threadIndex, threadCount) =>
            {
                int len = total_len / threadCount;
                int lowBound = total_min + len * threadIndex;
                int hiBound = (threadIndex == threadCount - 1) ? total_len : lowBound + len;
                for (int i = lowBound; i < hiBound; i++)
                {
                    double[] a;
                    a = new double[m]; // coordinates of a current point
                    for (int j = 0; j < m; j++) 
                        a[j] = koord[i, j];

                    for (int y = 0; y < KolFunc; y++)//column of all parameters
                    {
                        f[i, y] = 0;
                        String b = funct[y].fun;
                        b = FunctionManager.ReplaceMathFunctions(b);
                        b = FunctionManager.GetFinalFunction(b, a, m);

                        MathParser.Parser p = new MathParser.Parser();
                        if (p.Evaluate(b)) f[i, y] = p.Result;
                    }
                }
            });

        }

        // check for being into feasible area
        private bool Check(int nom)
        {
            double[] mas;
            mas = new double[m]; // coordinates of point number nom

            for (int j = 0; j < m; j++) 
                mas[j] = koord[nom, j];

            //building conditions
            bool res = true;
            for (int i = 0; i < KolOgr; i++)
            {
                double l = 0;
                String lef = uslovia[i].left;
                lef = FunctionManager.ReplaceMathFunctions(lef);
                lef = FunctionManager.GetFinalFunction(lef, mas, m);
                MathParser.Parser p = new MathParser.Parser();
                if (p.Evaluate(lef)) l = p.Result;

                double r = 0;
                String rig = uslovia[i].right;
                rig = FunctionManager.ReplaceMathFunctions(rig);
                rig = FunctionManager.GetFinalFunction(rig, mas, m);

                MathParser.Parser p1 = new MathParser.Parser();
                if (p1.Evaluate(rig)) r = p1.Result;

                switch (uslovia[i].znak)
                {
                    case "==": if (!(l == r)) res = false; break;
                    case "!=": if (!(l != r)) res = false; break;
                    case ">=": if (!(l >= r)) res = false; break;
                    case "<=": if (!(l <= r)) res = false; break;
                    case ">": if (!(l > r)) res = false; break;
                    case "<": if (!(l < r)) res = false; break;
                }
            }


            return res;

        }

        //class constructor
        public GenAlgorythm(int nn, int mm, double epss, int koll, uslov[] usloviaa, int kolOgrr, functional[] functionall, int KolPokl, int maxval, MainForm _form)
        {
            
            form = _form;
            maxvalue = maxval;
            n = nn;
            m = mm;
            kol = koll;
            eps = epss;
            uslovia = usloviaa;
            funct = functionall;
            KolOgr = kolOgrr;
            KolPok = KolPokl;
            KolFunc = funct.Length;

            //progressbar parameters
            form.pBar1.Visible = true;
            form.pBar1.Minimum = 1;
            form.pBar1.Maximum = KolPok + 1;
            form.pBar1.Value = 1;
            form.pBar1.Step = 1;

            //allocate memory for arrays
            InitializeArrays();

            //main processing
            StartCalculations();
        }
            
    }

        // structure for target functions
        public struct functional 
        {
            public string fun;//function
            public int mima; //its direction (max - 1, min - 0)
        };

        // structure for conditions
        public struct uslov 
        {
            public string left;//left side of a condition
            public string znak;//condition sign
            public string right;//left side of a condition
        };
    }
