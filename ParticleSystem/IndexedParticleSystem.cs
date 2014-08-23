using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace LudumDare.ParticleSystem
{
    public class IndexedParticleSystem
    {
        public Particle[] Particles = new Particle[4096];
        public List<Emitter> Emitters = new List<Emitter>();
        public Vector2f Gravity = new Vector2f(0, 9.7f);
        public Random random;
        public Color color;

        public IndexedParticleSystem()
        {
            random = new Random();
        }

        public void AddEmitter(Emitter emitter)
        {
            Emitters.Add(emitter);
        }

        public void Render(RenderTarget target, Sprite particle)
        {
            for (int i = Particles.Length - 1; i >= 0; i--)
            {
                if (Particles[i].Time <= 0) continue;
                particle.Position = Particles[i].Position;
                particle.Rotation = Particles[i].Rotation;
                particle.Color = new Color(Particles[i].Color.R, Particles[i].Color.G, Particles[i].Color.B, (byte)(255 * (Particles[i].Time)));
                target.Draw(particle);
            }
        }

        public void Update(TimeSpan delta)
        {
            for (int i = Particles.Length - 1; i >= 0; i--)
            {
                if (Particles[i].Time <= 0) continue;
                Particles[i].Time -= (float)(delta.TotalSeconds * 0.1);
                Particles[i].Velocity += Gravity;
                Particles[i].Position += Particles[i].Velocity * (float)delta.TotalSeconds;
                Particles[i].Rotation += Particles[i].RotationVelocity;
            }

            foreach (Emitter e in Emitters)
            {
                for (int i = (int)Math.Round(delta.TotalSeconds * (float)e.ParticlesSpawnRate - 1); i >= 0; i--)
                    AddParticle(e.Color, e.Position, new Vector2f((float)((random.NextDouble() * 2 - 1) * e.Spread), (float)((random.NextDouble() * 2 - 1) * e.Spread)));
            }
        }

        public void ReplaceParticleAtIndex(Color color, Vector2f position, Vector2f spread, int i)
        {
            Particles[i] = new Particle();
            Particles[i].Time = 1;
            Particles[i].Position = position + spread;
            Particles[i].Velocity = spread * 11;
            Particles[i].Rotation = (float)(random.NextDouble() * 360);
            Particles[i].Color = color;
        }

        public void AddParticle(Color color, Vector2f position, Vector2f spread)
        {
            for (int i = 0; i < Particles.Length; i++)
            {
                if (Particles[i].Time <= 0)
                {
                    ReplaceParticleAtIndex(color, position, spread, i);
                    return;
                }
            }
        }
    }
}