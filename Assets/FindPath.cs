using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPath : MonoBehaviour {
    public bool UseEuclideanDistance;
    public float heuristic;
    public List<Node> GetPath(Node Start,Node Target)
    {
        List<Node> Closed = new List<Node>();
        List<Node> Open = new List<Node>();
        Open.Add(Start);
        while (Open.Count > 0)
        {
            Node CurrentNode = Open[0];
            foreach(Node node in Open)
            {
                if (node.FCost < CurrentNode.FCost) //Finding node with lowest cost
                {
                    CurrentNode = node;
                }
            }
            Open.Remove(CurrentNode); //Switching current node to closed list
            Closed.Add(CurrentNode);
            List<Node> AdjacentNodes = GetComponent<GridController>().FindAdjacentNodes(CurrentNode); //Finding nodes adjacent to current node
            foreach(Node AdjacentNode in AdjacentNodes)
            {
                if (!AdjacentNode.IsWalkable || Closed.Contains(AdjacentNode)) //Ignore if its obstacle or already evaluated node
                {
                    continue;
                }
                else if (!Open.Contains(AdjacentNode)) //Calculating node costs and setting its parent to currentNode
                {
                    Open.Add(AdjacentNode);
                    AdjacentNode.Parent = CurrentNode;
                    AdjacentNode.GCost = AdjacentNode.Parent.GCost + CalculateDistance(AdjacentNode, AdjacentNode.Parent);
                    AdjacentNode.HCost = CalculateDistance(AdjacentNode, Target);
                }
                else
                {
                    if (AdjacentNode.GCost > CurrentNode.GCost + CalculateDistance(AdjacentNode, CurrentNode)) //Check if path through currentNode is better than its current path
                    {
                        AdjacentNode.Parent = CurrentNode;
                        AdjacentNode.GCost = CurrentNode.GCost + CalculateDistance(AdjacentNode, CurrentNode);
                    }
                }
                

            }
            if (CurrentNode == Target) //Create path by going back through parents
            {
                List<Node> FinalPath = new List<Node>();
                Node Step = Target;
                while (Step != Start)
                {
                    FinalPath.Add(Step);
                    Step = Step.Parent;
                }
                FinalPath.Reverse();
                return FinalPath;
            }
        }
        Debug.Log("NotFound");
        return null; //path not found
    }
    float CalculateDistance(Node Start, Node End)
    {
        if(UseEuclideanDistance)
        return heuristic*Mathf.Pow((Start.Position.x - End.Position.z), 2) + Mathf.Pow((Start.Position.z - End.Position.z), 2);
        else
        {
        float dx = Mathf.Abs(Start.Position.x - End.Position.x);
        float dy = Mathf.Abs(Start.Position.z - End.Position.z);
        return heuristic* (dx + dy);
        }

    }
}
