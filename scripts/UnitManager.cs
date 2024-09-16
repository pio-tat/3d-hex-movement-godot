using Godot;
using System;

public partial class UnitManager : Node3D
{
	Unit selectedUnit;
	[Export] MovementSystem moveSys;
	// public bool isMovingUnit = false;
	
	public void HandleUnitSelected(Node3D unitReference)
	{
		GD.Print("nigger");
		Unit unit = unitReference as Unit;
		if(!CheckIfTheSameUnitSelected(unit)){
			selectedUnit = unit;
			moveSys.HighlightPath(unit);
		}
	}

	public void OnSelectionStateChanged(int oldOne, int newOne)
	{
		if(oldOne != (int)SelectionStates.movingUnit) return;

		moveSys.ClearHighilightedPath();
		selectedUnit = null;
	}

	bool CheckIfTheSameUnitSelected(Unit unit)
	{
		if(selectedUnit == unit){
			return true;
		}

		return false;
	}

	public void OnHexSelected(Hex hex)
	{
		moveSys.HexSelected(hex);
	}
}
