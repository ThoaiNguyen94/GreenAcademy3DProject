using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    #region Serialize Private Variables
    [SerializeField] private Image xpBar;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text skillPointText;
    [SerializeField] private GameObject levelUpVFX;
    #endregion

    #region Private Variables
    private PlayerStatsSystem playerStats;
    private GameObject levelUpObject;
    private float requiredExp;
    private readonly float additionMultiplier = 300;
    private readonly float powerMultiplier = 2;
    private readonly float divisionMultiplier = 7;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GetComponent<PlayerStatsSystem>();
        xpBar.fillAmount = playerStats.currentExp / requiredExp;
        requiredExp = CalculateForRequiredExp();
        levelText.text = "Level " + playerStats.level;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateXpUI();
        if (playerStats.currentExp >= requiredExp)
        {
            LevelUp();
        }
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            GainXp(10000000);
            //GainXp(100);
        }
    }

    private void UpdateXpUI()
    {
        float xp = playerStats.currentExp / requiredExp;
        xpBar.fillAmount = xp;
        xpText.text = playerStats.currentExp.ToString("#,##0") + "/" + requiredExp.ToString("#,##0");
    }

    public void GainXp(int xpGained)
    {
        playerStats.currentExp += xpGained;
    }

    //public void GainXpScalable(float xpGained, int passedLevel)
    //{
    //    if(passedLevel < playerStats.level)
    //    {
    //        float multiplier = ((playerStats.level - passedLevel) * 0.1f) + 1;
    //        playerStats.currentExp += xpGained * multiplier;
    //    }
    //    else
    //    {
    //        playerStats.currentExp += xpGained;
    //    }
    //}

    private void LevelUp()
    {
        playerStats.level++;
        levelUpObject = Instantiate(levelUpVFX, transform.position, Quaternion.identity);
        Destroy(levelUpObject, 1);
        levelText.text = "Level " + playerStats.level;
        xpBar.fillAmount = 0f;
        playerStats.currentExp = Mathf.RoundToInt(playerStats.currentExp - requiredExp);
        playerStats.IncreaseStatsPerLevel(playerStats.level);
        playerStats.UpdateCurrentHealth(playerStats.maxHealth, playerStats.currentHealth);
        playerStats.UpdateCurrentMana(playerStats.maxMana, playerStats.currentMana);
        requiredExp = CalculateForRequiredExp();
        skillPointText.text = "Skill Point: " + playerStats.skillPoint;
    }

    private int CalculateForRequiredExp()
    {
        int solveForRequiedXp = 0;
        for(int levelCycle = 1; levelCycle <= playerStats.level; levelCycle++)
        {
            solveForRequiedXp += (int)Mathf.Floor(levelCycle + additionMultiplier * Mathf.Pow(powerMultiplier, levelCycle / divisionMultiplier));
        }
        return solveForRequiedXp / 4;
    }
}
