using Godot;
using System;

namespace BSteroids.Scripts.Game {

    public partial class Bullet : Area2D
    {
        [Export]
        int BulletSpeed = 700;
                        
        public Vector2 Direction;


        Timer timer;

        public override void _Process(double delta)
        {
            Position = Position + (Direction * BulletSpeed * (float)delta);
        }
    }
}
