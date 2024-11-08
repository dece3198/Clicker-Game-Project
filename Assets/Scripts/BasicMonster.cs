using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BasicMonsterState
{
    Idle, Walk, Attack, TakeHit, Die
}

public class BasicMonster : Monster,IInteractable
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
                if(basicMonsterState != BasicMonsterState.Die)
                {
                    ChangeState(BasicMonsterState.Die);
                }
            }
        }
    }

    public BasicMonsterState basicMonsterState;
    private StateMachine<BasicMonsterState, BasicMonster> stateMachine = new StateMachine<BasicMonsterState, BasicMonster>();
    public SkinnedMeshRenderer meshRenderer;
    public ParticleSystem dieParticle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        rigid = GetComponent<Rigidbody>();
        stateMachine.Reset(this);
        stateMachine.AddState(BasicMonsterState.Idle, new BasicStates.BasicMonsterIdle());
        stateMachine.AddState(BasicMonsterState.Attack, new BasicStates.BasicMonsterAttack());
        stateMachine.AddState(BasicMonsterState.TakeHit, new BasicStates.BasicMonsterTakeHit());
        stateMachine.AddState(BasicMonsterState.Die, new BasicStates.BasicMonsterDie());
        ChangeState(BasicMonsterState.Idle);
        maxHp = Hp;
    }

    private void OnEnable()
    {
        slider.value = Hp / maxHp;
    }

    private void Start()
    {
        player = GameManager.instance.player;
        slider.value = Hp / maxHp;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void PlayerAttack()
    {
        if (viewDetector.Target != null)
        {
            viewDetector.Target.GetComponent<IInteractable>().TakeHit(damage);
            viewDetector.AtkTarget.GetComponent<Rigidbody>().AddForce(transform.forward * power, ForceMode.Impulse);
        }
    }

    public void ChangeState(BasicMonsterState state)
    {
        basicMonsterState = state;
        stateMachine.ChangeState(state);
    }

    public void TakeHit(float damage)
    {
        Hp -= damage;
        slider.value = Hp / maxHp;
        if (Hp > 0)
        {
            ChangeState(BasicMonsterState.TakeHit);
        }
    }
}
