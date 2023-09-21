using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int G = 1;
    public int H;
    public int F => G + H;
    public bool moveAble = true;
    public Vector3Int pos;
    public Node parentNode;
    public void Reset()
    {
        parentNode = null;
        G = 1;
        H = 0;
    }
}
public class TileData
{
    public Node node;
    public TileCover tileCover;
    public GameObject tileObject;
    public SpriteRenderer sprite;
}
