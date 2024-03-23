
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData_", menuName = "Unit_Data/Player")]
public class ConstPlayerData : ScriptableObject
{
    #region  Movement Consts
    [Header("Movement Constants")]
    public float movementSpeed = 40f;
    public float velPow = 1f;
    public float runAccel = 2f;
    public float runDecel = 3f;
    public float jumpPower = 35f;
    public float jumpAirMoveTime = 0.8f;
    public float coyoteTime = 0.3f;
    public float defGrav = 1f;
    public float gravAccel = 2f;
    public int numberOfJumps = 1;
    #endregion

    [Space(20)]

    #region Combat Consts
    [Header("Combat Constants")]
    public float CombatCD = 5f;
    #endregion
    
    [Space(20)]

    #region  Status Consts
    [Header("Status Constants")]
    public float maxHealth;
    #endregion

    [Space(20)]
    
    public float Difficulty = 0f;
    [SerializeField] Color fromColor, toColor;
}

