using Godot;
using System;
using System.Collections.Generic;

public partial class SearchGraph
{
    public static BFSResult BFSGetRange(HexGrid hexGrid, Vector3I startPoint, int movementPoints)
    {
        Dictionary<Vector3I, Vector3I?> tilesInRange = new(); //stores tiles that are in range and a tile that enable to get to that tile at smaller cost (questionmark is because without value cannot be null)
        Dictionary<Vector3I, int> tilesCost = new(); //stores tiles and their cost
        Queue<Vector3I> tilesToCheck = new(); //stores tiles that algorithm needs to check

        tilesToCheck.Enqueue(startPoint);
        tilesCost.Add(startPoint, 0);
        tilesInRange.Add(startPoint, null);

        while(tilesToCheck.Count > 0){
            Vector3I tile = tilesToCheck.Dequeue();
            foreach(Vector3I neighbourTile in hexGrid.GetNeighboursFor(tile)){
                Hex hex = hexGrid.GetHex(neighbourTile);
                
                if(hex.IsObstacle()) continue; //if obstacle - skip

                int newCost = tilesCost[tile]; //gets cost of parent tile
                newCost += hexGrid.GetHex(neighbourTile).GetCost(); //adds to parent's tile cost own cost

                if(newCost > movementPoints) continue; //if cost is to big skips that tile

                if(!tilesInRange.ContainsKey(neighbourTile)){ //if not adds tile to list
                    tilesInRange.Add(neighbourTile, tile);
                    tilesCost.Add(neighbourTile, newCost);
                    tilesToCheck.Enqueue(neighbourTile);
                }else if(tilesCost[neighbourTile] > newCost){ //or update it if it's already in the list but the new cost is smaller
                    tilesInRange[neighbourTile] = tile;
                    tilesCost[neighbourTile] = newCost;
                    tilesToCheck.Enqueue(neighbourTile);
                }
            }
        }

        return new  BFSResult{tilesInRange = tilesInRange};
    }
}

public struct BFSResult
{
    public Dictionary<Vector3I, Vector3I?> tilesInRange;

    public Stack<Vector3I> GetPathTo(Vector3I destination)
    {
        if(!tilesInRange.ContainsKey(destination)) return new Stack<Vector3I>();

        Vector3I currentHex = destination;
        Stack<Vector3I> path = new();
        while(tilesInRange[currentHex] != null){
            path.Push(currentHex);
            currentHex = (Vector3I)tilesInRange[currentHex];
        }

        return path;
    }
}