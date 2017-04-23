using UnityEngine;
using System.Collections;

public class messagebox_script : MonoBehaviour {

	int message_timer=0;

	// Use this for initialization

	void Start () {
		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		message_timer--;
		//this.gameObject.transform.localScale = new Vector3 ((50 - message_timer)/10, (50 - message_timer)/10);

		this.GetComponent<TextMesh> ().characterSize = 2 - message_timer / 30.0f;

		if (message_timer < 0) {

			this.GetComponent<TextMesh> ().characterSize = 1;
			this.gameObject.SetActive (false);
		}
	}

	void OnEnable()
	{
		message_timer = 30;

	}
}
