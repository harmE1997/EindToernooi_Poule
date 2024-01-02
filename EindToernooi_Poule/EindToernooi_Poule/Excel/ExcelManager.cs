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

        public void ExportPlayersToExcel(List<Player> Players, int weeknr)
        {
            InitialiseWorkbook(GeneralConfiguration.AdminFileLocation, ExcelConfiguration.RankingSheet);
            int y = 3;
            foreach (Player player in Players)
            {
                var playerweek = player.Poules[weeknr];
                xlRange.Cells[y, 1].value2 = player.Ranking;
                xlRange.Cells[y, 2].value2 = player.PreviousRanking;
                xlRange.Cells[y, 3].value2 = player.RankingDifference;
                xlRange.Cells[y, 4].value2 = player.Name;
                xlRange.Cells[y, 5].value2 = player.Town;
                xlRange.Cells[y, 6].value2 = player.TotalScore;
                xlRange.Cells[y, 7].value2 = playerweek.PouleMatchesScore;
                xlRange.Cells[y, 8].value2 = player.BonusScore;
                y++;
            }
            CleanWorkbook();
        }

        public Dictionary<int, Poule> ReadPredictions(string filename, int sheet, int miss, Dictionary<int, Poule> Weeks = null)
        {
            var weeks = new Dictionary<int, Poule>();
            if (Weeks != null)
                weeks = Weeks;

            var StartWeek = 0;
            var Endweek = ExcelConfiguration.NrBlocks;
            
            try
            {
                if (!File.Exists(filename))
                {
                    PopupManager.ShowMessage("Cannot read host. Admin cannot be found");
                    return weeks;
                }

                InitialiseWorkbook(filename, sheet);
                for (int i = StartWeek; i < Endweek; i++)
                {
                    var matches = ReadSingleWeek(i, miss);
                    if (weeks.ContainsKey(i + 1))
                        weeks[i + 1] = new Poule(i + 1, matches);
                    else
                    weeks.Add(i + 1, new Poule((i + 1), matches));
                }
                CleanWorkbook();
                return weeks;
            }

            catch (Exception e) { CleanWorkbook(); return weeks; }
        }

        public BonusQuestions ReadBonus()
        {
            InitialiseWorkbook(GeneralConfiguration.AdminFileLocation, ExcelConfiguration.HostSheet);
            try
            {
                int[] weeks = new int[13];
                string[] answers = new string[13];
                for (int i = ExcelConfiguration.BonusStartRow; i < (ExcelConfiguration.BonusStartRow + weeks.Length); i++)
                {
                    weeks[i - ExcelConfiguration.BonusStartRow] = Convert.ToInt32(xlRange.Cells[i, ExcelConfiguration.BonusWeeksColumn].value2);
                    answers[i - ExcelConfiguration.BonusStartRow] = xlRange.Cells[i, ExcelConfiguration.BonusAnswerColumn].value2;
                }

                BonusQuestions bonus = new BonusQuestions(answers, weeks);
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

            catch
            {
                return scorers;
            }
            finally { CleanWorkbook(); }
        }

        private Match[] ReadSingleWeek(int week, int miss)
        {
            Match[] Week = new Match[9];

            int startrow = ExcelConfiguration.StartRow + (ExcelConfiguration.BlockSize + 1) * (week) + miss;
            if (week >= ExcelConfiguration.FirstHalfSize)
                startrow += ExcelConfiguration.HalfWayJump;

            try
            {
                for (int rowschecked = 0; rowschecked < ExcelConfiguration.BlockSize; rowschecked++)
                {
                    double a = 99;
                    double b = 99;
                    double p = 0; 
                    int currentRow = startrow + rowschecked;
                
                    var pt = xlRange.Cells[currentRow, ExcelConfiguration.PostponementColumn].Value2;
                    var at = xlRange.Cells[currentRow, ExcelConfiguration.HomeColumn].Value2;
                    var bt = xlRange.Cells[currentRow, ExcelConfiguration.OutColumn].Value2;
                    

                    if (at != null && bt != null)
                    {
                        a = at;
                        b = bt;
                    }

                    if (pt != null)
                        p = pt;

                    Match match = new Match(Convert.ToInt16(a), Convert.ToInt16(b), Convert.ToInt16(p));
                    Week[rowschecked] = match;
                }
                return Week;
            }
            catch (Exception e) { return Week; }
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
