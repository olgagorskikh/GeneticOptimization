/*
 * Copyright https://usings.ru/2009/06/19/multithread/
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GenProject
{

    /// <summary>
    /// Класс предназначен для параллельного выполнения вычислений.
    /// Эффективен в случае многоядерных процессоров (включая процессора с HT).
    /// <remarks>В вычислительной функции для потоков: при использовании локальных переменных вызывающего метода вместо внутренних полей класса, содержащего этот метод - резкое снижение эффективности.</remarks>
    /// </summary>
    public class MultiThread
    {
        /// <summary>
        /// Вычислительная функция для потока threadIndex
        /// </summary>
        /// <param name="threadIndex">Порядковый номер потока</param>
        /// <param name="threadCount">Количество созданных потоков</param>
        public delegate void ThreadHandler(int threadIndex, int threadCount);


        static MultiThread()
        {
            int maxIOThreads;

            ThreadPool.GetMinThreads(out minThreads, out maxIOThreads);
        }

        /// <summary>
        /// Создать класс с максимально допустимым количеством потоков MaxThreadsCount
        /// </summary>
        /// <param name="MaxThreadsCount">Максимально допустимое количество потоков</param>
        public MultiThread(int MaxThreadsCount)
        {
            threadCount = MaxThreadsCount < minThreads ? MaxThreadsCount : minThreads;
        }

        /// <summary>
        /// Создать класс с максимально допустимым количеством потоков MaxThreadsCount
        /// </summary>
        /// <param name="MinThreadsCount">Миниимально допустимое количество потоков</param>
        /// <param name="MaxThreadsCount">Максимально допустимое количество потоков</param>
        public MultiThread(int MinThreadsCount, int MaxThreadsCount)
        {
            if (MinThreadsCount > MaxThreadsCount)
                MinThreadsCount = 1;

            threadCount = MaxThreadsCount < minThreads ? MaxThreadsCount : minThreads;

            if (MinThreadsCount > threadCount)
                threadCount = MinThreadsCount;
        }

        /// <summary>
        /// Создать класс с максимально допустимым количеством потоков MaxThreadsCount и дождаться окончания выполнения вычислительной функции для потоков handler
        /// </summary>
        /// <param name="MaxThreadsCount">Максимально допустимое количество потоков</param>
        /// <param name="handler">Вычислительная функция для потоков</param>
        public MultiThread(int MaxThreadsCount, ThreadHandler handler)
            : this(MaxThreadsCount)
        {
            Run(handler);
        }

        /// <summary>
        /// Создать класс с максимально допустимым количеством потоков MaxThreadsCount и дождаться окончания выполнения вычислительной функции для потоков handler
        /// </summary>
        /// <param name="MinThreadsCount">Миниимально допустимое количество потоков</param>
        /// <param name="MaxThreadsCount">Максимально допустимое количество потоков</param>
        /// <param name="handler">Вычислительная функция для потоков</param>
        public MultiThread(int MinThreadsCount, int MaxThreadsCount, ThreadHandler handler)
            : this(MinThreadsCount, MaxThreadsCount)
        {
            Run(handler);
        }

        /// <summary>
        /// Запустить вычисления в функции для потоков handler и дождаться окончания выполнения
        /// </summary>
        /// <param name="MaxThreadsCount">Максимально допустимое количество потоков</param>
        /// <param name="handler">Вычислительная функция для потоков</param>
        public static void RunThreads(int MaxThreadsCount, ThreadHandler handler)
        {
            new MultiThread(MaxThreadsCount, handler);
        }

        /// <summary>
        /// Запустить вычисления в функции для потоков handler и дождаться окончания выполнения
        /// </summary>
        /// <param name="handler">Вычислительная функция для потоков</param>
        public void Run(ThreadHandler handler)
        {
            Thread[] t = new Thread[threadCount];

            for (int j = 0; j < threadCount; j++)
            {
                t[j] = new Thread(delegate(object tn)
                {
                    handler((int)tn, threadCount);
                });
                t[j].Start(j);
            }

            for (int j = 0; j < threadCount; j++)
                t[j].Join();
        }

        /// <summary>
        /// NUmber of threads created
        /// </summary>
        public int ThreadCount
        {
            get { return threadCount; }
        }

        private readonly int threadCount;
        private readonly static int minThreads;

    }
}
