using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : MonoBehaviour {
	//the A* grid to pathfind on
	public AStarGrid grid;

	//for debugging, draws the last path calculated
	public bool drawPath;

	//for use with drawPath
	private List<AStarNode> draw = new List<AStarNode>();

	public List<AStarNode> FindPath(Vector3 sPos, Vector3 ePos) {
        //get the nodes to travel between
        AStarNode startNode = grid.NodeFromWorldPoint(sPos);
        AStarNode endNode = grid.NodeFromWorldPoint(ePos);

		//openList is the neighbors remaining
		//closedList is the nodes visited
		List<AStarNode> openList = new List<AStarNode>();
		HashSet<AStarNode> closedList = new HashSet<AStarNode>();

		openList.Add(startNode);

		while (openList.Count > 0) {
			//get the last element of the openList
			AStarNode currentNode = openList[openList.Count - 1];

			//update our lists
			openList.RemoveAt(openList.Count - 1);
			closedList.Add(currentNode);

			//if we're done, return a path
			if (currentNode == endNode) {
				return MakePath(startNode, endNode);
			}

			//list to hold neighbors
			List<AStarNode> toMerge = new List<AStarNode>();
			foreach (AStarNode neighbor in grid.GetNeighboringNodes(currentNode)) {
				//skip walls and already visited nodes
				if (neighbor.isSolid || closedList.Contains(neighbor)) {
					continue;
				}
				//otherwise, update costs and parent node (to build path)
				if (!openList.Contains(neighbor)) {
					neighbor.parent = currentNode;
					neighbor.gCost = currentNode.gCost + getManDistance(neighbor, currentNode);
					neighbor.hCost = getManDistance(neighbor, endNode);
					toMerge.Add(neighbor);
				}
			}

			//sort the neighbors
			toMerge.Sort((x, y) => y.fCost - x.fCost);

		  //Do the merge part of mergesort on the sorted new neighbors and the existing list, basically implements a priority queue where the next element is the end of the list
			openList = mergeLists(openList, toMerge);
		}

		return new List<AStarNode>();
	}

	public List<AStarNode> MakePath(AStarNode start, AStarNode end) {
		List<AStarNode> path = new List<AStarNode>();
        AStarNode current = end;

		//iterate across the elements adding them and their then parents, etc.
		while (current != start) {
			path.Add(current);
			current = current.parent;
		}

		//and flip it so the next node in the path is at [0]
		path.Reverse();

		//if debugging, copy the path into draw
		if (drawPath) {
			draw = new List<AStarNode>(path);
		}

		return path;
	}

	public int getManDistance(AStarNode start, AStarNode end) {
		//return Manhattan Distance between nodes
		return Mathf.Abs(start.posX - end.posX) + Mathf.Abs(start.posY - end.posY);
	}

	public List<AStarNode> mergeLists(List<AStarNode> list1, List<AStarNode> list2) {
		//performs a merge from mergesort on two node lists, preferring list2
		List<AStarNode> result = new List<AStarNode>();
		int i = 0, j = 0;

		while (i < list1.Count || j < list2.Count) {
			if (i >= list1.Count) {
				result.Add(list2[j]);
				j++;
				continue;
			}
			if (j >= list2.Count) {
				result.Add(list1[i]);
				i++;
				continue;
			}

			if (list1[i].fCost > list2[j].fCost) {
				result.Add(list1[i]);
				i++;
			} else {
				result.Add(list2[j]);
				j++;
			}
		}

		return result;
	}

	public Vector3 WorldPointFromNode(AStarNode node) {
		//for use by entities using the pathfinding
		return grid.WorldPointFromNode(node);
	}

	private void OnDrawGizmos() {
		//Draw the path in red if debugging
		if (drawPath) {
			Gizmos.color = Color.red;
			if (draw.Count > 0) {
				foreach (AStarNode n in draw) {
					Gizmos.DrawWireCube(WorldPointFromNode(n), new Vector3(1, 1, 1) * grid.resolution);
				}
			}
		}
	}
}
