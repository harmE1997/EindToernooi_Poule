using EindToernooi_Poule.Code;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private bool last32inplay;
        public bool Last32InPlay { get => last32inplay; set => this.RaiseAndSetIfChanged(ref last32inplay, value); }

        private bool bronzeinplay;
        public bool BronzeInPlay { get => bronzeinplay; set => this.RaiseAndSetIfChanged(ref bronzeinplay, value); }

        public ReactiveCommand<Unit, Unit> ChampionsCommand { get; set; }
        public ReactiveCommand<Unit, Unit> NederlandCommand { get; set; }
        public ReactiveCommand<Unit, Unit> TopscorersCommand { get; set; }
        public ReactiveCommand<Unit, Unit> BronzeCommand { get; set; }

        public ReactiveCommand<Unit, Unit> Last32Command { get; set; }
        public ReactiveCommand<Unit, Unit> Last16Command { get; set; }
        public ReactiveCommand<Unit, Unit> QuarterCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SemiCommand { get; set; }
        public ReactiveCommand<Unit, Unit> FinalCommand { get; set; }

        public scrStatsVm()
        {
            stats = new List<Stat>();
            StatsFields = new List<StatsField>();
            ChampionsCommand = ReactiveCommand.Create(() => { this.ActionStats(BonusKeys.Kampioen); });
            NederlandCommand = ReactiveCommand.Create(() => { this.ActionStats(BonusKeys.Nederland); });
            TopscorersCommand = ReactiveCommand.Create(() => { this.ActionStats(BonusKeys.Topscorer); });
            BronzeCommand = ReactiveCommand.Create(() => { this.ActionStats(BonusKeys.Bronze); });

            Last32Command = ReactiveCommand.Create(() => { this.ActionStats(koKey: KOKeys.LAST32); });
            Last16Command = ReactiveCommand.Create(() => { this.ActionStats(koKey: KOKeys.LAST16); });
            QuarterCommand = ReactiveCommand.Create(() => { this.ActionStats(koKey: KOKeys.QUARTER); });
            SemiCommand = ReactiveCommand.Create(() => { this.ActionStats(koKey: KOKeys.SEMI); });
            FinalCommand = ReactiveCommand.Create(() => { this.ActionStats(koKey: KOKeys.FINAL); });

            SettingsVm.SettingsEvent += ImplementNewSettings;
        }

        public void ActionStats(BonusKeys bonusKey = BonusKeys.Default, KOKeys koKey = KOKeys.DEFAULT)
        {
            stats.Clear();
            foreach (Player player in scrPlayersVm.PlayerManager.Players)
            {
                var name = player.Name;
                if (bonusKey != BonusKeys.Default)
                {
                    var answer = player.Questions.Answers[bonusKey];
                    foreach (var e in answer.Answer)
                    {
                        UpdateStats(e, name);
                    }
                }

                else
                {
                    var ans = player.KnockoutPhase.Stages[koKey];
                    foreach (var t in ans.teams)
                    {
                        UpdateStats(t, name);
                    }
                }
            }
            UpdateListBox();
        }

        private void ImplementNewSettings()
        {
            BronzeInPlay = GeneralConfiguration.Bronze;
            Last32InPlay = GeneralConfiguration.Last32;
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
