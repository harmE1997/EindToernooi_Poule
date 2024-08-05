using EindToernooi_Poule.Code;
using EindToernooi_Poule.Excel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EindToernooi_Poule.ViewModels
{
    public class MatchField : IComparable<MatchField>
    {
        public string Result { get; set; }
        public int NrPredictions { get; set; }
        public string Names { get; set; }

        public int CompareTo(MatchField? other)
        {
            return string.Compare(other.Result, Result);
        }
    }

    public class scrMatchesVm : ViewModelBase
    {
        private List<string> matches;
        public List<string> Matches { get => matches; set => this.RaiseAndSetIfChanged(ref matches, value); }

        private List<string> weeks;
        public List<string> Weeks { get => weeks; set => this.RaiseAndSetIfChanged(ref weeks, value); }

        private string selectedweek;
        public string SelectedWeek { get => selectedweek; set => this.RaiseAndSetIfChanged(ref selectedweek, value); }

        private string selectedmatch;
        public string SelectedMatch { get => selectedmatch; set => this.RaiseAndSetIfChanged(ref selectedmatch, value); }

        private List<MatchField> outputs;
        public List<MatchField> Outputs { get => outputs; set => this.RaiseAndSetIfChanged(ref outputs, value); }  


        public scrMatchesVm()
        {
            Matches = new List<string>() {"1","2","3","4","5","6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
            SelectedMatch = Matches[0];
            Weeks = new List<string>() {"1", "2", "3", "4", "5", "6", "KO"};
            SelectedWeek = Weeks[0];
            Outputs = new List<MatchField>();
        }

        public void GetPredictionsCommand()
        {
            var r = int.TryParse(SelectedWeek, out int week);
            Dictionary<string, MatchField> results = new Dictionary<string, MatchField>();

            foreach (Player p in scrPlayersVm.PlayerManager.Players)
            {
                int matchID = 8;
                if (SelectedMatch != "MOTW")
                    matchID = Convert.ToInt16(SelectedMatch) - 1;

                Match match = null;
                if (r)
                {
                    if(matchID <= 6)
                        match = p.Poules[week].Matches[matchID];
                }
                else
                {
                    KOKeys key = KOKeys.LAST32;
                    if (!GeneralConfiguration.Last32)
                        key = KOKeys.LAST16;
                    match = p.KnockoutPhase.Stages[key].Matches[matchID];
                }

                if (match != null)
                {
                    if (results.ContainsKey(match.Winner))
                        results[match.Winner].NrPredictions++;
                    else
                        results.Add(match.Winner, new MatchField() { Result = match.Winner, NrPredictions = 1, Names = "" });

                    if (!r)
                    {
                        if (results.ContainsKey((match as KOMatch).AdditionalTime.ToString()))
                            results[(match as KOMatch).AdditionalTime.ToString()].NrPredictions++;
                        else
                            results.Add((match as KOMatch).AdditionalTime.ToString(), new MatchField() { Result = (match as KOMatch).AdditionalTime.ToString(), NrPredictions = 1, Names = "" });
                    }

                    if (results.ContainsKey(match.MatchToString()))
                        results[match.MatchToString()].NrPredictions++;
                    else
                        results.Add(match.MatchToString(), new MatchField() { Result = match.MatchToString(), NrPredictions = 1, Names = "" });

                    if (results[match.MatchToString()].Names == "")
                        results[match.MatchToString()].Names += p.Name;
                    else
                        results[match.MatchToString()].Names += "\n" + p.Name;
                }
            }

            var output = new List<MatchField>();
            foreach (var result in results)
            {
                output.Add(result.Value);
            }
            output.Sort();
            Outputs = output;
        }
    }
}
