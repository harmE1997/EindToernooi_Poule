using PoolsBase.Interfaces;
using System.Text.Json;

namespace PoolsBase
{
    [Serializable]
    public class PlayerManager<T, U> where T : PlayerBase<U>
    {
        private JsonSerializerOptions jsonSerializerOptions;
        public List<T> Players { get; private set; }

        public PlayerManager()
        {
            jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
            Players = new List<T>();
        }

        public int AddPlayer(T player, bool AllowOverwrite)
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
                try
                {
                    string output = JsonSerializer.Serialize(Players, jsonSerializerOptions);
                    File.WriteAllText(GeneralConfiguration.SaveFileLocation, output);
                }

                catch { return; }
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
            Players = JsonSerializer.Deserialize<List<T>>(input, jsonSerializerOptions);
        }

        public T FindPlayer(string name)
        {
            foreach (T player in Players)
            {
                if (player.Name == name)
                {
                    return player;
                }
            }

            return default(T);
        }

        public int RemovePlayer(string name)
        {
            T exitplayer = FindPlayer(name);
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

        public void CheckAllPlayers(IHost host)
        {
            foreach (T player in Players)
            {
                player.CheckPlayer(host, host.GetTopscorers());
            }

            SavePlayers();
        }
    }
}
