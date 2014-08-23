using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var test = new LevelEditor())
                test.Run();
        }
    }
}