using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallRipState : PlushieDamageBaseState
{
    public SmallRipState(PlushieDamageSM stateMachine) : base("Small rip plushie damage state", stateMachine)
    {
    }

    public override void CompleteRepair() {
        this.stateMachine.ChangeState(((PlushieDamageSM) this.stateMachine).repairFinishState);
    }
}
