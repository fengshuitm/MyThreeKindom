using UnityEngine;
using System.Collections;
public class TestCode : MonoBehaviour
{
    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }
    public ArrayList pathArray;
    GameObject objStartCube, objEndCube;
    private float elapsedTime = 0.0f;
    //Interval time between pathfinding  
    public float intervalTime = 1.0f;

    void Start()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Player");
        objEndCube = GameObject.FindGameObjectWithTag("Enemy");
        pathArray = new ArrayList();
        FindPath();
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }
    }

    void FindPath()
    {
     /*   startPos = objStartCube.transform;
        endPos = objEndCube.transform;
        startNode = new Node(GridManager.getInstance().GetGridCellCenter(
                GridManager.getInstance().GetGridIndex(startPos.position)));
        goalNode = new Node(GridManager.getInstance().GetGridCellCenter(
                GridManager.getInstance().GetGridIndex(endPos.position)));
        pathArray = AStar_FS.FindPath(startNode, goalNode);
        */
    }

    void OnDrawGizmos()
    {
        if (pathArray == null)
            return;
        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];

                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(node.position, nextNode.position);
                    index++;
                }
            }
        }
    }

}