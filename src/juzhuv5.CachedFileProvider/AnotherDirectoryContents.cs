using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.Collections;

namespace JuntaoV5.FileProviders.Cached
{
    class AnotherDirectoryContents : IDirectoryContents
    {
        IEnumerable<IFileInfo> fileInfos;

        public AnotherDirectoryContents(IEnumerable<IFileInfo> fileInfos)
        {
            this.fileInfos = fileInfos;
        }

        public bool Exists => true;

        public IEnumerator<IFileInfo> GetEnumerator()
        {
            return fileInfos.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return fileInfos.GetEnumerator();
        }
    }
}
