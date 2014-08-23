using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudumDare
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