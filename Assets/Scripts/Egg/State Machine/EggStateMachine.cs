using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggStateMachine
{
    public EggState CurrentEggState { get; set; }

    public void Initialize(EggState startingState)
    {
        CurrentEggState = startingState;
        CurrentEggState.EnterState();
    }

    public void ChangeState(EggState newState)
    {
        CurrentEggState.ExitState();
        CurrentEggState = newState;
        CurrentEggState.EnterState();
    }
}
