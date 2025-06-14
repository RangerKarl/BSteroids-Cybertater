using Godot;
using Godot.Collections;
using System;

/// <summary>
/// This class defines the boundaries of the game space. All objects should respect this boundary 
/// and flip over to the other side
/// </summary>
public partial class ScreenBoundsMvmnt : Node
{
	[Export]
	Node2D node;

    BoundsStruct bounds;

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

		bounds.top		= (cameraPosition.Y - size.Y) / 2;
        bounds.bottom	= (cameraPosition.Y + size.Y) / 2;
		bounds.left		= (cameraPosition.X - size.X) / 2;
		bounds.right	= (cameraPosition.X + size.X) / 2;

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var globalPosit = node.GlobalPosition;
		
		if (globalPosit.Y > bounds.bottom)
            globalPosit.Y = bounds.top;
		else if (globalPosit.Y <= bounds.top)
			globalPosit.Y = bounds.bottom;

		if (globalPosit.X < bounds.left)
			globalPosit.X = bounds.right;
		else if (globalPosit.X >= bounds.right)
			globalPosit.X = bounds.left;

		node.GlobalPosition = globalPosit;
	}
}

struct BoundsStruct
{
	[Export]
	public float top, bottom, left, right;
}
