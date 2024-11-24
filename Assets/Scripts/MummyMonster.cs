using System.Collections;
using Unity.VisualScripting;
using UnityEngine;



public enum MummyState
{
    Idle, TakeHit, Attack, Skill, Die
}

public class MummyIdle : BaseState<MummyMonster>
{
    bool isAtkCool = true;

    public override void Enter(MummyMonster monster)
    {

    }

    public override void Exit(MummyMonster monster)
    {

    }

    public override void Update(MummyMonster monster)
    {
        monster.viewDetector.FindTarget();
        monster.viewDetector.FindAttackTarget();
        monster.transform.LookAt(monster.player.transform.position);

        if (monster.viewDetector.Target != null)
        {

            if ((monster.player.transform.position - monster.transform.position).magnitude > 1.5)
            {
                monster.animator.SetBool("Walk", true);
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, monster.player.transform.position, 3 * Time.deltaTime);
            }
            else
            {
                monster.animator.SetBool("Walk", false);
            }
        }
        else
        {
            monster.animator.SetBool("Walk", false);
        }

        if (monster.viewDetector.AtkTarget != null)
        {
            if (isAtkCool)
            {
                monster.StartCoroutine(AtkCo());
                monster.ChangeState(MummyState.Attack);
            }
        }
    }

    private IEnumerator AtkCo()
    {
        isAtkCool = false;
        yield return new WaitForSeconds(3.5f);
        isAtkCool = true;
    }
}

public class MummyTakeHit : BaseState<MummyMonster>
{
    public override void Enter(MummyMonster monster)
    {
        monster.animator.SetTrigger("TakeHit");
        monster.StartCoroutine(TakeHitCo(monster));
    }

    public override void Exit(MummyMonster monster)
    {
    }

    public override void Update(MummyMonster monster)
    {
    }

    private IEnumerator TakeHitCo(MummyMonster monster)
    {
        for(int i = 0; i < monster.meshRenderer.Length; i++)
        {
            monster.meshRenderer[i].material.color = Color.red;
        }
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < monster.meshRenderer.Length; i++)
        {
            monster.meshRenderer[i].material.color = Color.white;
        }
        if (monster.Hp > 0)
        {
            monster.ChangeState(MummyState.Idle);
        }
    }
}

public class MummyAttack : BaseState<MummyMonster>
{
    public override void Enter(MummyMonster monster)
    {
        monster.animator.SetBool("Walk", false);
        monster.animator.SetTrigger("Atk");
    }

    public override void Exit(MummyMonster monster)
    {

    }

    public override void Update(MummyMonster monster)
    {
        if(monster.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            monster.ChangeState(MummyState.Idle);
        }
    }
}

public class MummySkill : BaseState<MummyMonster>
{
    public override void Enter(MummyMonster monster)
    {
        monster.StartCoroutine(SkillCo(monster));
    }

    public override void Exit(MummyMonster monster)
    {

    }

    public override void Update(MummyMonster monster)
    {

    }

    private IEnumerator SkillCo(MummyMonster monster)
    {
        monster.animator.SetTrigger("Jump");
        monster.rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
        float time = 2.1f;
        monster.projectorObj.SetActive(true);
        monster.projector.orthographicSize = 0.01f;

        while(time > 0)
        {
            time -= Time.deltaTime;
            monster.projector.orthographicSize += 4f * Time.deltaTime;
            yield return null;
        }
        monster.skill.transform.position = monster.transform.position;
        monster.skill.Play();
        monster.projectorObj.SetActive(false);

        monster.viewDetector.FindSkillTarget(monster.skillDamage);
        if(monster.viewDetector.SkillTarget != null)
        {
            monster.viewDetector.SkillTarget.GetComponent<Rigidbody>().AddForce(monster.transform.forward * monster.power, ForceMode.Impulse);
        }
        monster.ChangeState(MummyState.Idle);
    }
}

public class MummyDie : BaseState<MummyMonster>
{
    public override void Enter(MummyMonster monster)
    {
        monster.gameObject.layer = 0;
        monster.animator.Play("Die");
        monster.StartCoroutine(DieCo(monster));
        monster.StartCoroutine(CoinCo(monster));
    }

    public override void Exit(MummyMonster monster)
    {

    }

    public override void Update(MummyMonster monster)
    {

    }

    private IEnumerator DieCo(MummyMonster monster)
    {
        yield return new WaitForSeconds(5f);
        MonsterManager.instance.EnterPool(monster.gameObject);
    }

    private IEnumerator CoinCo(MummyMonster monster)
    {
        for (int i = 0; i < monster.coinCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            CoinManager.instacne.ExitPool(monster.coinPos);
        }
    }
}



public class MummyMonster : Monster, IInteractable
{
    [SerializeField] private float hp;
    public float Hp
    {
        get { return hp; }
        set 
        { 
            hp = value; 
            if(hp <= 0)
            {
                if(mummyState != MummyState.Die)
                {
                    ChangeState(MummyState.Die);
                }
            }
        }
    }

    private StateMachine<MummyState, MummyMonster> stateMachine = new StateMachine<MummyState, MummyMonster>();
    public MummyState mummyState;
    public GameObject projectorObj;
    public Projector projector;
    public ParticleSystem skill;
    public float skillDamage;
    public SkinnedMeshRenderer[] meshRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        rigid = GetComponent<Rigidbody>();
        stateMachine.Reset(this);
        stateMachine.AddState(MummyState.Idle, new MummyIdle());
        stateMachine.AddState(MummyState.TakeHit, new MummyTakeHit());
        stateMachine.AddState(MummyState.Attack, new MummyAttack());
        stateMachine.AddState(MummyState.Skill, new MummySkill()); ;
        stateMachine.AddState(MummyState.Die, new MummyDie());
        ChangeState(MummyState.Idle);
        maxHp = Hp;
    }

    private void OnEnable()
    {
        Hp = maxHp + (maxHp * StageManager.instance.stageCount);
        maxHp = Hp;
        slider.value = Hp / maxHp;
        ChangeState(MummyState.Idle);
    }

    private void Start()
    {
        player = GameManager.instance.player;
    }


    private void Update()
    {
        stateMachine.Update();
    }

    public void BasicAttack()
    {
        viewDetector.FindAttackTarget();

        if(viewDetector.AtkTarget != null)
        {
            viewDetector.AtkTarget.GetComponent<IInteractable>().TakeHit(damage);
            viewDetector.AtkTarget.GetComponent<Rigidbody>().AddForce(transform.forward * 5, ForceMode.Impulse);
        }
    }

    public void ChangeState(MummyState state)
    {
        mummyState = state;
        stateMachine.ChangeState(state);
    }

    public void TakeHit(float damage)
    {
        Hp -= damage;
        slider.value = Hp / maxHp;
        if(Hp > 0)
        {
            ChangeState(MummyState.TakeHit);
        }
    }

    private IEnumerator SkillCo()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);
            ChangeState(MummyState.Skill);
        }
    }
}
