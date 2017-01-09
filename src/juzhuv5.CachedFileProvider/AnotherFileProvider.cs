using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System.IO;

namespace JuntaoV5.FileProviders.Cached
{

    public class CachedFileProvider : IFileProvider
    {
        IFileProvider provider;
        AnotherCache<IFileInfo> fileInfoCache = new AnotherCache<IFileInfo>();
        AnotherCache<IDirectoryContents> directoryContentsCache = new AnotherCache<IDirectoryContents>();
        AnotherCache<byte[]> fileContentCache = new AnotherCache<byte[]>();

        public CachedFileProvider(IFileProvider provider)
        {
            this.provider = provider;
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            IDirectoryContents contents;
            bool found = directoryContentsCache.TryGet(subpath, out contents);
            if (!found)
            {
                contents = provider.GetDirectoryContents(subpath);
                if (contents.Exists)
                {
                    LinkedList<IFileInfo> fileInfos = new LinkedList<IFileInfo>();
                    foreach (var i in contents)
                    {
                        IFileInfo info = new AnotherFileInfo(this, subpath + Path.DirectorySeparatorChar + i.Name, i.LastModified, i.Length, i.Name, i.PhysicalPath);
                        fileInfos.Append(info);
                    }
                    contents = new AnotherDirectoryContents(fileInfos);
                }
                else
                {
                    //Use the NotFoundDirectoryContents object returned by the internal provider
                }
                directoryContentsCache.Add(subpath, contents);
            }

            return contents;
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            IFileInfo info;
            bool found = fileInfoCache.TryGet(subpath, out info);
            if (!found)
            {
                info = provider.GetFileInfo(subpath);

                if (info.Exists)
                {
                    if (info.IsDirectory)
                    {
                        info = new AnotherDirectoryInfo(info.LastModified, info.Name, info.PhysicalPath);
                    }
                    else
                    {
                        info = new AnotherFileInfo(this, subpath, info.LastModified, info.Length, info.Name, info.PhysicalPath);
                    }
                }
                else
                {
                    //Use the NotFoundFileInfo object returned by the internal provider
                }
                fileInfoCache.Add(subpath, info);
            }

            return info;
        }

        internal byte[] GetFileContent(string subpath)
        {
            byte[] content;
            if (!fileContentCache.TryGet(subpath, out content))
            {

                IFileInfo fileInfo = provider.GetFileInfo(subpath);
                using (Stream stream = fileInfo.CreateReadStream())
                {
                    long bytesToRead = fileInfo.Length;
                    content = new byte[bytesToRead];
                    long bytesRead = 0;
                    int readCount;

                    while ((readCount = stream.Read(content, (int)bytesRead, (int)bytesToRead)) > 0)
                    {
                        bytesRead += readCount;
                        bytesToRead -= readCount;
                    }
                }
                fileContentCache.Add(subpath, content);
            }
            return content;
        }

        public IChangeToken Watch(string filter)
        {
            return provider.Watch(filter);
        }
    }
}
