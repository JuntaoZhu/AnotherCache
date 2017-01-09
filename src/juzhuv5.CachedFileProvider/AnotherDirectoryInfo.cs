using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace JuntaoV5.FileProviders.Cached
{
    class AnotherDirectoryInfo : IFileInfo
    {
        public bool Exists => true;

        public bool IsDirectory => true;

        readonly DateTimeOffset lastModified;
        public DateTimeOffset LastModified
        {
            get { return lastModified; }
        }

        public long Length => -1;

        readonly string name;
        public string Name
        {
            get { return Name; }
        }

        readonly string physicalPath;
        public string PhysicalPath
        {
            get { return physicalPath; }
        }

        private AnotherDirectoryInfo() { }

        public AnotherDirectoryInfo(DateTimeOffset lastModified, string name, string physicalPath)
        {
            this.lastModified = lastModified;
            this.name = name;
            this.physicalPath = physicalPath;
        }

        public Stream CreateReadStream()
        {
            throw new InvalidOperationException("Cannot create a stream for a directory.");
        }
    }
}
