using Godot;
using System;

public partial class Building : StaticBody3D, IHighlightble
{
	HexGrid hexGrid;
	[Export] public int health;
	[Export] int cost;
	public int Cost {get => cost;}
	Hex hex;
	public Hex Hex {get => hex;}

	public Highlight Highlight {get => highlight;}
	Highlight highlight;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		hexGrid = GetTree().CurrentScene.GetNode<HexGrid>("HexGrid");
		hex = hexGrid.GetHex(Position);
		highlight = GetNode<Highlight>("Highlight");
	}
}
