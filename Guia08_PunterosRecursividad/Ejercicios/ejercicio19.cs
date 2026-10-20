using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    internal class ejercicio19
    {
        public static void Main(string[] args)
        {
            int[] temperaturas = { 24, 27, 23, 29, 26, 31, 25 };
            Console.Write("Temperaturas de la semana: ");
            MostrarArreglo(temperaturas, 0);
            Console.WriteLine("Máxima de la semana en Cochabamba: " + Maximo(temperaturas, 0) + " °C");
        }
        static void MostrarArreglo(int[] v, int i)
        {
            if (i == v.Length)
            {
                Console.WriteLine();
                return;
            }
            Console.Write(v[i] + " ");
            MostrarArreglo(v, i + 1);
        }
        static int Maximo(int[] v, int i)
        {
            if (i == v.Length - 1) 
            {
                return v[i];
            }

            int mayorDelResto = Maximo(v, i + 1); 
            if (v[i] > mayorDelResto)
            {
                return v[i];
            }
            return mayorDelResto;
        }
    }
}