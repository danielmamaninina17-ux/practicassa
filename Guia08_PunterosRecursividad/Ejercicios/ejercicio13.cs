using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class ejercicio13
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Parejas de cuyes en una granja de Punata:");
            for (int mes = 1; mes <= 10; mes++)
            {
                Console.WriteLine("Mes " + mes + ": " + Fibonacci(mes) + " parejas");
            }
        }

        static int Fibonacci(int n)
        {
            if (n <= 2) 
            {
                return 1;
            }

            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
    }
}
