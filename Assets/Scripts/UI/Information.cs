using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
    public static Information instance;
    public Image itemImage;
    [SerializeField] private ItemSlot slot;
    [SerializeField] private GameObject backImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI[] value;
    [SerializeField] private GameObject upButton;
    [SerializeField] private GameObject equipped;
    [SerializeField] private TextMeshProUGUI priceText;
    public CopySlot copySlot;

    private void Awake()
    {
        instance = this;
        Clear();
    }

    private void Update()
    {
        if(copySlot.item != null)
        {
            Vector2 mousePos = Input.mousePosition;
            copySlot.transform.position = mousePos;
        }
    }

    public void AddItem(Item item, ItemSlot itemSlot)
    {
        itemImage.sprite = item.itemImage;
        slot = itemSlot;
        backImage.SetActive(false);
        nameText.text = item.itemName;
        priceText.text = slot.price.ToString();

        if (slot.type == ItemSlotType.ItemSlot)
        {
            equipped.SetActive(true);
            upButton.SetActive(false);
        }
        else
        {
            equipped.SetActive(false);
            upButton.SetActive(true);
        }

        for(int i = 0;i < item.upGradeValue.Length; i++)
        {
            if (item.upGradeValue[i] > 0)
            {
                if(slot.type == ItemSlotType.EquippedSlot)
                {
                    if(slot.grade == 0)
                    {
                        slot.value[i] = item.upGradeValue[i];
                    }
                    value[i].text = slot.item.value[i] + " " + slot.value[i].ToString();
                }
                else
                {
                    value[i].text = slot.item.value[i] + " " + slot.item.upGradeValue[i].ToString();
                }
            }
        }
    }


    private void Clear()
    {
        itemImage.sprite = null;
        backImage.SetActive(true);
        nameText.text = "";
        upButton.SetActive(false);
        equipped.SetActive(false);
        for(int i = 0; i < value.Length; i++)
        {
            value[i].text = "";
        }
    }

    public void UpGradeButton()
    {
        if(GameManager.instance.gold >= slot.price)
        {
            GameManager.instance.gold -= slot.price;
            slot.grade++;
            slot.gradeText.text = "+ " + slot.grade.ToString();
            slot.price += 100 * slot.grade;
            priceText.text = slot.price.ToString();
            for(int i = 0; i < slot.item.value.Length; i++)
            {
                switch (slot.item.upType[i])
                {
                    case ItemUpType.Atk:AtkUp(i); break;
                }
            }
        }
    }

    private void AtkUp(int i)
    {
        GameManager.instance.playerSkill.basicDamage += 1;
        slot.value[i] += 1;
        value[i].text = slot.item.value[i] + " " + slot.value[i].ToString();
    }

    public void EquippeItem()
    {
        for (int i = 0; i < copySlot.item.value.Length; i++)
        {
            switch (copySlot.item.upType[i])
            {
                case ItemUpType.Atk: GameManager.instance.playerSkill.basicDamage += copySlot.item.upGradeValue[i]; break;
            }
        }
    }

    public void EquippedButton()
    {
        copySlot.gameObject.SetActive(true);
        copySlot.AddItem(slot.item);
        Clear();
        slot.ClearSlot();
    }
}
