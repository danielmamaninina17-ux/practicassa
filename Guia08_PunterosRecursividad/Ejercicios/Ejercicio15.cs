using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class Ejercicio15
    {
        public static void Main(string[] args)
        {
            int pasajes = 4;
            int precio = 35;
            Console.WriteLine(pasajes + " pasajes Cochabamba - Santa Cruz a Bs " + precio);
            Console.WriteLine("Total: Bs " + Multiplicar(pasajes, precio));
        }
        static int Multiplicar(int a, int b)
        {
            if (a == 0) 
            {
                return 0;
            }

            return b + Multiplicar(a - 1, b);
        }
    }
}
