using System;
using System.IO;

namespace algoritmos
{
    public class ejercicio7
    {
        static long pasos = 0;
        static bool Cond(bool resultado)
        {
            pasos++;
            return resultado;
        }
        static double TotalConIva(double[] precios)
        {
            double total = 0; pasos++;
            for (int i = 0; Cond(i < precios.Length); i++)
            {
                double conIva = precios[i] * 1.13; pasos++;
                total += conIva; pasos++;
            }
            pasos++; return total;
        }
        static long MiFormula(long n)
        {
            return 0;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("    n | pasos | tu T(n) | ¿coincide?");
            foreach (int n in new[] { 1, 2, 3, 10 })
            {
                double[] precios = new double[n];
                pasos = 0;
                TotalConIva(precios);
                long tuyo = MiFormula(n);
                string ok = tuyo == pasos ? "sí" : "NO";
                Console.WriteLine($"{n,5} | {pasos,5} | {tuyo,7} | {ok}");
            }
        }
    }
}