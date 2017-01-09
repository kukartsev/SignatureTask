using System;
using System.IO;
using System.Threading;

namespace Signature
{
    class HashCalculator
    {
        private AutoResetEvent[] readyToWorkFlags;
        private Worker[] workers;
        /// <summary>
        /// Класс управляет рабочими потоками, рассчитывающими хеш. Количество создаваемых потоков равно количеству ядер процессора.
        /// </summary>
        public HashCalculator()
        {            
            workers = new Worker[Environment.ProcessorCount];
            readyToWorkFlags = new AutoResetEvent[workers.Length];                       
        }
        /// <summary>
        /// Создание рабочих потоков
        /// </summary>
        private void CreateWorkers()
        {
            for (int i = 0; i < workers.Length; i++)
            {
                workers[i] = new Worker();
                readyToWorkFlags[i] = workers[i].finished;
            }
        }
        /// <summary>
        /// Считывает блок из файла, ждет, когда освободится хотя бы один из потоков, и передает блок освободившемуся рабочему потоку. 
        /// </summary>
        public void Calculate(string path, int blockSize)
        {
            CreateWorkers();
            FileStream stream = File.OpenRead(path);
            long size = stream.Length;            
            long blockCount = (size + blockSize - 1) / blockSize;
            using (stream)
            {
                for (int i = 0; i < blockCount; i++)
                {                    
                    byte[] buffer = new byte[blockSize];
                    int readed = stream.Read(buffer, 0, blockSize);

                    int index = WaitHandle.WaitAny(readyToWorkFlags);
                    workers[index].WakeUp(buffer, readed, i);
                }
            }
            Finalise();
        }
        /// <summary>
        /// Дожидается завершения вычисений во всех рабочих потоках и завершает их.
        /// </summary>
        public void Finalise()
        {
            WaitHandle.WaitAll(readyToWorkFlags);
            for (int i = 0; i < workers.Length; i++) workers[i].Dispose();
        }
    }
}
