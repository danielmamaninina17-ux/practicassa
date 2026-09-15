using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1.LaParca
{
    public class Muerte
    {
        public static void Main(string[] args)
        {
            double a;
            double b;

            Console.WriteLine("Dame el valor de a: ");
            a = double.Parse(Console.ReadLine());
            Console.WriteLine("Dame el valor de b: ");
            b = double.Parse(Console.ReadLine());

            double suma = a + b;
            double resta = a - b;
            double multiplicacion = a * b; 
            double division = b / a;

            Console.WriteLine("La suma es: " + suma);
            Console.WriteLine("La resta es: " + resta);
            Console.WriteLine("Lamultiplicacion es: " + multiplicacion);
            Console.WriteLine("La division es: " + division);

            Console.WriteLine("====================The fin=======================");
        }

    }
}
