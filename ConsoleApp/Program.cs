using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Process starting, input a number:");
            int myCount = Console.Read();
            testFunc(myCount);

            Console.WriteLine("Process finished.");

            Console.Read();
        }

        private static void testFunc(int myCount)
        {
            int[] array1 = { myCount };

            for (int i = 0; i < array1.Length; i++)
            {
                Console.WriteLine(array1[i]);
            }

        }
    }
}
