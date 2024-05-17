using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggState
{

    protected Egg egg;
    protected EggStateMachine eggStateMachine;

    public EggState(Egg egg, EggStateMachine eggStateMachine)
    {
        this.egg = egg;
        this.eggStateMachine = eggStateMachine;
    }

    public virtual void EnterState() { }

    public virtual void ExitState() { }

    public virtual void FrameUpdate() { }

    public virtual void PhysicsUpdate() { }

}
