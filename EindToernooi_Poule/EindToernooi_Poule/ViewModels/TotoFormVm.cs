using Microsoft.CodeAnalysis.Operations;
using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Linq;
using EindToernooi_Poule.Code;
using EindToernooi_Poule.Views;
using Avalonia.Controls;

namespace EindToernooi_Poule.ViewModels
{
    public class TotoFormVm : ViewModelBase
    {
        public bool PredictionsSubmittedFlag = false;
        private Window totoformWindow;
        private Poule activeWeek;
        private List<string> scoreProperties = new List<string>() { 
            "Score1A", "Score1B", "Score2A", "Score2B", "Score3A", "Score3B", "Score4A", "Score4B",
        "Score5A", "Score5B", "Score6A", "Score6B"};


        private int currentWeek;
        public int CurrentWeek { get => currentWeek; set { SetCurrentWeek(value); this.RaisePropertyChanged(); } }
        
        private Player activeplayer;
        public Player ActivePlayer { get => activeplayer; private set { activeplayer = value; CurrentWeek = 1; } }
        
        public string PlayerName { get => activeplayer.Name; set { ActivePlayer.Name = value; this.RaisePropertyChanged(); } }
        public string PlayerTown { get => ActivePlayer.Town;  set { ActivePlayer.Town = value; this.RaisePropertyChanged(); } }

        private string currentweektext;
        public string CurrentWeekText { get => currentweektext; set => this.RaiseAndSetIfChanged(ref currentweektext, value); }

        private string predictionsfilename;
        public string PredictionsFileName { get => predictionsfilename; set => this.RaiseAndSetIfChanged(ref predictionsfilename, value); }

        private int miss;
        public int Miss { get => miss; set => this.RaiseAndSetIfChanged(ref miss, value); }

        private bool last32active;
        public bool Last32Active { get => last32active; set => this.RaiseAndSetIfChanged(ref last32active, value); }

        public int Score1A { get => activeWeek.Matches[0].ResultA; set { activeWeek.Matches[0].ResultA = value; this.RaisePropertyChanged(); } }
        public int Score1B { get => activeWeek.Matches[0].ResultB; set { activeWeek.Matches[0].ResultB = value; this.RaisePropertyChanged(); } }
        public int Score2A { get => activeWeek.Matches[1].ResultA; set { activeWeek.Matches[1].ResultA = value; this.RaisePropertyChanged(); } }
        public int Score2B { get => activeWeek.Matches[1].ResultB; set { activeWeek.Matches[1].ResultB = value; this.RaisePropertyChanged(); } }
        public int Score3A { get => activeWeek.Matches[2].ResultA; set { activeWeek.Matches[2].ResultA = value; this.RaisePropertyChanged(); } }
        public int Score3B { get => activeWeek.Matches[2].ResultB; set { activeWeek.Matches[2].ResultB = value; this.RaisePropertyChanged(); } }
        public int Score4A { get => activeWeek.Matches[3].ResultA; set { activeWeek.Matches[3].ResultA = value; this.RaisePropertyChanged(); } }
        public int Score4B { get => activeWeek.Matches[3].ResultB; set { activeWeek.Matches[3].ResultB = value; this.RaisePropertyChanged(); } }
        public int Score5A { get => activeWeek.Matches[4].ResultA; set { activeWeek.Matches[4].ResultA = value; this.RaisePropertyChanged(); } }
        public int Score5B { get => activeWeek.Matches[4].ResultB; set { activeWeek.Matches[4].ResultB = value; this.RaisePropertyChanged(); } }
        public int Score6A { get => activeWeek.Matches[5].ResultA; set { activeWeek.Matches[5].ResultA = value; this.RaisePropertyChanged(); } }
        public int Score6B { get => activeWeek.Matches[5].ResultB; set { activeWeek.Matches[5].ResultB = value; this.RaisePropertyChanged(); } }

        public List<string> Last32 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].teams = value; this.RaisePropertyChanged(); } }
        public List<string> Last16 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].teams = value; this.RaisePropertyChanged(); } }
        public List<string> Quarter { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.QUARTER].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.QUARTER].teams = value; this.RaisePropertyChanged(); } }
        public List<string> Semi { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.SEMI].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.SEMI].teams = value; this.RaisePropertyChanged(); } }
        public List<string> Final { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.FINAL].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.FINAL].teams = value; this.RaisePropertyChanged(); } }

        public string Champion { get => ActivePlayer.Questions.Answers[BonusKeys.Kampioen].Answer[0]; set {ActivePlayer.Questions.Answers[BonusKeys.Kampioen].Answer[0] = value.ToLower(); this.RaisePropertyChanged(); } }
        public string Nederland { get => ActivePlayer.Questions.Answers[BonusKeys.Nederland].Answer[0]; set { ActivePlayer.Questions.Answers[BonusKeys.Nederland].Answer[0] = value.ToLower(); this.RaisePropertyChanged(); } }
        public string Topscorer { get => ActivePlayer.Questions.Answers[BonusKeys.Topscorer].Answer[0]; set { ActivePlayer.Questions.Answers[BonusKeys.Topscorer].Answer[0] = value.ToLower(); this.RaisePropertyChanged(); } }
        public string Bronze { get => ActivePlayer.Questions.Answers[BonusKeys.Bronze].Answer[0]; set { ActivePlayer.Questions.Answers[BonusKeys.Bronze].Answer[0] = value.ToLower(); this.RaisePropertyChanged(); } }

        public ReactiveCommand<Unit,Unit> NextWeekCommand { get; set; }
        public ReactiveCommand<Unit,Unit> PreviousWeekCommand { get; set; }
        public ReactiveCommand<Unit,Unit> ReadExcelCommand { get; set; }

        public TotoFormVm(Player activeplayer, Window totoformwindow)
        {
            if (activeplayer == null)
                this.activeplayer = ActivePlayer = CreateDefaultActivePlayer();
            else
                this.activeplayer = ActivePlayer = activeplayer;
            CurrentWeek = 1;        

            var NextWeekCommandCanExecute = this.WhenAnyValue(
                x => x.CurrentWeek,
                (a) => { return a < GeneralConfiguration.NrPoules; }).ObserveOn(RxApp.MainThreadScheduler);        

            var PreviousWeekCommandCanExecute = this.WhenAnyValue(
                x => x.CurrentWeek,
                (a) => { return a > 1; }).ObserveOn(RxApp.MainThreadScheduler);

            var ReadExcelCommandCanExecute = this.WhenAnyValue(
                x => x.PredictionsFileName,
                (a) => { return !string.IsNullOrEmpty(a); }).ObserveOn(RxApp.MainThreadScheduler);

            NextWeekCommand = ReactiveCommand.Create(() => { this.ChangeWeek(1); }, NextWeekCommandCanExecute);
            PreviousWeekCommand = ReactiveCommand.Create(() => { this.ChangeWeek(-1); }, PreviousWeekCommandCanExecute);
            ReadExcelCommand = ReactiveCommand.Create(() => { this.ReadPredictionsFromExcel(); }, ReadExcelCommandCanExecute);
            Miss = 0;
            predictionsfilename = "";
            PredictionsSubmittedFlag = false;
            totoformWindow = totoformwindow;
            Last32Active = GeneralConfiguration.Last32;
        }

        public void SubmitCommand()
        {
            bool invalidpredictions = false;
            foreach (var kostage in ActivePlayer.KnockoutPhase.Stages)
            {
                if (kostage.Key == KOKeys.LAST32 && !GeneralConfiguration.Last32)
                    continue;
                foreach(var team in kostage.Value.teams) 
                {
                    if (team == "")
                        invalidpredictions = true;
                }
            }

            foreach (var ans in ActivePlayer.Questions.Answers)
            {
                foreach (var ansfield in ans.Value.Answer)
                {
                    if (ansfield == "")
                        invalidpredictions = true;
                }
            }

            if (invalidpredictions)
                PopupManager.ShowMessage("cannot submit predictions. One or more bonusquestions have not been filled in.");

            else
            {
                PredictionsSubmittedFlag = true;
                totoformWindow.Close();
            }
        }

        public void ToggleLast32Command()
        {
            if(GeneralConfiguration.Last32)
                Last32Active = !Last32Active;
        }

        public void ReadPredictionsFromExcel()
        {
            Excel.ExcelManager em = new Excel.ExcelManager();
            ActivePlayer.Poules = em.ReadPredictions(PredictionsFileName, 1, Miss, ActivePlayer.Poules);
            ActivePlayer.KnockoutPhase = em.readKnockout(PredictionsFileName, 1);
            this.RaisePropertyChanged(nameof(Last32));
            this.RaisePropertyChanged(nameof(Last16));
            this.RaisePropertyChanged(nameof(Quarter));
            this.RaisePropertyChanged(nameof(Semi));
            this.RaisePropertyChanged(nameof(Final));
            CurrentWeek = 1;
            PopupManager.ShowMessage("Predictions read");
        }

        private void ChangeWeek(int change)
        {
            CurrentWeek += change;
        }

        private Player CreateDefaultActivePlayer()
        {
            var weeks = new Dictionary<int, Poule>();
            for (int i = 1; i<= GeneralConfiguration.NrPoules; i++)
            {
                var matches = new Match[6];
                for(int x = 0; x < 6; x++)
                {
                    matches[x] = new Match(99, 99);             
                }

                weeks.Add(i,new Poule(i, matches));
            }
            return new Player("", "", weeks, new KnockoutPhase(), new BonusQuestions(new string[4]));
        }

        private void SetCurrentWeek(int value)
        {
            currentWeek = value;
            CurrentWeekText = "Poule " + value;
            activeWeek = ActivePlayer.Poules[value];
            foreach (var prop in scoreProperties)
                this.RaisePropertyChanged(prop);
        }
    }
}
