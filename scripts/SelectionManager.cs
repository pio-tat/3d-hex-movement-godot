using Godot;
using Godot.Collections;
using System;

public partial class SelectionManager : Node3D
{
	[Signal] public delegate void UnitSelectedEventHandler(Node3D unitReference);
	[Signal] public delegate void HexSelectedEventHandler(Hex hex);
	[Signal] public delegate void SelectionStateChangedEventHandler(int previous, int newOne);

	[Export] PlacementManager placementManager;
	[Export] Camera3D mainCamera;
	[Export] Shader highlightShader;
	[Export] Material normalMat;
	[Export] Material highlightMat;
	IHighlightble last;
	public static SelectionStates selectionState = SelectionStates.free;
	public static SelectionManager manager;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		manager = this;
	}

	public void LeftClick(Vector2 mousePos)
	{
		if(FindTarget(mousePos, out Node3D result)){
			if(result is Hex){
				EmitSignal(SignalName.HexSelected, result as Hex);
				GD.Print((result as Hex).offset);
			}

			if(selectionState == SelectionStates.free){
				if(result is Unit){
					EmitSignal(SignalName.UnitSelected, result);
					ChangeSelectionState(SelectionStates.movingUnit);
				}
					
			}

			if(selectionState == SelectionStates.movingUnit){

			}
		}

	}

	public void RightClick(Vector2 mousePos)
	{
		ChangeSelectionState(SelectionStates.free);
	}

    bool FindTarget(Vector2 mousePos, out Node3D result)
	{
		//gets start and end points of ray;
		Vector3 from = mainCamera.ProjectRayOrigin(mousePos);
		Vector3 to = from + mainCamera.ProjectRayNormal(mousePos) * 1000.0f;

		PhysicsDirectSpaceState3D spaceState = GetWorld3D().DirectSpaceState; //gets physic world or sth idk

		var query = PhysicsRayQueryParameters3D.Create(from, to); //creates ray parameters or sth
		Dictionary hit = spaceState.IntersectRay(query); //casts ray

		//checks if the ray hit sth
		if(hit.Count > 0){
			result = hit["collider"].As<Node3D>();
			return true;
		}
		result = null;
		return false;
	}

    public override void _Input(InputEvent inputEvent)
    {
        if(inputEvent is InputEventMouseMotion mouseMotion){
			last?.Highlight.ToggleGlow(false); //removes glow from last object
			last = null;
			if(FindTarget(mouseMotion.Position, out Node3D result) && result is IHighlightble && selectionState != SelectionStates.movingUnit){
				IHighlightble current = result as IHighlightble; //hets interfaces from result

				//prevents from making disabled tiles glow
				if(selectionState == SelectionStates.placing && current.Highlight.state == HighlightType.disabled)
					return;

                current.Highlight.ToggleGlow(); //toggles glow in result
				
				last = current; //setting last to current object so it can remove highlight if necessery
			}
			
		}
    }

	public void ChangeSelectionState(SelectionStates newState)
	{
		EmitSignal(SignalName.SelectionStateChanged, (int)selectionState, (int)newState);
		selectionState = newState;
	}
}

public enum SelectionStates
{
	free,
	placing,
	movingUnit
}