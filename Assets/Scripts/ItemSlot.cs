using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ItemSlotType
{
    ItemSlot, EquippedSlot
}

public class ItemSlot : MonoBehaviour
{
    public Item item;
    [SerializeField] private Image itemImage;
    [SerializeField] private GameObject backImage;
    public TextMeshProUGUI gradeText;
    public ItemSlotType type;
    public int grade = 0;
    public int price;
    public float[] value;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
    }

    public void AddItem(Item _item)
    {
        item = _item;
        itemImage.sprite = _item.itemImage;
        backImage.SetActive(false);
        if(gradeText != null)
        {
            gradeText.gameObject.SetActive(true);
            gradeText.text = "+ " + grade.ToString();
        }
        SetColor(1);
    }

    public void Click()
    {
        if(Information.instance.copySlot.item != null)
        {
            if (type == ItemSlotType.EquippedSlot)
            {
                if (item != null)
                {
                    InventoryManager.instance.AcquireItem(item);
                    ClearSlot();
                }
                AddItem(Information.instance.copySlot.item);
                Information.instance.EquippeItem();
                Information.instance.copySlot.Clear();
            }
        }
        else
        {
            if(item != null)
            {
                Information.instance.AddItem(item, this);
            }
        }
    }


    public void ClearSlot()
    {
        item = null;
        itemImage.sprite = null;
        backImage.SetActive(true);
        if(type == ItemSlotType.ItemSlot)
        {
            SetColor(0);
            gameObject.SetActive(false);
        }
        if(gradeText != null)
        {
            gradeText.gameObject.SetActive(false);
        }
    }

    private void SetColor(float alpha)
    {
        Color color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }
}
