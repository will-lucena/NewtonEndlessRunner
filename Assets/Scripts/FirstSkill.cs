using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSkill : MonoBehaviour
{
    public KeyCode skillKey;
    public new string name;
    public string description;
    public int cooldown;
    public float range;
    public float force;

    private float clock;

    private void Start()
    {
        clock = cooldown;
    }

    private void Update()
    {
        clock += Time.deltaTime;
        if (Input.GetKeyDown(skillKey) && isAvailable(clock))
        {
            clock = 0;
            Debug.Log("pushed");
            effect();
        }
        else if (Input.GetKeyDown(skillKey) && !isAvailable(clock))
        {
            Debug.Log(string.Format("wait more {0:#.##} seconds to push again", cooldown - clock));
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
            hit.transform.GetComponentInParent<Obstacle>().pushTo(newPosition);
        }
    }
}
