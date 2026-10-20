using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    public class ejercicio7
    {
        public static void Main(string[] args)
        {
            Console.Write("Sensor del río Rocha, lectura 13 en binario: ");
            ImprimirBinario(13);
            Console.WriteLine();
            Console.Write("Lectura 40 en binario: ");
            ImprimirBinario(40);
            Console.WriteLine();
        }

        static void ImprimirBinario(int n)
        {
            if (n < 2)
            {
                Console.Write(n);
                return;
            }

            ImprimirBinario(n / 2);
            Console.Write(n % 2);    
        }
    }
}