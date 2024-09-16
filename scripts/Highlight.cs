using Godot;
using System;
using System.Collections.Generic;

public partial class Highlight : Node3D
{
	[Export] Shader glowShader;
	[Export] Shader disabledShader;

	Dictionary<MeshInstance3D, StandardMaterial3D> normalMats = new();
	Dictionary<MeshInstance3D, StandardMaterial3D> glowMats = new();
	Dictionary<MeshInstance3D, StandardMaterial3D> disabledMats = new();

	bool isGlowing = false;
	public HighlightType state;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//gets all meshes in object and make materials from them
		foreach(Node child in GetParent().GetChildren()){
			if(child is not MeshInstance3D) continue;
			MeshInstance3D mesh = child as MeshInstance3D;

			//normal material
			StandardMaterial3D normalMat = mesh.MaterialOverride.Duplicate() as StandardMaterial3D;
			normalMats.Add(mesh, normalMat);

			//glow material
			StandardMaterial3D glowMat = normalMat.Duplicate() as StandardMaterial3D;
			glowMat.NextPass = new ShaderMaterial(){Shader = glowShader};
			glowMats.Add(mesh, glowMat);

			//disabled material
			StandardMaterial3D disabledMat = normalMat.Duplicate() as StandardMaterial3D;
			disabledMat.NextPass = new ShaderMaterial(){Shader = disabledShader};
			disabledMats.Add(mesh, disabledMat);
		}
	}

	public void ToggleGlow(bool state)
	{
		if(isGlowing == state) return;
		isGlowing = state;
		this.state = state ? HighlightType.glow : HighlightType.none;
		foreach(MeshInstance3D mesh in glowMats.Keys){
			if(state)
				mesh.MaterialOverride = glowMats[mesh];
			else
				mesh.MaterialOverride = normalMats[mesh];
		}
	}
	public void ToggleGlow()
	{
		isGlowing = !isGlowing;
		this.state = isGlowing ? HighlightType.glow : HighlightType.none;
		foreach(MeshInstance3D mesh in glowMats.Keys){
			if(isGlowing)
				mesh.MaterialOverride = glowMats[mesh];
			else
				mesh.MaterialOverride = normalMats[mesh];
		}
	}
	public void ToggleDisabled(bool state)
	{
		if(state == (this.state == HighlightType.disabled)) return;
		this.state = state ? HighlightType.disabled : HighlightType.none;
		foreach(MeshInstance3D mesh in disabledMats.Keys){
			if(state)
				mesh.MaterialOverride = disabledMats[mesh];
			else
				mesh.MaterialOverride = normalMats[mesh];
		}
		isGlowing = false;
	}
}

interface IHighlightble
{
	public Highlight Highlight {get;}
}

public enum HighlightType
{
	none,
	glow,
	disabled
}