using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatsData : ScriptableObject
{
    #region Public ID player character
    public string objectName;
    #endregion

    #region Public Movement Variables
    public float moveSpeed;
    public int runSpeed;
    public int rotationSpeed;
    public int jumpForce;
    #endregion

    #region Public Survival Variables
    public int maxHealth;
    public int currentHealth;
    public int regenHealth;
    public int maxMana;
    public int currentMana;
    public int regenMana;
    #endregion

    #region Public Combat Variables
    public int attackDamage;
    #endregion

    #region Public Level and Skill Variables
    public int level;
    public int skillPoint = 0;
    public float currentExp;
    #endregion

}
