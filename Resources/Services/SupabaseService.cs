using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GMA_List.Resources.Models;
using GMA_List.Resources.Services.Interfaz;
using SupabaseClient = Supabase.Client;
using Supabase;
using Postgrest;
using static Postgrest.Constants;

namespace GMA_List.Resources.Services
{
    public class SupabaseService : ISupabaseService
    {
        private readonly SupabaseClient _supabase;

        public SupabaseService(string supabaseUrl, string supabaseKey)
        {
            var options = new SupabaseOptions
            {
                AutoRefreshToken = true,
                AutoConnectRealtime = true
            };
            _supabase = new SupabaseClient(supabaseUrl, supabaseKey, options);
        }

        public async Task<List<ContenidoCompleto>> ObtenerContenidosPorTipoAsync(
            string tipo,
            string? filtroNombre = null,
            bool? filtroVisto = null)
        {
            var query = _supabase
                .From<Contenido>()
                .Where(c => c.Tipo == tipo)
                .Order("fecha_agregado", Ordering.Descending);

            if (!string.IsNullOrWhiteSpace(filtroNombre))
                query = query.Where(c => c.Nombre.ToLower().Contains(filtroNombre.ToLower()));

            var contenidos = await query.Get();
            var resultado = new List<ContenidoCompleto>();

            foreach (var contenido in contenidos.Models)
            {
                var resenas = await _supabase
                    .From<Resena>()
                    .Where(r => r.ContenidoId == contenido.Id)
                    .Get();

                var contenidoCompleto = new ContenidoCompleto
                {
                    Contenido = contenido,
                    ResenaYo = resenas.Models.FirstOrDefault(r => r.Usuario == "Yo"),
                    ResenaMarcela = resenas.Models.FirstOrDefault(r => r.Usuario == "Marcela")
                };

                if (filtroVisto.HasValue)
                {
                    var cumpleFiltro = filtroVisto.Value
                        ? contenidoCompleto.VistoAlgunos
                        : !contenidoCompleto.VistoAlgunos;

                    if (cumpleFiltro)
                        resultado.Add(contenidoCompleto);
                }
                else
                {
                    resultado.Add(contenidoCompleto);
                }
            }

            return resultado;
        }

        public async Task<ContenidoCompleto?> ObtenerContenidoCompletoAsync(Guid id)
        {
            try
            {
                var contenido = await _supabase
                    .From<Contenido>()
                    .Where(c => c.Id == id)
                    .Single();

                if (contenido == null)
                    return null;

                var resenas = await _supabase
                    .From<Resena>()
                    .Where(r => r.ContenidoId == id)
                    .Get();

                return new ContenidoCompleto
                {
                    Contenido = contenido,
                    ResenaYo = resenas.Models.FirstOrDefault(r => r.Usuario == "Yo"),
                    ResenaMarcela = resenas.Models.FirstOrDefault(r => r.Usuario == "Marcela")
                };
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ActualizarContenidoAsync(
            Contenido contenido,
            Resena resenaYo,
            Resena resenaMarcela)
        {
            try
            {
                await _supabase.From<Contenido>().Update(contenido);
                await _supabase.From<Resena>().Update(resenaYo);
                await _supabase.From<Resena>().Update(resenaMarcela);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarContenidoAsync(Guid id)
        {
            try
            {
                await _supabase
                    .From<Contenido>()
                    .Where(c => c.Id == id)
                    .Delete();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> GuardarContenidoAsync(
            Contenido contenido,
            Resena resenaYo,
            Resena resenaMarcela)
        {
            try
            {
                contenido.FechaAgregado = DateTime.UtcNow;
                var contenidoGuardado = await _supabase.From<Contenido>().Insert(contenido);

                resenaYo.ContenidoId = contenidoGuardado.Models.First().Id;
                resenaMarcela.ContenidoId = contenidoGuardado.Models.First().Id;

                await _supabase.From<Resena>().Insert(new[] { resenaYo, resenaMarcela });

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> SubirImagenAsync(byte[] imagenBytes, string nombreArchivo)
        {
            try
            {
                var bucket = _supabase.Storage.From("imagenes-contenido");
                var path = $"{Guid.NewGuid()}_{nombreArchivo}";

                await bucket.Upload(imagenBytes, path);
                return bucket.GetPublicUrl(path);
            }
            catch
            {
                return null;
            }
        }
    }
}