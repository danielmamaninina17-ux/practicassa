using System;

namespace Recursividad
{
    public class ejercicio3
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Tabla de precios: salteñas a Bs 7");
            TablaDePrecios(7, 1);
        }

        static void TablaDePrecios(int precio, int cantidad)
        {
            if (cantidad > 10) // CASO BASE: llegamos a 10 salteñas
            {
                return;
            }

            Console.WriteLine(cantidad + " x " + precio + " = Bs " + (cantidad * precio));

            TablaDePrecios(precio, cantidad + 1); // CASO RECURSIVO
        }
    }
}