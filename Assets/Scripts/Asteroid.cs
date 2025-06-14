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

        // update the Sprite2D with a random asset from the image paths

        Sprite2D sprite = (Sprite2D)this.GetNode("./Sprite2D");
        var img = (Texture2D)GD.Load(imagePaths[rnd.Next(0, 3)]);
        sprite.Texture = img;

    }

    public override void _Process(double delta)
    {
        Position += Direction * Speed * (float)delta;
    }
}
