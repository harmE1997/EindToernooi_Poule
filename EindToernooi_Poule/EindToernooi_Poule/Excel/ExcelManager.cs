using EindToernooi_Poule.Code;
using PoolsBase;
using PoolsBase.Excel;
using System;
using System.Collections.Generic;
using System.IO;



namespace EindToernooi_Poule.Excel
{
    public class ExcelManager : ExcelBase
    {
        public void ExportPlayersToExcel(List<Player> Players)
        {
            InitialiseWorkbook(GeneralConfiguration.AdminFileLocation, ExcelBaseConfiguration.RankingSheet);
            Dictionary<string, int> reads = new Dictionary<string, int>();
            if (xlRange.Cells[2, 2].value2 == null)
            {
                foreach (var player in Players)
                    reads.Add(player.Name, 0);
            }

            else
            {
                for (int i = 2; i < (Players.Count + 2); i++)
                {
                    string name = xlRange.Cells[i, 2].value2;
                    var oldscore = xlRange.Cells[i, 4].value2;
                    if (oldscore == null)
                        reads.Add(name.ToString(), 0);
                    else
                        reads.Add(name.ToString(), Convert.ToInt32(oldscore));
                }
            }

            int y = 2;
            foreach (Player player in Players)
            {
                xlRange.Cells[y, 1].value2 = player.Ranking;
                xlRange.Cells[y, 2].value2 = player.Name;
                xlRange.Cells[y, 3].value2 = player.Town;
                xlRange.Cells[y, 8].value2 = player.TotalScore - reads[player.Name];
                xlRange.Cells[y, 4].value2 = player.TotalScore;
                xlRange.Cells[y, 5].value2 = player.PoulesScore;
                xlRange.Cells[y, 6].value2 = player.KnockoutScore;
                xlRange.Cells[y, 7].value2 = player.BonusScore;
                y++;
            }
            CleanWorkbook();
        }

        public Dictionary<int, Poule> ReadGroupPhase(string filename, int sheet, int miss, Dictionary<int, Poule> Poules = null, bool host = false)
        {
            var poules = new Dictionary<int, Poule>();
            if (Poules != null)
                poules = Poules;

            try
            {
                if (!File.Exists(filename))
                {
                    PopupManager.ShowMessage("Cannot read host. Admin cannot be found");
                    return poules;
                }

                InitialiseWorkbook(filename, sheet);
                for (int i = 0; i < LocalConfiguration.NrPoules; i++)
                {
                    var matches = ReadBlock(i, LocalConfiguration.PouleSize, miss, host);
                    if (matches == null)
                    {
                        PopupManager.ShowMessage("Cannot read predictions. Problem at poule " + (i + 1));
                        CleanWorkbook();
                        return null;
                    }

                    if (poules.ContainsKey(i + 1))
                        poules[i + 1] = new Poule(i + 1, matches);
                    else
                        poules.Add(i + 1, new Poule((i + 1), matches));
                }
                CleanWorkbook();
                return poules;
            }

            catch (Exception e) { CleanWorkbook(); return poules; }
        }

        public KnockoutPhase readKnockout(string filename, int sheet, int miss, bool host = false)
        {
            InitialiseWorkbook(filename, sheet);
            try
            {
                KnockoutPhase ko = new KnockoutPhase();
                foreach (var phase in ExcelLocalConfiguration.KoSettings)
                {
                    if (phase.PhaseKey == KOKeys.LAST32)
                    {
                        if (LocalConfiguration.Last32)
                        {
                            ko.Stages[phase.PhaseKey].Matches = ReadKnockOutPoule(miss, phase.Size, phase.StartRow, host);
                            ko.Stages[phase.PhaseKey].UseMatches = true;
                        }
                        continue;
                    }

                    if (phase.PhaseKey == KOKeys.LAST16 && !LocalConfiguration.Last32)
                    {
                        ko.Stages[phase.PhaseKey].Matches = ReadKnockOutPoule(miss, phase.Size, phase.StartRow, host);
                        ko.Stages[phase.PhaseKey].UseMatches = true;
                        continue;
                    }

                    ko.Stages[phase.PhaseKey].teams.Clear();
                    ko.Stages[phase.PhaseKey].UseMatches = false;
                    for (int i = 0; i < phase.Size; i++)
                    {
                        int row = phase.StartRow + (phase.GapSize * i);
                        var team = xlRange.Cells[row, phase.Column].value2;
                        if (team == null)
                        {
                            if (host)
                            {
                                team = "";
                            }

                            else
                            {
                                PopupManager.ShowMessage("Cannot read predictions. Problem at stage " + phase.PhaseKey);
                                return null;
                            }
                        }

                        ko.Stages[phase.PhaseKey].teams.Add(team.ToLower());
                    }
                }
                return ko;
            }
            catch (Exception e) { return null; }
            finally { CleanWorkbook(); }
        }

        private KOMatch[] ReadKnockOutPoule(int miss, int size, int startrow, bool host = false)
        {
            KOMatch[] Poule = new KOMatch[size];

            int Startrow = startrow + miss;

            try
            {
                for (int rowschecked = 0; rowschecked < size; rowschecked++)
                {
                    double a = 99;
                    double b = 99;
                    bool ad = false;
                    int currentRow = Startrow + rowschecked;

                    var at = xlRange.Cells[currentRow, ExcelBaseConfiguration.HomeColumn].Value2;
                    var bt = xlRange.Cells[currentRow, ExcelBaseConfiguration.OutColumn].Value2;
                    string adt = xlRange.Cells[currentRow, ExcelBaseConfiguration.OutColumn + 1].Value2;

                    if (at == null || bt == null || adt == null)
                    {
                        if (!host)
                            return null;
                    }

                    else
                    {
                        a = at;
                        b = bt;
                        if (adt.ToLower() == "ja")
                            ad = true;
                    }

                    KOMatch match = new KOMatch(Convert.ToInt16(a), Convert.ToInt16(b), ad, 0);
                    Poule[rowschecked] = match;
                }
                return Poule;
            }
            catch (Exception e) { return null; }
        }
    }
}
