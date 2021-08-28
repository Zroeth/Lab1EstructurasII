using System;
using System.Collections.Generic;
using System.Text;

namespace Arbol_y_pruebas
{
    public class Nodo<TK,TP>
    {
        private int grado;
        public List<Nodo<TK, TP>> Hijos { get; set; }

        public List<Datos<TK, TP>> Entradas { get; set; }

        public Nodo(int grado)
        {
            this.grado = grado;
            Hijos = new List<Nodo<TK, TP>>(grado);
            Entradas = new List<Datos<TK, TP>>(grado);
        }

        public bool EsHoja
        {
            get
            {
                return this.Hijos.Count == 0;
            }
        }
        public bool MaximaEntradas
        {
            get
            {
                return this.Entradas.Count == (2 * this.grado) - 1;
            }
        }
        public bool MinimoEntradas
        {
            get
            {
                return this.Entradas.Count == this.grado - 1;
            }
        }
    }
}
