using SFML.Graphics;
using sfml_ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudumDare.GameView.Views
{
    public class InGameView : IGameView
    {
        private Scene scene;

        public InGameView()
        {
            scene = new Scene(ScrollInputs.None);
        }

        public void Update(TimeSpan delta)
        {
        }

        public void Render(TimeSpan delta, RenderTarget target, UISceneManager ui)
        {
            ui.CurrentScene = scene;
        }

        public event EventHandler<IGameView> Next;
    }
}