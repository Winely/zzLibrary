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
            var e = new MD5EncryptorClass();
            while (true)
            {
                var a = Console.ReadLine();
                Console.WriteLine(e.encrypt(a));
            }
            Console.ReadKey();
        }
    }
}
