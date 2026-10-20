using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class ejercicio12
    {
        public static void Main(string[] args)
        {
            int codigo = 58036;
            int suma = SumarDigitos(codigo);
            Console.WriteLine("Código del producto: " + codigo);
            Console.WriteLine("Suma de dígitos: " + suma);
            Console.WriteLine("Dígito verificador (suma % 10): " + (suma % 10));
        }

        static int SumarDigitos(int n)
        {
            if (n == 0) 
            {
                return 0;
            }

            return (n % 10) + SumarDigitos(n / 10); 
        }
    }
}