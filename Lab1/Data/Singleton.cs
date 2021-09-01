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
        public Arbol<string, string> miarbol { get; set; }
        
        private Singleton()
        {
            this.miarbol = new Arbol<string, string>(3);

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
