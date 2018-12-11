using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillReloadUIEffect : MonoBehaviour
{
    private Image mask;

    private void Start()
    {
        mask = GetComponent<Image>();
        mask.enabled = false;
    }

    private void fillMask(float amount)
    {
        if (mask.enabled)
        {
            mask.fillAmount = amount;
            if (amount >= 0.95f)
            {
                mask.enabled = false;
            }
        }
        else if (amount <= 0.98f)
        {
            mask.enabled = true;
            mask.fillAmount = amount;
        }
    }

    public void setSkillScript(Skill skill)
    {
        skill.notifyReload += fillMask;
    }
}
