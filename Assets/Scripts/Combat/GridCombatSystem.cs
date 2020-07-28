using CodeMonkey.Utils;
using GridPathfindingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MovementTilemap;

public class GridCombatSystem : MonoBehaviour
{
    [SerializeField] private UnitGridCombat unitGridCombat;


    // Start is called before the first frame update
    void Start()
    {
        int maxMoveDistance = 5;
        GameHandler gameHandler = GameHandler.Instance;
        GridPathfinding gridPathfinding = gameHandler.gridPathfinding;
        Grid<EmptyGridObject> grid = gameHandler.GetGrid();
        grid.GetXY(unitGridCombat.GetPosition(), out int unitX, out int unitY);

        gameHandler.GetMovementTilemap().SetAllTilemapSprite(TilemapObject.TilemapSprite.None);

        for (int x = unitX - maxMoveDistance; x < unitX + maxMoveDistance; x++)
        {
            for (int y = unitY - maxMoveDistance; y < unitY + maxMoveDistance; y++)
            {
                if(gridPathfinding.IsWalkable(x, y))
                {
                    if(gridPathfinding.HasPath(unitX, unitY, x, y))
                    {
                        if(gridPathfinding.GetPath(unitX, unitY, x, y).Count <= maxMoveDistance)
                        {
                            gameHandler.GetMovementTilemap().SetTilemapSprite(x, y, TilemapObject.TilemapSprite.Move);
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = UtilsClass.GetMouseWorldPosition();
            unitGridCombat.MoveTo(pos, ()=> { });
            Start();
        }
    }
}
