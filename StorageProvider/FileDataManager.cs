using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Orleans;

namespace StorageProvider
{
    internal class FileDataManager : IDisposable
    {
        private readonly object _lockObject = new object();
        private readonly DirectoryInfo _directory;
        private readonly Dictionary<string, object> _lockObjects = new Dictionary<string, object>();

        public FileDataManager(string storageDirectory)
        {
            _directory = new DirectoryInfo(storageDirectory);
            if (!_directory.Exists)
                _directory.Create();
        }

        public Task Delete(string collectionName, string key)
        {
            FileInfo fileInfo = GetStorageFilePath(collectionName, key);

            if (fileInfo.Exists)
                fileInfo.Delete();

            return TaskDone.Done;
        }

        public async Task<string> Read(string collectionName, string key)
        {
            FileInfo fileInfo = GetStorageFilePath(collectionName, key);

            if (!fileInfo.Exists)
                return null;

            var lo = GetLock(fileInfo.FullName);
            lock (lo)
            {
                using (var stream = fileInfo.OpenText())
                {
                    return stream.ReadToEnd();
                }
            }
        }

        private object GetLock(string filename)
        {
            object lo = null;
            lock (_lockObjects)
            {
                _lockObjects.TryGetValue(filename, out lo);
                if (null == lo)
                {
                    lo = new object();
                    _lockObjects.Add(filename, lo);
                }
            }
            return lo;
        }

        public Task Write(string collectionName, string key, string entityData)
        {
            FileInfo fileInfo = GetStorageFilePath(collectionName, key);
            var lo = GetLock(fileInfo.FullName);
            lock (lo)
            {
                using (var str = fileInfo.Open(FileMode.Create, FileAccess.Write))
                using (var stream = new StreamWriter(str))
                {
                    stream.Write(entityData);
                }
            }

            return TaskDone.Done;
        }

        public void Dispose()
        {
        }

        private FileInfo GetStorageFilePath(string collectionName, string key)
        {
            string fileName = key + "." + collectionName;
            string path = Path.Combine(_directory.FullName, fileName);
            return new FileInfo(path);
        }

    }
}
