namespace PruebasCSF.Modelo
{
    public class Libro
    {
        public int Id { get; set; } 
        public string Titulo { get; set; } 
        public string ISBN { get; set; } 
        public int AñoPublicacion { get; set; } 

        public int AutorId { get; set; } 
        public Autor Autor { get; set; } 
    }
}
