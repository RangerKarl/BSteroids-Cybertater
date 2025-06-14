using Godot;
using System;

public partial class Asteroid : Area2D
{
    [Export]
    string[] imagePaths = new string[4];

    [Export]
    float Speed = 50f;

    Vector2 Direction;

    public override void _Ready()
    {
        Random rnd = new Random();
        var x = rnd.Next(-1, 1);
        var y = rnd.Next(-1, 1);
        Direction = new Vector2(x, y);
    }

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }
}
