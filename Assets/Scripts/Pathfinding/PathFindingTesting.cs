using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class PathFindingTesting : MonoBehaviour
{

    [SerializeField] private PathfindingUnit unit;
    private Pathfinding pathfinding;


    // Start is called before the first frame update
    void Start()
    {
        pathfinding = new Pathfinding(10, 10);
        unit.SetTargetPosition(Vector3.zero);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(worldPos, out int x, out int y);

            Vector3 unitPost = unit.GetPosition();
            pathfinding.GetGrid().GetXY(unitPost, out int ux, out int uy);

            List<PathNode> path =  pathfinding.FindPath(ux, uy, x, y);

            if(path != null)
            {
                for(int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10 + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 10);
                }
            }

            unit.SetTargetPosition(worldPos);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 worldPos = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(worldPos, out int x, out int y);
            pathfinding.GetNode(x, y)./*IncreaseCostMod();*/SetWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }
    }
}
