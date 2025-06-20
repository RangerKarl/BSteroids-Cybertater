using BSteroids.Scripts.Utilities;
using Godot;
using System;

namespace BSteroids.Scripts.Game
{
    public partial class Asteroid : Area2D
    {
        [Export]
        string[] imagePaths = new string[4];

        [Export]
        float Speed = 50f;

        Vector2 Direction;

        internal AsteroidSizes Size = AsteroidSizes.BIG;
        
        Sprite2D sprite;

        public override void _Ready()
        {
            GD.Print("Asteroid spawn");
            // scale value depend on size

            var scaleValue = 1 / ((int)Size + 1);

            Scale = new Vector2(scaleValue, scaleValue);

            Random rnd = new Random();
            var x = rnd.Next(-1, 1);
            var y = rnd.Next(-1, 1);
            Direction = new Vector2(x, y);

            // update the Sprite2D with a random asset from the image paths

            sprite = (Sprite2D)this.GetNode("./Sprite2D");
            var img = (Texture2D)GD.Load(imagePaths[rnd.Next(0, imagePaths.Length -1)]);
            sprite.Texture = img;

        }

        public override void _Process(double delta)
        {
            Position += Direction * Speed * (float)delta;
        }

        private void OnBodyEntered(Node2D node)
        {
            //GD.Print("BODY ENTERED");
            if (node is PlayerMovement)
            {
                // nuke the player
                node.QueueFree();
            }
        }

    }

}
