using Godot;
using System;

namespace BSteroids.Scripts.Game
{
    public partial class Explosion : CpuParticles2D
    {
        Timer timer;

        Action EndExplosion;

        public override void _Ready()
        {
            timer = GetNode<Timer>("Timer");
            EndExplosion = delegate () { QueueFree(); };
            timer.WaitTime = Lifetime;
            timer.Timeout += EndExplosion;

            base._Ready();
        }

        public override void _Process(double delta)
        {
            if (Emitting && timer.IsStopped())
            {
                timer.Start();
            }
        }
    }

}
