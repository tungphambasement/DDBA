INFORMATION ABOUT EACH ACTION:

Player Move:
*For easy player turning and non-linear movement
SpeedDif = targetVeloc - currentVeloc
Run Accel = SpeedDif * AccelRate 

*Near immediate stopping we will add friction when movement input is 0

Crouching will increase AccelRate and decrease MovementSpeed

PlayerJump:

Add an impulse force of jumpPower to Player

Accelerate gravity as player Jump and Fall:
 
Instanteous Gravity
G(t) = defaultGravity * (1 + t*gravAccelRate) 

Cumulative Grav Force Applied
G_Cum(t) = -1 * defaultGravity * (t + 1/2 * t^2 * gravAccelRate)

Vertical Force on Player after jump t seconds

F(t) = jumpPower + G_Cum(t) = jumpPower - defaultGravity * (t + 1/2 * t^2 * gravAccelRate)

time when player peak = time when vertical force gets negative <=> jumpPower < G_Cum(t): 
    (1/2 * t^2 * gravAccelRate + t) * defaultGravity - jumpPower > 0
    t^2 * gravAccelRate * defaultGravity + 2 * t * defaultGravity - 2 * jumpPower > 0
    


