using System;

namespace Recursividad
{
    public class ejercicio1

    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Cabinas de la Línea Roja de Mi Teleférico:");
            ContarCabinas(1, 6);
        }

        static void ContarCabinas(int actual, int total)
        {
            if (actual > total)
            {
                Console.WriteLine("Todas las cabinas revisadas.");
                return;
            }

            Console.WriteLine("Cabina " + actual + " lista");

            ContarCabinas(actual + 1, total);
        }
    }
}