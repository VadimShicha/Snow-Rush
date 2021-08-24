using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float spinSpeed = 0.01f;
    public float spinAmount = 1;

	bool spun = true;

	void FixedUpdate()
	{
		if(spun == true)
		{
			spun = false;
			StartCoroutine("Spin");
		}
	}

	IEnumerator Spin()
	{
		gameObject.transform.Rotate(new Vector3(0, 0, spinAmount));

		yield return new WaitForSeconds(spinSpeed);
		spun = true;
	}

}
