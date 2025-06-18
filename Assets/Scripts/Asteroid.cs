using BSteroids.Scripts.Utilities;
using Godot;
using System;

namespace BSteroids.Scripts.Game
{
    public partial class Asteroid : Area2D
    {
        string[] imagePaths = { "res://Assets/Textures/Asteroid_01.png",
                                "res://Assets/Textures/Asteroid_02.png",
                                "res://Assets/Textures/Asteroid_03.png",
                                "res://Assets/Textures/Asteroid_04.png"};

        [Export]
        float Speed = 50f;

        ExplosionParticles explosion;

        Vector2 Direction;

        internal AsteroidSizes Size = AsteroidSizes.BIG;
        
        Sprite2D sprite;

        [Signal]
        public delegate void AsteroidIsDestroyedEventHandler(AsteroidSizes size, Vector2 position);

        public override void _Ready()
        {
            GD.Print("Asteroid spawn");
            // scale value depend on size

            var scaleValue = (float)1 / ((int)Size + 1);

            Scale = new Vector2(scaleValue, scaleValue);

            if (0.1 < scaleValue && scaleValue < 0.5)
            {
                var collision = (CollisionShape2D)this.GetNode("./CollisionShape2D");
                var collisionShape = collision.Shape as CircleShape2D;
                var newCollisionShape = new CircleShape2D();
                newCollisionShape.Radius = collisionShape.Radius * scaleValue;
                collision.Shape = newCollisionShape;
            }


            Random rnd = new Random();
            SetDirection(rnd);

            // update the Sprite2D with a random asset from the image paths

            ApplySpriteFromSheet(rnd);

            explosion = (ExplosionParticles)this.GetNode("./ExplosionParticles");

        }

        private void ApplySpriteFromSheet(Random rnd)
        {
            sprite = (Sprite2D)this.GetNode("./Sprite2D");
            var img = (Texture2D)GD.Load(imagePaths[rnd.Next(0, imagePaths.Length - 1)]);
            sprite.Texture = img;
        }

        private void SetDirection(Random rnd)
        {
            var x = rnd.Next(-1, 1);
            var y = rnd.Next(-1, 1);
            Direction = new Vector2(x, y);
        }

        public override void _Process(double delta)
        {
            Position += Direction * Speed * (float)delta;
        }

        private void OnBodyEntered(Node2D node)
        {
            GD.Print("ASTEROID BODY ENTERED");

            if (node is PlayerMovement movement)
            {
                movement.OnPlayerBodyEntered();
            }
        }


        public void Explode()
        {
            //AsteroidData data = new AsteroidData { Position = this.Position, Size = this.Size  };

            // nuke the asteroid!
            EmitExplosion();
            // destroy player
            QueueFree();

            EmitSignal(SignalName.AsteroidIsDestroyed, (int)Size, Position);
        }
        private void OnAreaEntered(Area2D area)
        {
            //GD.Print("ASTEROID BODY ENTERED");

            //if (area is Bullet && ((Bullet)area).BulletOwner == BulletOwner.PLAYER)
            //{
            //    // nuke the asteroid!
            //    EmitExplosion();
            //    // destroy player
            //    QueueFree();
            //}
        }

        private void EmitExplosion()
        {
            // play explosion
            explosion.Emitting = true;
            // to keep explosion happening
            explosion.Reparent(GetTree().Root);
        }

    }

}
