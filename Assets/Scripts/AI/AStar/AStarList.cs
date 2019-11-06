using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarList
{
    private ArrayList nodes = new ArrayList();
    private ListOrderComparer listOrder = new ListOrderComparer();

    public int Length {
        get { return this.nodes.Count; }
    }

    public bool Contains(object node)
    {
        return nodes.Contains(node);
    }

    public AStarNode First()
    {
        if (nodes.Count > 0)
            return (AStarNode)this.nodes[0];

        return null;
    }

    public void Add(AStarNode node)
    {
        nodes.Add(node);
        nodes.Sort(listOrder);
    }

    public void Remove(AStarNode node)
    {
        nodes.Remove(node);
        nodes.Sort(listOrder);
    }

    public void Sort()
    {
        nodes.Sort(listOrder);
    }

    public class ListOrderComparer : IComparer
    {
        static AStarNode n1;
        static AStarNode n2;

        public int Compare(object a, object b)
        {
            n1 = a as AStarNode;
            n2 = b as AStarNode;

            if (n1.F > n2.F) return 1;
            if (n1.F < n2.F) return -1;

            return 0;
        }
    }
}
