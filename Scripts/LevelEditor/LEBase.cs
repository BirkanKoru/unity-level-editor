using System.Collections.Generic;
using UnityEngine;

public class LEBase : MonoBehaviour
{
    public Sprite itemAddIcon = null;
    public Sprite itemRemoveIcon = null;

    [Space(30)]
    [SerializeField] private List<LEItemModel> itemModels;
    public List<LEItemModel> ItemModels { get { return itemModels; } set { itemModels = value; }}

    public void AddEraser()
    {
        LEItemModel eraser = new LEItemModel(ID: 0);
        eraser.itemName = "Eraser";

        itemModels.Add(eraser);
    }

    public LEItemModel AddNewItemModel()
    {
        LEItemModel newItemModel = new LEItemModel(ID: itemModels.Count);
        itemModels.Add(newItemModel);

        return newItemModel;
    }

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

    public LEItemModel GetLastItemModel()
    {
        if(itemModels.Count > 0)
            return itemModels[itemModels.Count - 1];
        else
            return null;
    }

    public int GetItemModelCount()
    {
        return itemModels.Count;
    }
}