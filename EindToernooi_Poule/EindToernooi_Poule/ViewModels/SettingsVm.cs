using EindToernooi_Poule.Code;
using EindToernooi_Poule.Excel;
using ReactiveUI;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace EindToernooi_Poule.ViewModels
{
    public class Configurables
    {
        public string SaveFileLocation = "";
        public string AdminFileLocation = "";
        public int NrPoules;
        public bool Bronze;
        public bool Last32;
        public int StartRow;
        public int HomeColumn;
        public int OutColumn;
        public int PostponementColumn;
        public int HostSheet;
        public int RankingSheet;
        public int TopscorersSheet;
        public int BonusAnswerColumn;
        public int BonusStartRow;
    }
    public class SettingsVm : ViewModelBase
    {
        public static event SettingsEventHandler? SettingsEvent;
        public delegate void SettingsEventHandler();

        private const string configeFileName = "Eindtoernooi_settings.json";
        private Configurables defaults;
        private Configurables configurables;
        private JsonSerializerOptions jsonSerializerOptions;

        public List<string> PoulesOptions { get; set; }

        public string AdminFileLocation { get => configurables.AdminFileLocation; set { this.RaiseAndSetIfChanged(ref configurables.AdminFileLocation, value); SaveCommandEnabled = true; } }
        public string SaveFileFolder { get => Path.GetDirectoryName(configurables.SaveFileLocation); set { this.RaiseAndSetIfChanged(ref configurables.SaveFileLocation, Path.Combine(value, SaveFileName)); SaveCommandEnabled = true; } }
        public string SaveFileName { get => Path.GetFileName(configurables.SaveFileLocation); set { this.RaiseAndSetIfChanged(ref configurables.SaveFileLocation, Path.Combine(SaveFileFolder, value)); ; SaveCommandEnabled = true; } }
        public string NrPoules { get => configurables.NrPoules.ToString(); set { this.RaiseAndSetIfChanged(ref configurables.NrPoules, Convert.ToInt32(value)); SaveCommandEnabled = true; } }
        public bool Bronze { get => configurables.Bronze; set { this.RaiseAndSetIfChanged(ref configurables.Bronze, value); SaveCommandEnabled = true; } }
        public bool Last32 { get => configurables.Bronze; set { this.RaiseAndSetIfChanged(ref configurables.Bronze, value); SaveCommandEnabled = true; } }

        public int StartRow { get => configurables.StartRow; set { this.RaiseAndSetIfChanged(ref configurables.StartRow, value); SaveCommandEnabled = true; } }
        public int HomeColumn { get => configurables.HomeColumn; set { this.RaiseAndSetIfChanged(ref configurables.HomeColumn, value); SaveCommandEnabled = true; } }
        public int OutColumn { get => configurables.OutColumn; set { this.RaiseAndSetIfChanged(ref configurables.OutColumn, value); SaveCommandEnabled = true; } }
        public int PostponementColumn { get => configurables.PostponementColumn; set { this.RaiseAndSetIfChanged(ref configurables.PostponementColumn, value); SaveCommandEnabled = true; } }
        public int HostSheet { get => configurables.HostSheet; set { this.RaiseAndSetIfChanged(ref configurables.HostSheet, value); SaveCommandEnabled = true; } }
        public int RankingSheet { get => configurables.RankingSheet; set { this.RaiseAndSetIfChanged(ref configurables.RankingSheet, value); SaveCommandEnabled = true; } }
        public int TopscorersSheet { get => configurables.TopscorersSheet; set { this.RaiseAndSetIfChanged (ref configurables.TopscorersSheet, value); SaveCommandEnabled = true; } }
        public int BonusAnswerColumn { get => configurables.BonusAnswerColumn; set { this.RaiseAndSetIfChanged(ref configurables.BonusAnswerColumn, value); SaveCommandEnabled = true; } }
        public int BonusStartRow { get => configurables.BonusStartRow; set { this.RaiseAndSetIfChanged (ref configurables.BonusStartRow, value); SaveCommandEnabled = true; } }

        private bool savecommandenabled;
        public bool SaveCommandEnabled { get => savecommandenabled; set => this.RaiseAndSetIfChanged(ref savecommandenabled, value); }

        public SettingsVm()
        {
            var savefilelocation = "Eindtoernooi24.json";
            var adminloc = "Eindtoernooi 2024.xlsx";
            jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, IncludeFields = true, };
            PoulesOptions = new List<string> { "6", "8", "12" };
            defaults = new Configurables()
            {
                SaveFileLocation = savefilelocation,
                AdminFileLocation = adminloc,
                NrPoules = 6,
                Last32 = true,
                Bronze = false,
                StartRow = 12,         
                HomeColumn = 7,
                OutColumn = 8,
                PostponementColumn = 6,
                HostSheet = 6,
                RankingSheet = 2,
                TopscorersSheet = 8,
                BonusStartRow = 365,
                BonusAnswerColumn = 7
            };
            configurables = new();
            ReadConfigFromXml();
            SaveCommandEnabled = false;
            SettingsEvent?.Invoke();
        }

        public void SaveSettingsCommand()
        {
            WriteConfigToXml();
            SaveCommandEnabled = false;
        }

        private void WriteConfigToXml()
        {
            string output = JsonSerializer.Serialize(configurables, jsonSerializerOptions);
            File.WriteAllText(configeFileName, output);
            ConfigurablesToConfigurations();
            SettingsEvent?.Invoke();
        }

        private void ReadConfigFromXml()
        {
            if (!File.Exists(configeFileName))
            {
                configurables = defaults;
                WriteConfigToXml();
                return;
            }
            string input = File.ReadAllText(configeFileName);
            configurables = JsonSerializer.Deserialize<Configurables>(input,jsonSerializerOptions);
            ConfigurablesToConfigurations();
        }

        private void ConfigurablesToConfigurations()
        {
            GeneralConfiguration.AdminFileLocation = configurables.AdminFileLocation;
            GeneralConfiguration.SaveFileLocation = configurables.SaveFileLocation;
            GeneralConfiguration.NrPoules = configurables.NrPoules;
            GeneralConfiguration.Last32 = configurables.Last32;
            GeneralConfiguration.Bronze = configurables.Bronze;

            ExcelConfiguration.HomeColumn = configurables.HomeColumn;
            ExcelConfiguration.HostSheet = configurables.HostSheet;
            ExcelConfiguration.OutColumn = configurables.OutColumn;
            ExcelConfiguration.PostponementColumn = configurables.PostponementColumn;
            ExcelConfiguration.RankingSheet = configurables.RankingSheet;
            ExcelConfiguration.StartRow = configurables.StartRow;
            ExcelConfiguration.TopscorersSheet = configurables.TopscorersSheet;
            ExcelConfiguration.BonusStartRow = configurables.BonusStartRow;
            ExcelConfiguration.BonusAnswerColumn = configurables.BonusAnswerColumn;          
        }
    }
}
