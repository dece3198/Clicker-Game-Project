using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public GameObject target;
    private Rigidbody rigid;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float power;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if(target != null)
        {
            transform.LookAt(target.GetComponent<Monster>().headPos.position);
        }
        StartCoroutine(BulletCo());
    }

    private void Update()
    {
        rigid.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IInteractable>() != null)
        {
            other.GetComponent<IInteractable>().TakeHit(damage);
            other.GetComponent<Monster>().rigid.AddForce((transform.forward + transform.up) * power, ForceMode.Impulse);
        }
    }

    private IEnumerator BulletCo()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
        GameManager.instance.player.EnterBullet(gameObject);
    }
}
 