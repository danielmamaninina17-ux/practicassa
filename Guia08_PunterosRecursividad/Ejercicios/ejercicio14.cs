using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class ejercicio14
    {
        public static void Main(string[] args)
        {
            int largo = 84;
            int ancho = 36;
            int lado = MCD(largo, ancho);
            Console.WriteLine("Terreno de " + largo + " m x " + ancho + " m en Sacaba");
            Console.WriteLine("Parcela cuadrada más grande: " + lado + " m de lado");
            Console.WriteLine("Cantidad de parcelas: " + (largo / lado) * (ancho / lado));
        }

        static int MCD(int a, int b)
        {
            if (b == 0) 
            {
                return a;
            }

            return MCD(b, a % b);
        }
    }
}