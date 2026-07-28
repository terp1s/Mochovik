using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridMaker : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private SardinePuzzleManager manager;
    private GridTile[,] _grid;

    

    public int width;
    public int height;

    private void Awake()
    {

    }
    public GridTile[,] GetGrid()
    {
        if (_grid == null)
        {
            CreateGrid();
        }
        return _grid;
    }


    void CreateGrid()
    {
        _grid = new GridTile[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                GameObject go = Instantiate(tilePrefab, transform);
                GridTile gt = go.GetComponent<GridTile>();
                gt.manager = manager;
                gt.coordinates = new Vector2Int(x, y);
                _grid[x, y] = gt;
            }
        }
    }

}