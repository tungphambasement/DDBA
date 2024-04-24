using System.Collections.Generic;
using UnityEngine;


public class SlimeSmash : SlimeMovementBase
{ 
    private List<Collider> collidersDamaged = new();

    public SlimeSmash(Slime_Data data) : base(data)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter(); 
        movementDirection = data.directionToPlayer.normalized;
        adjustFlipSprite();
    }

    public override void OnFixedHandle()
    {   
        base.OnFixedHandle();
        
    }
}
