using System;
using System.IO;
using scan_util.Input;
using scan_util.Scanner;

namespace scan_util
{
    class Program
    {
        private static void Main(string[] args)
        {
            ArgumentReader argumentReader = new ArgumentReader();
            DirectoryInfo directory = argumentReader.ReadDirectoryInfo(args, 0);

            MalwareScanner scanner = new MalwareScanner();
            var scannerResult = scanner.ScanDirectory(directory);
            
            Console.WriteLine("====== Scan result ======");
            Console.WriteLine(scannerResult);
            Console.WriteLine("=========================");
        }
    }
}