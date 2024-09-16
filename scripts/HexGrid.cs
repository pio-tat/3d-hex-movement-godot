using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

public partial class HexGrid : Node3D
{
	Dictionary<Vector3I, Hex> hexDict = new();
	public Dictionary<Vector3I, Hex> HexDict {get => hexDict;}

	[Export] Vector2I mapSize;
	[Export] PackedScene hexScn;

	public static HexGrid grid;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//GenerateGrid();
		Node3D[] children = GetChildren().Cast<Node3D>().ToArray();
		foreach(Node3D hex in children){
			hexDict.Add(Hex.ConvertPositionToOffset(hex.Position), hex as Hex);
		}

		grid = this;
	}

	public Hex GetHex(Vector3I offset)
	{
		offset.Y = 0;
		Hex searchedHex = null;
		hexDict.TryGetValue(offset, out searchedHex);
		return searchedHex;
	}
	public Hex GetHex(Godot.Vector3 position)
	{
		Vector3I offset = Hex.ConvertPositionToOffset(position);
		offset.Y = 0;
		Hex searchedHex = hexDict[offset];
		return searchedHex;
	}
	public Hex[] GetAllHexes()
	{
		return hexDict.Values.Cast<Hex>().ToArray();
	}

	public List<Vector3I> GetNeighboursFor(Vector3I hex)
	{
		List<Vector3I> neighbours = new();

		foreach(Vector3I neighbour in Directions.GetDirectionList(hex.Z)){
			if(hexDict.ContainsKey(neighbour + hex))
				neighbours.Add(neighbour + hex);
		}

		return neighbours;
	}

	void GenerateGrid()
	{
		int startX = mapSize.X / -2;
		for(int x = 0; x < mapSize.X; x++){
			int startY = mapSize.Y / -2;
			for(int y = 0; y < mapSize.Y; y++){
				Hex hex = hexScn.Instantiate<Hex>();
				GD.Print("I am here!");
				hex.Position = Hex.ConvertOffsetToPosition(new Vector3I(x + startX, 0, y + startY));
				AddChild(hex);
			}
		}
	}
}

public static class Directions
{
	public static Vector3I[] even = 
	{
		new Vector3I(1, 0, 0), //E
		new Vector3I(1, 0, 1), //N1
		new Vector3I(0, 0, 1), //N2
		new Vector3I(-1, 0, 0), //W
		new Vector3I(0, 0, -1), //S1
		new Vector3I(1, 0, -1) //S2
	};

	public static Vector3I[] odd = 
	{
		new Vector3I(1, 0, 0), //E
		new Vector3I(0, 0, 1), //N1
		new Vector3I(-1, 0, 1), //N2
		new Vector3I(-1, 0, 0), //W
		new Vector3I(-1, 0, -1), //S1
		new Vector3I(0, 0, -1) //S2
	};

	public static Vector3I[] GetDirectionList(int z) => z % 2 == 0 ? even : odd;
}