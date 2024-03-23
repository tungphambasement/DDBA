using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMovement
{
    protected RaycastHit[] castCollisions;
    protected ContactFilter2D movementFilter;
    protected Rigidbody2D rb;
    protected GameObject playerBody;
    protected PlayerData data;
    protected Transform _GFX;
    protected GroundChecker groundChecker;
    protected Animator animator;

    public PlayerBaseMovement(PlayerData _data)
    {
        data = _data;
        SelfInitialize();
        SetUpConstants();
    }

    public virtual void SelfInitialize()
    {
        rb = data.rb;
        playerBody = data.playerBody;
        _GFX = data.GFX;
        groundChecker = data.groundChecker;
        animator = data.animator;
    }

    public virtual void SetUpConstants()
    {
        
    }
}
