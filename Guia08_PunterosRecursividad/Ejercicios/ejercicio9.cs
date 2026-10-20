using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
   public class ejercicio9
    {
        public static void Main(string[] args)
        {
            int fraternidades = 6;
            long formas = Factorial(fraternidades);
            Console.WriteLine("Fraternidades en el Carnaval de Oruro: " + fraternidades);
            Console.WriteLine("Formas distintas de ordenarlas: " + formas);
        }

        static long Factorial(int n)
        {
            if (n <= 1) 
            {
                return 1;
            }

            return n * Factorial(n - 1); 
        }
    }
}
