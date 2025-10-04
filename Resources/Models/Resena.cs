// Models/Resena.cs
using Postgrest.Attributes;
using Postgrest.Models;

namespace GMA_List.Resources.Models
{
    [Table("resenas")]
    public class Resena : BaseModel
    {
        [PrimaryKey("id")]
        public Guid Id { get; set; }

        [Column("contenido_id")]
        public Guid ContenidoId { get; set; }

        [Column("usuario")]
        public string Usuario { get; set; } = string.Empty;

        [Column("resena")]
        public string? Texto { get; set; }

        [Column("visto")]
        public bool Visto { get; set; }
    }
}