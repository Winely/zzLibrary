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
            var a = new MD5EncryptorClass().encrypt("123445");
            Console.WriteLine(a);
            Console.ReadKey();
        }
    }
}
