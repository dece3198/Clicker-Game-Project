using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{

    private PlayerController playerController;
    private ViewDetector viewDetector;
    private Animator animator;
    [SerializeField] private Animator autoAnimator;

    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Transform bulletPos;

    [SerializeField] private ParticleSystem bulletParticleA;
    [SerializeField] private ParticleSystem bulletParticleB;

    [SerializeField] private GameObject bulletParent;
    [SerializeField] private GameObject guuA;
    [SerializeField] private GameObject gunB;
    public BulletSkill skillA;
    [SerializeField] private ParticleSystem skillB;
    public float skillBDamage;

    [SerializeField] private Image basicAtk;

    private bool isAtkCool = true;
    private bool isAtuo = false;

    private Stack<GameObject> bulletStack = new Stack<GameObject>();
    public List<BasicBullet> bulletList = new List<BasicBullet>();

    public Dictionary<SkillType, IEnumerator> skillDic = new Dictionary<SkillType, IEnumerator>();

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        viewDetector = GetComponent<ViewDetector>();
        animator = GetComponent<Animator>();

        skillDic.Add(SkillType.SKillA, SkillA());
        skillDic.Add(SkillType.SkillB, SkillB());
        skillDic.Add(SkillType.SkillC, SkillC());
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject _bullet = Instantiate(bullet, bulletParent.transform);
            GameObject _explosion = Instantiate(explosion, bulletParent.transform);
            _bullet.GetComponent<BasicBullet>().explosion = _explosion;
            bulletStack.Push(_bullet);
            bulletList.Add(_bullet.GetComponent<BasicBullet>());
        }
    }

    private void Update()
    {
        if(isAtuo)
        {
            Auto();
        }
    }

    private void Auto()
    {
        viewDetector.FindAutoTarget();
        if (viewDetector.AutoTarget != null)
        {
            if (viewDetector.Target == null)
            {
                playerController.isMove = true;
                transform.position = Vector3.MoveTowards(transform.position, viewDetector.AutoTarget.transform.position, playerController.moveSpeed * Time.deltaTime);
                transform.LookAt(viewDetector.AutoTarget.transform);
                viewDetector.FindTarget();
            }
            else
            {
                playerController.isMove = false;
                ClickAtkButton();
            }

        }
    }

    public void ClickAtkButton()
    {
        if (isAtkCool)
        {
            viewDetector.FindTarget();
            if (viewDetector.Target != null)
            {
                StartCoroutine(AtkCo());
                animator.SetTrigger("AttackA");
                transform.LookAt(viewDetector.Target.transform);
            }
        }
    }

    public void BasicAttack()
    {
        viewDetector.FindTarget();
        if (viewDetector.Target != null)
        {
            bulletParticleA.Play();
            GameObject _bullet = bulletStack.Pop();
            _bullet.GetComponent<BasicBullet>().target = viewDetector.Target;
            _bullet.transform.position = bulletPos.position;
            _bullet.gameObject.SetActive(true);
        }
    }

    public void EnterBullet(GameObject _bullet)
    {
        bulletStack.Push(_bullet);
    }

    public void AutoButton()
    {
        isAtuo = !isAtuo;
        autoAnimator.SetBool("Auto", isAtuo);

    }

    private IEnumerator SkillA()
    {
        animator.SetTrigger("SkillA");
        yield return new WaitForSeconds(10f);
    }

    private IEnumerator SkillB()
    {
        animator.SetTrigger("SkillB");
        float time = 1;
        Vector3 temp = guuA.transform.localEulerAngles;
        while (time > 0)
        {
            time -= Time.deltaTime;
            guuA.transform.Rotate(new Vector3(10, 0, 0));
            yield return null;
        }
        viewDetector.FindTarget();
        guuA.transform.localEulerAngles = temp;
        if (viewDetector.Target != null)
        {
            transform.LookAt(viewDetector.Target.transform);
        }
        yield return new WaitForSeconds(0.6f);
        skillA.transform.position = bulletPos.position;
        viewDetector.FindTarget();
        skillA.target = viewDetector.Target;
        skillA.gameObject.SetActive(true);
        skillA.GetComponent<ParticleSystem>().Play();
    }

    private IEnumerator SkillC()
    {
        animator.SetTrigger("SkillC");
        yield return new WaitForSeconds(0.3f);
        bulletParticleA.Play();
        yield return new WaitForSeconds(0.2f);
        bulletParticleA.Play();
        yield return new WaitForSeconds(0.5f);
        gunB.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        bulletParticleB.Play();
        yield return new WaitForSeconds(0.5f);
        gunB.SetActive(false);
        viewDetector.FindAttackTarget();
        if (viewDetector.AtkTarget != null)
        {
            skillB.transform.position = viewDetector.AtkTarget.transform.position;
            skillB.Play();
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.2f);
                skillB.GetComponent<ViewDetector>().FindSkillTarget(skillBDamage);
            }
        }
    }

    private IEnumerator AtkCo()
    {
        isAtkCool = false;
        float time = 1;
        basicAtk.gameObject.SetActive(true);
        while(time > 0)
        {
            time -= Time.deltaTime;
            basicAtk.fillAmount = time;
            yield return null;
        }
        basicAtk.gameObject.SetActive(false);
        isAtkCool = true;
    }
}
