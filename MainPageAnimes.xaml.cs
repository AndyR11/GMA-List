using GMA_List.Resources.ViewModels;
using GMA_List.Resources.Services.Interfaz;

namespace GMA_List
{
    public partial class MainPageAnimes : ContentPage
    {
        public MainPageAnimes(ISupabaseService supabaseService)
        {
            InitializeComponent();
            BindingContext = new ListadoViewModel(supabaseService, "anime");
        }
    }
}