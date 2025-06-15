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
        
        Sprite2D _sprite;

        Explosion _explosion;

        [Signal]
        public delegate void OnAsteroidDestroyedEventHandler(AsteroidSizes NextSize, Vector2 position);

        public override void _Ready()
        {
            // scale value depend on size

            var scaleValue = 1 / ((int)Size + 1);

            Scale = new Vector2(scaleValue, scaleValue);

            Random rnd = new Random();
            var x = rnd.Next(-1, 1);
            var y = rnd.Next(-1, 1);
            Direction = new Vector2(x, y);

            // update the Sprite2D with a random asset from the image paths

            _sprite = this.GetNode<Sprite2D>("./Sprite2D");
            var img = (Texture2D)GD.Load(imagePaths[rnd.Next(0, imagePaths.Length -1)]);
            _sprite.Texture = img;

            // attach explosion
            // something wrong with getnode here, it's not picking up - turns out it was the scene name causing the route to fail. Make sure the naming is right from the get go!
            _explosion = this.GetNode<Explosion>("Explosion");

            GD.Print("Asteroid spawn");

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
                OnDestroy();
            }
        }

        private void EmitExplosion()
        {
            _explosion.Emitting = true;
            _explosion.Reparent(GetTree().Root);
        }

        private void OnDestroy()
        {
            int NewAsteroidSize = (int)Size;

            if (NewAsteroidSize != (int)AsteroidSizes.SMALL)
                NewAsteroidSize -= 1;

            // NewAsteroidSize needs to be converted down as a limited set of types are C# compatible. Enums, apparently, are not.
            OnAsteroidDestroyed += (Size, Position) => { EmitSignal(SignalName.OnAsteroidDestroyed, NewAsteroidSize, Position); };

            EmitExplosion();
            QueueFree();


        }

    }

}
