using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    #region Public Variables
    public Skill[]       skills;
    public SkillButton[] skillButtons;
    public TMP_Text[]    skillLevelTexts;
    public Skill         activateSkill;
    #endregion

    #region Serialize Private Variables
    [SerializeField] private TMP_Text   skillPointText;
    [SerializeField] private TMP_Text   warningText;
    [SerializeField] private GameObject skillWarning;
    [SerializeField] private Image[]    skillIcons;
    #endregion

    #region Private Variables
    private PlayerStatsSystem playerStats;
    
    #endregion

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsSystem>();
        skillPointText.text = "Skill Point: " + playerStats.skillPoint;
        DisplaySkillLevel();

        // Use for no skill data save
        UpdateNewGameSkillImage();
        UpdateNewGameSkillIcon();

        // Use for skill data save
        //int loadGameSceneID = PlayerPrefs.GetInt("LoadGameButton");
        //if (loadGameSceneID != 1)
        //{
        //    UpdateSavedGameSkillImage();
        //    UpdateSavedGameSkillIcon();
        //}
        //else
        //{
        //    UpdateNewGameSkillImage();
        //    UpdateNewGameSkillIcon();
        //}
        
    }

    #region Public Methods
    public void UpgradeButton()
    {
        if (playerStats.skillPoint >= activateSkill.skillDataCurrent.requiredSkillPoint)
        {
            if (playerStats.level >= activateSkill.skillDataCurrent.requiredLevel)
            {
                // Check tier 1 skill, tier 1 skill doesnt have previous skill
                if (activateSkill.skillDataCurrent.previousSkill.Length > 0)
                {
                    for (int i = 0; i < activateSkill.skillDataCurrent.previousSkill.Length; i++)
                    {
                        if (activateSkill.skillDataCurrent.previousSkill[i].isUpgrade == true)
                        {
                            activateSkill.skillDataCurrent.isUpgrade = true;
                            playerStats.skillPoint -= activateSkill.skillDataCurrent.requiredSkillPoint;
                            SkillLevelUp();
                        }
                        else if (activateSkill.skillDataCurrent.previousSkill[i].isUpgrade == false)
                        {
                            skillWarning.SetActive(true);
                            warningText.text = "Upgrade your previous skill first";
                        }
                    }
                }
                else
                {
                    activateSkill.skillDataCurrent.isUpgrade = true;
                    playerStats.skillPoint -= activateSkill.skillDataCurrent.requiredSkillPoint;
                    SkillLevelUp();
                }
            }
            else
            {
                skillWarning.SetActive(true);
                warningText.text = "Not enough level";
            }

        }
        else
        {
            skillWarning.SetActive(true);
            warningText.text = "Not enough skill point";
        }
        skillPointText.text = "Skill Point: " + playerStats.skillPoint;
        UpdateSkillImageWhenUpgrade();
        UpdateSkillIconWhenUpgrade();
        DisplaySkillLevel();
    }

    public void CloseWarning(GameObject warningPanel)
    {
        warningPanel.SetActive(false);
    }

    public void DisplaySkillLevel()
    {
        if (activateSkill != null)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                if (skills[i].skillDataCurrent.isUpgrade == true)
                {
                    skillLevelTexts[i].text = "Lv. " + skills[i].skillDataCurrent.skillLevel.ToString();
                }
                else if (skills[i].skillDataCurrent.isUpgrade == false)
                {
                    skillLevelTexts[i].text = "";
                }
            }
        }
        else
        {
            for (int i = 0; i < skills.Length; i++)
            {
                if (skills[i].skillDataCurrent.isUpgrade == true)
                {
                    skillLevelTexts[i].text = "Lv. " + skills[i].skillDataCurrent.skillLevel.ToString();
                }
                else if (skills[i].skillDataCurrent.isUpgrade == false)
                {
                    skillLevelTexts[i].text = "";
                }
            }
        }
    }

    #endregion

    #region Private Methods

    private void SkillLevelUp()
    {
        activateSkill.skillDataCurrent.skillLevel++;

        // Increase skill data when the skill level > 1
        if (activateSkill.skillDataCurrent.skillLevel > 1)
        {
            // Decrease mana cost per skill level up
            int manaDecreasePerLevel = Mathf.FloorToInt(activateSkill.skillDataCurrent.manaCost * 0.05f);
            activateSkill.skillDataCurrent.manaCost -= manaDecreasePerLevel;

            // Decrease cooldown time per skill level up
            activateSkill.skillDataCurrent.cooldownTime *= 0.95f;

            // Increase attack per skill level up
            activateSkill.skillDataCurrent.multipleAttack *= 1.1f;
        }
    }

    // Update skill images when in game upgrade
    private void UpdateSkillImageWhenUpgrade()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].skillDataCurrent.isUpgrade == true)
            {
                skills[i].GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
            }
            else if (skills[i].skillDataCurrent.isUpgrade == false)
            {
                skills[i].GetComponent<Image>().color = new Vector4(0.39f, 0.39f, 0.39f, 1f);
            }
        }
    }

    // Update skill icons when in game upgrade
    private void UpdateSkillIconWhenUpgrade()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].skillDataCurrent.isUpgrade == true)
            {
                skillIcons[i].sprite = skills[i].skillDataCurrent.skillSprite;

                Image cooldown = skillIcons[i].transform.Find("Cooldown").GetComponent<Image>();
                cooldown.sprite = skills[i].skillDataCurrent.skillSprite;

                Image frame = skillIcons[i].transform.Find("Frame").GetComponent<Image>();
                frame.sprite = skills[i].skillDataCurrent.frameSprite;

                skillIcons[i].gameObject.SetActive(true);
            }
            else if (skills[i].skillDataCurrent.isUpgrade == false)
            {
                skillIcons[i].gameObject.SetActive(false);
            }
        }
    }

    // Update skill images when start a new game
    private void UpdateNewGameSkillImage()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].GetDataFromBase();
            if (skills[i].skillDataCurrent.isUpgrade == true)
            {
                skills[i].GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
            }
            else if (skills[i].skillDataCurrent.isUpgrade == false)
            {
                skills[i].GetComponent<Image>().color = new Vector4(0.39f, 0.39f, 0.39f, 1f);
            }
        }
    }

    // Update skill icons when start a new game
    private void UpdateNewGameSkillIcon()
    {
        for (int i = 0; i < skills.Length; i++)
        {
            skills[i].GetDataFromBase();
            if (skills[i].skillDataCurrent.isUpgrade == true)
            {
                skillIcons[i].sprite = skills[i].skillDataCurrent.skillSprite;

                Image cooldown  = skillIcons[i].transform.Find("Cooldown").GetComponent<Image>();
                cooldown.sprite = skills[i].skillDataCurrent.skillSprite;

                Image noMana  = skillIcons[i].transform.Find("NoMana").GetComponent<Image>();
                noMana.sprite = skills[i].skillDataCurrent.skillSprite;

                Image frame  = skillIcons[i].transform.Find("Frame").GetComponent<Image>();
                frame.sprite = skills[i].skillDataCurrent.frameSprite;

                skillIcons[i].gameObject.SetActive(true);
            }
            else if (skills[i].skillDataCurrent.isUpgrade == false)
            {
                skillIcons[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateSavedGameSkillImage()
    {
        Debug.Log("Update Saved Game Skill Image");
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].skillDataCurrent.isUpgrade == true)
            {
                skills[i].GetComponent<Image>().color = new Vector4(1f, 1f, 1f, 1f);
            }
            else if (skills[i].skillDataCurrent.isUpgrade == false)
            {
                skills[i].GetComponent<Image>().color = new Vector4(0.39f, 0.39f, 0.39f, 1f);
            }
        }
    }

    private void UpdateSavedGameSkillIcon()
    {
        Debug.Log("Update Saved Game Skill Icon");
        for (int i = 0; i < skills.Length; i++)
        {
            if (skills[i].skillDataCurrent.isUpgrade == true)
            {
                skillIcons[i].sprite = skills[i].skillDataCurrent.skillSprite;

                Image cooldown = skillIcons[i].transform.Find("Cooldown").GetComponent<Image>();
                cooldown.sprite = skills[i].skillDataCurrent.skillSprite;

                Image noMana = skillIcons[i].transform.Find("NoMana").GetComponent<Image>();
                noMana.sprite = skills[i].skillDataCurrent.skillSprite;

                Image frame = skillIcons[i].transform.Find("Frame").GetComponent<Image>();
                frame.sprite = skills[i].skillDataCurrent.frameSprite;

                skillIcons[i].gameObject.SetActive(true);
            }
            else if (skills[i].skillDataCurrent.isUpgrade == false)
            {
                skillIcons[i].gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
