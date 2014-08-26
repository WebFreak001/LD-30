using System;
using System.Collections.Generic;

namespace LudumDare.Json
{
    public class GameScene
    {
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public Point StartCoords { get; set; }

        public Point FinishLow { get; set; }

        public Point FinishHigh { get; set; }

        public List<SceneObject> Objects { get; set; }
    }
}