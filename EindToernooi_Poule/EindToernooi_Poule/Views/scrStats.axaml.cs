using Avalonia.Controls;
using EindToernooi_Poule.ViewModels;

namespace EindToernooi_Poule.Views
{
    public partial class scrStats : UserControl
    {
        public scrStats()
        {
            InitializeComponent();
            DataContext = new scrStatsVm();
        }
    }
}
