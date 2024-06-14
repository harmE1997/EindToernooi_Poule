using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;

namespace EindToernooi_Poule.Code
{
    [Serializable]
    public class PlayerManager
    {
        public List<Player> Players { get; private set; }

        public PlayerManager()
        {
            Players = new List<Player>();
        }

        public int AddPlayer(Player player, bool AllowOverwrite)
        {
            if (player == null)
                return 1;
            
            if (FindPlayer(player.Name) != null && !AllowOverwrite)
                return 2;

            else if (FindPlayer(player.Name) != null && AllowOverwrite)
                RemovePlayer(player.Name);

            Players.Add(player);
            SavePlayers();
            return 0;          
        }

        private void SavePlayers()
        {
            if (!string.IsNullOrEmpty(GeneralConfiguration.SaveFileLocation))
            {
                string output = JsonSerializer.Serialize(Players, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(GeneralConfiguration.SaveFileLocation, output);
            }
        }

        public void LoadPlayers()
        {
            if (string.IsNullOrEmpty(GeneralConfiguration.SaveFileLocation) || !File.Exists(GeneralConfiguration.SaveFileLocation))
            {
                SavePlayers();
                return;
            }

            string input = File.ReadAllText(GeneralConfiguration.SaveFileLocation);
            Players = JsonSerializer.Deserialize<List<Player>>(input, new JsonSerializerOptions { WriteIndented = true });
        }

        public void RankPlayers()
        {
            Players = Players.OrderBy(p => p.TotalScore).ToList();
            Players.Reverse();
            int ranking = 1;
            int counter = 1;
            int lastplayerscore = -1;
            foreach (Player player in Players)
            {
                if (player.TotalScore != lastplayerscore)
                {
                    ranking = counter;
                }
                player.Ranking = ranking;
                lastplayerscore = player.TotalScore;
                counter++;
            }
        }

        public Player FindPlayer(string name)
        {
            foreach (Player player in Players)
            {
                if (player.Name == name)
                {
                    return player;
                }
            }

            return null;
        }

        public int RemovePlayer(string name)
        {
            Player exitplayer = FindPlayer(name);
            if (exitplayer != null)
            {
                Players.Remove(exitplayer);
                SavePlayers();
                return 0;
            }

            else
            {
                return 1;
            }

        }

        public void CheckAllPlayers(Host host)
        {
            foreach (Player player in Players)
            {
                player.CheckPlayer(host, host.getTopscorers());
            }

            SavePlayers();
        }
    }
}
