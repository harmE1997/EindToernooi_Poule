using Avalonia.Controls;
using EindToernooi_Poule.ViewModels;

namespace EindToernooi_Poule.Views
{
    public partial class scrMatches : UserControl
    {
        public scrMatches()
        {
            InitializeComponent();
            DataContext = new scrMatchesVm();
        }
    }
}
