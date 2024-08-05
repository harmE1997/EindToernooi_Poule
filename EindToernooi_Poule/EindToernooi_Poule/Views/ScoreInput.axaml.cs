using Avalonia;
using Avalonia.Controls;
using System;

namespace EindToernooi_Poule.Views
{
    public partial class ScoreInput : UserControl
    {
        private int scoreA;
        public int ScoreA
        {
            get { return scoreA; }
            set
            {
                SetAndRaise(ScoreAProperty, ref scoreA, value);
                this.FindControl<TextBox>("tbScoreA").Text = value.ToString();
            }
        }
        public static readonly DirectProperty<ScoreInput, int> ScoreAProperty = AvaloniaProperty.RegisterDirect<ScoreInput, int>(nameof(ScoreA), o => o.ScoreA, (o, v) => o.ScoreA = Convert.ToInt32(v), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        private int scoreB;
        public int ScoreB
        {
            get { return scoreB; }
            set
            {
                SetAndRaise(ScoreBProperty, ref scoreB, value);
                this.FindControl<TextBox>("tbScoreB").Text = value.ToString();
            }
        }
        public static readonly DirectProperty<ScoreInput, int> ScoreBProperty = AvaloniaProperty.RegisterDirect<ScoreInput, int>(nameof(ScoreB), o => o.ScoreB, (o, v) => o.ScoreB = Convert.ToInt32(v), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);


        private bool add;
        public bool Add
        {
            get { return add; }
            set
            {
                SetAndRaise(AddProperty, ref add, value);
                this.FindControl<CheckBox>("cbAdditionalTime").IsChecked = value;
            }
        }
        public static readonly DirectProperty<ScoreInput, bool> AddProperty = AvaloniaProperty.RegisterDirect<ScoreInput, bool>(nameof(Add), o => o.Add, (o, v) => o.Add = v, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        public ScoreInput()
        {
            InitializeComponent();
            ScoreA = 99;
            ScoreB = 99;
            Add = false;
        }
    }
}
