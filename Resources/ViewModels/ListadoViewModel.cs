using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using GMA_List.Resources.Models;
using GMA_List.Resources.Services.Interfaz;
using MvvmHelpers;

namespace GMA_List.Resources.ViewModels
{
    public class ListadoViewModel : BaseViewModel
    {
        private readonly ISupabaseService _supabaseService;
        private ObservableCollection<ContenidoCompleto> _contenidos = new();
        private string _filtroNombre = string.Empty;
        private bool? _filtroVisto;
        private string _tipo = string.Empty;
        private bool _isBusy;

        public ObservableCollection<ContenidoCompleto> Contenidos
        {
            get => _contenidos;
            set => SetProperty(ref _contenidos, value);
        }

        public string FiltroNombre
        {
            get => _filtroNombre;
            set
            {
                SetProperty(ref _filtroNombre, value);
                _ = CargarContenidosAsync();
            }
        }

        public string Tipo
        {
            get => _tipo;
            set => SetProperty(ref _tipo, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand CargarCommand { get; }
        public ICommand AgregarCommand { get; }
        public ICommand VerDetalleCommand { get; }
        public ICommand FiltrarTodosCommand { get; }
        public ICommand FiltrarVistosCommand { get; }
        public ICommand FiltrarNoVistosCommand { get; }

        public ListadoViewModel(ISupabaseService supabaseService, string tipo)
        {
            _supabaseService = supabaseService;
            Tipo = tipo;

            CargarCommand = new Command(async () => await CargarContenidosAsync());
            AgregarCommand = new Command(async () => await NavegarAgregarAsync());
            VerDetalleCommand = new Command<ContenidoCompleto>(async (c) => await NavegarDetalleAsync(c));

            FiltrarTodosCommand = new Command(() =>
            {
                _filtroVisto = null;
                _ = CargarContenidosAsync();
            });

            FiltrarVistosCommand = new Command(() =>
            {
                _filtroVisto = true;
                _ = CargarContenidosAsync();
            });

            FiltrarNoVistosCommand = new Command(() =>
            {
                _filtroVisto = false;
                _ = CargarContenidosAsync();
            });

            _ = CargarContenidosAsync();
        }

        private async Task CargarContenidosAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var contenidos = await _supabaseService.ObtenerContenidosPorTipoAsync(
                    Tipo,
                    FiltroNombre,
                    _filtroVisto
                );
                Contenidos = new ObservableCollection<ContenidoCompleto>(contenidos);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task NavegarAgregarAsync()
        {
            // TODO: Implementar navegación a página de agregar
            await Shell.Current.GoToAsync("agregar", new Dictionary<string, object>
            {
                { "Tipo", Tipo }
            });
        }

        private async Task NavegarDetalleAsync(ContenidoCompleto contenido)
        {
            if (contenido == null) return;

            // TODO: Implementar navegación a página de detalle
            await Shell.Current.GoToAsync("detalle", new Dictionary<string, object>
            {
                { "ContenidoId", contenido.Contenido.Id }
            });
        }


    }
}