using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace JuntaoV5.FileProviders.Cached
{
    class AnotherFileInfo : IFileInfo
    {
        CachedFileProvider provider;
        readonly string subpath;

        public bool Exists => true;

        public bool IsDirectory => false;

        readonly DateTimeOffset lastModified;
        public DateTimeOffset LastModified
        {
            get { return lastModified; }
        }

        readonly long length;
        public long Length
        {
            get { return length; }
        }

        readonly string name;
        public string Name
        {
            get { return name; }
        }

        readonly string physicalPath;
        public string PhysicalPath
        {
            get { return physicalPath; }
        }

        private AnotherFileInfo() { }

        public AnotherFileInfo(CachedFileProvider provider, string subpath, DateTimeOffset lastModified, long length, string name, string physicalPath)
        {
            this.provider = provider;
            this.subpath = subpath;

            this.lastModified = lastModified;
            this.length = length;
            this.name = name;
            this.physicalPath = physicalPath;
        }

        public Stream CreateReadStream()
        {
            byte[] fileContent = provider.GetFileContent(subpath);
            return new MemoryStream(fileContent, false);
        }
    }
}
