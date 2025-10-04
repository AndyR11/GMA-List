using GMA_List.Resources.ViewModels;
using GMA_List.Resources.Services.Interfaz;

namespace GMA_List
{
    public partial class MainPageSeries : ContentPage
    {
        public MainPageSeries(ISupabaseService supabaseService)
        {
            InitializeComponent();
            BindingContext = new ListadoViewModel(supabaseService, "serie");
        }
    }
}