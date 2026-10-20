using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
public class ejercicio4
    {
     
       public static void Main(string[] args)
            {
                Console.WriteLine("Lluvia en Cochabamba (1 * = 10 mm)");
                Console.Write("Enero   ");
                DibujarBarra(9);
                Console.WriteLine();
                Console.Write("Febrero ");
                DibujarBarra(7);
                Console.WriteLine();
                Console.Write("Marzo   ");
                DibujarBarra(4);
                Console.WriteLine();
            }

            static void DibujarBarra(int n)
            {
                if (n == 0) 
                {
                    return;
                }

                Console.Write("*"); 

                DibujarBarra(n - 1); 
            }
        }
    }