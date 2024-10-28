using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BasicMonsterState
{
    Idle, Walk, Attack, TakeHit, Die
}

public enum MonsterType
{
    Cactus, Mushroom
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
                ChangerState(BasicMonsterState.Die);
            }
        }
    }

    public BasicMonsterState basicMonsterState;
    public MonsterType monsterType;
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
        ChangerState(BasicMonsterState.Idle);
    }

    private void Start()
    {
        player = GameManager.instance.player;
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public void PlayerAttack()
    {
        if(viewDetector.Target != null)
        {
            viewDetector.Target.GetComponent<IInteractable>()?.TakeHit(damage);
        }
    }

    public void ChangerState(BasicMonsterState state)
    {
        basicMonsterState = state;
        stateMachine.ChangeState(state);
    }

    public void TakeHit(float damage)
    {
        Hp -= damage;
        if(Hp > 0)
        {
            ChangerState(BasicMonsterState.TakeHit);
        }
    }
}
