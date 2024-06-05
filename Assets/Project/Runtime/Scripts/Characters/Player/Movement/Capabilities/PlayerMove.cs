using System.Collections;
using UnityEngine;

public class PlayerMove : PlayerBaseMovement
{
    private float movementSpeed => data.movementSpeed;
    private float velPow => data.velPow;
    private float acceleration => data.runAccel;
    private float deceleration => data.runDecel;
    private bool canMove = true;
    private float frictionConst = 0.2f;
    private float maxSlopeRunAngle = 95f;
    private float climbSpeed => data.climbSpeed;
    private Vector2 playerPos {
        get {
            return rb.position + data.playerHitbox.offset;
        }
    }

    private Vector2 colliderSize{
        get {
            return data.playerHitbox.size;
        }
    }
    LayerMask objectMask = LayerMask.GetMask("CollidableObjects");

    public PlayerMove(PlayerData _data) : base(_data)
    {
        data = _data;
        SelfInitialize();
        SetUpConstants();
    }

    public override void SelfInitialize()
    {
        base.SelfInitialize();
        data.isCrouching = false;
    }

    public override void SetUpConstants()
    {
        base.SetUpConstants();
    }

    #region Run
    private float getAccel(float val)
    {
        return (Mathf.Abs(val) > 0.01f) ? acceleration : deceleration;
    }

    private float getFriction(float curVel)
    {
        //if (Mathf.Abs(curInput) > 0.01f) return 0f;
        return Mathf.Max(-1f * curVel * frictionConst, -20f);
    }

    private Vector2 slopeNormalPerp ;

    public void RunCharacter(Vector2 direction)
    {
        GetSlope();
        animationManager.ForceAdd(1, "Run");
        
        //Add Force when player run
        float targetSpeed = movementSpeed * direction.x;
        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate = getAccel(targetSpeed);
        float curSpeed = speedDif* accelRate;
        //Debug.Log(rb.velocity + " " + speedDif + " " + accelRate);
        //Debug.Log(slopeNormalPerp.normalized + " " + curSpeed);
        Vector2 forcetoAdd = curSpeed * slopeNormalPerp.normalized;
        if(slopeNormalPerp.normalized.y > 0.9) forcetoAdd = new Vector2(forcetoAdd.x, forcetoAdd.y * 0.95f);
        //Debug.Log(forcetoAdd);
        rb.AddForce(forcetoAdd);
        animator.SetFloat("RunSpeed",Mathf.Max(1f, 2 * Mathf.Abs(rb.velocity.x) / (Mathf.Abs(rb.velocity.x) + 10)));
    }

    private void GetSlope(){
        slopeNormalPerp = new Vector2(1,0);
        if(!data.groundChecker.isGrounded()) return;
        Vector2 checkPos = playerPos-new Vector2(0f,colliderSize.y/2-0.1f);
        GetSlopeVertical(checkPos);

        GetSlopeHorizontal(checkPos);
    }
    
    private void GetSlopeVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos,Vector2.down,objectMask);
        if(hit.collider == null) return;
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
        //Debug.Log(slopeAngle);
        if(slopeAngle > maxSlopeRunAngle) return;
        slopeNormalPerp = -Vector2.Perpendicular(hit.normal);
        //Debug.DrawRay(hit.point, hit.normal, Color.blue);
        //Debug.DrawRay(hit.point, slopeNormalPerp, Color.yellow);
    }

    private void GetSlopeHorizontal(Vector2 checkPos){
        RaycastHit2D hit = Physics2D.Raycast(checkPos,Vector2.right*_GFX.localScale.x,colliderSize.x/2,objectMask);
        if(hit.collider == null) return;
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
        //Debug.Log(slopeAngle);
        if(slopeAngle > maxSlopeRunAngle) return;
        slopeNormalPerp = -Vector2.Perpendicular(hit.normal);
        //Debug.DrawRay(hit.point, hit.normal, Color.red);
        //Debug.DrawRay(hit.point, slopeNormalPerp, Color.green);
    }

    public void BrakeCharacter(Vector2 direction)
    {
        animationManager.RemoveAnim(1);
        animator.SetFloat("RunSpeed", 1);
        Vector2 frictionForce = new Vector2(getFriction(rb.velocity.x), 0f);
        rb.AddForce(frictionForce, ForceMode2D.Impulse);
    }


    #endregion

    #region Crouch
    public void CrouchCharacter()
    {
        if(data.isCrouching) return;
        data.isCrouching = true;
        animationManager.AddAnim(2, "Crouch");
        customGravity.gravityScale = 3;
        rb.velocity *= 0.8f;
    }

    public void UnCrouchCharacter()
    {
        if(!data.isCrouching) return;
        //Debug.Log("Cancel Crouch");
        data.isCrouching = false;
        animationManager.SafeRemove(2, "Crouch");
        customGravity.gravityScale = 1;
    }
    #endregion

    #region Climb
    float lastVerticalVelocity = 0;
    public void ClimbCharacter()
    {
        //Animation
        animationManager.ForceAdd(3, "WallClimb");
        animator.SetFloat("ClimbSpeed", Mathf.Sign(data.movementInput.y));

        //Physics
        float climbAnimLength = data.anims["WallClimb"].length;
        float newVerticalVelocity = 0.8f / climbAnimLength * 4 * Mathf.Sign(data.movementInput.y);
        //Debug.Log(newVerticalVelocity + " " + lastVerticalVelocity);
        rb.velocity += new Vector2(0, newVerticalVelocity - lastVerticalVelocity);
        Collider2D[] climbHits = new Collider2D[10];
        //Debug.Log(Vector2.up*Mathf.Sign(data.movementInput.y));
        climbHits = Physics2D.OverlapBoxAll(
            rb.position + data.playerHitbox.offset + Vector2.up*Mathf.Sign(data.movementInput.y) * data.playerHitbox.size.y * 1/2, 
            new Vector2(data.playerHitbox.size.x-0.1f,0.1f),
            0f);
        bool isWallAhead = false;
        foreach(Collider2D hit in climbHits){
            if(hit.transform == null) break;
            if(hit.transform.CompareTag("Object"))
               isWallAhead = true;
        }
        if(!isWallAhead)
            lastVerticalVelocity = newVerticalVelocity;
        else
            lastVerticalVelocity = 0;
    }

    public void StopClimbCharacter()
    {
        animationManager.RemoveAnim(3);
        //if(Mathf.Abs(lastVerticalVelocity) > 0f) Debug.Log(lastVerticalVelocity);
        rb.velocity -= new Vector2(0f, lastVerticalVelocity);
        lastVerticalVelocity = 0;
    }
    #endregion

    #region Slide
    private bool nearGround(float nearGroundThreshold){
        Debug.Log("" + nearGroundThreshold);
        RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(rb.position, Vector2.down, nearGroundThreshold);
        bool canSlide = false;
        Debug.Log(raycastHit2Ds.Length);
        foreach(RaycastHit2D hit in raycastHit2Ds){
            Debug.Log(hit.collider.name + ' ');
            if(hit.transform.CompareTag("Object")){
                canSlide = true;
                break;
            }
        }
        return canSlide;
    }

    private Coroutine slideRoutine;
    private bool slideFrictionAdded;

    public void SlideCharacter(){
        //Debug.Log("start slide");
        if(!nearGround(8f)) return;
        //Debug.Log("slide started");
        if(slideRoutine == null) { 
            slideRoutine = data.StartCoroutine(StartSlide());
        }

    }

    public void CancelSlide(){
        if(slideRoutine == null) return;
        //Debug.Log("cancelling slide");
        data.StopCoroutine(slideRoutine);
        animationManager.SafeRemove(2, "SlideLoop");
        animationManager.SafeRemove(2, "SlideStand");
        data.isSliding = false;
        data.playerHitbox.direction = CapsuleDirection2D.Vertical;
        data.canMove = true;
        data.canFlip = true;
        slideRoutine = null;
        if(slideFrictionAdded) data.playerHitbox.sharedMaterial.friction /= 1.25f;
    }

    private IEnumerator StartSlide(){
        data.isSliding = true;
        data.canMove = false;
        data.canFlip = false;
        data.playerHitbox.direction = CapsuleDirection2D.Horizontal;
        yield return new WaitUntil(()=>nearGround(4f));
        //Debug.Log(rb.velocity);
        rb.velocity += new Vector2(Mathf.Sign(rb.velocity.x) * Mathf.Abs(rb.velocity.y)*0.8f,0);
        //Debug.Log(rb.velocity);
        animationManager.ForceAdd(2, "SlideLoop"); 
        data.playerHitbox.sharedMaterial.friction *= 1.25f;
        slideFrictionAdded = true;
        yield return new WaitUntil(()=>(rb.velocity.magnitude<10f||!data.groundChecker.isGrounded()));
        data.playerHitbox.direction = CapsuleDirection2D.Vertical;
        animationManager.ForceAdd(2, "SlideStand");
        yield return new WaitForSeconds(data.anims["SlideStand"].length-0.001f);
        CancelSlide();
    }
    #endregion
}


