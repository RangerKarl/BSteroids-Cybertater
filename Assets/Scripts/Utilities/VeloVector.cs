using Godot;
using System;

public partial class VeloVector : Line2D
{
    public override void _Process(double delta)
    {
        GlobalRotation = 0;
    }
}