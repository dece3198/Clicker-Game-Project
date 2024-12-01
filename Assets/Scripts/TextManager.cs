using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] private GameObject damageText;
    private Stack<GameObject> textStack = new Stack<GameObject>();
    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject text = Instantiate(damageText, transform);
            textStack.Push(text);
        }
    }
    
    public void ExitPool(float damage)
    {
        float rnadX = Random.Range(-(rect.rect.width / 2), rect.rect.width / 2);
        Vector2 pos = new Vector2(rnadX, 0);

        GameObject text = textStack.Pop();

        text.GetComponent<RectTransform>().localPosition = pos;
        text.GetComponent<TextMeshProUGUI>().text = damage.ToString("N0");
        text.SetActive(true);
    }

    public void EnterPool(GameObject text)
    {
        textStack.Push(text);
        text.SetActive(false);
    }
}
