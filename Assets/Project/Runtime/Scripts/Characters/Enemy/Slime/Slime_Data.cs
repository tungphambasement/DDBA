using UnityEngine;

public class Slime_Data : EnemyData
{
    public float currentHealth, maxHealth = 30;

    public float defaultDashCD = 4, dashCD;

    public float acceleration = 2f, deceleration = 2f;
}