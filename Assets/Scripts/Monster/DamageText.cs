using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float alphaSpeed;
    private TextMeshProUGUI text;
    Color alpha;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        alpha = text.color;
    }

    private void OnEnable()
    {
        alpha.a = 1;
        text.color = alpha;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        alpha.a = Mathf.Lerp(alpha.a,0, Time.deltaTime * alphaSpeed);
        text.color = alpha;

        if(alpha.a <= 0.3)
        {
            transform.parent.GetComponent<TextManager>().EnterPool(gameObject);
        }
    }
}
