using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the base functionality for level editor item models.
/// </summary>
public class LEBase : MonoBehaviour
{
     [Header("Icons")]
    public Sprite itemAddIcon = null;
    public Sprite itemRemoveIcon = null;

    [Space(30)]
    [SerializeField] private List<LEItemModel> itemModels;
    public List<LEItemModel> ItemModels { get { return itemModels; } set { itemModels = value; }}

    /// <summary>
    /// Adds a default eraser model to the item list.
    /// </summary>
    public void AddEraser()
    {
        LEItemModel eraser = new LEItemModel(ID: 0, itemType: LEItemType.Color, itemName: "Eraser");
        itemModels.Add(eraser);
    }

    /// <summary>
    /// Adds a new item model with default settings to the item list.
    /// </summary>
    public LEItemModel AddNewItemModel()
    {
        LEItemModel newItemModel = new LEItemModel
        (
            ID: itemModels.Count, 
            itemIcons: new List<Sprite>(), 
            itemHealth: 1
        );

        newItemModel.itemIcons.Add(null); // Ensure at least one placeholder icon
        itemModels.Add(newItemModel);

        return newItemModel;
    }

    /// <summary>
    /// Updates an existing item model based on its ID.
    /// </summary>
    public void UpdateModel(LEItemModel updatedModel)
    {
        for(int i=0; i < itemModels.Count; i++)
        {
            if(itemModels[i].ID == updatedModel.ID)
            {
                itemModels[i] = updatedModel;
            }
        }
    }

    /// <summary>
    /// Removes an item model from the list by its ID.
    /// </summary>
    public void RemoveModel(LEItemModel removedModel)
    {
        for(int i=0; i < itemModels.Count; i++)
        {
            if(itemModels[i].ID == removedModel.ID)
            {
                itemModels.RemoveAt(i);
                break;
            }
        }
    }

    /// <summary>
    /// Retrieves the last item model in the list.
    /// </summary>
    public LEItemModel GetLastItemModel()
    {
        if(itemModels.Count > 0)
            return itemModels[itemModels.Count - 1];
        else
            return null;
    }

     /// <summary>
    /// Retrieves a random item model based on the given neighbor points.
    /// </summary>
    public LEItemModel GetRandomItem(GridPoint[] neighbourPoints)
    {
        //Simple Algorithm - You can change the logic
        List<int> ColorList = new List<int>();

        for(int i=1; i < itemModels.Count; i++)
        {
            if(itemModels[i].itemType == LEItemType.Color)
                ColorList.Add(itemModels[i].ID);
        }

        //Add Neighbour IDs into ColorList
        for(int i=0; i < neighbourPoints.Length; i++)
        {
            if(neighbourPoints[i].CurrItem != null && neighbourPoints[i].CurrItem.CurrItemModel.itemType == LEItemType.Color)
                ColorList.Add(neighbourPoints[i].CurrItem.CurrItemModel.ID);
        }

        if(ColorList.Count > 0)
        {
            int selectedID = ColorList[Random.Range(0, ColorList.Count)];
            return FindTheModelByID(selectedID);
        }   
        
        return null;
    }

    /// <summary>
    /// Finds an item model by its ID.
    /// </summary>
    private LEItemModel FindTheModelByID(int ID)
    {
        for(int i=1; i < itemModels.Count; i++)
        {
            if(itemModels[i].ID == ID)
            {
                return itemModels[i];
            }
        }

        return itemModels[1];
    }
}