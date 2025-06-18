using Godot;
using System;

namespace BSteroids.Scripts.Game
{
    public partial class ExplosionParticles : CpuParticles2D
    {
        [Export]
        public Timer timer;

        public override void _Ready()
        {
            timer.WaitTime = Lifetime;

            timer.Timeout += () => QueueFree();
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
