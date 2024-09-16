using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

public partial class MovementSystem : Node3D
{
	[Export] HexGrid hexGrid;
    public List<Hex> path = new();

    Unit selectedUnit;
    BFSResult currentRange;

    SelectionManager selectionManager;

    public override void _Ready()
    {
        hexGrid = GetParent().GetNode<HexGrid>("HexGrid");
        selectionManager = GetParent().GetNode("CameraPivot").GetNode<SelectionManager>("SelectionManager");
    }

    public void HighlightPath(Unit unit)
    {
        BFSResult bfs = SearchGraph.BFSGetRange(hexGrid, unit.currentHex.offset, unit.movePoints);

        currentRange = bfs;

        foreach(Vector3I hex in bfs.tilesInRange.Keys){
            GD.Print("Hex: " + hex + " From: " + bfs.tilesInRange[hex]);
        }

        foreach(Vector3I hexPos in bfs.tilesInRange.Keys){
            Hex hex = hexGrid.GetHex(hexPos);

            path.Add(hex);
            hex.Highlight.ToggleGlow(true);
        }

        selectedUnit = unit;
    }

    public void ClearHighilightedPath()
    {
        GD.Print("clearing!");
        foreach(Hex hex in path){
            hex.Highlight.ToggleGlow(false);
        }
        path.Clear();

        selectedUnit = null;
    }

    public void HexSelected(Hex hex)
    {
        if(SelectionManager.selectionState != SelectionStates.movingUnit) return;

		if(!path.Contains(hex)){
			SelectionManager.manager.ChangeSelectionState(SelectionStates.free);
			return;
		}

        if(hex == selectedUnit.currentHex) return;
            
        MoveUnit(hex); 
    }
    void MoveUnit(Hex hex)
    {
        selectedUnit.GiveDestination(hex, currentRange);

        ClearHighilightedPath();
        selectionManager.ChangeSelectionState(SelectionStates.free);
    }
}
