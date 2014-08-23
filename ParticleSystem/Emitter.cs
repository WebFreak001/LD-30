using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudumDare.ParticleSystem
{
    public class Emitter
    {
        public Vector2f Position;
        public int ParticlesSpawnRate;
        public float Spread;
        public Color Color;
    }
}