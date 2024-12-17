namespace PruebasCSF.Modelo
{
    public class Prestamo
    {
        public int Id { get; set; } 
        public int LibroId { get; set; } 
        public Libro Libro { get; set; } 

        public DateTime FechaPrestamo { get; set; } 
        public DateTime? FechaDevolucion { get; set; } 
    }
}
