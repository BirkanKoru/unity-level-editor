using System.Linq;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [SerializeField] private GridPoint pointPrefab; // Prefab for grid points
    [SerializeField] private Item itemPrefab;       // Prefab for items

    public Vector2 GridSize { get; private set; }   // Dimensions of the grid
    public GridPoint[,] GridPoints { get; private set; } // 2D array holding all grid points

    private FileManager fileManager;  // Reference to FileManager for grid setup
    private int[,] levelGridInfo;     // Grid data defining the initial state
    
    /// <summary>
    /// Initializes the grid.
    /// </summary>
    public void CreateGrid()
    {
        SetupGridSize();
        InitializeGridPoints();
    }

    /// <summary>
    /// Configures the grid size based on the FileManager data.
    /// </summary>
    private void SetupGridSize()
    {
        fileManager = FileManager.Instance;

        if (fileManager == null)
        {
            Debug.LogError("FileManager instance is null. Cannot set up grid.");
            return;
        }

        GridSize = new Vector2(fileManager.CurrColumnCount, fileManager.CurrRowCount);
        levelGridInfo = fileManager.CurrGridInfo;
    }

    /// <summary>
    /// Instantiates the grid points and positions them in the scene.
    /// </summary>
    private void InitializeGridPoints()
    {
        GridPoints = new GridPoint[(int)GridSize.x, (int)GridSize.y];

        for(int y=0; y < (int)GridSize.y; y++)
        {
            for(int x=0; x < (int)GridSize.x; x++)
            {
                GridPoint newPoint = Instantiate(pointPrefab, this.transform);
                newPoint.transform.localPosition = new Vector3(x * GameConstants.GRID_POINT_DISTANCE_OFFSET, y * GameConstants.GRID_POINT_DISTANCE_OFFSET, 0f);
                newPoint.transform.name = "Point_" + x + "_" + y;
                newPoint.SetPoint(new Vector2(x, y), levelGridInfo[x, y], itemPrefab);

                GridPoints[x, y] = newPoint;
            }
        }

        // Center the grid in the scene
        this.transform.localPosition = new Vector3(((GridSize.x - 1f) / 2f) * -1f, ((GridSize.y - 1f) / 2f) * -1f );
    }

    /// <summary>
    /// Retrieves a grid point at a specific (x, y) position, ensuring it's within bounds.
    /// </summary>
    public GridPoint GetPoint(int x, int y)
    {
        if(x >= 0 && x < (int)GridSize.x && y >= 0 && y < (int)GridSize.y)
        {
            return GridPoints[x, y];
        }

        return null;
    }
}