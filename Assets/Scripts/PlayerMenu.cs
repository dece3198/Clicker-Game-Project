using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    [SerializeField] private GameObject characterUi;
    [SerializeField] private GameObject selectImage;
    [SerializeField] private GameObject upGradeUi;
    [SerializeField] private GameObject inventoryUi;
    [SerializeField] private GameObject curUi;
    [SerializeField] private TextMeshProUGUI goldText;

    private void Update()
    {
        goldText.text = GameManager.instance.gold.ToString();
    }

    public void XButton()
    {
        characterUi.SetActive(false);
    }

    public void CharacterButton()
    {
        characterUi.SetActive(true);
    }

    public void UpGradeUI()
    {
        if(curUi != null)
        {
            curUi.SetActive(false);
        }
        curUi = upGradeUi;
        selectImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 550);
        upGradeUi.SetActive(true);
    }

    public void Inventory()
    {
        if (curUi != null)
        {
            curUi.SetActive(false);
        }
        curUi = inventoryUi;
        selectImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 400);
        curUi.SetActive(true);
    }
}
