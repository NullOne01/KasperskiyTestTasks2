using System;
using System.IO;
using scan_service.Input;
using scan_service.Scanner;

namespace scan_service
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