using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SecondSkill : Skill
{
    public float range;
    public float minForce;
    public float maxForce;
    public int maxEnergy;

    private float clock;
    private float currentEnergy;
    private float timeCharging;
    private bool isPressed;

    private void Start()
    {
        clock = cooldown;
        currentEnergy = maxEnergy;
        reloadUI.setSkillScript(this);
    }

    private void Update()
    {
        if (isPressed)
        {
            if (hasEnergy() && isAvailable(clock))
            {
                timeCharging += Time.deltaTime;
                if (timeCharging >= currentEnergy || timeCharging * force >= maxForce)
                {
                    Debug.Log(string.Format("Skill used with force = {0}, forced", timeCharging));
                    releaseSkill(timeCharging * force);
                }
            }
        }
        else if (timeCharging > 0)
        {
            float force = minForce > timeCharging * this.force ? minForce : timeCharging * this.force;
            Debug.Log(string.Format("Skill used with force = {0}, released", force));
            releaseSkill(force);
        }
        //verificar se tem energia
        /*
        if (Input.GetKey(skillKey) && hasEnergy() && isAvailable(clock))
        {
            timeCharging += Time.deltaTime;
            if (timeCharging >= currentEnergy || timeCharging * force >= maxForce)
            {
                Debug.Log(string.Format("Skill used with force = {0}, forced", timeCharging));
                releaseSkill(timeCharging * force);
            }
        }

        if (timeCharging > 0 && Input.GetKeyUp(skillKey))
        {
            float force = minForce > timeCharging * this.force ? minForce : timeCharging * this.force;
            Debug.Log(string.Format("Skill used with force = {0}, released", force));
            releaseSkill(force);
        }
        /**/

        if (!hasEnergy())
        {
            clock += Time.deltaTime;
            if (!isAvailable(clock))
            {
                notifyReload?.Invoke(clock / cooldown);
                //Debug.Log(string.Format("wait more {0:#.##} seconds to push again", cooldown - clock));
                return;
            }
            currentEnergy = maxEnergy;
        }
    }

    private bool isAvailable(float time)
    {
        return time >= cooldown;
    }

    private bool hasEnergy()
    {
        return currentEnergy >= minForce;
    }

    private void releaseSkill(float forceToUse)
    {
        effect(forceToUse);

        timeCharging = 0;
        currentEnergy -= forceToUse;
        if (!hasEnergy())
        {
            clock = 0;
        }
    }

    private void effect(float force)
    {
        RaycastHit hit;
        if (Physics.Raycast(new Ray(new Vector3(transform.position.x, 0.5f, transform.position.z), Vector3.forward), out hit, range))
        {
            Vector3 newPosition = new Vector3(hit.transform.position.x, hit.transform.position.y, hit.transform.position.z + force);
            Obstacle go = hit.transform.GetComponentInParent<Obstacle>();
            if (go != null)
            {
                go.pushTo(newPosition);
            }
        }
    }

    public override void activateSkill(bool isPressed = false)
    {
        this.isPressed = isPressed;
    }
}
