using EindToernooi_Poule.Code;
using PoolsBase;
using ReactiveUI;
using System;
using System.Collections.Generic;

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

        private List<string> poules;
        public List<string> Poules { get => poules; set => this.RaiseAndSetIfChanged(ref poules, value); }

        private string selectedpoule;
        public string SelectedPoule { get => selectedpoule; set => this.RaiseAndSetIfChanged(ref selectedpoule, value); }

        private string selectedmatch;
        public string SelectedMatch { get => selectedmatch; set => this.RaiseAndSetIfChanged(ref selectedmatch, value); }

        private List<MatchField> outputs;
        public List<MatchField> Outputs { get => outputs; set => this.RaiseAndSetIfChanged(ref outputs, value); }


        public scrMatchesVm()
        {
            Matches = new List<string>() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16" };
            SelectedMatch = Matches[0];
            Poules = new List<string>() { "1", "2", "3", "4", "5", "6", "KO" };
            SelectedPoule = Poules[0];
            Outputs = new List<MatchField>();
        }

        public void GetPredictionsCommand()
        {
            var r = int.TryParse(SelectedPoule, out int poule);
            Dictionary<string, MatchField> results = new Dictionary<string, MatchField>();

            foreach (Player p in scrPlayersVm.PlayerManager.Players)
            {
                int matchID = 8;
                if (SelectedMatch != "MOTW")
                    matchID = Convert.ToInt16(SelectedMatch) - 1;

                Match match = null;
                if (r)
                {
                    if (matchID <= 6)
                        match = p.Poules[poule].Matches[matchID];
                }
                else
                {
                    KOKeys key = KOKeys.LAST32;
                    if (!LocalConfiguration.Last32)
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
