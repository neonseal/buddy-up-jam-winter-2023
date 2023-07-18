using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlushieDamageBaseState : BaseState
{
    public PlushieDamageBaseState(string name, PlushieDamageSM stateMachine) : base(name, stateMachine)
    {
    }

    public virtual void CompleteRepair() {}
}
