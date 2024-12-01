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


    //기본 공격
    public float basicDamage;
    public float bulletSpeed;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Transform bulletPos;
    [SerializeField] private ParticleSystem bulletParticleA;
    [SerializeField] private ParticleSystem bulletParticleB;
    [SerializeField] private GameObject guuA;
    [SerializeField] private GameObject gunB;
    [SerializeField] private Image basicAtk;
    [SerializeField] private GameObject bulletParent;

    //스킬 A
    [SerializeField] private GameObject skillAParticle;
    public float skillADamage;
    private Buff buff;

    //스킬 B
    public BulletSkill skillB;

    //스킬 C
    public float skillCDamage;
    [SerializeField] private ParticleSystem skillC;

    private Stack<GameObject> bulletStack = new Stack<GameObject>();
    public List<BasicBullet> bulletList = new List<BasicBullet>();

    private bool isAtkCool = true;
    private bool isAtuo = false;
    private bool isSkill = true;
    private bool isPlaySkill = true;

    [SerializeField] private AudioClip[] audioClips;
    private AudioSource audioSource;


    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        viewDetector = GetComponent<ViewDetector>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
        if(isPlaySkill)
        {
            StartCoroutine(PlaySkill());
        }

        viewDetector.FindAutoTarget();
        if(isSkill)
        {
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
            audioSource.PlayOneShot(audioClips[0]);
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

    private IEnumerator PlaySkill()
    {
        isPlaySkill = false;
        for (int i = 0; i < SkillManager.instance.slots.Length; i++)
        {
            if (SkillManager.instance.slots[i].skill != null)
            {
                viewDetector.FindTarget();
                if (viewDetector.Target != null)
                {
                    if(SkillManager.instance.slots[i].isCool)
                    {
                        SkillManager.instance.slots[i].PlaySKill();
                        yield return new WaitForSeconds(3);
                    }
                }
            }
        }
        isPlaySkill = true;
    }


    public IEnumerator SkillA(Skill skill)
    {
        animator.SetTrigger("SkillA");
        skillAParticle.SetActive(true);

        float tempDamage = basicDamage;
        float damage = (basicDamage * skillADamage);

        basicDamage += damage;

        float time = 5;

        for(int i = 0; i < BuffManager.instance.buffImage.Length;i++)
        {
            if (BuffManager.instance.buffImage[i].sprite == null)
            {
                BuffManager.instance.buffImage[i].sprite = skill.skillImage;
                BuffManager.instance.buffImage[i].gameObject.SetActive(true);
                buff = BuffManager.instance.buffImage[i].GetComponent<Buff>();
                break;
            }
        }

        while(time > 0)
        {
            time -= Time.deltaTime;
            buff.buffText.text = time.ToString("N0") + "초";
            yield return null;  
        }
        for (int i = 0; i < bulletList.Count; i++)
        {
            basicDamage = tempDamage;
        }
        buff.GetComponent<Image>().sprite = null;
        skillAParticle.SetActive(false);
        buff.gameObject.SetActive(false);
    }

    public IEnumerator SkillB()
    {
        isSkill = false;
        animator.SetTrigger("SkillB");
        float time = 1;
        Vector3 temp = guuA.transform.localEulerAngles;
        while (time > 0)
        {
            time -= Time.deltaTime;
            guuA.transform.Rotate(new Vector3(10, 0, 0));
            yield return null;
        }
        guuA.transform.localEulerAngles = temp;
        yield return new WaitForSeconds(0.6f);
        viewDetector.FindTarget();
        if (viewDetector.Target != null)
        {
            transform.LookAt(viewDetector.Target.transform);
        }
        skillB.transform.position = bulletPos.position;
        viewDetector.FindTarget();
        audioSource.PlayOneShot(audioClips[1]);
        skillB.target = viewDetector.Target;
        skillB.gameObject.SetActive(true);
        skillB.GetComponent<ParticleSystem>().Play();
        isSkill = true;
    }

    public IEnumerator SkillC()
    {
        isSkill = false;
        animator.SetTrigger("SkillC");
        yield return new WaitForSeconds(0.3f);
        bulletParticleA.Play();
        audioSource.PlayOneShot(audioClips[2]);
        yield return new WaitForSeconds(0.2f);
        bulletParticleA.Play();
        audioSource.PlayOneShot(audioClips[2]);
        yield return new WaitForSeconds(0.5f);
        gunB.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        bulletParticleB.Play();
        audioSource.PlayOneShot(audioClips[2]);
        yield return new WaitForSeconds(0.5f);
        gunB.SetActive(false);
        isSkill = true;
        viewDetector.FindAttackTarget();
        if (viewDetector.AtkTarget != null)
        {
            skillC.transform.position = viewDetector.AtkTarget.transform.position;
            skillC.Play();
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForSeconds(0.2f);
                float damage = (basicDamage / skillCDamage);
                audioSource.PlayOneShot(audioClips[3]);
                skillC.GetComponent<ViewDetector>().FindSkillTarget(damage);
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
