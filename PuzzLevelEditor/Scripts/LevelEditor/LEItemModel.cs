using System;
using UnityEngine;


[Serializable]
public class LEItemModel
{
    public int ID = -1;
    public LEItemType itemType;
    public string itemName;
    public Sprite itemIcon;

    public LEItemModel(int ID = -1, LEItemType itemType = LEItemType.Color, string itemName = "ItemName", Sprite itemIcon = null)
    {
        this.ID = ID;
        this.itemType = itemType;
        this.itemName = itemName;
        this.itemIcon = itemIcon;
    }
}

public enum LEItemType
{
    Color,
    Obstacle,
    Blank
}