using Newtonsoft.Json;

namespace LudumDare.Json
{
    public class SceneLoader
    {
        private GameScene scene;

        public SceneLoader(string file)
        {
            scene = JsonConvert.DeserializeObject<GameScene>(file);
        }
    }
}