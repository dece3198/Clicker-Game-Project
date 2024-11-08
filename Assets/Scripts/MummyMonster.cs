using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;



public enum MummyState
{
    Idle, Attack,Skill, Die
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
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, monster.player.transform.position, 2 * Time.deltaTime);
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

        monster.viewDetector.FindSkillTarget();
        if(monster.viewDetector.SkillTarget != null)
        {
            monster.viewDetector.SkillTarget.GetComponent<IInteractable>().TakeHit(monster.skillDamage);
            monster.viewDetector.SkillTarget.GetComponent<Rigidbody>().AddForce(monster.transform.forward * 5, ForceMode.Impulse);
        }
        monster.ChangeState(MummyState.Idle);
    }
}

public class MummyDie : BaseState<MummyMonster>
{
    public override void Enter(MummyMonster monster)
    {
        MapManager.instance.MapCount--;
        StageManager.instance.stageCount++;
    }

    public override void Exit(MummyMonster monster)
    {

    }

    public override void Update(MummyMonster monster)
    {

    }
}



public class MummyMonster : Monster, IInteractable
{
    [SerializeField] private float hp;
    public float Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    private StateMachine<MummyState, MummyMonster> stateMachine = new StateMachine<MummyState, MummyMonster>();
    public MummyState mummyState;
    public GameObject projectorObj;
    public Projector projector;
    public ParticleSystem skill;
    public float skillDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        rigid = GetComponent<Rigidbody>();
        stateMachine.Reset(this);
        stateMachine.AddState(MummyState.Idle, new MummyIdle());
        stateMachine.AddState(MummyState.Attack, new MummyAttack());
        stateMachine.AddState(MummyState.Skill, new MummySkill()); ;
        stateMachine.AddState(MummyState.Die, new MummyDie());
        ChangeState(MummyState.Idle);
    }

    private void Start()
    {
        Hp = 500 + (100 * StageManager.instance.stageCount);
        maxHp = Hp;
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
    }
}
