using System;

namespace LudumDare
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (var game = new LevelEditor())
                game.Run();
        }
    }
}