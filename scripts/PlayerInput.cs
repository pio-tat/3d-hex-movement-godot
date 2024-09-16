using Godot;
using System;

public partial class PlayerInput : Node3D
{
	[Signal] public delegate void RightClickEventHandler(Vector2 mousePos);
	[Signal] public delegate void LeftClickEventHandler(Vector2 mousePos);
	// public event InputEventHandler OnRightClick;
	// public event InputEventHandler OnLeftClick;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventMouseButton eventMouseButton){
			if(eventMouseButton.ButtonIndex == MouseButton.Left && eventMouseButton.IsPressed())
				EmitSignal(SignalName.LeftClick, eventMouseButton.Position);
			if(eventMouseButton.ButtonIndex == MouseButton.Right && eventMouseButton.IsPressed())
				EmitSignal(SignalName.RightClick, eventMouseButton.Position);
		}
    }
}
