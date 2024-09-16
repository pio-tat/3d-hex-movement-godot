using Godot;
using System;

public partial class BuildingButton : Button
{
	[Export] PlacementManager manager;
    [Export] public PackedScene objectScn;

    public override void _Ready()
    {
        //manager = GetNode<PlacementManager>("PlacementManager");
    }

    public void OnToggled(bool state)
	{
		manager.ChangePlacementState(state, this);
	}
}