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
    public class KoPhaseSettings : ViewModelBase
    {
        public KOKeys PhaseKey;
        private int column;
        public int Column { get => column; set => this.RaiseAndSetIfChanged(ref column, value);}
        private int startrow;
        public int StartRow { get => startrow; set => this.RaiseAndSetIfChanged(ref startrow, value); }
        private int gapsize;
        public int GapSize { get => gapsize; set => this.RaiseAndSetIfChanged(ref gapsize, value); }
        private int size;
        public int Size { get => size; set => this.RaiseAndSetIfChanged(ref size, value); }
    }

    public class Configurables
    {
        public string SaveFileLocation = "";
        public string AdminFileLocation = "";
        public int NrPoules;
        public bool Bronze;
        public bool NlPresent;
        public bool Last32;
        public int StartRow;
        public int HomeColumn;
        public int OutColumn;
        public int HostGroupSheet;
        public int HostKOSheet;
        public int RankingSheet;
        public int TopscorersSheet;
        public int BonusAnswerColumn;
        public int BonusStartRow;
        public List<KoPhaseSettings> KoPhaseSettingsList;
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

        // general settings
        public string AdminFileLocation { get => configurables.AdminFileLocation; set { this.RaiseAndSetIfChanged(ref configurables.AdminFileLocation, value); SaveCommandEnabled = true; } }
        public string SaveFileFolder { get => Path.GetDirectoryName(configurables.SaveFileLocation); set { this.RaiseAndSetIfChanged(ref configurables.SaveFileLocation, Path.Combine(value, SaveFileName)); SaveCommandEnabled = true; } }
        public string SaveFileName { get => Path.GetFileName(configurables.SaveFileLocation); set { this.RaiseAndSetIfChanged(ref configurables.SaveFileLocation, Path.Combine(SaveFileFolder, value)); ; SaveCommandEnabled = true; } }
        public string NrPoules { get => configurables.NrPoules.ToString(); set { this.RaiseAndSetIfChanged(ref configurables.NrPoules, Convert.ToInt32(value)); SaveCommandEnabled = true; } }
        public bool Bronze { get => configurables.Bronze; set { this.RaiseAndSetIfChanged(ref configurables.Bronze, value); SaveCommandEnabled = true; } }
        public bool NlPresent { get => configurables.NlPresent; set { this.RaiseAndSetIfChanged(ref configurables.NlPresent, value); SaveCommandEnabled = true; } }
        public bool Last32 { get => configurables.Last32; set { this.RaiseAndSetIfChanged(ref configurables.Last32, value); SaveCommandEnabled = true; } }

        //excel settings
        public int StartRow { get => configurables.StartRow; set { this.RaiseAndSetIfChanged(ref configurables.StartRow, value); SaveCommandEnabled = true; } }
        public int HomeColumn { get => configurables.HomeColumn; set { this.RaiseAndSetIfChanged(ref configurables.HomeColumn, value); SaveCommandEnabled = true; } }
        public int OutColumn { get => configurables.OutColumn; set { this.RaiseAndSetIfChanged(ref configurables.OutColumn, value); SaveCommandEnabled = true; } }
        public int HostGroupSheet { get => configurables.HostGroupSheet; set { this.RaiseAndSetIfChanged(ref configurables.HostGroupSheet, value); SaveCommandEnabled = true; } }
        public int HostKOSheet { get => configurables.HostKOSheet; set { this.RaiseAndSetIfChanged(ref configurables.HostKOSheet, value); SaveCommandEnabled = true; } }
        public int RankingSheet { get => configurables.RankingSheet; set { this.RaiseAndSetIfChanged(ref configurables.RankingSheet, value); SaveCommandEnabled = true; } }
        public int TopscorersSheet { get => configurables.TopscorersSheet; set { this.RaiseAndSetIfChanged (ref configurables.TopscorersSheet, value); SaveCommandEnabled = true; } }
        public int BonusAnswerColumn { get => configurables.BonusAnswerColumn; set { this.RaiseAndSetIfChanged(ref configurables.BonusAnswerColumn, value); SaveCommandEnabled = true; } }
        public int BonusStartRow { get => configurables.BonusStartRow; set { this.RaiseAndSetIfChanged (ref configurables.BonusStartRow, value); SaveCommandEnabled = true; } }

        private Dictionary<KOKeys, KoPhaseSettings> kosettings;
        public Dictionary<KOKeys, KoPhaseSettings> KoSettings { get => kosettings; set { this.RaiseAndSetIfChanged(ref kosettings, value); SaveCommandEnabled = true; } }

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
                Last32 = false,
                Bronze = false,
                NlPresent = true,
                StartRow = 12,
                HomeColumn = 7,
                OutColumn = 8,
                HostGroupSheet = 6,
                HostKOSheet = 7,
                RankingSheet = 2,
                TopscorersSheet = 8,
                BonusStartRow = 365,
                BonusAnswerColumn = 7,
                KoPhaseSettingsList = new List<KoPhaseSettings>() { 
                    new KoPhaseSettings(){PhaseKey = KOKeys.LAST32, Column = 4, StartRow = 83, GapSize = 2, Size = 16 },
                    new KoPhaseSettings(){PhaseKey = KOKeys.LAST16, Column = 4, StartRow = 83, GapSize = 2, Size = 16 },
                    new KoPhaseSettings(){PhaseKey = KOKeys.QUARTER, Column = 6, StartRow = 84, GapSize = 4, Size = 8 },
                    new KoPhaseSettings(){PhaseKey = KOKeys.SEMI, Column = 8, StartRow = 86, GapSize = 8, Size = 4 },
                    new KoPhaseSettings(){PhaseKey = KOKeys.FINAL, Column = 11, StartRow = 89, GapSize = 16, Size = 2 }
                }
            };
            configurables = new();
            ReadConfigFromXml();
            SettingsEvent?.Invoke();

            this.WhenAnyValue(x => x.KoSettings[KOKeys.LAST32].Column, x => x.KoSettings[KOKeys.LAST32].GapSize, x => x.KoSettings[KOKeys.LAST32].Size, x => x.KoSettings[KOKeys.LAST32].StartRow).Subscribe(a => SaveCommandEnabled = true);
            this.WhenAnyValue(x => x.KoSettings[KOKeys.LAST16].Column, x => x.KoSettings[KOKeys.LAST16].GapSize, x => x.KoSettings[KOKeys.LAST16].Size, x => x.KoSettings[KOKeys.LAST16].StartRow).Subscribe(a => SaveCommandEnabled = true);
            this.WhenAnyValue(x => x.KoSettings[KOKeys.QUARTER].Column, x => x.KoSettings[KOKeys.QUARTER].GapSize, x => x.KoSettings[KOKeys.QUARTER].Size, x => x.KoSettings[KOKeys.QUARTER].StartRow).Subscribe(a => SaveCommandEnabled = true);
            this.WhenAnyValue(x => x.KoSettings[KOKeys.SEMI].Column, x => x.KoSettings[KOKeys.SEMI].GapSize, x => x.KoSettings[KOKeys.SEMI].Size, x => x.KoSettings[KOKeys.SEMI].StartRow).Subscribe(a => SaveCommandEnabled = true);
            this.WhenAnyValue(x => x.KoSettings[KOKeys.FINAL].Column, x => x.KoSettings[KOKeys.FINAL].GapSize, x => x.KoSettings[KOKeys.FINAL].Size, x => x.KoSettings[KOKeys.FINAL].StartRow).Subscribe(a => SaveCommandEnabled = true);
            SaveCommandEnabled = false;
        }

        public void SaveSettingsCommand()
        {
            configurables.KoPhaseSettingsList = KoSettings.Values.ToList();
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
            }
            string input = File.ReadAllText(configeFileName);
            configurables = JsonSerializer.Deserialize<Configurables>(input,jsonSerializerOptions);

            var kosettings = new Dictionary<KOKeys, KoPhaseSettings>();
            foreach (var kosetting in configurables.KoPhaseSettingsList)
                kosettings.Add(kosetting.PhaseKey, kosetting);
            KoSettings = kosettings;
            ConfigurablesToConfigurations();
        }

        private void ConfigurablesToConfigurations()
        {
            GeneralConfiguration.AdminFileLocation = configurables.AdminFileLocation;
            GeneralConfiguration.SaveFileLocation = configurables.SaveFileLocation;
            GeneralConfiguration.NrPoules = configurables.NrPoules;
            GeneralConfiguration.Last32 = configurables.Last32;
            GeneralConfiguration.Bronze = configurables.Bronze;
            GeneralConfiguration.NlPresent = configurables.NlPresent;

            ExcelConfiguration.HomeColumn = configurables.HomeColumn;
            ExcelConfiguration.HostGroupSheet = configurables.HostGroupSheet;
            ExcelConfiguration.HostKOSheet = configurables.HostKOSheet;
            ExcelConfiguration.OutColumn = configurables.OutColumn;
            ExcelConfiguration.RankingSheet = configurables.RankingSheet;
            ExcelConfiguration.StartRow = configurables.StartRow;
            ExcelConfiguration.TopscorersSheet = configurables.TopscorersSheet;
            ExcelConfiguration.BonusStartRow = configurables.BonusStartRow;
            ExcelConfiguration.BonusAnswerColumn = configurables.BonusAnswerColumn;
            ExcelConfiguration.KoSettings = configurables.KoPhaseSettingsList;
        }
    }
}
