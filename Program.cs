﻿namespace LudumDare
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (var game = new LevelEditor())
                game.Run();
        }
    }
}