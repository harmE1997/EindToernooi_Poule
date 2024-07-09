using EindToernooi_Poule.Code;
using excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace EindToernooi_Poule.Excel
{
    public class ExcelManager
    {
        private excel.Application xlApp;
        private excel.Workbook xlWorkbook;
        private excel._Worksheet xlWorksheet;
        private excel.Range xlRange;

        public void ExportPlayersToExcel(List<Player> Players)
        {
            InitialiseWorkbook(GeneralConfiguration.AdminFileLocation, ExcelConfiguration.RankingSheet);
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
                for (int i = 0; i < GeneralConfiguration.NrPoules; i++)
                {
                    var matches = ReadSinglePoule(i, miss, host);
                    if (matches == null)
                    {
                        PopupManager.ShowMessage("Cannot read predictions. Problem at week " + (i + 1));
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

        public KnockoutPhase readKnockout(string filename, int sheet, bool host = false)
        {
            InitialiseWorkbook(filename, sheet);
            try
            {
                KnockoutPhase ko = new KnockoutPhase();
                foreach (var phase in ExcelConfiguration.KoSettings)
                {
                    if (!GeneralConfiguration.Last32 && phase.PhaseKey == KOKeys.LAST32)
                        continue;
                    ko.Stages[phase.PhaseKey].teams.Clear();
                    for (int i = 0; i < phase.Size; i++)
                    {
                        int row = phase.StartRow + (phase.GapSize * i);
                        string team = xlRange.Cells[row, phase.Column].value2;
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


        public BonusQuestions ReadBonus(string filename, int sheet)
        {
            InitialiseWorkbook(filename, sheet);
            try
            {
                string[] answers = new string[4];
                for (int i = ExcelConfiguration.BonusStartRow; i < ExcelConfiguration.BonusStartRow; i++)
                {
                    string value = xlRange.Cells[i, ExcelConfiguration.BonusAnswerColumn].value2;
                    answers[i - ExcelConfiguration.BonusStartRow] = value.ToLower();
                }

                BonusQuestions bonus = new BonusQuestions(answers);
                return bonus;
            }
            catch (Exception e) { return null; }
            finally { CleanWorkbook(); }
        }

        public Dictionary<string, int> readtopscorers()
        {
            Dictionary<string, int> scorers = new Dictionary<string, int>();
            InitialiseWorkbook(GeneralConfiguration.AdminFileLocation, ExcelConfiguration.TopscorersSheet);
            try
            {
                int i = 2;
                while (true)
                {
                    string name = Convert.ToString(xlRange.Cells[i, 1].value2);
                    if (string.IsNullOrEmpty(name))
                        break;
                    var total = Convert.ToInt32(xlRange.Cells[i, 3].value2);
                    scorers.Add(name, total);
                    i++;
                }
                return scorers;
            }

            catch (Exception e)
            {
                return scorers;
            }
            finally { CleanWorkbook(); }
        }

        private Match[] ReadSinglePoule(int poule, int miss, bool host = false)
        {
            Match[] Poule = new Match[GeneralConfiguration.PouleSize];

            int startrow = ExcelConfiguration.StartRow + (GeneralConfiguration.PouleSize + 1) * (poule) + miss;

            try
            {
                for (int rowschecked = 0; rowschecked <GeneralConfiguration.PouleSize; rowschecked++)
                {
                    double a = 99;
                    double b = 99;
                    int currentRow = startrow + rowschecked;
                
                    var at = xlRange.Cells[currentRow, ExcelConfiguration.HomeColumn].Value2;
                    var bt = xlRange.Cells[currentRow, ExcelConfiguration.OutColumn].Value2;

                    if (at == null || bt == null)
                    {
                        if (!host)
                            return null;
                    }

                    else
                    {
                        a = at;
                        b = bt;
                    }

                    Match match = new Match(Convert.ToInt16(a), Convert.ToInt16(b), 0);
                    Poule[rowschecked] = match;
                }
                return Poule;
            }
            catch (Exception e) { return null; }
        }

        private void InitialiseWorkbook(string filename, int sheet)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            xlApp = new excel.Application();
            xlWorkbook = xlApp.Workbooks.Open(filename);
            xlWorksheet = xlWorkbook.Sheets[sheet];
            xlRange = xlWorksheet.UsedRange;
        }

        private void CleanWorkbook()
        {
            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
