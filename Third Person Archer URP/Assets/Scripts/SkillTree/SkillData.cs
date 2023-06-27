using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SkillData : ScriptableObject
{
    public string skillName;
    public Sprite skillSprite;
    public Sprite frameSprite;
    public int    manaCost;
    public int    skillLevel;
    public int    requiredLevel;
    public int    requiredSkillPoint;
    public float  multipleAttack;
    public float  cooldownTime;
    public float  skillRange;
    public bool   isUpgrade;

    [TextArea(1, 3)] public string skillDescription;

    public SkillData[] previousSkill;

    //public int skillType;
}
