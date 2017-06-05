using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour {
   public Node[,] Grid;
    public int GridSize;
    public float OverlapRadius;
    public LayerMask ObstacleLayerMask;
    public Transform Target, Player;
    List<Node> path;
    // Use this for initialization
    void Start () {
        GenerateNodes();
    }
	
	// Update is called once per frame
	void Update () {
        GenerateNodes();
        path = GetComponent<FindPath>().GetPath(FindNode(Player.position), FindNode(Target.position));
    }
    void GenerateNodes()
    {
        Grid = new Node[GridSize, GridSize];
        for (int x = 0; x < GridSize; x++)
        {
            for (int y = 0; y < GridSize; y++)
            {
                Grid[x, y] = new Node();
                Grid[x, y].Position = new Vector3(x,0, y);
                Grid[x, y].IsWalkable = !Physics.CheckSphere(Grid[x, y].Position, OverlapRadius, ObstacleLayerMask);
            }
        }
    }
    void OnDrawGizmos()
    {
        if (path != null)
        {
            foreach (Node p in path)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(p.Position, new Vector3(1, 0.1f, 1));
            }
        }
        

    }
   public List<Node> FindAdjacentNodes(Node TargetNode)
    {
        List<Node> AdjacentNodes = new List<Node>();
        for (int x = -1; x <=1; x++)
        {
            for (int y = -1; y <=1; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                if(TargetNode.Position.x+x>=0&& TargetNode.Position.x+x<GridSize&& TargetNode.Position.z + y >= 0 && TargetNode.Position.z + y < GridSize)
                {
                    AdjacentNodes.Add(Grid[((int)TargetNode.Position.x + x), (int)TargetNode.Position.z + y]);
                }
            }
        }
        return AdjacentNodes;
    }
    public Node FindNodeTarget()
    {
        int x = Mathf.RoundToInt(Target.position.x);
        int y = Mathf.RoundToInt(Target.position.z);
        return Grid[x, y];
    }
    public Node FindNodePlayer()
    {
        int x = Mathf.RoundToInt(Player.position.x);
        int y = Mathf.RoundToInt(Player.position.z);
        return Grid[x, y];
    }
    public Node FindNode(Vector3 Position)
    {
        int x = Mathf.RoundToInt(Position.x);
        int y = Mathf.RoundToInt(Position.z);
        return Grid[x, y];
    }
}
