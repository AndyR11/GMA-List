using GMA_List.Resources.ViewModels;
using GMA_List.Resources.Services.Interfaz;

namespace GMA_List
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Obtener el servicio desde el contenedor de DI
            var supabaseService = Handler?.MauiContext?.Services.GetService<ISupabaseService>();

            if (supabaseService != null)
            {
                // Detectar tipo según la ruta
                var tipo = Shell.Current.CurrentState.Location.ToString().Contains("animes")
                    ? "anime"
                    : "serie";

                BindingContext = new ListadoViewModel(supabaseService, tipo);
            }
        }
    }
}