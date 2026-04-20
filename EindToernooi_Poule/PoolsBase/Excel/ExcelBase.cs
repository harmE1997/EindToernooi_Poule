using System.Runtime.InteropServices;
using excel = Microsoft.Office.Interop.Excel;

namespace PoolsBase.Excel
{
    public class ExcelBase
    {
        public excel.Application xlApp { get; private set; }
        public excel.Workbook xlWorkbook { get; private set; }
        public excel._Worksheet xlWorksheet { get; private set; }
        public excel.Range xlRange { get; private set; }

        public void InitialiseWorkbook(string filename, int sheet)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException();

            xlApp = new excel.Application();
            xlWorkbook = xlApp.Workbooks.Open(filename);
            xlWorksheet = xlWorkbook.Sheets[sheet];
            xlRange = xlWorksheet.UsedRange;
        }

        public void CleanWorkbook()
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

        public Dictionary<string, Topscorer> readtopscorers(bool readScoresPerRound = false)
        {
            Dictionary<string, Topscorer> scorers = new Dictionary<string, Topscorer>();
            InitialiseWorkbook(GeneralConfiguration.AdminFileLocation, ExcelBaseConfiguration.TopscorersSheet);
            try
            {
                int i = 2;
                while (true)
                {
                    Topscorer ts = new Topscorer() { Total = 0, Rounds = new List<int>() };
                    string name = Convert.ToString(xlRange.Cells[i, 1].value2).ToLower();
                    if (string.IsNullOrEmpty(name))
                        break;
                    ts.Total = Convert.ToInt32(xlRange.Cells[i, 3].value2);

                    if (readScoresPerRound)
                    {
                        for (int x = 0; x < 34; x++)
                        {
                            var round = Convert.ToInt32(xlRange.Cells[i, x + 4].value2);
                            ts.Rounds.Add(round);
                        }
                    }

                    scorers.Add(name, ts);
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

        public Dictionary<string, int> ReadBonus(string filename, int sheet, bool host = false)
        {
            InitialiseWorkbook(filename, sheet);
            try
            {
                var answers = new Dictionary<string, int>();
                for (int i = ExcelBaseConfiguration.BonusStartRow; i < (ExcelBaseConfiguration.BonusStartRow + ExcelBaseConfiguration.NrBonusAnswers); i++)
                {
                    int round = 1;
                    if (host)
                        round = Convert.ToInt32(xlRange.Cells[i, ExcelBaseConfiguration.BonusRoundsColumn].value2);

                    string value = xlRange.Cells[i, ExcelBaseConfiguration.BonusAnswerColumn].value2;
                    if (string.IsNullOrEmpty(value))
                        answers.Add(value, round);

                    else
                        answers.Add(value.ToLower(), round);
                }

                return answers;
            }
            catch (Exception e) { return null; }
            finally { CleanWorkbook(); }
        }

        public Match[] ReadBlock(int blocknr, int blocksize, int miss, bool host = false)
        {
            Match[] Poule = new Match[blocksize];

            int startrow = ExcelBaseConfiguration.StartRow + (blocksize + 1) * (blocknr) + miss;

            try
            {
                for (int rowschecked = 0; rowschecked < blocksize; rowschecked++)
                {
                    double a = 99;
                    double b = 99;
                    int currentRow = startrow + rowschecked;

                    var at = xlRange.Cells[currentRow, ExcelBaseConfiguration.HomeColumn].Value2;
                    var bt = xlRange.Cells[currentRow, ExcelBaseConfiguration.OutColumn].Value2;

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
    }
}
