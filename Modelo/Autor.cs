namespace PruebasCSF.Modelo
{
    public class Autor
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}
