using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace LudumDare
{
    public class Point
    {
        [DefaultValue(0)]
        public float X { get; set; }

        [DefaultValue(0)]
        public float Y { get; set; }

        public Point()
            : this(0, 0)
        {
        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }
    }
}