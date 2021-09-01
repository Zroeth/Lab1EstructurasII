using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Model;
using Lab1.Data.ArbolB;

namespace Lab1.Data
{
    public sealed class Singleton
    {
        private readonly static Singleton _instance = new Singleton();
        public Arbol<int, int> miarbol { get; set; }
        
        private Singleton()
        {
            
            this.miarbol = new Arbol<int, int>(3);
            miarbol.Insertar(10, 0); // valor, puntero
            miarbol.Insertar(1, 0);
            miarbol.Insertar(7, 0);
            miarbol.Insertar(3, 0);
            miarbol.Insertar(5, 0);
            miarbol.Insertar(15, 0);
            miarbol.Insertar(12, 0);   
        }

        public static Singleton GetInstance
        {
            get
            {
                return _instance;
            }
        }
    }
}
