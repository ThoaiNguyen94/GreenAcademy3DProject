using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateCharStats : MonoBehaviour
{
    #region Serialize Field Private Variables
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text healthRegenText;
    [SerializeField] private TMP_Text manaText;
    [SerializeField] private TMP_Text manaRegenText;
    [SerializeField] private TMP_Text attackDamage;
    [SerializeField] private TMP_Text attackRange;
    [SerializeField] private TMP_Text moveSpeed;
    [SerializeField] private TMP_Text runSpeed;
    #endregion

    private PlayerStatsSystem playerStats;

    public void UpdateCharacterStats()
    {
        playerStats = GetComponentInParent<PlayerStatsSystem>();

        healthText.text = "Health: " + playerStats.currentHealth.ToString("#,##0") + "/" + playerStats.maxHealth.ToString("#,##0");
        healthRegenText.text = "Health regen: " + playerStats.regenHealth.ToString("#,##0") + " per second";

        manaText.text = "Mana: " + playerStats.currentMana.ToString("#,##0") + "/" + playerStats.maxMana.ToString("#,##0");
        manaRegenText.text = "Mana regen: " + playerStats.regenMana.ToString("#,##0") + " per second";

        attackDamage.text = "Attack damage: " + (playerStats.attackDamage + playerStats.weaponDamage).ToString("#,##0");
        attackRange .text = "Weapon range: "  + playerStats.weaponRange .ToString("#,##0");

        moveSpeed   .text = "Move speed: "    + playerStats.moveSpeed   .ToString("#,##0");
        runSpeed    .text = "Run speed: "     + playerStats.runSpeed    .ToString("#,##0");

        // ToString("#,##0") format a number like this "123,456"
    }
}
