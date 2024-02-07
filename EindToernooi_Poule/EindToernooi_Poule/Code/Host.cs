using EindToernooi_Poule.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EindToernooi_Poule.Code
{
    public class Host : Player
    {
        private Dictionary<string, int> Topscorers;
        public ExcelManager excelManager;
        public bool HostSet = false;

        public Host() : base("", "", null, null, null)
        { 
            excelManager = new ExcelManager();
        }

        public Dictionary<string, int> getTopscorers()
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
                Topscorers = new Dictionary<string, int>();
                Poules = excelManager.ReadPredictions(GeneralConfiguration.AdminFileLocation, ExcelConfiguration.HostSheet, 0);
                Questions = excelManager.ReadBonus();
                setTopscorers();
                HostSet = true;
            }
        }
    }
}
