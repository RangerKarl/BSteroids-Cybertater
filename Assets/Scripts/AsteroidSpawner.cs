using Godot;
using System;
using BSteroids.Scripts.Utilities;

namespace BSteroids.Scripts.Game
{
    public partial class AsteroidSpawner : Node
    {
        [Export]
        int count = 6;

        [Export]
        PackedScene AsteroidScene;

        float[] bounds = new float[4];

        public override void _Ready()
        {
            // pick a random point on scene
            for (int i = 0; i < count; i++)
            {
                // get random spawn pos
                var randomSpawnPosition = GetRandomPositionFromScreenRect();
                SpawnAsteroid(AsteroidSizes.BIG, randomSpawnPosition);
            }
        }



        private void SpawnAsteroid(AsteroidSizes size, Vector2 position)
        {
            var asteroid = (Asteroid)AsteroidScene.Instantiate();
            // kind of an awkward way to defer a call, reference
            // https://docs.godotengine.org/en/stable/tutorials/scripting/c_sharp/c_sharp_basics.html#current-gotchas-and-known-issues
            GetTree().Root.CallDeferred(Node.MethodName.AddChild, asteroid);
            asteroid.GlobalPosition = position;
            asteroid.Size = size;
            asteroid.AsteroidIsDestroyed += AsteroidDestroyed;
        }

        private void AsteroidDestroyed(AsteroidSizes size, Vector2 position)
        {
            GD.Print($"AsteroidSpawner hears asteroid of size {size.ToString()} destroyed");

            // spawn 3 every time destroyed
            for (int i = 0; i < 2; i++)
            {
                if (size != AsteroidSizes.SMALL)
                {
                    SpawnAsteroid(size + 1, position);
                }
            }


        }

        private Vector2 GetRandomPositionFromScreenRect()
        {
            var rect = GetViewport().GetVisibleRect();
            var camera = GetViewport().GetCamera2D();
            var zoom = camera.Zoom;
            var cameraPosition = camera.Position;
            var size = rect.Size / zoom;
            var rng = new Random();

            bounds[(int)Directions.Top] = (cameraPosition.Y - size.Y) / 2;
            bounds[(int)Directions.Bottom] = (cameraPosition.Y + size.Y) / 2;
            bounds[(int)Directions.Left] = (cameraPosition.X - size.X) / 2;
            bounds[(int)Directions.Right] = (cameraPosition.X + size.X) / 2;

            var x = rng.NextDouble() * Double.Abs(bounds[(int)Directions.Left] - bounds[(int)Directions.Right]);
            var y = rng.NextDouble() * Double.Abs(bounds[(int)Directions.Top] - bounds[(int)Directions.Bottom]);

            return new Vector2((float)x, (float)y);
        }
    }

}
