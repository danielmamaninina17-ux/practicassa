using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    public class ejercio2
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Mojones de la carretera La Paz - Oruro (cada 2 km):");
            MostarMonjes(12);
            

        }
        static void MostarMonjes(int km)
        {
            if (km < 0) // CASO BASE
            {
                return;
            }
            Console.WriteLine("Km " + km);
            MostarMonjes(km - 2); // CASO RECURSIVO
      
        }
    }
}
