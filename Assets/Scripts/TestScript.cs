using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class TestScript : MonoBehaviour
{
    private Grid<HeatMapGridObject> grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<HeatMapGridObject>(4, 2, 5f, Vector3.zero, (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = UtilsClass.GetMouseWorldPosition();
            HeatMapGridObject obj = grid.GetGridObject(pos);
            if (obj != null)
                obj.AddValue(5);
        }

        if (Input.GetMouseButtonDown(1))
        {
           Debug.Log(grid.GetGridObject(UtilsClass.GetMouseWorldPosition()));
        }
    }
}

public class HeatMapGridObject
{
    private const int MIN = 0;
    private const int MAX = 100;

    private Grid<HeatMapGridObject> grid;
    private int value;
    private int x, y;

    public HeatMapGridObject(Grid<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int value)
    {
        this.value += value;
        Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()
    {
        return (float)value / MAX;
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
