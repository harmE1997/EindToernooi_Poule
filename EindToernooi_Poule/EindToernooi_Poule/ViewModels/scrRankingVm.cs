using EindToernooi_Poule.Code;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EindToernooi_Poule.ViewModels
{
    public class RankingField
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public int Total { get; set; }
        public int Matches { get; set; }
        public int Knockout { get; set; }
        public int Bonus { get; set; }
        
    }
    public class scrRankingVm : ViewModelBase
    {
        private Host host;

        private List<RankingField> ranking;
        public List<RankingField> Ranking { get => ranking; set => this.RaiseAndSetIfChanged(ref ranking, value); }

        public scrRankingVm()
        {
            host = new Host();
            Ranking = new List<RankingField>();
            SettingsVm.SettingsEvent += RefreshRanking;
        }

        public void CalculateNewRanking()
        {
            try
            {
                host.setHost();
                scrPlayersVm.PlayerManager.CheckAllPlayers(host);
                RefreshRanking();
                PopupManager.ShowMessage("New ranking calculated");
            }

            catch (FileNotFoundException) { PopupManager.ShowMessage("Excel file does not exist"); }
            catch (Exception e){ PopupManager.ShowMessage(e.Message); }
        }

        public void ExportRanking()
        {
            var excelManager = new Excel.ExcelManager();

            excelManager.ExportPlayersToExcel(scrPlayersVm.PlayerManager.Players);
            PopupManager.ShowMessage("Ranking sucessfully exported");
        }

        public void ResetHost()
        {
            host.HostSet = false;
            host.setHost();
            PopupManager.ShowMessage("Host was reset succesfully");
        }

        private void RefreshRanking()
        {
            //sort players by score
            scrPlayersVm.PlayerManager.RankPlayers();
            List<RankingField> rank = new List<RankingField>();
            foreach (Player player in scrPlayersVm.PlayerManager.Players)
            {
                rank.Add(new RankingField() { Rank = player.Ranking, Name = player.Name, Total = player.TotalScore, 
                    Matches = player.PoulesScore, Knockout=player.KnockoutScore, Bonus=player.BonusScore }) ;
            }

            Ranking = rank;
        }
    }
}
