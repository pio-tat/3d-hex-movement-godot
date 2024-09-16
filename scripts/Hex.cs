using Godot;
using System;

public partial class Hex : StaticBody3D, IHighlightble
{
	public static readonly float xOffset = 0.866f * 2, yOffset = 1, zOffset = 1.5f;
	public Vector3I offset;

	[Export] HexType hexType;
	public HexType HexType{get => hexType;}

	[Export] public MeshInstance3D mesh;
	[Export] Shader highlightShader;
	[Export] Shader disabledShader;
	
	StandardMaterial3D normaldMat;
	StandardMaterial3D glowMat;
	StandardMaterial3D disabledMat;

	public Highlight Highlight {get => highlight;}
	Highlight highlight;

	public Node3D occupator;

	public bool Occupied {get => occupator != null;}
	bool isGlowing = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		offset = ConvertPositionToOffset(Position);
		mesh = GetChild<MeshInstance3D>(0);

		normaldMat = mesh.MaterialOverride.Duplicate() as StandardMaterial3D;
		glowMat = normaldMat.Duplicate() as StandardMaterial3D;
		glowMat.NextPass = new ShaderMaterial(){Shader = highlightShader};

		disabledMat = normaldMat.Duplicate() as StandardMaterial3D;
		disabledMat.NextPass = new ShaderMaterial(){Shader = disabledShader};

		highlight = GetNode<Highlight>("Highlight");
	}

	public static Vector3I ConvertPositionToOffset(Vector3 position)
	{
		int x = Mathf.CeilToInt(position.X / xOffset);
		int y = Mathf.CeilToInt(position.Y / yOffset);
		int z = Mathf.CeilToInt(position.Z / zOffset);

		return new Vector3I(x, y, z);
	}
	public static Vector3 ConvertOffsetToPosition(Vector3I offset)
	{
		float x = offset.X * xOffset;
		float y = offset.Y * yOffset;
		float z = offset.Z * zOffset;
		if(offset.Z % 2 != 0) x += xOffset / 2f;

		return new Vector3(x, y, z);
	}

	public int GetCost() => hexType switch
	{
		HexType.normal => 2,
		HexType.hard => 4,
		HexType.easy => 1,
		_ => throw new Exception($"Hex type {hexType} not supported")
	};

	public void ChangeOccupator(Node3D newOccupator)
	{
		occupator = newOccupator;
	}

	public bool IsObstacle() => hexType == HexType.obstacle;
}

public enum HexType
{
	normal,
	hard,
	easy,
	obstacle
}