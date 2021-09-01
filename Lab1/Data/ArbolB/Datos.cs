using System;
using System.Collections.Generic;
using System.Text;

namespace Lab1.Data.ArbolB
{
    public class Datos<TK, TP> : IEquatable<Datos<TK, TP>>
    {
        public TK Dato { get; set; }
        public TP Valor { get; set; }
        public bool Equals(Datos<TK, TP> other)
        {
            return this.Dato.Equals(other.Dato) && this.Valor.Equals(other.Valor);
        }
    }
}
