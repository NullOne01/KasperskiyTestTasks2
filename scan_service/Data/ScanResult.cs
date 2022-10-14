using System;
using System.Collections.Generic;

namespace scan_service.Data
{
    public class ScanResult
    {
        public long ExecutionTime { private get; set; } = 0;
        public int ErrorsNum { get; set; } = 0;
        public int FileProcessedNum { get; set; } = 0;

        private readonly Dictionary<string, int> _malwareCounter = new Dictionary<string, int>();

        public void LoadMalwareName(string malwareName)
        {
            if (!_malwareCounter.ContainsKey(malwareName))
            {
                _malwareCounter.Add(malwareName, 0);
            }
            
            _malwareCounter[malwareName]++;
        }

        public override string ToString()
        {
            // Processed files: 150
            // JS detects: 5
            // rm -rf detects: 1
            // Rundll32 detects: 2
            // Errors: 1
            // Execution time: 00:00:31

            TimeSpan timeSpan = TimeSpan.FromMilliseconds(ExecutionTime);
            
            var answerStr = $"Processed files: {FileProcessedNum}\n" +
                            $"JS detects: {_malwareCounter.GetValueOrDefault("MALWARE_JS")}\n" +
                            $"rm -rf detects: {_malwareCounter.GetValueOrDefault("MALWARE_RM_RF")}\n" +
                            $"Rundll32 detects: {_malwareCounter.GetValueOrDefault("MALWARE_SUS")}\n" +
                            $"Errors: {ErrorsNum}\n" +
                            $"Execution time: {timeSpan}";

            return answerStr;
        }
    }
}