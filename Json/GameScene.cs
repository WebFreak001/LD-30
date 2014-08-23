using System;
using System.Collections.Generic;

namespace LudumDare.Json
{
    public class GameScene
    {
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public List<SceneObject> Objects { get; set; }
    }
}