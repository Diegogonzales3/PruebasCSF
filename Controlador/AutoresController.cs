using System.Data;
using Microsoft.AspNetCore.Mvc;
using PruebasCSF.Data;
using Microsoft.Data.SqlClient;

namespace PruebasCSF.Controlador
{
    [ApiController]
    [Route("api/[controller]")]
    public class AutoresController : ControllerBase
    {
        // Obtener todos los autores
        [HttpGet]
        public IActionResult ObtenerAutores()
        {
            string query = "SELECT * FROM Autores";
            DataTable tabla = ConexionDb.EjecutarConsulta(query);

            var autores = new List<dynamic>();
            foreach (DataRow fila in tabla.Rows)
            {
                autores.Add(new
                {
                    Id = Convert.ToInt32(fila["Id"]),
                    Nombres = fila["Nombres"].ToString(),
                    Apellidos = fila["Apellidos"].ToString(),
                    FechaNacimiento = Convert.ToDateTime(fila["FechaNacimiento"]).ToString("yyyy-MM-dd")
                });
            }

            return Ok(new { Success = true, Data = autores });
        }

        // Crear un nuevo autor
        [HttpPost]
        public IActionResult CrearAutor([FromBody] dynamic autor)
        {
            string query = "INSERT INTO Autores (Nombres, Apellidos, FechaNacimiento) VALUES (@Nombres, @Apellidos, @FechaNacimiento)";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Nombres", autor.Nombres.ToString()),
                new SqlParameter("@Apellidos", autor.Apellidos.ToString()),
                new SqlParameter("@FechaNacimiento", DateTime.Parse(autor.FechaNacimiento.ToString()))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);
            return Ok(new { Success = filas > 0 });
        }

        // Actualizar un autor
        [HttpPut("{id}")]
        public IActionResult ActualizarAutor(int id, [FromBody] dynamic autor)
        {
            string query = "UPDATE Autores SET Nombres = @Nombres, Apellidos = @Apellidos, FechaNacimiento = @FechaNacimiento WHERE Id = @Id";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id),
                new SqlParameter("@Nombres", autor.Nombres.ToString()),
                new SqlParameter("@Apellidos", autor.Apellidos.ToString()),
                new SqlParameter("@FechaNacimiento", DateTime.Parse(autor.FechaNacimiento.ToString()))
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);
            return Ok(new { Success = filas > 0 });
        }

        // Eliminar un autor
        [HttpDelete("{id}")]
        public IActionResult EliminarAutor(int id)
        {
            string query = "DELETE FROM Autores WHERE Id = @Id";
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            int filas = ConexionDb.EjecutarComando(query, parametros);
            return Ok(new { Success = filas > 0 });
        }
    }
}
