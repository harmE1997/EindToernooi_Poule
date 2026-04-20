using EindToernooi_Poule.Excel;
using System.Collections.Generic;
using VoetbalPoolsBase;
using VoetbalPoolsBase.Interfaces;

namespace EindToernooi_Poule.Code
{
    public class Host : Player, IHost
    {
        private Dictionary<string, Topscorer> Topscorers;
        public ExcelManager excelManager;
        public bool HostSet = false;

        public Host() : base("", "", null, null, null)
        {
            excelManager = new ExcelManager();
        }

        public Dictionary<string, Topscorer> GetTopscorers()
        {
            if (Topscorers.Count == 0)
                setTopscorers();
            return Topscorers;
        }

        public void setTopscorers()
        {
            Topscorers = new ExcelManager().readtopscorers();
        }

        public void setHost()
        {
            if (!HostSet)
            {
                Topscorers = new Dictionary<string, Topscorer>();
                Poules = excelManager.ReadGroupPhase(GeneralConfiguration.AdminFileLocation, ExcelLocalConfiguration.HostSheet, 0, host: true);
                KnockoutPhase = excelManager.readKnockout(GeneralConfiguration.AdminFileLocation, ExcelLocalConfiguration.HostSheet, true);
                Questions = new(excelManager.ReadBonus(GeneralConfiguration.AdminFileLocation, ExcelLocalConfiguration.HostSheet));
                setTopscorers();
                HostSet = true;
            }
        }
    }
}
