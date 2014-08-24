using Newtonsoft.Json;
using System.IO;

namespace LudumDare.Json
{
    public class SceneLoader
    {
        public static GameScene Load(string file)
        {
            return JsonConvert.DeserializeObject<GameScene>(File.ReadAllText(file));
        }
    }
}