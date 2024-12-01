using System.Collections;
using UnityEngine;

public class BulletSkill : MonoBehaviour
{
    public GameObject target;
    public GameObject explosion;
    private Rigidbody rigid;
    private ViewDetector viewDetector;
    [SerializeField] private float speed;
    [SerializeField] private float power;
    public float damage;
    private IEnumerator bulletCo;
    [SerializeField]private int count = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        viewDetector = GetComponent<ViewDetector>();    
    }

    private void OnEnable()
    {
        if(target != null)
        {
            transform.LookAt(target.GetComponent<Monster>().headPos.position);
        }
        gameObject.GetComponent<ParticleSystem>().Play();
        bulletCo = BulletCo();
        StartCoroutine(bulletCo);
    }

    private void Update()
    {
        rigid.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == target)
        {
            if(other.GetComponent<IInteractable>() != null)
            {

                float skillDamage = (GameManager.instance.playerSkill.basicDamage * damage);
                other.GetComponent<IInteractable>().TakeHit(skillDamage);
                other.GetComponent<Monster>().rigid.AddForce((transform.forward + transform.up) * power, ForceMode.Impulse);
                explosion.SetActive(true);
                explosion.transform.position = transform.position;
                explosion.GetComponent<ParticleSystem>().Play();
                StopCoroutine(bulletCo);
                count++;

                viewDetector.FindSkillBTarget(target);
                if (viewDetector.Target != null)
                {
                    target = viewDetector.Target;
                    gameObject.SetActive(false);
                }
                else
                {
                    target = null;
                    gameObject.SetActive(false);
                    count = 0;
                }

                if (count >= 3)
                {
                    target = null;
                    gameObject.SetActive(false);
                    count = 0;
                }
                else
                {
                    gameObject.SetActive(true);
                }
            }
        }
    }


    private IEnumerator BulletCo()
    {
        yield return new WaitForSeconds(3f);
        target = null;
        gameObject.SetActive(false);
    }
}
