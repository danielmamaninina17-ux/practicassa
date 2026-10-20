using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    public class ejercicio5
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Andenes de cultivo en Tiwanaku:");
            DibujarAndenes(1, 5);
            static void DibujarFila(int n)
            {
                if (n == 0)
                {
                    Console.WriteLine();
                    return;
                }
                Console.Write("[]");
                DibujarFila(n - 1);
            }


            static void DibujarAndenes(int fila, int total)
            {
                if (fila > total)
                {
                    return;
                }
                DibujarFila(fila);
                DibujarAndenes(fila + 1, total); 
            }
        }
    }
}