using EindToernooi_Poule.Excel;
using VoetbalPoolsBase;
using VoetbalPoolsBase.Interfaces;
using System.Collections.Generic;

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
                SetTopscorers();
            return Topscorers;
        }

        public void SetTopscorers()
        {
            Topscorers = new ExcelManager().readtopscorers();
        }

        public void setHost()
        {
            if (!HostSet)
            {
                Topscorers = new Dictionary<string, Topscorer>();
                Poules = excelManager.ReadGroupPhase(GeneralConfiguration.AdminFileLocation, ExcelLocalConfiguration.HostGroupSheet, 0, host: true);
                KnockoutPhase = excelManager.readKnockout(GeneralConfiguration.AdminFileLocation, ExcelLocalConfiguration.HostKOSheet, 0, true);
                Questions = new(excelManager.ReadBonus(GeneralConfiguration.AdminFileLocation, ExcelLocalConfiguration.HostGroupSheet, true));
                SetTopscorers();
                HostSet = true;
            }
        }
    }
}
