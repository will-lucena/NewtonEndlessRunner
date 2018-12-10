using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdSkill : MonoBehaviour
{
    public KeyCode skillKey;
    public new string name;
    public string description;
    public int cooldown;
    public float force;

    private float clock;
    private bool isActive;

    private void Start()
    {
        clock = cooldown;
        isActive = false;
        GameController.instance.consumeShield += effect;
        GameController.instance.unsubscribeMethods += reset;
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
        return new Vector3(transform.position.x, transform.position.y, transform.position.z - force);
    }

    public void activateSkill()
    {
        if (isAvailable(clock))
        {
            clock = 0;
            Debug.Log("shield working");
            isActive = true;
            GameController.instance.activeShield();
        }
        else if (!isAvailable(clock))
        {
            Debug.Log(string.Format("wait more {0:#.##} seconds to use shield again", cooldown - clock));
        }
    }

}
