using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
//using EncryptorLib;

namespace test
{
    class Program
    {
        [DllImport(@"../../../Release/Penalty.dll", EntryPoint = "penalty", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]

        extern static double penalty(double time);
        static void Main(string[] args)
        {
            //var a = new MD5EncryptorClass().encrypt("123445");
            //Console.WriteLine(a);
            double time1 = 6;
            Console.WriteLine(penalty(time1));
            double time2 = 8;
            Console.WriteLine(penalty(time2));
            double time3 = 388;
            Console.WriteLine(penalty(time3));
            Console.ReadKey();
        }
    }
}
