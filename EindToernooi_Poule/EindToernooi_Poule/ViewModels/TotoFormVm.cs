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
        private Poule activePoule;
        private List<string> scoreProperties = new List<string>() { 
            "PouleScore1A", "PouleScore1B", "PouleScore2A", "PouleScore2B", "PouleScore3A", "PouleScore3B", "PouleScore4A", "PouleScore4B", "PouleScore5A", "PouleScore5B", "PouleScore6A", "PouleScore6B", 
            "L32Score1A", "L32Score1B", "L32Score2A", "L32Score2B", "L32Score3A", "L32Score3B", "L32Score4A", "L32Score4B",
            "L32Score5A", "L32Score5B", "L32Score6A", "L32Score6B", "L32Score7A", "L32Score7B", "L32Score8A", "L32Score8B",
            "L32Score9A", "L32Score9B", "L32Score10A", "L32Score10B", "L32Score11A", "L32Score11B", "L32Score12A", "L32Score12B",
            "L32Score13A", "L32Score13B", "L32Score14A", "L32Score14B", "L32Score15A", "L32Score15B", "L32Score16A", "L32Score16B",
            "L32AdditionalTime1", "L32AdditionalTime2", "L32AdditionalTime3", "L32AdditionalTime4", "L32AdditionalTime5", "L32AdditionalTime6", "L32AdditionalTime7", "L32AdditionalTime8",
            "L32AdditionalTime9", "L32AdditionalTime10", "L32AdditionalTime11", "L32AdditionalTime12", "L32AdditionalTime13", "L32AdditionalTime14", "L32AdditionalTime15", "L32AdditionalTime16",
            "L16Score1A", "L16Score1B", "L16Score2A", "L16Score2B", "L16Score3A", "L16Score3B", "L16Score4A", "L16Score4B",
            "L16Score5A", "L16Score5B", "L16Score6A", "L16Score6B", "L16Score7A", "L16Score7B", "L16Score8A", "L16Score8B",
            "L16AdditionalTime1", "L16AdditionalTime2", "L16AdditionalTime3", "L16AdditionalTime4", "L16AdditionalTime5", "L16AdditionalTime6", "L16AdditionalTime7", "L16AdditionalTime8"};


        private int currentPoule;
        public int CurrentPoule { get => currentPoule; set { SetCurrentPoule(value); this.RaisePropertyChanged(); } }
        
        private Player activeplayer;
        public Player ActivePlayer { get => activeplayer; private set { activeplayer = value; CurrentPoule = 1; } }
        
        public string PlayerName { get => activeplayer.Name; set { ActivePlayer.Name = value; this.RaisePropertyChanged(); } }
        public string PlayerTown { get => ActivePlayer.Town;  set { ActivePlayer.Town = value; this.RaisePropertyChanged(); } }

        private string currentpouletext;
        public string CurrentPouleText { get => currentpouletext; set => this.RaiseAndSetIfChanged(ref currentpouletext, value); }

        private string predictionsfilename;
        public string PredictionsFileName { get => predictionsfilename; set => this.RaiseAndSetIfChanged(ref predictionsfilename, value); }

        private int miss;
        public int Miss { get => miss; set => this.RaiseAndSetIfChanged(ref miss, value); }

        private bool groupphasechecked;
        public bool GroupPhaseChecked { get => groupphasechecked; set => this.RaiseAndSetIfChanged(ref  groupphasechecked, value); }

        private bool knockoutphasechecked;
        public bool KnockOutPhaseChecked { get => knockoutphasechecked; set => this.RaiseAndSetIfChanged(ref knockoutphasechecked, value); }

        private bool last32active;
        public bool Last32Active { get => last32active; set => this.RaiseAndSetIfChanged(ref last32active, value); }

        private bool last32inplay;
        public bool Last32InPlay { get => last32inplay; set => this.RaiseAndSetIfChanged(ref last32inplay, value); }

        private bool bronzeinplay;
        public bool BronzeInPlay { get => bronzeinplay; set => this.RaiseAndSetIfChanged(ref  bronzeinplay, value); }

        private bool nlpresent;
        public bool NlPresent { get => nlpresent; set => this.RaiseAndSetIfChanged(ref nlpresent, value); }

        #region PouleScores
        public int PouleScore1A { get => activePoule.Matches[0].ResultA; set { activePoule.Matches[0].ResultA = value; this.RaisePropertyChanged(); } }
        public int PouleScore1B { get => activePoule.Matches[0].ResultB; set { activePoule.Matches[0].ResultB = value; this.RaisePropertyChanged(); } }
        public int PouleScore2A { get => activePoule.Matches[1].ResultA; set { activePoule.Matches[1].ResultA = value; this.RaisePropertyChanged(); } }
        public int PouleScore2B { get => activePoule.Matches[1].ResultB; set { activePoule.Matches[1].ResultB = value; this.RaisePropertyChanged(); } }
        public int PouleScore3A { get => activePoule.Matches[2].ResultA; set { activePoule.Matches[2].ResultA = value; this.RaisePropertyChanged(); } }
        public int PouleScore3B { get => activePoule.Matches[2].ResultB; set { activePoule.Matches[2].ResultB = value; this.RaisePropertyChanged(); } }
        public int PouleScore4A { get => activePoule.Matches[3].ResultA; set { activePoule.Matches[3].ResultA = value; this.RaisePropertyChanged(); } }
        public int PouleScore4B { get => activePoule.Matches[3].ResultB; set { activePoule.Matches[3].ResultB = value; this.RaisePropertyChanged(); } }
        public int PouleScore5A { get => activePoule.Matches[4].ResultA; set { activePoule.Matches[4].ResultA = value; this.RaisePropertyChanged(); } }
        public int PouleScore5B { get => activePoule.Matches[4].ResultB; set { activePoule.Matches[4].ResultB = value; this.RaisePropertyChanged(); } }
        public int PouleScore6A { get => activePoule.Matches[5].ResultA; set { activePoule.Matches[5].ResultA = value; this.RaisePropertyChanged(); } }
        public int PouleScore6B { get => activePoule.Matches[5].ResultB; set { activePoule.Matches[5].ResultB = value; this.RaisePropertyChanged(); } }
        #endregion

        #region L32Scores
        public int L32Score1A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[0].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[0].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score1B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[0].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[0].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime1 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[0].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[0].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score2A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[1].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[1].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score2B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[1].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[1].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime2 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[1].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[1].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score3A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[2].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[2].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score3B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[2].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[2].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime3 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[2].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[2].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score4A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[3].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[3].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score4B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[3].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[3].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime4 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[3].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[3].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score5A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[4].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[4].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score5B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[4].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[4].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime5 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[4].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[4].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score6A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[5].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[5].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score6B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[5].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[5].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime6 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[5].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[5].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score7A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[6].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[6].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score7B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[6].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[6].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime7 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[6].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[6].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score8A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[7].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[7].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score8B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[7].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[7].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime8 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[7].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[7].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score9A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[8].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[8].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score9B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[8].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[8].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime9 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[8].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[8].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score10A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[9].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[9].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score10B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[9].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[9].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime10 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[9].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[9].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score11A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[10].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[10].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score11B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[10].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[10].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime11 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[10].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[10].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score12A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[11].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[11].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score12B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[11].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[11].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime12 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[11].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[11].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score13A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[12].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[12].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score13B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[12].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[12].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime13 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[12].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[12].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score14A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[13].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[13].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score14B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[13].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[13].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime14 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[13].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[13].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score15A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[14].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[14].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score15B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[14].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[14].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime15 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[14].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[14].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L32Score16A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[15].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[15].ResultA = value; this.RaisePropertyChanged(); } }
        public int L32Score16B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[15].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[15].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L32AdditionalTime16 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[15].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST32].Matches[15].AdditionalTime = value; this.RaisePropertyChanged(); } }
        #endregion

        #region L16Scores
        public int L16Score1A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[0].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[0].ResultA = value; this.RaisePropertyChanged(); } }
        public int L16Score1B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[0].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[0].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L16AdditionalTime1 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[0].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[0].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L16Score2A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[1].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[1].ResultA = value; this.RaisePropertyChanged(); } }
        public int L16Score2B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[1].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[1].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L16AdditionalTime2 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[1].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[1].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L16Score3A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[2].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[2].ResultA = value; this.RaisePropertyChanged(); } }
        public int L16Score3B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[2].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[2].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L16AdditionalTime3 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[2].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[2].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L16Score4A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[3].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[3].ResultA = value; this.RaisePropertyChanged(); } }
        public int L16Score4B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[3].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[3].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L16AdditionalTime4 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[3].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[3].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L16Score5A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[4].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[4].ResultA = value; this.RaisePropertyChanged(); } }
        public int L16Score5B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[4].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[4].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L16AdditionalTime5 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[4].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[4].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L16Score6A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[5].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[5].ResultA = value; this.RaisePropertyChanged(); } }
        public int L16Score6B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[5].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[5].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L16AdditionalTime6 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[5].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[5].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L16Score7A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[6].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[6].ResultA = value; this.RaisePropertyChanged(); } }
        public int L16Score7B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[6].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[6].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L16AdditionalTime7 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[6].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[6].AdditionalTime = value; this.RaisePropertyChanged(); } }
        public int L16Score8A { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[7].ResultA; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[7].ResultA = value; this.RaisePropertyChanged(); } }
        public int L16Score8B { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[7].ResultB; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[7].ResultB = value; this.RaisePropertyChanged(); } }
        public bool L16AdditionalTime8 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[7].AdditionalTime; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].Matches[7].AdditionalTime = value; this.RaisePropertyChanged(); } }
        #endregion

        public List<string> Last16 { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.LAST16].teams = value; this.RaisePropertyChanged(); } }
        public List<string> Quarter { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.QUARTER].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.QUARTER].teams = value; this.RaisePropertyChanged(); } }
        public List<string> Semi { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.SEMI].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.SEMI].teams = value; this.RaisePropertyChanged(); } }
        public List<string> Final { get => ActivePlayer.KnockoutPhase.Stages[KOKeys.FINAL].teams; set { ActivePlayer.KnockoutPhase.Stages[KOKeys.FINAL].teams = value; this.RaisePropertyChanged(); } }

        public string Champion { get => ActivePlayer.Questions.Answers[BonusKeys.Kampioen].Answer[0]; set {ActivePlayer.Questions.Answers[BonusKeys.Kampioen].Answer[0] = value.ToLower(); this.RaisePropertyChanged(); } }     
        public string Topscorer { get => ActivePlayer.Questions.Answers[BonusKeys.Topscorer].Answer[0]; set { ActivePlayer.Questions.Answers[BonusKeys.Topscorer].Answer[0] = value.ToLower(); this.RaisePropertyChanged(); } }
        public string Nederland { get => ActivePlayer.Questions.Answers[BonusKeys.Nederland].Answer[0]; set { ActivePlayer.Questions.Answers[BonusKeys.Nederland].Answer[0] = value.ToLower(); this.RaisePropertyChanged(); } }
        public string Bronze { get => ActivePlayer.Questions.Answers[BonusKeys.Bronze].Answer[0]; set { ActivePlayer.Questions.Answers[BonusKeys.Bronze].Answer[0] = value.ToLower(); this.RaisePropertyChanged(); } }

        public ReactiveCommand<Unit,Unit> NextPouleCommand { get; set; }
        public ReactiveCommand<Unit,Unit> PreviousPouleCommand { get; set; }
        public ReactiveCommand<Unit,Unit> ReadExcelCommand { get; set; }

        public TotoFormVm(Player activeplayer, Window totoformwindow)
        {
            if (activeplayer == null)
                this.activeplayer = ActivePlayer = CreateDefaultActivePlayer();
            else
                this.activeplayer = ActivePlayer = activeplayer;
            CurrentPoule = 1;        

            var NextWeekCommandCanExecute = this.WhenAnyValue(
                x => x.CurrentPoule,
                (a) => { return a < GeneralConfiguration.NrPoules; }).ObserveOn(RxApp.MainThreadScheduler);        

            var PreviousWeekCommandCanExecute = this.WhenAnyValue(
                x => x.CurrentPoule,
                (a) => { return a > 1; }).ObserveOn(RxApp.MainThreadScheduler);

            var ReadExcelCommandCanExecute = this.WhenAnyValue(
                x => x.PredictionsFileName,
                (a) => { return !string.IsNullOrEmpty(a); }).ObserveOn(RxApp.MainThreadScheduler);

            NextPouleCommand = ReactiveCommand.Create(() => { this.ChangeWeek(1); }, NextWeekCommandCanExecute);
            PreviousPouleCommand = ReactiveCommand.Create(() => { this.ChangeWeek(-1); }, PreviousWeekCommandCanExecute);
            ReadExcelCommand = ReactiveCommand.Create(() => { this.ReadPredictionsFromExcel(); }, ReadExcelCommandCanExecute);
            Miss = 0;
            predictionsfilename = "";
            PredictionsSubmittedFlag = false;
            totoformWindow = totoformwindow;
            Last32Active = GeneralConfiguration.Last32;
            Last32InPlay = GeneralConfiguration.Last32;
            BronzeInPlay = GeneralConfiguration.Bronze;
            NlPresent = GeneralConfiguration.NlPresent;
            SettingsVm.SettingsEvent += ImplementNewSettings;
        }

        public void SubmitCommand()
        {
            bool invalidpredictions = false;
            foreach (var ans in ActivePlayer.Questions.Answers)
            {
                foreach (var ansfield in ans.Value.Answer)
                {
                    if (ansfield == "" && !(ans.Key == BonusKeys.Bronze && !BronzeInPlay))
                        invalidpredictions = true;
                    if (ansfield == "" && !(ans.Key == BonusKeys.Nederland && !NlPresent))
                        invalidpredictions = true;
                }
            }

            if (invalidpredictions)
                PopupManager.ShowMessage("cannot submit predictions. Bonusquestions have not been filled in properly.");

            else
            {
                PredictionsSubmittedFlag = true;
                totoformWindow.Close();
            }
        }

        public void ToggleLast32Command()
        {
            Last32Active = !Last32Active;
        }

        public void ReadPredictionsFromExcel()
        {
            Excel.ExcelManager em = new Excel.ExcelManager();

            if (GroupPhaseChecked)
            {
                var poulephase = em.ReadGroupPhase(PredictionsFileName, 1, Miss, ActivePlayer.Poules);
                if (poulephase == null)
                    return;
                ActivePlayer.Poules = poulephase;
                CurrentPoule = 1;


                var bonus = em.ReadBonus(PredictionsFileName, 1);
                if (bonus == null)
                    return;
                ActivePlayer.Questions = bonus;
                this.RaisePropertyChanged(nameof(Champion));
                this.RaisePropertyChanged(nameof(Nederland));
                this.RaisePropertyChanged(nameof(Topscorer));
                this.RaisePropertyChanged(nameof(Bronze));
            }

            if (KnockOutPhaseChecked)
            {
                var kophase = em.readKnockout(PredictionsFileName,1, Miss, false);
                if (kophase == null)
                    return;
                ActivePlayer.KnockoutPhase = kophase;
                this.RaisePropertyChanged(nameof(Last16));
                this.RaisePropertyChanged(nameof(Quarter));
                this.RaisePropertyChanged(nameof(Semi));
                this.RaisePropertyChanged(nameof(Final));

                foreach (var prop in scoreProperties)
                    this.RaisePropertyChanged(prop);
            }

            PopupManager.ShowMessage("Predictions read");
        }

        private void ImplementNewSettings()
        {
            CurrentPoule = 1;
            Last32InPlay = GeneralConfiguration.Last32;
            Last32Active = GeneralConfiguration.Last32;
            BronzeInPlay = GeneralConfiguration.Bronze;
            NlPresent = GeneralConfiguration.NlPresent;
        }

        private void ChangeWeek(int change)
        {
            CurrentPoule += change;
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

        private void SetCurrentPoule(int value)
        {
            string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L" };
            currentPoule = value;
            CurrentPouleText = "Poule " + letters[value-1];
            activePoule = ActivePlayer.Poules[value];
            foreach (var prop in scoreProperties)
                this.RaisePropertyChanged(prop);
        }
    }
}
