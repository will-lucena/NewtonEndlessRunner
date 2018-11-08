using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

	public void pushTo(Vector3 position)
	{
		transform.position = position;
	}
}
