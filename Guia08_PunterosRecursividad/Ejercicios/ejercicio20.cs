using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class ejercicio20
    {
        public static void Main(string[] args)
        {
            Revisar("oruro");
            Revisar("reconocer");
            Revisar("sucre");
            Revisar("anilina");
        }
        static void Revisar(string palabra)
        {
            if (EsPalindromo(palabra, 0, palabra.Length - 1))
            {
                Console.WriteLine(palabra + " -> SÍ es palíndromo");
            }
            else
            {
                Console.WriteLine(palabra + " -> NO es palíndromo");
            }
        }
        static bool EsPalindromo(string s, int izq, int der)
        {
            if (izq >= der) 
            {
                return true;
            }
            if (s[izq] != s[der]) 
            {
                return false;
            }

            return EsPalindromo(s, izq + 1, der - 1); 
        }
    }
}