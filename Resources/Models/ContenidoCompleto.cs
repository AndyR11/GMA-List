// Models/ContenidoCompleto.cs
namespace GMA_List.Resources.Models
{
    public class ContenidoCompleto
    {
        public Contenido Contenido { get; set; } = new();
        public Resena? ResenaYo { get; set; }
        public Resena? ResenaMarcela { get; set; }

        public bool VistoYo => ResenaYo?.Visto ?? false;
        public bool VistoMarcela => ResenaMarcela?.Visto ?? false;
        public bool VistoAlgunos => VistoYo || VistoMarcela;
    }
}