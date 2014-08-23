﻿using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LudumDare.ParticleSystem
{
    public struct Particle
    {
        public Vector2f Position;
        public float Rotation;
        public Vector2f Scaling;
        public float Time;
        public float RotationVelocity;
        public Vector2f Velocity;
        public Color Color;
    }
}