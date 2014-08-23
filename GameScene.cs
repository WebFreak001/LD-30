using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudumDare
{
    public class GameScene
    {
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public List<SceneObject> Objects { get; set; }
    }
}