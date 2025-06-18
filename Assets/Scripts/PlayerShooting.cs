using Godot;
using System;

namespace BSteroids.Scripts.Game
{
    public partial class PlayerShooting : Node2D
    {
        [Export]
        PackedScene BulletScene;

        public override void _Process(double delta)
        {
            if (Input.IsActionJustPressed("fire"))
            {
                var bullet = (Bullet)BulletScene.Instantiate();
                GetTree().Root.AddChild(bullet);

                var parentNode2D = (Node2D)GetParent();
                bullet.Direction = new Vector2(0, 1).Rotated(parentNode2D.Rotation);
                bullet.Position= GlobalPosition;
            }
        }
    }

}
