using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Platformer
{
    public static class LoadSaveManager
    {
        public static bool IsSaving = false;

        public static bool SaveGame(Hero gameHero)
        {
            IsSaving = false;
            try
            {
                string heroJson = JsonConvert.SerializeObject(gameHero, Formatting.Indented);
                string enemyJson = JsonConvert.SerializeObject(EnemyManager.Instance.Enemies, Formatting.Indented);

                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "SavedGames", "Platformer"));
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Saved Games", "Platformer", "hero.txt"), heroJson);
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Saved Games", "Platformer", "enemies.txt"), enemyJson);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool LoadGame(ref Hero gameHero)
        {
            try
            {
                string heroJson = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Saved Games", "Platformer", "hero.txt"));
                string enemyJson = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Saved Games", "Platformer", "enemies.txt"));

                gameHero = JsonConvert.DeserializeObject<Hero>(heroJson);
                EnemyManager.Instance.Enemies = JsonConvert.DeserializeObject<List<Enemy>>(enemyJson);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
