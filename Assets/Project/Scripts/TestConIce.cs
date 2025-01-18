using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class TestConIce : MonoBehaviour
{
	public float intensity = 5;
	public GameObject SolverObj;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.W))
		{
			GetComponent<ObiActor>().AddForce(Vector3.forward * intensity, ForceMode.VelocityChange);
		}
		if (Input.GetKeyDown(KeyCode.A))
		{
			GetComponent<ObiActor>().AddForce(Vector3.left * intensity, ForceMode.VelocityChange);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			GetComponent<ObiActor>().AddForce(Vector3.back * intensity, ForceMode.VelocityChange);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			GetComponent<ObiActor>().AddForce(Vector3.right * intensity, ForceMode.VelocityChange);
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (GetComponent<ObiSoftbody>().deformationResistance <= 0.2f)
			{
				SolverObj.SetActive(false);
				GetComponent<ObiSoftbody>().deformationResistance = 1.0f;
			}
			else
			{
				SolverObj.SetActive(true);
				GetComponent<ObiSoftbody>().deformationResistance = 0.1f;
			}
		}
	}
}
