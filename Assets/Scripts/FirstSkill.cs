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
    }

    private bool isAvailable(float time)
    {
        return time >= cooldown;
    }

    public void active()
    {
        if (isAvailable(clock))
        {
            clock = 0;
            Debug.Log("pushed");
            effect();
        }
        else
        {
            Debug.Log(string.Format("wait more {0:#.##} seconds to push again", cooldown - clock));
        }
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
