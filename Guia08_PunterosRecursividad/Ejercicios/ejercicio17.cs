using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class ejercicio17
    {
        public static void Main(string[] args)
        {
            string[] departamentos = { "La Paz", "Cochabamba", "Santa Cruz", "Oruro", "Potosí",
                                       "Chuquisaca", "Tarija", "Beni", "Pando" };
            Console.WriteLine("Departamentos de Bolivia:");
            MostrarDepartamentos(departamentos, 0);
        }

        static void MostrarDepartamentos(string[] lista, int i)
        {
            if (i == lista.Length)
            {
                return;
            }

            Console.WriteLine((i + 1) + ". " + lista[i]);

            MostrarDepartamentos(lista, i + 1); 
        }
    }
}