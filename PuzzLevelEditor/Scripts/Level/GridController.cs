using System.Collections;
using System.Collections.Generic;
using System.IO;
using Gametator;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GridPoint pointPrefab;
    [SerializeField] private Item itemPrefab;

    public Vector2 GridSize { get; private set; }
    public GridPoint[,] GridPoints { get; private set; } 

    private FileManager fileManager;
    private int[,] levelGridInfo;
    
    public void CreateGrid()
    {
        GetGridSize();
        CreatePoints();
    }

    private void GetGridSize()
    {
        fileManager = FileManager.Instance;

        GridSize = new Vector2(fileManager.CurrColumnCount, fileManager.CurrRowCount);
        levelGridInfo = fileManager.CurrGridInfo;
    }

    private void CreatePoints()
    {
        GridPoints = new GridPoint[(int)GridSize.x, (int)GridSize.y];

        int currXPos = (((int)GridSize.x - 1) / 2) * -1;
        int currYPos = (((int)GridSize.y - 1) / 2);


        for(int y=0; y < (int)GridSize.y; y++)
        {
            currXPos = (((int)GridSize.x - 1) / 2) * -1;

            for(int x=0; x < (int)GridSize.x; x++)
            {
                GridPoint newPoint = Instantiate(pointPrefab, this.transform);
                newPoint.transform.localPosition = new Vector3(currXPos * GameConstants.GRID_POINT_DISTANCE_OFFSET, currYPos * GameConstants.GRID_POINT_DISTANCE_OFFSET, 0f);
                newPoint.transform.name = "Point_" + x + "_" + y;
                newPoint.SetPoint(new Vector2(x, y), levelGridInfo[x, y], itemPrefab);

                GridPoints[x, y] = newPoint;
                currXPos++;
            }

            currYPos--;
        }
    }
}
