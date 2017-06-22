using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using EncryptorLib;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var e = new EncryptorClass();
            Console.WriteLine(e.Add(23, 45));
            Console.ReadKey();
        }
    }
}
