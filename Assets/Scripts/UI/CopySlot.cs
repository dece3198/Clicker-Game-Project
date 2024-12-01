using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CopySlot : MonoBehaviour
{
    public Item item;
    [SerializeField] private Image itemImage;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
    }

    public void AddItem(Item _item)
    {
        item = _item;
        itemImage.sprite = item.itemImage;
    }

    public void Clear()
    {
        item = null;
        itemImage.sprite = null;
        gameObject.SetActive(false);
    }
}


