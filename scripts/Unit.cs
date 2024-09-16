using Godot;
using System;
using System.Collections.Generic;

public partial class Unit : CharacterBody3D, IHighlightble
{
	Highlight highlight;
	public Highlight Highlight {get => highlight;}

	public Hex currentHex;
	[Export] public int maxMovePoints = 5;
	public int movePoints;

	bool isMoving = false;
	Vector3 targetPos;
	[Export] const float SPEED = 3.5f;

	Stack<Vector3I> pathToFollow = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		highlight = GetNode<Highlight>("Highlight");

		Vector3 hexPos = new(Position.X, 0, Position.Z);
		GD.Print("Unit position: " + hexPos);
		currentHex = HexGrid.grid.GetHex(hexPos);

		movePoints = maxMovePoints;
	}

	public void GiveDestination(Hex destination, BFSResult bfs)
	{
		pathToFollow = bfs.GetPathTo(destination.offset);
		MoveTo(pathToFollow.Pop());
	}

	void MoveTo(Vector3I coords)
	{
		Hex hex = HexGrid.grid.GetHex(coords);

		targetPos = new Vector3(hex.Position.X, 1.5f, hex.Position.Z);
		isMoving = true;

		currentHex.ChangeOccupator(null);
		currentHex = hex;
		currentHex.ChangeOccupator(this);
		
		movePoints -= hex.GetCost();
	}

    public override void _Process(double delta)
    {
		Vector3 direction = targetPos - GlobalPosition;
		if(direction.Length() > 0.05f && isMoving){
			GlobalTranslate(direction.Normalized() * SPEED * (float)delta);
		}
		if(direction.Length() < 0.05f && isMoving){
			isMoving = false;
			if(pathToFollow.Count > 0){
				MoveTo(pathToFollow.Pop());
			}
		}
    }
}
