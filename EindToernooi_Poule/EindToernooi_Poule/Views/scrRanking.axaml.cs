using Avalonia.Controls;
using EindToernooi_Poule.ViewModels;

namespace EindToernooi_Poule.Views
{
    public partial class scrRanking : UserControl
    {
        public scrRanking()
        {
            InitializeComponent();
            DataContext = new scrRankingVm();
        }
    }
}
