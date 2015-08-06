using UnityEngine;
using System.Collections;

public class SeePlayer : MonoBehaviour {

	public bool isSee;

	void Update () {
		if (isSee) 
			this.GetComponent<MeshRenderer> ().enabled = true;
		else
			this.GetComponent<MeshRenderer> ().enabled = false;
	}
}
