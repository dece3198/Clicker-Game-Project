using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBullet : MonoBehaviour
{
    public GameObject target;
    public GameObject explosion;
    private Rigidbody rigid;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float power;
    private IEnumerator bulletCo;
    

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
        bulletCo = BulletCo();
        StartCoroutine(bulletCo);
    }

    private void Update()
    {
        rigid.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IInteractable>() != null)
        {
            other.GetComponent<IInteractable>().TakeHit(damage);
            other.GetComponent<Monster>().rigid.AddForce((transform.forward + transform.up) * power, ForceMode.Impulse);
            explosion.SetActive(true);
            explosion.transform.position = transform.position;
            explosion.GetComponent<ParticleSystem>().Play();
            StopCoroutine(bulletCo);
            gameObject.SetActive(false);
            GameManager.instance.player.EnterBullet(gameObject);
        }
    }

    private IEnumerator BulletCo()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
        GameManager.instance.player.EnterBullet(gameObject);
    }
}
 