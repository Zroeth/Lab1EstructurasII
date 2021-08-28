using System;
using System.Collections.Generic;
using System.Text;

namespace Arbol_y_pruebas
{
    public class Datos<TK,TP>:IEquatable<Datos<TK,TP>>
    {
        public TK Dato { get; set; }
        public TP Valor { get; set; }
        public Nodo<TK, TP> siguiente { get; set; }
        public bool Equals(Datos<TK, TP> other)
        {
            return this.Dato.Equals(other.Dato) && this.Valor.Equals(other.Valor)&& this.siguiente.Equals(other.siguiente);
        }
    }
}
