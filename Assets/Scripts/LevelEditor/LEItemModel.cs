using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents an item model in the level editor, including its type, name, icons, and health.
/// </summary>
[Serializable]
public class LEItemModel
{
    /// <summary>
    /// Unique identifier for the item.
    /// </summary>
    public int ID = -1;

    /// <summary>
    /// Type of the item (e.g., Color, Obstacle, etc.).
    /// </summary>
    public LEItemType itemType;

    /// <summary>
    /// Name of the item.
    /// </summary>
    public string itemName;

    /// <summary>
    /// List of item icons associated with this model.
    /// </summary>
    public List<Sprite> itemIcons;

     /// <summary>
    /// Health of the item, representing how many hits it can take.
    /// </summary>
    public int itemHealth = 1;

    /// <summary>
    /// Constructor for initializing an item model.
    /// </summary>
    public LEItemModel(int ID = -1, LEItemType itemType = LEItemType.Color, string itemName = "ItemName", Sprite itemIcon = null, List<Sprite> itemIcons = null, int itemHealth = 1)
    {
        this.ID = ID;
        this.itemType = itemType;
        this.itemName = itemName;
        this.itemIcons = itemIcons;
        this.itemHealth = itemHealth;
    }
}

/// <summary>
/// Enum representing the various types of level editor items.
/// </summary>
public enum LEItemType
{
    Color,
    Obstacle,
    Breakable,
    Blank
}