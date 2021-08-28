using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace Arbol_y_pruebas
{
    public class Arbol<TK, TP> where TK : IComparable<TK>
    {

        public int Grado { get; private set; }
        public int Altura { get; private set; }
        public Nodo<TK, TP> Raiz { get; private set; }

        public Arbol(int grado)
        {
            if (grado < 3 || grado % 2 == 0)
            {
                throw new ArgumentException("El grado debe ser mayor o igual a 3 e impar");
            }

            this.Raiz = new Nodo<TK, TP>(grado);
            this.Grado = grado;
            this.Altura = 1;
        }

        /*Busca la llave en el arbolb, regresa la entrada con el dato y 
         * el apuntador
         *Llave==dato que se esta buscando
         *Regresa la entrada o si no null
         */
        public Datos<TK, TP> Buscar(TK llave)
        {
            return this.BusquedaInterna(this.Raiz, llave);
        }

        private Datos<TK, TP> BusquedaInterna(Nodo<TK, TP> nodo, TK llave)
        {
            int i = nodo.Entradas.TakeWhile(entry => llave.CompareTo(entry.Dato) > 0).Count();
            if (i < nodo.Entradas.Count && nodo.Entradas[i].Dato.CompareTo(llave) == 0)
            {
                return nodo.Entradas[i];
            }
            return nodo.EsHoja ? null : this.BusquedaInterna(nodo.Hijos[i], llave);
        }

        public Datos<TK, TP> MostrarP(ref int aux)
        {
            return this.Mostrar(this.Raiz,ref aux);
        }

        private Datos<TK, TP> Mostrar(Nodo<TK, TP> nodo,ref int aux)
        {
            int i = aux;
            if (aux < nodo.Entradas.Count)
            {

                aux++;
                return nodo.Entradas[i];
            }
            aux++;

            if(nodo.EsHoja)
            {
                return null;
            }
            else
            {
               
                return this.Mostrar(nodo.Hijos[i], ref aux);   
            }
        }

    




        /*
         * Se inserta un dato asociado con un apuntador en el arbol b
         * La operacion divide al nodo si se requiere
         */
        public void Insertar(TK nuevoDato, TP nuevoApuntador)
        {
            // si hay espacio en el nodo raiz
            if (!this.Raiz.MaximaEntradas)
            {
                this.InsertarEnNodo(this.Raiz, nuevoDato, nuevoApuntador);
                return;
            }

            // crear un nuevo nodo y separarlo
            Nodo<TK, TP> anteriorRaiz = this.Raiz;
            this.Raiz = new Nodo<TK, TP>(this.Grado);
            this.Raiz.Hijos.Add(anteriorRaiz);
            this.SepararHijo(this.Raiz, 0, anteriorRaiz);
            this.InsertarEnNodo(this.Raiz, nuevoDato, nuevoApuntador);
            this.Altura++;
        }

        private void InsertarEnNodo(Nodo<TK, TP> nodo, TK nuevoDato, TP nuevoApuntador)
        {
            int posicionAinsertar = nodo.Entradas.TakeWhile(entry => nuevoDato.CompareTo(entry.Dato) >= 0).Count();

            // Nodo hoja
            if (nodo.EsHoja)
            {
                nodo.Entradas.Insert(posicionAinsertar, new Datos<TK, TP>() { Dato = nuevoDato, Valor = nuevoApuntador });
                return;
            }

            // Si no es un nodo hoja
            Nodo<TK, TP> hijo = nodo.Hijos[posicionAinsertar];
            if (hijo.MaximaEntradas)
            {
                this.SepararHijo(nodo, posicionAinsertar, hijo);
                if (nuevoDato.CompareTo(nodo.Entradas[posicionAinsertar].Dato) > 0)
                {
                    posicionAinsertar++;
                }
            }

            this.InsertarEnNodo(nodo.Hijos[posicionAinsertar], nuevoDato, nuevoApuntador);
        }

        /// Metodo que trabaja la division de los nodos cuando estan llenos 
        private void SepararHijo(Nodo<TK, TP> nodoPadre, int indiceNodoASeparar, Nodo<TK, TP> nodoASeparar)
        {
            var nuevoNodo = new Nodo<TK, TP>(this.Grado);

            nodoPadre.Entradas.Insert(indiceNodoASeparar, nodoASeparar.Entradas[this.Grado - 1]);
            nodoPadre.Hijos.Insert(indiceNodoASeparar + 1, nuevoNodo);

            nuevoNodo.Entradas.AddRange(nodoASeparar.Entradas.GetRange(this.Grado, this.Grado - 1));

            // Remueve la entrada del nodo ques se convertira a padre
            nodoASeparar.Entradas.RemoveRange(this.Grado - 1, this.Grado);

            if (!nodoASeparar.EsHoja)
            {
                nuevoNodo.Hijos.AddRange(nodoASeparar.Hijos.GetRange(this.Grado, this.Grado));
                nodoASeparar.Hijos.RemoveRange(this.Grado, this.Grado);
            }
        }

        /*Borra un dato del arbolB, hace que las llaves y los nodos se muevan
         * como lo requiere un arbolB
         * 
         */

        public void Borrar(TK datoABorrar)
        {
            this.BorrarInterior(this.Raiz, datoABorrar);

            // if root's last entry was moved to a child node, remove it
            if (this.Raiz.Entradas.Count == 0 && !this.Raiz.EsHoja)
            {
                this.Raiz = this.Raiz.Hijos.Single();
                this.Altura--;
            }
        }
    
        /*BORRA el dato de un nodo hoja o un nodo interno
         */
        private void BorrarDatodeNodo(Nodo<TK, TP> nodo, TK datoABorrar, int indexdelDatoEnNodo)
        {
            /*Si es una hoja, el dato se remueve de la lista de entradas asegurado que se tenga
             * el minimo necesario del nodo
             */


            if (nodo.EsHoja)
            {
                nodo.Entradas.RemoveAt(indexdelDatoEnNodo);
                return;
            }

            Nodo<TK, TP> HijoAnterior = nodo.Hijos[indexdelDatoEnNodo];
            if (HijoAnterior.Entradas.Count >= this.Grado)
            {
                Datos<TK, TP> Predecesor = this.BorrarDatoPredecesor(HijoAnterior);
                nodo.Entradas[indexdelDatoEnNodo] = Predecesor;
            }
            else
            {
                Nodo<TK, TP> HijoSucesor = nodo.Hijos[indexdelDatoEnNodo + 1];
                if (HijoSucesor.Entradas.Count >= this.Grado)
                {
                    Datos<TK, TP> Sucesor = this.BorrarDatoSucesor(HijoAnterior);
                    nodo.Entradas[indexdelDatoEnNodo] = Sucesor;
                }
                else
                {
                    HijoAnterior.Entradas.Add(nodo.Entradas[indexdelDatoEnNodo]);
                    HijoAnterior.Entradas.AddRange(HijoSucesor.Entradas);
                    HijoAnterior.Hijos.AddRange(HijoSucesor.Hijos);

                    nodo.Entradas.RemoveAt(indexdelDatoEnNodo);
                    nodo.Hijos.RemoveAt(indexdelDatoEnNodo + 1);

                    this.BorrarInterior(HijoAnterior, datoABorrar);
                }
            }
        }
        private void BorrarInterior(Nodo<TK, TP> nodo, TK datoABorrar)
        {
            int i = nodo.Entradas.TakeWhile(entry => datoABorrar.CompareTo(entry.Dato) > 0).Count();

            // found key in node, so delete if from it
            if (i < nodo.Entradas.Count && nodo.Entradas[i].Dato.CompareTo(datoABorrar) == 0)
            {
                this.BorrarDatodeNodo(nodo, datoABorrar, i);
                return;
            }

            // delete key from subtree
            if (!nodo.EsHoja)
            {
                this.BorrarDatodeSubArbol(nodo, datoABorrar, i);
            }
        }

        private void BorrarDatodeSubArbol(Nodo<TK, TP> nodoPadre, TK datoABorrar, int indiceDelNodoDelSubArbol)
        {
            Nodo<TK, TP> nodoHijo = nodoPadre.Hijos[indiceDelNodoDelSubArbol];

            /*El nodo alcanzo el minimo de entrdas,y si se remueve algun dato de ese nodo romperia el arbolB
             * Este metodo se asegura de que ese nodo hijo tenga almenos el grado minimo de nodos moviendo
             * una entrada del nodo hermano o jutando nodos
             */
            if (nodoHijo.MinimoEntradas)
            {
                int indiceIzquierdo = indiceDelNodoDelSubArbol - 1;
                Nodo<TK, TP> hermanoIzquierdo = indiceDelNodoDelSubArbol > 0 ? nodoPadre.Hijos[indiceIzquierdo] : null;

                int indiceDerecho = indiceDelNodoDelSubArbol + 1;
                Nodo<TK, TP> hermanoDerecho = indiceDelNodoDelSubArbol < nodoPadre.Hijos.Count - 1
                                                ? nodoPadre.Hijos[indiceDerecho]
                                                : null;

                if (hermanoIzquierdo != null && hermanoIzquierdo.Entradas.Count > this.Grado - 1)
                {
                     
                    /* El hermano izquierdo tiene un espacio en el nodo para prestar, mueve el dato 
                     * disponible al nodo que lo necesita al nodo padre
                     */

                    nodoHijo.Entradas.Insert(0, nodoPadre.Entradas[indiceDelNodoDelSubArbol]);
                    nodoPadre.Entradas[indiceDelNodoDelSubArbol] = hermanoIzquierdo.Entradas.Last();
                    hermanoIzquierdo.Entradas.RemoveAt(hermanoIzquierdo.Entradas.Count - 1);

                    if (!hermanoIzquierdo.EsHoja)
                    {
                        nodoHijo.Hijos.Insert(0, hermanoIzquierdo.Hijos.Last());
                        hermanoIzquierdo.Hijos.RemoveAt(hermanoIzquierdo.Hijos.Count - 1);
                    }
                }
                else if (hermanoDerecho != null && hermanoDerecho.Entradas.Count > this.Grado - 1)
                {
                    /* El hermano derecho tiene un espacio en el nodo para prestar, mueve el dato 
                    * disponible al nodo que lo necesita al nodo padre
                    */
                    nodoHijo.Entradas.Add(nodoPadre.Entradas[indiceDelNodoDelSubArbol]);
                    nodoPadre.Entradas[indiceDelNodoDelSubArbol] = hermanoDerecho.Entradas.First();
                    hermanoDerecho.Entradas.RemoveAt(0);

                    if (!hermanoDerecho.EsHoja)
                    {
                        nodoHijo.Hijos.Add(hermanoDerecho.Hijos.First());
                        hermanoDerecho.Hijos.RemoveAt(0);
                    }
                }
                else
                {
                    /*Este bloque junta ya sea los hermanos derechos o izquierdos en el nodo actual "hijo"
                     */
                    if (hermanoIzquierdo != null)
                    {
                        nodoHijo.Entradas.Insert(0, nodoPadre.Entradas[indiceDelNodoDelSubArbol]);
                        var entradasAnteriores = nodoHijo.Entradas;
                        nodoHijo.Entradas = hermanoIzquierdo.Entradas;
                        nodoHijo.Entradas.AddRange(entradasAnteriores);
                        if (!hermanoIzquierdo.EsHoja)
                        {
                            var hijoAnterior = nodoHijo.Hijos;
                            nodoHijo.Hijos = hermanoIzquierdo.Hijos;
                            nodoHijo.Hijos.AddRange(hijoAnterior);
                        }

                        nodoPadre.Hijos.RemoveAt(indiceIzquierdo);
                        nodoPadre.Entradas.RemoveAt(indiceDelNodoDelSubArbol);
                    }
                    else
                    {
                        Debug.Assert(hermanoDerecho != null, "El nodo debe tener almenos un hermano");
                        nodoHijo.Entradas.Add(nodoPadre.Entradas[indiceDelNodoDelSubArbol]);
                        nodoHijo.Entradas.AddRange(hermanoDerecho.Entradas);
                        if (!hermanoDerecho.EsHoja)
                        {
                            nodoHijo.Hijos.AddRange(hermanoDerecho.Hijos);
                        }

                        nodoPadre.Hijos.RemoveAt(indiceDerecho);
                        nodoPadre.Entradas.RemoveAt(indiceDelNodoDelSubArbol);
                    }
                }
            }

            /*En este punto sabemos que el hijo tiene almenos el grado minimo de datos,
             * asi que se puede mover, garantizando que si cualquier dato que necesita ser 
             * removido del nodo cumpla las propiedades del arbolB
             */
            this.BorrarInterior(nodoHijo, datoABorrar);
        }


        /*Borrar el dato anterior del nodo dado
         * 
         */
        private Datos<TK, TP> BorrarDatoPredecesor(Nodo<TK, TP> nodo)
        {
            if (nodo.EsHoja)
            {
                var resultado = nodo.Entradas[nodo.Entradas.Count - 1];
                nodo.Entradas.RemoveAt(nodo.Entradas.Count - 1);
                return resultado;
            }

            return this.BorrarDatoPredecesor(nodo.Hijos.Last());
        }
        private Datos<TK, TP> BorrarDatoSucesor(Nodo<TK, TP> nodo)
        {
            if (nodo.EsHoja)
            {
                var resultado = nodo.Entradas[0];
                nodo.Entradas.RemoveAt(0);
                return resultado;
            }

            return this.BorrarDatoPredecesor(nodo.Hijos.First());
        }



        /*
        public string toString()
        {
            return toString(Raiz, Altura, "") + "\n";
        }

        private string toString(Nodo<TK, TP> h, int ht, string indent)
        {
            string s = null;
            List<Datos<TK, TP>> children = h.Entradas;

            if (ht == 0)
            {
                for (int j = 0; j < Grado; j++)
                {
                    s += indent + children[j].Dato + " " + children[j].Apuntador + "\n";
                }
            }
            else
            {
                for (int j = 0; j < Grado; j++)
                {
                    if (j > 0) s += indent + "(" + children[j].Dato + ")\n";
                    s += toString(children[j].siguiente , ht - 1, indent + "     ");
                }
            }
            return s;
        }
        */

    }
}
