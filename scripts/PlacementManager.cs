using Godot;
using System;

public partial class PlacementManager : Node3D
{
	// public bool isPlacing = false;
	BuildingButton lastButton;
	[Export] PackedScene building;
	[Export] HexGrid hexGrid;
	[Export] SelectionManager selectionManager;

	public void OnSelectionStateChanged(int oldOne, int newOne)
	{
		if(oldOne != (int)SelectionStates.placing) return;
		lastButton.ButtonPressed = false;
		DisableOccupiedHexes(false);
	}

	public void ChangePlacementState(bool state, BuildingButton button)
	{
		if(lastButton != null)
			lastButton.ButtonPressed = false;

		selectionManager.ChangeSelectionState(state ? SelectionStates.placing : SelectionStates.free);
		lastButton = button;
        DisableOccupiedHexes(state);

		building = button.objectScn;
	}

	public void Place(Hex hex)
	{
		if(SelectionManager.selectionState != SelectionStates.placing) return;
		if(hex.Occupied) return;

		hex.Highlight.ToggleDisabled(true);
		
		Node3D building = this.building.Instantiate<Node3D>();
		building.Position = new Vector3(hex.Position.X, 1.5f, hex.Position.Z);

		GetTree().CurrentScene.AddChild(building);
	}

	public void DisableOccupiedHexes(bool state)
	{
		foreach(Hex hex in hexGrid.HexDict.Values){
			if(hex.Occupied)
				hex.Highlight.ToggleDisabled(state);
		}
	}
}

public class SpawningData
{
	public PackedScene objectScn;
	enum Type
	{
		building,
		unit
	}
}