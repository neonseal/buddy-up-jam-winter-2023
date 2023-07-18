using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingStuffingState : PlushieDamageBaseState
{
    public MissingStuffingState(PlushieDamageSM stateMachine) : base("Missing stuffing plushie damage state", stateMachine)
    {
    }

    public override void CompleteRepair() {
        this.stateMachine.ChangeState(((PlushieDamageSM) this.stateMachine).largeRipState);
    }
}
