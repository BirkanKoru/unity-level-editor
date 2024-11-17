using System.Collections;
using System.Collections.Generic;
using Gametator;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    public Vector2 GridPosition { get; private set; }

    public Item CurrItem { get; private set; }

    private Item itemPrefab;

    public void SetPoint(Vector2 GridPosition, int GridInfo, Item itemPrefab)
    {
        this.GridPosition = GridPosition;
        this.itemPrefab = itemPrefab;

        CreateItem(FileManager.Instance.GetModel(GridInfo));
    }

    private void CreateItem(LEItemModel itemModel)
    {
        CurrItem = Instantiate(itemPrefab, this.transform);
        CurrItem.transform.localPosition = Vector3.zero;

        CurrItem.SetItem(itemModel);
    }
}
