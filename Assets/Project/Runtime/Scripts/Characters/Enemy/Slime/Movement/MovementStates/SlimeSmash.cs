using System.Collections.Generic;
using UnityEngine;


public class SlimeSmash : SlimeMovementBase
{ 
    private List<Collider> collidersDamaged = new();

    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine); 

        BaseUpdate();
        movementDirection = directionToPlayer.normalized;
        adjustFlipSprite();
    }

    public override void OnFixedHandle()
    {   
        base.OnFixedHandle();
        
    }
}
