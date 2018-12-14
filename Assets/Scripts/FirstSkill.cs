using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSkill : Skill
{
    public float range;

    private float clock;

    private void Start()
    {
        clock = cooldown;
        reloadUI.setSkillScript(this);
    }

    private void Update()
    {
        clock += Time.deltaTime;
        if (!isAvailable(clock))
        {
            notifyReload?.Invoke(clock / cooldown);
        }
    }

    private bool isAvailable(float time)
    {
        return time >= cooldown;
    }

    private void effect()
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

    public override void activateSkill(bool boolean = false)
    {
        if (isAvailable(clock))
        {
            clock = 0;
            Debug.Log("pushed");
            effect();
        }
    }
}
