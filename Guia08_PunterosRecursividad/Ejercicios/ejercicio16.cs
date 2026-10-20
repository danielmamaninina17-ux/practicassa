using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class ejercicio16
    {
        public static void Main(string[] args)
        {
            string nevado = "illimani";
            Console.WriteLine("Vocales en " + nevado + ": " + ContarVocales(nevado, 0));
            string valle = "tiquipaya";
            Console.WriteLine("Vocales en " + valle + ": " + ContarVocales(valle, 0));
        }

        static int ContarVocales(string texto, int i)
        {
            if (i == texto.Length)
            {
                return 0;
            }

            int esVocal = 0;
            if ("aeiou".IndexOf(texto[i]) >= 0)
            {
                esVocal = 1;
            }

            return esVocal + ContarVocales(texto, i + 1); 
        }
    }
}