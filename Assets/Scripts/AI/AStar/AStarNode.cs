using System;

[Serializable]
public struct NodePosition
{
    public float x;
    public float y;

    public NodePosition(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}

public class AStarNode : IComparable
{
    public float F { get; set; }
    public float G { get; set; }
    public float H { get; set; }
    public bool IsObstacle { get; set; }
    public AStarNode Parent { get; set; }
    public NodePosition Position { get; set; }

    public AStarNode()
    {
        H = 0.0f;
        G = 1.0f;
        F = G + H;
        IsObstacle = false;
        Parent = null;
    }

    public AStarNode(float x, float y)
    {
        H = 0.0f;
        G = 1.0f;
        F = G + H;
        IsObstacle = false;
        Parent = null;
        Position = new NodePosition(x, y);
    }

    public int CompareTo(object obj)
    {
        AStarNode node = (AStarNode)obj;

        // Negative means object comes before this node in the sorted list
        if (this.H < node.H)
            return -1;

        // Positive means node comes before object in the sorted list
        if (this.H > node.H)
            return 1;

        return 0;
    }
}
