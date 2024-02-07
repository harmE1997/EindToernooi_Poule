using EindToernooi_Poule.Code;
using EindToernooi_Poule.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EindToernooi_Poule.Excel
{
    public static class ExcelConfiguration
    {
        public static int StartRow;
        public static int HomeColumn;
        public static int OutColumn;
        public static int HostSheet;
        public static int RankingSheet;
        public static int TopscorersSheet;
        public static int BonusStartRow;
        public static int BonusAnswerColumn;
        public static List<KoPhaseSettings> KoSettings;
    }
}
