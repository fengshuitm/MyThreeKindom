using UnityEngine;
using System.Collections;
using System;
using CreativeSpore.SuperTilemapEditor;

public class Node : IComparable
{
    public float nodeTotalCost;
    public float estimatedCost;
    public bool bObstacle;
    public Node parent;
    public Vector3 position;

    public Node()
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
    }

    public Node(Vector3 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
        this.position = pos;
    }

    public Node(Vector2 pos)
    {
        this.estimatedCost = 0.0f;
        this.nodeTotalCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
        this.position.x = pos.x;
        this.position.y = pos.y;
        this.position.z = 0;
    }

    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }

    public bool IsObstacle()
    {
        Tilemap tilemap = GridManager.getInstance().tiledmap.GetComponent<Tilemap>();

        uint rawTileData = tilemap.GetTileData(position);
        TileData data = new TileData(rawTileData);

        /*bool flipVertical = data.flipVertical;
        bool flipHorizontal = data.flipHorizontal;
        bool rot90 = data.rot90;
        int brushId = data.brushId;
        int tileId = data.tileId;
        */

        return false;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        //Negative value means object comes before this in the sort order.  
        if (this.estimatedCost < node.estimatedCost)
            return -1;
        //Positive value means object comes after this in the sort order.  
        if (this.estimatedCost > node.estimatedCost)
            return 1;
        return 0;
    }
}