using UnityEngine;

public class GridPoint : MonoBehaviour
{
    /// <summary>
    /// The grid position of this point.
    /// </summary>
    public Vector2 GridPosition { get; private set; }

    /// <summary>
    /// The current state of this grid point.
    /// </summary>
    public GameConstants.GridPointState CurrState;

    /// <summary>
    /// The current item at this grid point.
    /// </summary>
    public Item CurrItem { get; private set; }

    private Item itemPrefab;

    /// <summary>
    /// Initializes the grid point with its position and initial item information.
    /// </summary>
    public void SetPoint(Vector2 gridPosition, int gridInfo, Item itemPrefab)
    {
        this.GridPosition = gridPosition;
        this.itemPrefab = itemPrefab;

        var itemModel = FileManager.Instance.GetModel(gridInfo);
        CreateItem(itemModel);
    }

    /// <summary>
    /// Creates an item at this grid point.
    /// </summary>
    public void CreateItem(LEItemModel itemModel)
    {
        CurrItem = Instantiate(itemPrefab, this.transform);
        CurrItem.transform.localPosition = Vector3.zero;
        CurrItem.SetItem(itemModel);

        if(itemModel.itemType == LEItemType.Color || itemModel.itemType == LEItemType.Breakable)
            CurrState = GameConstants.GridPointState.Full;
        else if(itemModel.itemType == LEItemType.Obstacle)
            CurrState = GameConstants.GridPointState.Obstacle;
    }
}
