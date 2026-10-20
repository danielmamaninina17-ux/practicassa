using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    public class ejercicio11
    {
        public static void Main(string[] args)
        {
            int ci = 8456123;
            Console.WriteLine("El CI " + ci + " tiene " + ContarDigitos(ci) + " dígitos");
        }

        static int ContarDigitos(int n)
        {
            if (n < 10)
            {
                return 1;
            }

            return 1 + ContarDigitos(n / 10);
        }
    }
}