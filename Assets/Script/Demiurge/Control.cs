using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		foreach (GameObject b in GameObject.FindGameObjectsWithTag ("Player")) {
			b.GetComponent<Renderer> ().enabled = false; 
		}
		foreach (GameObject b in GameObject.FindGameObjectsWithTag ("Monster")) {
			List<GameObject> tmp = b.GetComponent<IA> ().SeePlayer;
			if (tmp != null)
				foreach (GameObject s in tmp) {
					s.GetComponent<Renderer> ().enabled = true;
				}
		}

		foreach (GameObject b in GameObject.FindGameObjectsWithTag ("Sentry")) {
			List<GameObject> tmp = b.GetComponent<Static_IA> ().SeePlayer;
			if (tmp != null)
				foreach (GameObject s in tmp) {
					s.GetComponent<Renderer> ().enabled = true;

				}
		}
	}
}
