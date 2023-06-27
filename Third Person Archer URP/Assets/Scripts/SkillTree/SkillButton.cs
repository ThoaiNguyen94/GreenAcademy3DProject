using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Image skillImage;
    public Image frameImage;
    public TMP_Text skillName;
    public TMP_Text skillDescription;
    public TMP_Text skillManaCost;
    public TMP_Text skillCooldownTime;
    public TMP_Text attackPower;
    public TMP_Text skillRequiedLevel;
    public TMP_Text skillRequiredPoint;
    public TMP_Text skillPoint;

    public int skillButtonID;

    [SerializeField] private SkillManager skillManager;

    private void Start()
    {
        SetDefaultSkill();
    }

    public void PressSkillButton()
    {
        skillManager.activateSkill = transform.GetComponent<Skill>();
        DisplaySkillData(skillButtonID);
    }

    private void SetDefaultSkill()
    {
        // Set a default skill display on Skill Details Window
        DisplaySkillData(0);
    }

    private void DisplaySkillData(int buttonID)
    {
        skillImage.sprite     = skillManager.skills[buttonID].skillDataCurrent.skillSprite;
        frameImage.sprite     = skillManager.skills[buttonID].skillDataCurrent.frameSprite;
        skillName.text        = skillManager.skills[buttonID].skillDataCurrent.skillName;
        skillDescription.text = skillManager.skills[buttonID].skillDataCurrent.skillDescription;

        skillManaCost.text = "Mana: " + skillManager.skills[buttonID].skillDataCurrent.manaCost.ToString();
        skillCooldownTime.text = "CD Time: " + skillManager.skills[buttonID].skillDataCurrent.cooldownTime.ToString("#,###.##");
        attackPower.text = "Multiple Attack: x" + skillManager.skills[buttonID].skillDataCurrent.multipleAttack.ToString("#,###.##");
        skillRequiedLevel.text = "Requied Lv: " + skillManager.skills[buttonID].skillDataCurrent.requiredLevel.ToString("#,###.##");
        skillRequiredPoint.text = "Required Point: " + skillManager.skills[buttonID].skillDataCurrent.requiredSkillPoint.ToString("#,###.##");
    }

}
