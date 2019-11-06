using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarCompiler : MonoBehaviour
{
    [SerializeField] private LayerMask[] obstacleMasks;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Calculate Euclidean Cost
    private float CalculateEuclideanCost(AStarNode startNode, AStarNode endNode)
    {
        // find vector between positions
        Vector2 vecCost = new Vector2(startNode.Position.x - endNode.Position.x, startNode.Position.y - endNode.Position.y);
        // return the magnitude
        return vecCost.magnitude;
    }

    // Create A* Path


    // Calculate Path

}
