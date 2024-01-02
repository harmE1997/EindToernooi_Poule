using EindToernooi_Poule.Code;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace EindToernooi_Poule.ViewModels
{
    public class StatsField 
    {
        public string Name { get; set; }
        public int Number { get; set; }

        public string Names { get; set; }
    }
    public class scrStatsVm : ViewModelBase
    {
        private List<string> output;
        public List<string> Output { get => output; set => this.RaiseAndSetIfChanged(ref output, value); }
        private List<Stat> stats;

        private List<StatsField> statsFields;
        public List<StatsField> StatsFields { get => statsFields; set => this.RaiseAndSetIfChanged(ref statsFields, value); }

        public ReactiveCommand<Unit, Unit> ChampionsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NederlandCommand { get; set; }
        public ReactiveCommand<Unit, Unit> TopscorersCommand { get; set; }
        public ReactiveCommand<Unit, Unit> BronzeCommand { get; set; }

        public scrStatsVm()
        {
            stats = new List<Stat>();
            StatsFields = new List<StatsField>();
            ChampionsCommand = ReactiveCommand.Create(() => { this.ActionBonusQuestion(BonusKeys.Kampioen); });
            NederlandCommand = ReactiveCommand.Create(() => { this.ActionBonusQuestion(BonusKeys.Nederland); });
            TopscorersCommand = ReactiveCommand.Create(() => { this.ActionBonusQuestion(BonusKeys.Topscorer); });
            BronzeCommand = ReactiveCommand.Create(() => { this.ActionBonusQuestion(BonusKeys.Bronze); });
        }

        public void ActionBonusQuestion(BonusKeys Key)
        {
            stats.Clear();
            foreach (Player player in scrPlayersVm.PlayerManager.Players)
            {
                var Name = player.Name;
                var answer = player.Questions.Answers[Key];
                foreach (var e in answer.Answer)
                {
                    UpdateStats(e, Name);
                }
            }
            UpdateListBox();
        }

        private void UpdateStats(string stat, string playername)
        {
            Stat existingStat = null;
            foreach (Stat oldstat in stats)
            {
                if (oldstat.Name == stat)
                {
                    existingStat = oldstat;
                }
            }

            if (existingStat == null)
            {
                Stat newstat = new Stat(stat, playername);
                stats.Add(newstat);
            }

            else
            {
                existingStat.Add(playername);
            }
        }

        private void UpdateListBox()
        {
            List<StatsField> newoutput = new List<StatsField>();
            stats.Sort();
            foreach (Stat stat in stats)
            {
                var field = new StatsField() { Name = stat.Name, Number = stat.Number };
                foreach (var name in stat.Names)
                {
                    field.Names += name;
                    if(stat.Names.Last() != name)
                        field.Names += "\n";
                }
                newoutput.Add(field);
            }

            StatsFields = newoutput;
        }
    }
}
