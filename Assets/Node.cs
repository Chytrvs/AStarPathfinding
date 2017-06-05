using System.Collections;
using UnityEngine;

public class Node  {

    public bool IsWalkable;
    public Vector3 Position;
    public float GCost;
    public float FCost { get { return GCost + HCost; } }
    public float HCost;
    public Node Parent;

}
