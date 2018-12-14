using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdSkill : Skill
{
    private float clock;
    private bool isActive;
    public GameObject vfx;

    private void Start()
    {
        clock = cooldown;
        isActive = false;
        GameController.instance.consumeShield += effect;
        GameController.instance.unsubscribeMethods += reset;
        reloadUI.setSkillScript(this);
    }

    private void reset()
    {
        GameController.instance.consumeShield -= effect;
        GameController.instance.unsubscribeMethods -= reset;
    }

    private void Update()
    {
        if (!isActive)
        {
            clock += Time.deltaTime;
            /*
            if (Input.GetKeyDown(skillKey) && isAvailable(clock))
            {
                clock = 0;
                Debug.Log("shield working");
                isActive = true;
                GameController.instance.activeShield();
            }
            else if (Input.GetKeyDown(skillKey) && !isAvailable(clock))
            {
                Debug.Log(string.Format("wait more {0:#.##} seconds to use shield again", cooldown - clock));
            }
            /**/
            notifyReload?.Invoke(clock / cooldown);
        }
    }

    private bool isAvailable(float time)
    {
        return time >= cooldown;
    }

    private Vector3 effect(Obstacle obstacle)
    {
        obstacle.pushTo(new Vector3(obstacle.transform.position.x, obstacle.transform.position.y, obstacle.transform.position.z + force));
        isActive = false;
        vfx.SetActive(false);
        return new Vector3(transform.position.x, transform.position.y, transform.position.z - force);
    }

    public override void activateSkill(bool boolean = false)
    {
        if (isAvailable(clock))
        {
            clock = 0;
            Debug.Log("shield working");
            isActive = true;
            GameController.instance.activeShield();
            vfx.SetActive(true);
        }
        else if (!isAvailable(clock))
        {
            Debug.Log(string.Format("wait more {0:#.##} seconds to use shield again", cooldown - clock));
        }
    }

}
