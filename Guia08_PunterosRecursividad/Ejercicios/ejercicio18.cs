using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class ejercicio18
    {
        public static void Main(string[] args)
        {
            int[] ventas = { 320, 450, 280, 510, 390, 620, 700 };
            int total = SumarVentas(ventas, 0);
            Console.WriteLine("Feria 16 de Julio (El Alto) - ventas de la semana");
            Console.WriteLine("Total: Bs " + total);
            Console.WriteLine("Promedio diario: Bs " + (total / ventas.Length));
        }

        static int SumarVentas(int[] v, int i)
        {
            if (i == v.Length) 
            {
                return 0;
            }

            return v[i] + SumarVentas(v, i + 1); 
        }
    }
}