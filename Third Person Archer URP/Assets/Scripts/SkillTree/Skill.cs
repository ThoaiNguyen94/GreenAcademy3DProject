using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Skill : MonoBehaviour
{
    public SkillData skillDataCurrent;
    [SerializeField] private SkillData skillDataNew;

    private void Awake()
    {
        GetDataFromBase();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get skill image and update on skill tree panel
        GetComponent<Image>().sprite = skillDataCurrent.skillSprite;

        // Get skill image and update on skill tree panel
        Image frame = transform.Find("Frame").GetComponent<Image>();
        frame.sprite = skillDataCurrent.frameSprite;
    }

    public void GetDataFromBase()
    {
        skillDataCurrent.skillName          = skillDataNew.skillName;
        skillDataCurrent.skillSprite        = skillDataNew.skillSprite;
        skillDataCurrent.frameSprite        = skillDataNew.frameSprite;
        skillDataCurrent.manaCost           = skillDataNew.manaCost;
        skillDataCurrent.skillLevel         = skillDataNew.skillLevel;
        skillDataCurrent.requiredLevel      = skillDataNew.requiredLevel;
        skillDataCurrent.requiredSkillPoint = skillDataNew.requiredSkillPoint;
        skillDataCurrent.multipleAttack     = skillDataNew.multipleAttack;
        skillDataCurrent.cooldownTime       = skillDataNew.cooldownTime;
        skillDataCurrent.skillRange         = skillDataNew.skillRange;
        skillDataCurrent.isUpgrade          = skillDataNew.isUpgrade;
        skillDataCurrent.skillDescription   = skillDataNew.skillDescription;
    }

    
}
