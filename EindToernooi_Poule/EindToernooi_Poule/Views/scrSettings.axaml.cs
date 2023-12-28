using Avalonia.Controls;

namespace EindToernooi_Poule.Views
{
    public partial class scrSettings : UserControl
    {
        public scrSettings()
        {
            InitializeComponent();
            DataContext = new ViewModels.SettingsVm();
        }
    }
}
