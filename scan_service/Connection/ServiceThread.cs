using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using scan_service.Data;
using scan_service.Scanner;

namespace scan_service.Connection
{
    public class ServiceThread
    {
        private int _lastId = 0;
        private Dictionary<int, ScanResult> _results = new Dictionary<int, ScanResult>();
        private ConcurrentQueue<Task> _taskQueue = new ConcurrentQueue<Task>();

        private MalwareScanner _scanner = new MalwareScanner();

        public void Start()
        {
            while (true)
            {
                while (!_taskQueue.IsEmpty)
                {
                    if (!_taskQueue.TryDequeue(out var newTask))
                    {
                        return;
                    }

                    newTask.Start();
                    newTask.Wait();
                }
            }
        }

        public void ScanDirectory(string path, int id)
        {
            var scannerResult = _scanner.ScanDirectory(new DirectoryInfo(path));
            _results[id] = scannerResult;
        }

        public ScanResult GetResult(int id)
        {
            if (_results.ContainsKey(id))
            {
                return _results[id];
            }

            return null;
        }

        public int EnqueueScanTask(string path)
        {
            var newTask = new Task(() => ScanDirectory(path, _lastId));
            _taskQueue.Enqueue(newTask);

            _lastId++;
            return _lastId;
        }
    }
}