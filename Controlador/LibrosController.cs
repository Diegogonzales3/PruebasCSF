using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using PruebasCSF.Data;

namespace PruebasCSF.Controlador
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : ControllerBase
    {
        // Obtener todos los libros
        [HttpGet]
        public IActionResult ObtenerLibros()
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

            return Ok(new { Success = true, Data = libros });
        }

        // Crear un nuevo libro
        [HttpPost]
        public IActionResult CrearLibro([FromBody] dynamic libro)
        {
            string query = "INSERT INTO Libros (Titulo, ISBN, AñoPublicacion, AutorId) VALUES (@Titulo, @ISBN, @AñoPublicacion, @AutorId)";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Titulo", libro.Titulo.ToString()),
                new SqlParameter("@ISBN", libro.ISBN.ToString()),
                new SqlParameter("@AñoPublicacion", Convert.ToInt32(libro.AñoPublicacion)),
                new SqlParameter("@AutorId", Convert.ToInt32(libro.AutorId))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);
            return Ok(new { Success = filas > 0 });
        }

        // Actualizar un libro
        [HttpPut("{id}")]
        public IActionResult ActualizarLibro(int id, [FromBody] dynamic libro)
        {
            string query = "UPDATE Libros SET Titulo = @Titulo, ISBN = @ISBN, AñoPublicacion = @AñoPublicacion, AutorId = @AutorId WHERE Id = @Id";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Titulo", libro.Titulo.ToString()),
                new SqlParameter("@ISBN", libro.ISBN.ToString()),
                new SqlParameter("@AñoPublicacion", Convert.ToInt32(libro.AñoPublicacion)),
                new SqlParameter("@AutorId", Convert.ToInt32(libro.AutorId))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);
            return Ok(new { Success = filas > 0 });
        }

        // Eliminar un libro
        [HttpDelete("{id}")]
        public IActionResult EliminarLibro(int id)
        {
            string query = "DELETE FROM Libros WHERE Id = @Id";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);
            return Ok(new { Success = filas > 0 });
        }
    }
}
