using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CactusState
{
    Idle, Walk, Attack, TakeHit, Die
}

public class CactusMonster : Monster,IInteractable
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
                ChangerState(CactusState.Die);
            }
        }
    }

    public CactusState cactusState;
    private StateMachine<CactusState, CactusMonster> stateMachine = new StateMachine<CactusState, CactusMonster>();
    public SkinnedMeshRenderer meshRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        rigid = GetComponent<Rigidbody>();
        stateMachine.Reset(this);
        stateMachine.AddState(CactusState.Idle, new CactusStates.CactusIdle());
        stateMachine.AddState(CactusState.Attack, new CactusStates.CactusAttack());
        stateMachine.AddState(CactusState.TakeHit, new CactusStates.CactusTakeHit());
        stateMachine.AddState(CactusState.Die, new CactusStates.CactusDie());
        ChangerState(CactusState.Idle);
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

    public void ChangerState(CactusState state)
    {
        cactusState = state;
        stateMachine.ChangeState(state);
    }

    public void TakeHit(float damage)
    {
        Hp -= damage;
        if(Hp > 0)
        {
            ChangerState(CactusState.TakeHit);
        }
    }
}
