﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LudumDare
{
    public class PhysicsShape
    {
        [DefaultValue("Circle")]
        public string Mesh { get; set; }

        [DefaultValue(0)]
        public float Radius { get; set; }

        public Point Dimension { get; set; }

        [DefaultValue("static")]
        public string Type { get; set; }

        public Point Position { get; set; }

        [DefaultValue(false)]
        public bool FixedRotation { get; set; }

        [DefaultValue(0)]
        public float Friction { get; set; }

        [DefaultValue(false)]
        public bool Bullet { get; set; }

        [DefaultValue(10)]
        public float Mass { get; set; }

        [DefaultValue(0)]
        public float Restitution { get; set; }

        [DefaultValue(0)]
        public float Rotation { get; set; }

        [DefaultValue("")]
        public string UserData { get; set; }

        [DefaultValue(false)]
        public bool IgnoreGravity { get; set; }
    }
}