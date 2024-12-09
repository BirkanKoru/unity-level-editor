using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private SpriteRenderer skin;

    public LEItemModel itemModel { get; private set; }

    public void SetItem(LEItemModel itemModel)
    {
        this.itemModel = itemModel;
        PaintTheSkin();
    }

    private void PaintTheSkin()
    {
        skin.sprite = itemModel.itemIcon;
    }
}
