using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {
	
	void Update () {
		foreach (GameObject b in GameObject.FindGameObjectsWithTag ("Runner")) {
			b.GetComponent<SeePlayer>().isSee = false;
		}
	}
}
