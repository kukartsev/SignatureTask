using System;
using System.Threading;
using System.Security.Cryptography;

namespace Signature
{    
    class Worker : IDisposable
    {        
        private Thread workerThread;
        private AutoResetEvent ready;
        public AutoResetEvent finished { get; private set; }
        private bool exitflag;
        private byte[] buffer;
        private int bufferLength;
        private int number;

        /// <summary>
        /// Экземпляр класса содержит в себе рабочий поток, работающий до завершения всех рассчетов.
        /// </summary>
        public Worker()
        {
            finished = new AutoResetEvent(true);
            exitflag = false;
            ready = new AutoResetEvent(false);
            workerThread = new Thread(DoWork) { IsBackground = true };
            workerThread.Start();
        }
        /// <summary>
        /// После завершения хеширования, главный поток ожидает завершения всех рабочих потоков, и вызывает метод Dispose останавливающий потоки.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                exitflag = true;
                workerThread.Abort();
            }
        }
        /// <summary> 
        /// Метод выполняемый в рабочем потоке. Вычисляет хэш, инициирует вывод в консоль, ждет новые данные от главного потока.
        /// </summary>
        public void DoWork()
        {
            using (SHA256 hasher = SHA256.Create())
            {
                while (!exitflag)
                {
                    ready.WaitOne();
                    string hash = BitConverter.ToString(hasher.ComputeHash(buffer, 0, bufferLength)).Replace("-", string.Empty);
                    Console.WriteLine("{0}: {1}",number, hash);                                                                 
                    finished.Set();
                }
            }
        }
        /// <summary>
        /// Метод позволяющий главному потоку запустить ожидающий рабочий поток с новыми данными.
        /// </summary>
        public void WakeUp(byte[] buffer, int bufferLength, int number)
        {
            this.buffer = buffer;
            this.bufferLength = bufferLength;
            this.number = number;
            ready.Set();
        }
    }

}
