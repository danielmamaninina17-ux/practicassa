using System;
using System.Collections.Generic;
using System.Text;

namespace Guia08_PunterosRecursividad.Ejercicios
{
    public class ejercicio8
    {
        public static void Main(string[] args)
        {
            int dias = 30;
            int total = AhorroTotal(dias);
            Console.WriteLine("Ahorro en " + dias + " días: Bs " + total);
        }


        static int AhorroTotal(int dia)
        {
            if (dia == 0) 
            {
                return 0;
            }

            return dia + AhorroTotal(dia - 1);
        }
    }
}