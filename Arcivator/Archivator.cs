using System;
using VeeamTestTask.Core.Interfaces;

namespace VeeamTestTask.Core
{
    public class Archivator : IDisposable
    {
        private IArchivator archivator;

        public Archivator(IArchivator archivator)
        {
            this.archivator = archivator;
        }

        public int Start()
        {
            return archivator.Start();
        }

        public void Dispose()
        {
            archivator.Dispose();
        }
    }
}
