using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace ConsoleApp1.guerraDelChaco
{
    public class miniJuego
    {
        public static void Main(string[] args)
        {
            int dia = 1;
            int agua = 100;
            int municion = 50;
            int moral = 80;
            int fortinEnPie = 1;

            Console.WriteLine("=============================================");
            Console.WriteLine("DEFENSA DEL FORTIN: CAMPAÑA DEL CHACO BOREAL");
            Console.WriteLine("=============================================");
            Console.WriteLine("Sobrevive 7 dias gestionanado los recursos del fortin");

            do
            {
                Console.WriteLine("===================================================");
                Console.WriteLine("Estado actual del recurso del dia " + dia);
                Console.WriteLine("Agua disponible" + agua);
                Console.WriteLine("Munucion disponible " + municion);
                Console.WriteLine("Moral de la tropa " + moral);

                int opcionEligida = 0;
                int entradaValida = 0;

                do
                {
                    Console.WriteLine("Que accion deas tomar hoy? ");
                    Console.WriteLine("1. Racionar agua estrictamente");
                    Console.WriteLine("2. Repartir agua normalmente");
                    Console.WriteLine("3. Entrar patrulla enemiga");
                    Console.WriteLine("Elige una opcion valida 1,2 o 3");

                    string entrada = Console.ReadLine();
                    if (entrada == "1")
                    {
                        opcionEligida = 1;
                        entradaValida = 1;
                    }
                    else if (entrada == "2")
                    {
                        opcionEligida = 2;
                        entradaValida = 1;

                    }
                    else if (entrada == "3")
                    {
                        opcionEligida = 3;
                        entradaValida = 1;
                    }
                    else
                    {
                        Console.WriteLine("Error, carracter no valido debes de ingresar1,2 o 3");
                    }
                }
                while (entradaValida == 0);
                if (opcionEligida == 1)
                {
                    Console.WriteLine("Ordenaste racionar el agua estrictamente");
                    Console.WriteLine("Perdiste 10 litros de agua ");
                    Console.WriteLine("Perdiste 5% de moral");
                    agua = agua - 10;
                    moral = moral - 5;
                }
                else if (opcionEligida == 2)
                {
                    Console.WriteLine("la tropa recibe racion completa de agua");
                    Console.WriteLine("Perdiste 20 litros de agua");
                    Console.WriteLine("Ganaste 15% de moral ");
                    agua = agua - 20;
                    moral = moral + 15;

                    if (moral > 100)
                    {
                        moral = 100;
                    }
                }
                else if (opcionEligida == 3)
                {
                    if (municion >= 15)
                    {
                        Console.WriteLine("Combate en el monte, se repelio al adversario");
                        Console.WriteLine("Perdiste 15 municiones");
                        Console.WriteLine("Perdiste 10 litros de agua ");
                        Console.WriteLine("Ganaste 5% de moral");
                        municion = municion - 15;
                        agua = agua - 10;
                        moral = moral + 5;

                        if (moral > 100)
                        {
                            moral = 100;
                        }
                    }
                    else
                    {
                        Console.WriteLine("No hay suficiente municion para entablar combate");
                        Console.WriteLine("Perdiste 15% de moral por retroceder");
                        moral = moral - 15;
                    }
                }
                if (agua <= 30 && agua > 0) ;
                {
                    Console.WriteLine("Las reservas de agua se han agotado por completo");
                    agua = 0;
                    fortinEnPie = 0;
                }
                if (moral <= 0) ;
                {
                    Console.WriteLine("Latropa ha perdido la moral y abandono el fortin");
                    moral = 0;
                    fortinEnPie = 0;
                }
                dia = dia + 1;
            }
            while (fortinEnPie == 1 && dia <= 7);
            Console.WriteLine("=======================The fin===========================");

            if (fortinEnPie == 1 && agua > 0 && moral > 0)
            {
                Console.WriteLine("Victoria lograste resistir los 7 dias de asedio en el Chaco Boreal");
            }
            else
            {
                Console.WriteLine("Derrota el fortin no pudo superar las duras condiciones del Chaco Boreal");
            }
        }
    }           
}