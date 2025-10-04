using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMA_List.Resources.Models;

namespace GMA_List.Resources.Services.Interfaz
{
    public interface ISupabaseService
    {
        Task<List<ContenidoCompleto>> ObtenerContenidosPorTipoAsync(string tipo, string filtroNombre = null, bool? filtroVisto = null);
        Task<ContenidoCompleto> ObtenerContenidoCompletoAsync(Guid id);
        Task<bool> GuardarContenidoAsync(Contenido contenido, Resena resenaYo, Resena resenaMarcela);
        Task<bool> ActualizarContenidoAsync(Contenido contenido, Resena resenaYo, Resena resenaMarcela);
        Task<bool> EliminarContenidoAsync(Guid id);
        Task<string> SubirImagenAsync(byte[] imagenBytes, string nombreArchivo);
    }
}
