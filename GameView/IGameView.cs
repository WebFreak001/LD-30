using SFML.Graphics;
using sfml_ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare.GameView
{
    public interface IGameView
    {
        void Update();

        void Render(RenderTarget target, UISceneManager ui);

        event EventHandler<IGameView> Next;
    }
}