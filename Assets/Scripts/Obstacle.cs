using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float duration;
    private new ParticleSystem particleSystem;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    public void pushTo(Vector3 position)
	{
        particleSystem.Play();
        Vector3 startPos = transform.position;
        transform.position = Vector3.Lerp(startPos, position, duration);
        particleSystem.Stop();
    }
}
