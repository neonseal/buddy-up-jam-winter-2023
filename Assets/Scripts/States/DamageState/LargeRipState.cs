using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeRipState : PlushieDamageBaseState
{
    public LargeRipState(PlushieDamageSM stateMachine) : base("Large rip plushie damage state", stateMachine)
    {
    }

    public override void CompleteRepair() {
        this.stateMachine.ChangeState(((PlushieDamageSM) this.stateMachine).repairFinishState);
    }
}
