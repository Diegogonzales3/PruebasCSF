using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using PruebasCSF.Data;

namespace PruebasCSF.Controllers
{
    [ApiController]
    [Route("gestion")]
    public class AutorController : ControllerBase
    {
        [HttpPost]
        [Route("CrearAutor")]
        public IActionResult CrearAutor([FromBody] dynamic nuevoAutor)
        {
            string query = "INSERT INTO Autores (Nombres, Apellidos, FechaNacimiento) VALUES (@Nombres, @Apellidos, @FechaNacimiento)";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Nombres", nuevoAutor.Nombres.ToString()),
                new SqlParameter("@Apellidos", nuevoAutor.Apellidos.ToString()),
                new SqlParameter("@FechaNacimiento", DateTime.Parse(nuevoAutor.FechaNacimiento.ToString()))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);

            return Ok(new
            {
                Success = filas > 0,
                Message = filas > 0 ? "Autor creado" : "Error"
            });
        }

        [HttpPut]
        [Route("ActualizarAutor/{id}")]
        public IActionResult ActualizarAutor(int id, [FromBody] dynamic datosActualizados)
        {
            string query = "UPDATE Autores SET Nombres = @Nombres, Apellidos = @Apellidos, FechaNacimiento = @FechaNacimiento WHERE Id = @Id";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombres", datosActualizados.Nombres.ToString()),
                new SqlParameter("@Apellidos", datosActualizados.Apellidos.ToString()),
                new SqlParameter("@FechaNacimiento", DateTime.Parse(datosActualizados.FechaNacimiento.ToString()))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);

            return Ok(new
            {
                Success = filas > 0,
                Message = filas > 0 ? $"Autor {id} actualizado correctamente" : "Error"
            });
        }

        [HttpGet]
        [Route("ListarAutores")]
        public IActionResult ListarAutor()
        {
            string queryAutores = "SELECT * FROM Autores";
            DataTable tablaAutores = ConexionDb.EjecutarConsulta(queryAutores);

            string queryLibros = "SELECT * FROM Libros";
            DataTable tablaLibros = ConexionDb.EjecutarConsulta(queryLibros);

            var autores = new List<dynamic>();
            foreach (DataRow filaAutor in tablaAutores.Rows)
            {
                int autorId = Convert.ToInt32(filaAutor["Id"]);
                var libros = new List<dynamic>();

                foreach (DataRow filaLibro in tablaLibros.Rows)
                {
                    if (Convert.ToInt32(filaLibro["AutorId"]) == autorId)
                    {
                        libros.Add(new
                        {
                            Id = Convert.ToInt32(filaLibro["Id"]),
                            Titulo = filaLibro["Titulo"].ToString(),
                            ISBN = filaLibro["ISBN"].ToString(),
                            AñoPublicacion = Convert.ToInt32(filaLibro["AñoPublicacion"])
                        });
                    }
                }

                autores.Add(new
                {
                    Id = autorId,
                    Nombres = filaAutor["Nombres"].ToString(),
                    Apellidos = filaAutor["Apellidos"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(filaAutor["FechaNacimiento"]).ToString("yyyy-MM-dd"),
                    Libros = libros
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Lista de autores",
                Data = autores
            });
        }

        [HttpPost]
        [Route("CrearLibro")]
        public IActionResult CrearLibro([FromBody] dynamic nuevoLibro)
        {
            string query = "INSERT INTO Libros (Titulo, ISBN, AñoPublicacion, AutorId) VALUES (@Titulo, @ISBN, @AñoPublicacion, @AutorId)";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Titulo", nuevoLibro.Titulo.ToString()),
                new SqlParameter("@ISBN", nuevoLibro.ISBN.ToString()),
                new SqlParameter("@AñoPublicacion", Convert.ToInt32(nuevoLibro.AñoPublicacion)),
                new SqlParameter("@AutorId", Convert.ToInt32(nuevoLibro.AutorId))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);

            return Ok(new
            {
                Success = filas > 0,
                Message = filas > 0 ? "Libro creado" : "Error"
            });
        }

        [HttpGet]
        [Route("ObtenerLibroPorId/{id}")]
        public IActionResult ObtenerLibroPorId(int id)
        {
            string query = "SELECT * FROM Libros WHERE Id = @Id";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            DataTable tabla = ConexionDb.EjecutarConsulta(query, parametros);
            if (tabla.Rows.Count == 0)
            {
                return NotFound(new { Success = false, Message = $"No se encontró un libro {id}" });
            }

            DataRow fila = tabla.Rows[0];
            return Ok(new
            {
                Success = true,
                Message = "Libro encontrado",
                Data = new
                {
                    Id = Convert.ToInt32(fila["Id"]),
                    Titulo = fila["Titulo"].ToString(),
                    ISBN = fila["ISBN"].ToString(),
                    AñoPublicacion = Convert.ToInt32(fila["AñoPublicacion"]),
                    AutorId = Convert.ToInt32(fila["AutorId"])
                }
            });
        }

        [HttpGet]
        [Route("ListarLibros")]
        public IActionResult ListarLibros()
        {
            string query = "SELECT * FROM Libros";
            DataTable tabla = ConexionDb.EjecutarConsulta(query);

            var libros = new List<dynamic>();
            foreach (DataRow fila in tabla.Rows)
            {
                libros.Add(new
                {
                    Id = Convert.ToInt32(fila["Id"]),
                    Titulo = fila["Titulo"].ToString(),
                    ISBN = fila["ISBN"].ToString(),
                    AñoPublicacion = Convert.ToInt32(fila["AñoPublicacion"]),
                    AutorId = Convert.ToInt32(fila["AutorId"])
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Enhorabuena, los libros son los siguientes: ",
                Data = libros
            });
        }

        [HttpGet]
        [Route("ListarLibrosPorAutor/{id}")]
        public IActionResult ListarLibrosPorAutor(int id)
        {
            string query = "SELECT * FROM Libros WHERE AutorId = @AutorId";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@AutorId", id)
            };

            DataTable tabla = ConexionDb.EjecutarConsulta(query, parametros);

            var libros = new List<dynamic>();
            foreach (DataRow fila in tabla.Rows)
            {
                libros.Add(new
                {
                    Id = Convert.ToInt32(fila["Id"]),
                    Titulo = fila["Titulo"].ToString(),
                    ISBN = fila["ISBN"].ToString(),
                    AñoPublicacion = Convert.ToInt32(fila["AñoPublicacion"])
                });
            }

            return Ok(new
            {
                Success = true,
                Message = $"Lista de libros del autor = {id} obtenida correctamente",
                Data = libros
            });
        }

        [HttpPut]
        [Route("ActualizarLibro/{id}")]
        public IActionResult ActualizarLibro(int id, [FromBody] dynamic libroActualizado)
        {
            string query = "UPDATE Libros SET Titulo = @Titulo, ISBN = @ISBN, AñoPublicacion = @AñoPublicacion, AutorId = @AutorId WHERE Id = @Id";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Titulo", libroActualizado.Titulo.ToString()),
                new SqlParameter("@ISBN", libroActualizado.ISBN.ToString()),
                new SqlParameter("@AñoPublicacion", Convert.ToInt32(libroActualizado.AñoPublicacion)),
                new SqlParameter("@AutorId", Convert.ToInt32(libroActualizado.AutorId))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);

            return Ok(new
            {
                Success = filas > 0,
                Message = filas > 0 ? $"Libro n° {id} ha sido actualizado correctamente" : "Error"
            });
        }

        [HttpDelete]
        [Route("EliminarLibro/{id}")]
        public IActionResult EliminarLibro(int id)
        {
            string query = "DELETE FROM Libros WHERE Id = @Id";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);

            return Ok(new
            {
                Success = filas > 0,
                Message = filas > 0 ? $"Libro n° {id} ha sido eliminado correctamente" : "Error"
            });
        }

        [HttpPost]
        [Route("RegistrarPrestamo")]
        public IActionResult RegistrarPrestamo([FromBody] dynamic nuevoPrestamo)
        {
            string query = "INSERT INTO Prestamos (LibroId, FechaPrestamo, FechaDevolucion) VALUES (@LibroId, @FechaPrestamo, @FechaDevolucion)";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@LibroId", Convert.ToInt32(nuevoPrestamo.LibroId)),
                new SqlParameter("@FechaPrestamo", DateTime.Parse(nuevoPrestamo.FechaPrestamo.ToString())),
                new SqlParameter("@FechaDevolucion", string.IsNullOrEmpty(nuevoPrestamo.FechaDevolucion.ToString()) ? DBNull.Value : DateTime.Parse(nuevoPrestamo.FechaDevolucion.ToString()))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);

            return Ok(new
            {
                Success = filas > 0,
                Message = filas > 0 ? "El Préstamo ha sido registrado" : "Error"
            });
        }
    }
}
