// Models/Contenido.cs
using Postgrest.Attributes;
using Postgrest.Models;

namespace GMA_List.Resources.Models
{
    [Table("contenido")]
    public class Contenido : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("tipo")]
        public string Tipo { get; set; } = string.Empty;

        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Column("imagen_url")]
        public string? ImagenUrl { get; set; }

        [Column("temporadas")]
        public int? Temporadas { get; set; }

        [Column("comentarios")]
        public string? Comentarios { get; set; }

        [Column("fecha_agregado")]
        public DateTime FechaAgregado { get; set; }
    }
}