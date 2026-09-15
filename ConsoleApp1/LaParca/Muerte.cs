using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.LaParca
{
    public class Muerte
    {
        public static void Main(string[] args)
        {

            int a;
            int b;

            Console.WriteLine("Dame el valor de A: ");
            a = int.Parse(Console.ReadLine());
            Console.WriteLine("Dame el valor de B: ");
            b = int.Parse(Console.ReadLine());

            int suma = a + b;

           Console.WriteLine("La suma es: " + suma );


         }

    }
}
