using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    public class ejercicio10
    {
        public static void Main(string[] args)
        {
            for (int ronda = 1; ronda <= 5; ronda++)
            {
                Console.WriteLine("Ronda " + ronda + ": " + Potencia(3, ronda) + " personas reciben el aviso");
            }
        }

        static long Potencia(int numero, int exponente)
        {
            if (exponente == 0) 
            {
                return 1;
            }

            return numero * Potencia(numero, exponente - 1); 
        }
    }
}