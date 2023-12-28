using Avalonia.Controls;

namespace EindToernooi_Poule.Views
{
    public partial class scrPlayers : UserControl
    {
        public scrPlayers()
        {
            InitializeComponent();
            DataContext = new ViewModels.scrPlayersVm();
        }
    }
}
