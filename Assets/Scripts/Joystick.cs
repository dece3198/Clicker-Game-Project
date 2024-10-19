using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform backGround;
    [SerializeField] private RectTransform joyStcik;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dragImage;
    public float moveSpeed;

    private float radius;
    private bool isTouch = false;
    private Vector3 movePosition;

    private void Start()
    {
        radius = backGround.rect.width * 0.25f;
    }

    private void Update()
    {
        if (isTouch)
        {
            player.transform.position += movePosition;
            player.GetComponent<PlayerController>().isMove = movePosition.magnitude > 0;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)backGround.position;
        value = Vector2.ClampMagnitude(value, radius);
        joyStcik.localPosition = value;

        float distance = Vector2.Distance(backGround.position, joyStcik.position) / radius;
        value = value.normalized;
        movePosition = new Vector3(value.x * moveSpeed * distance * Time.deltaTime, 0, value.y * moveSpeed  * distance * Time.deltaTime);
        if(value.magnitude > 0)
        {
            player.transform.rotation = Quaternion.Euler(0f, Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg, 0f);
        }
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        joyStcik.localPosition = Vector3.zero;
        movePosition = Vector3.zero;
        player.GetComponent<PlayerController>().isMove = movePosition.magnitude > 0;
    }
}
