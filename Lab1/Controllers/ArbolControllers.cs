using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Lab1;
using Lab1.Data;
using Lab1.Data.ArbolB;
using Lab1.Model;
namespace Lab1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArbolController : ControllerBase
    {

        [HttpGet]
        public Arbol<int, int> Get()
        {
            return Singleton.GetInstance.miarbol;
        }

        [HttpGet("{recorrido}")]
        public List<int> Get([FromRoute] string recorrido)
        {
            if (recorrido == "in")
            {
                return Singleton.GetInstance.miarbol.RecorrerIn();
            }
            else if (recorrido == "post")
            {
                return Singleton.GetInstance.miarbol.RecorrerPost();
            }
            return new List<int>();
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            Singleton.GetInstance.miarbol = new Arbol<int, int>(3);
            return Ok("Arbol Limpiado");
        }

        [HttpPost]
        public ActionResult Post([FromBody] JsonElement jsonobj)
        {
            try
            {
                JsonElement json = jsonobj.GetProperty("orden");
                int grado = json.GetInt32();
                if (grado < 3)
                {
                    Console.Write("No se aceptan menores a Grado 3");
                    return StatusCode(500);
                }
                Singleton.GetInstance.miarbol = new Arbol<int, int>(grado);
                return Ok("Se inicializa árbol con grado " + grado);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [Route("populate")]
        [HttpPost]
        public ActionResult Populate([FromBody] PopulateInput input)
        {
            Console.WriteLine("Ingresa a populate");
            Console.Write(input.ToString());
            try
            {
                int idx;
                for (idx = 0; idx < input.elementos.Count; idx++)
                {
                    int elemento = input.elementos[idx];
                    Singleton.GetInstance.miarbol.Insertar(elemento, 0);
                }
                return Ok("Se agregaron " + idx + " elementos");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [Route("populate/{id}")]
        [HttpDelete("{id}")]
        public ActionResult PopulateDelete([FromRoute] int id)
        {
            try
            {
                if (Singleton.GetInstance.miarbol.Buscar(id) == null)
                {
                    return NotFound();
                }
                Singleton.GetInstance.miarbol.Borrar(id);
                return Ok("Se eliminó elemento " + id);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

    }

}