using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using LudumDare.Json;
using Microsoft.Xna.Framework;
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace LudumDare.Physics
{
    public class PhysicsWorld
    {
        public World world;
        public List<BodyEx> Bodies;

        private CircleShape unitCircle;
        private RectangleShape unitRect;

        public event EventHandler<Body> OnBodyAdded;

        public event EventHandler OnClear;

        private SceneRenderer renderer;

        public Body CamLock { get; set; }

        private bool dim0;
        private bool ignoreDim;

        public PhysicsWorld(World world, bool noDim = false)
        {
            this.world = world;
            renderer = new SceneRenderer(world);
            Bodies = new List<BodyEx>();
            ignoreDim = noDim;

            unitCircle = new CircleShape(5.0f, 32);
            unitRect = new RectangleShape(new Vector2f(10, 10));
            unitRect.Origin = new Vector2f(5, 5);
            unitCircle.Origin = new Vector2f(5, 5);
            unitCircle.FillColor = Color.White;
            unitRect.FillColor = Color.White;
        }

        public PhysicsWorld(bool noDim = false)
            : this(new World(new Vector2(0, 16.0f)), noDim)
        {
        }

        public void Add(Body body)
        {
            if (OnBodyAdded != null)
                OnBodyAdded(this, body);
            Bodies.Add(new BodyEx(body));
            ChangeDimension(dim0);
        }

        public void Add(BodyEx body)
        {
            if (OnBodyAdded != null)
                OnBodyAdded(this, body.Body);
            Bodies.Add(body);
            ChangeDimension(dim0);
        }

        public void Remove(Body body)
        {
            world.RemoveBody(body);
            Bodies.Remove(FindBody(body));
        }

        public void Step(float elapsed)
        {
            world.Step(elapsed);
        }

        public void Render(RenderTarget target)
        {
            //renderer.Render(target);
            foreach (BodyEx body in Bodies)
            {
                byte b = body.GameDimension == Dimension.None ? (byte)255 : (byte)0;
                byte g = body.GameDimension == Dimension.OneO ? (byte)255 : (byte)0;
                byte r = body.GameDimension == Dimension.TwoX ? (byte)255 : (byte)0;
                if (body.Shape == DisplayShape.Circle)
                {
                    unitCircle.Scale = new Vector2f(body.Length, body.Length) * 2;
                    unitCircle.Position = new Vector2f(body.Body.Position.X * 10, body.Body.Position.Y * 10);
                    unitCircle.Rotation = MathHelper.ToDegrees(body.Body.Rotation);
                    unitCircle.FillColor = new Color(r, g, b, 255);
                    if (body.Tex != null)
                    {
                        unitCircle.FillColor = new Color(255, 255, 255, 255);
                        unitCircle.Texture = body.Tex;
                    }
                    else
                    {
                        unitCircle.FillColor = new Color(r, g, b, 255);
                        unitCircle.Texture = null;
                    }
                    target.Draw(unitCircle);
                }
                else if (body.Shape == DisplayShape.Box)
                {
                    unitRect.Scale = new Vector2f(body.Dimension.X, body.Dimension.Y);
                    unitRect.Position = new Vector2f(body.Body.Position.X * 10, body.Body.Position.Y * 10);
                    unitRect.Rotation = MathHelper.ToDegrees(body.Body.Rotation);
                    unitRect.FillColor = new Color(r, g, b, 255);
                    if (body.Tex != null)
                    {
                        unitRect.FillColor = new Color(255, 255, 255, 255);
                        unitRect.Texture = body.Tex;
                    }
                    else
                    {
                        unitRect.FillColor = new Color(r, g, b, 255);
                        unitRect.Texture = null;
                    }
                    target.Draw(unitRect);
                }
                else if (body.Shape == DisplayShape.Capsule)
                {
                    unitCircle.Scale = new Vector2f(body.Dimension.Y, body.Dimension.Y) * 2;
                    unitCircle.Position = new Vector2f(body.Body.Position.X * 10, body.Body.Position.Y * 10 - body.Dimension.X * 5);
                    if (body.Tex != null)
                    {
                        unitCircle.FillColor = new Color(255, 255, 255, 255);
                        unitCircle.Texture = body.Tex;
                    }
                    else
                    {
                        unitCircle.FillColor = new Color(r, g, b, 255);
                        unitCircle.Texture = null;
                    }
                    target.Draw(unitCircle);
                    unitCircle.Position = new Vector2f(body.Body.Position.X * 10, body.Body.Position.Y * 10 + body.Dimension.X * 5);
                    target.Draw(unitCircle);
                }
            }
        }

        public Body CreateBox(Vector2f dimension, float density, BodyType type)
        {
            Body b = BodyFactory.CreateRectangle(world, dimension.X, dimension.Y, density);
            b.UserData = "box;" + dimension.X + ";" + dimension.Y + ";None;user";
            b.BodyType = type;
            Add(b);
            return b;
        }

        public Body CreateCircle(float radius, float density, BodyType type)
        {
            Body b = BodyFactory.CreateCircle(world, radius, density);
            b.UserData = "circle;" + radius + ";None;user";
            b.BodyType = type;
            Add(b);
            return b;
        }

        public void Clear()
        {
            if (OnClear != null)
                OnClear(this, EventArgs.Empty);

            world.Clear();
        }

        public BodyEx FindBody(Body body)
        {
            foreach (BodyEx b in Bodies)
            {
                if (b.Body == body)
                    return b;
            }
            return null;
        }

        public void ChangeDimension(bool dimension0)
        {
            if (!ignoreDim)
                foreach (BodyEx b in Bodies)
                {
                    if (b.GameDimension == Dimension.OneO)
                        b.Body.Enabled = dimension0;
                    if (b.GameDimension == Dimension.TwoX)
                        b.Body.Enabled = !dimension0;
                }
            dim0 = dimension0;
        }

        public void Copy(Body body, Vector2 position)
        {
            BodyEx b = FindBody(body);
            Body copy;
            if (b.Shape == DisplayShape.Box)
            {
                copy = BodyFactory.CreateRectangle(world, b.Dimension.X, b.Dimension.Y, b.Body.Mass, b.Shape.ToString().ToLower() + ";" + b.Dimension.X + ";" + b.Dimension.Y + ";" + b.GameDimension.ToString() + ";copy");
            }
            else if (b.Shape == DisplayShape.Circle)
            {
                copy = BodyFactory.CreateCircle(world, b.Length, b.Body.Mass, b.Shape.ToString().ToLower() + ";" + b.Dimension.X + ";" + b.Dimension.Y + ";" + b.GameDimension.ToString() + ";copy");
            }
            else
            {
                copy = BodyFactory.CreateCapsule(world, b.Dimension.X, b.Dimension.Y, b.Body.Mass, b.Shape.ToString().ToLower() + ";" + b.Dimension.X + ";" + b.Dimension.Y + ";" + b.GameDimension.ToString() + ";copy");
            }
            copy.BodyType = body.BodyType;
            copy.Friction = body.Friction;
            copy.Restitution = body.Restitution;
            copy.Rotation = body.Rotation;
            copy.Position = position;
            copy.IsBullet = body.IsBullet;
            Add(copy);
        }
    }
}