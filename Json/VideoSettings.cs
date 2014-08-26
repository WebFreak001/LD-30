using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare.Json
{
    public class GameSettings
    {
        public bool EnableClouds { get; set; }

        [DefaultValue(50.0f)]
        public float Volume { get; set; }
    }
}