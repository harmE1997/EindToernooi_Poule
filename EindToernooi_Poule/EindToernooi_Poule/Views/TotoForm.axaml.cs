using Avalonia.Controls;
using Avalonia.Interactivity;
using EindToernooi_Poule.Code;
using EindToernooi_Poule.ViewModels;
using System.Xml.Schema;

namespace EindToernooi_Poule.Views
{
    public partial class TotoForm : Window
    {
        private TotoFormVm viewmodel;

        public TotoForm()
        {
            InitializeComponent();
            viewmodel = new TotoFormVm(null, this);
            DataContext = viewmodel;
        }

        public TotoForm(Player activeplayer)
        {
            InitializeComponent();
            viewmodel = new TotoFormVm(activeplayer, this);
            DataContext = viewmodel;
        }
    }
}
