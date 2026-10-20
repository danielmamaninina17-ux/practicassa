using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    public class ejercicio6
    {
        public static void Main(string[] args)
        {
            Console.Write("Placa 4821 al revés: ");
            ImprimirAlReves(4821);
            Console.WriteLine();
        }

        static void ImprimirAlReves(int n)
        {
            if (n < 10)
            {
                Console.Write(n);
                return;
            }

            Console.Write(n % 10);

            ImprimirAlReves(n / 10);
        }
    }

}