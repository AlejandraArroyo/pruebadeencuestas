using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using pruebadeencuestas.Models;

namespace pruebadeencuestas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class encuestas : ControllerBase
    {
        private readonly string cadenaSQL;
       public encuestas(IConfiguration config)

        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");


        }



        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Encuestas objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("guardar_encuestas_prueba; select scope_identity()", conexion);
                    cmd.Parameters.AddWithValue("nombre", objeto.nombre);   
                    cmd.Parameters.AddWithValue("descripcion", objeto.descripcion);
                    
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    int g = Convert.ToInt32(cmd.ExecuteScalar());

                    var detall=  new SqlCommand("guardar_detalles", conexion);
                    foreach (var detalle_model in objeto.Detalles)
                    {
                        detall.Parameters.AddWithValue("encuesta", g);
                        detall.Parameters.AddWithValue("nombrecampo", detalle_model.nombrecampo);
                        detall.Parameters.AddWithValue("titulocampo", detalle_model.titulo_campo);
                        detall.Parameters.AddWithValue("esrequerido", detalle_model.esrequerido);
                        detall.Parameters.AddWithValue("tipo", detalle_model.tipo);
                    }
                    detall.CommandType = CommandType.StoredProcedure;
                    detall.ExecuteNonQuery();
                }



                return StatusCode(StatusCodes.Status200OK, new { mensaje = "encuesta agregada" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }


        [HttpGet]
        //[Route("Obtener")] // => Obtener?idProducto=13 | ampersand
        [Route("Obtener/{idencuesta:int}")]
        public IActionResult Obtener(int idencuesta)
        {

            List<Encuestas> lista = new List<Encuestas>();
            Encuestas encues = new Encuestas();

            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("lista_encabezado_encuesta", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {

                        while (rd.Read())
                        {

                            lista.Add(new Encuestas
                            {
                                id = Convert.ToInt32(rd["id"]),
                                nombre = rd["nombre"].ToString(),
                                descripcion = rd["descripcion"].ToString()
                            }); ;
                        }

                    }
                }

                encues = lista.Where(item => item.id == idencuesta).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = encues });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = encues });

            }
        }


        [HttpDelete]
        [Route("Eliminar/{idEncuesta:int}")]
        public IActionResult Eliminar(int idEncuesta)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("eliminar_encuesta", conexion);
                    cmd.Parameters.AddWithValue("idEncuesta", idEncuesta);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "encuesta eliminada" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }


    }
}
