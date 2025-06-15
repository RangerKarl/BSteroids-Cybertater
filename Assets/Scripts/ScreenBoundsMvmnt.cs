using BSteroids.Scripts.Utilities;
using Godot;

namespace BSteroids.Scripts.Game
{
    /// <summary>
    /// This class defines the boundaries of the game space. All objects should respect this boundary 
    /// and flip over to the other side
    /// </summary>
    public partial class ScreenBoundsMvmnt : Node
    {
        [Export]
        Node2D node;

        float[] bounds = new float[4];

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            // in C# must assign explicitly apparently, no idea why editor not letting it propagate 
            node = (Node2D)GetParent();

            var rect = GetViewport().GetVisibleRect();
            var camera = GetViewport().GetCamera2D();
            var zoom = camera.Zoom;
            var cameraPosition = camera.Position;
            var size = rect.Size / zoom;

            bounds[(int)Directions.Top] = (cameraPosition.Y - size.Y) / 2;
            bounds[(int)Directions.Bottom] = (cameraPosition.Y + size.Y) / 2;
            bounds[(int)Directions.Left] = (cameraPosition.X - size.X) / 2;
            bounds[(int)Directions.Right] = (cameraPosition.X + size.X) / 2;

        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            var globalPosit = node.GlobalPosition;

            // float variability bites us again
            // don't swap unless we have to. No need to spend the swaptime
            if (globalPosit.Y >= bounds[(int)Directions.Bottom])
            {
                SwapPositions(globalPosit, Directions.Top);
            }
            else if (globalPosit.Y <= bounds[(int)Directions.Top])
            {
                SwapPositions(globalPosit, Directions.Bottom);
            }

            if (globalPosit.X <= bounds[(int)Directions.Left])
            {
                SwapPositions(globalPosit, Directions.Right);

            }
            else if (globalPosit.X >= bounds[(int)Directions.Right])
            {
                SwapPositions(globalPosit, Directions.Left);
            }
        }

        private void SwapPositions(Vector2 globalPosit, Directions dir)
        {
            if (dir  == Directions.Bottom || dir == Directions.Top)
            {
                globalPosit.Y = bounds[(int)dir];
                node.GlobalPosition = globalPosit;
            }
            else if (dir == Directions.Left || dir == Directions.Right)
            {
                globalPosit.X = bounds[(int)dir];
                node.GlobalPosition = globalPosit;
            }
            
        }
    }

}
