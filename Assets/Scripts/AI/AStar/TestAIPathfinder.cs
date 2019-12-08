using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAIPathfinder : MonoBehaviour
{
    [SerializeField] private AStarPathfinding aStar;
    [SerializeField] private TestPlayerControl player;

    private float refreshTime = 5.0f;
    private float elapsedTime = 5.0f;

    private List<AStarNode> nodeList;
    private Vector2 currentTargetPos;

    private int currentNode = 0;

    void Start()
    {
        //nodeList = aStar.FindPath(transform.position, player.transform.position);
        //currentTargetPos = new Vector2(nodeList[0].posX, nodeList[0].posY);
    }

    // Update is called once per frame
    void Update()
    {
        if (refreshTime <= elapsedTime) {
            currentNode = 0;
            nodeList = aStar.FindPath(transform.position, player.transform.position);
            currentTargetPos = aStar.WorldPointFromNode(nodeList[currentNode]);

            elapsedTime = 0.0f;
        }

        if (Vector2.Distance(transform.position, currentTargetPos) < 0.05f) {

            if (currentNode == nodeList.Count - 1)
                elapsedTime = 6.0f;
            else {
                currentNode++;
                currentTargetPos = aStar.WorldPointFromNode(nodeList[currentNode]);
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, currentTargetPos, Time.deltaTime);

        elapsedTime += Time.deltaTime;
    }
}
