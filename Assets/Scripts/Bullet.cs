using BSteroids.Scripts.Utilities;
using Godot;
using System;

namespace BSteroids.Scripts.Game {

    public partial class Bullet : Area2D
    {
        [Export]
        int BulletSpeed = 700;

        [Export]
        public BulletOwner BulletOwner;
            
        public Vector2 Direction;

        Timer timer;

        public override void _Process(double delta)
        {
            Position = Position + (Direction * BulletSpeed * (float)delta);            
        }


        private void OnAreaEntered(Area2D area)
        {
            GD.Print("Bullet");

            if (area is Asteroid asteroid)
            {
                asteroid.Explode();
            }
        }
    }
}
