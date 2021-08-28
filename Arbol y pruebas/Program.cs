using System;

namespace Arbol_y_pruebas
{
    class Program
    {
        static void Main(string[] args)
        {

            var btree = new Arbol<int, string>(3);
            int i = 0;
        A:
            Console.WriteLine("Agregar==1");
            int menu = int.Parse(Console.ReadLine());

            if (menu == 1)
            {
                Console.Write("Dato a agregar: ");
                int dato = int.Parse(Console.ReadLine());
                Console.Write("Valor del dato a agregar: ");
                string valor =Console.ReadLine();

                btree.Insertar(dato, valor);
                i++;


            }
            else
            {
                Console.Write("Dato a buscar: ");
                int dato = int.Parse(Console.ReadLine());
                var busqueda = btree.Buscar(dato);
                if (busqueda != null)
                {
                    Console.WriteLine("Dato encontrado, valor: " +busqueda.Valor);

                }


            }

            goto A;
        }
    }
}
