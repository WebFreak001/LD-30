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
        void Update(TimeSpan delta);

        void Render(TimeSpan delta, RenderTarget target, UISceneManager ui);

        event EventHandler<IGameView> Next;
    }
}