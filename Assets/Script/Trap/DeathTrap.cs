using UnityEngine;
using System.Collections;

public class DeathTrap : MonoBehaviour {

	public Light light_;
	public AudioSource bip;
	public AudioSource boom;

	bool t = false;
	bool b = true;
	float i = 0;
	void Update(){
		if (t) {
			if (i > 1)
				Destroy(this.gameObject);
			else
				i += Time.deltaTime;
		} else {
			if (b) {
				if (i > 0.1f) {
					i = 0;
					b = false;
					light_.enabled = false;
				} else {
					i += Time.deltaTime;
				}
			} else {
				if (i > 3) {
					bip.Play ();
					i = 0;
					b = true;
					light_.enabled = true;
				} else {
					i += Time.deltaTime;
				}
			}
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.CompareTag ("Player")) {
			boom.Play ();
			Destroy (col.gameObject);
			t = true;
			i = 0;
		}
	}
}
