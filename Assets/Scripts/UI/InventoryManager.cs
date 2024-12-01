using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [SerializeField] private GameObject slotParent;
    public ItemSlot[] slots;
    public Item itemA;
    public Item itemB;

    private void Awake()
    {
        instance = this;
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
    }

    private void Start()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].gameObject.SetActive(false);
            }
        }

        AcquireItem(itemA);
        AcquireItem(itemB);
    }

    public void AcquireItem(Item _item)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(slots[i].item == null)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].AddItem(_item);
                return;
            }
        }
    }
}
