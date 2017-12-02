using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPull : MonoBehaviour {

	private void Start() {
		Destroy(gameObject, 5);
		Destroy(gameObject.GetComponent<Pickupable>());
		Destroy(gameObject.GetComponent<Rigidbody2D>());
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;

	}

	// Update is called once per frame
	void Update () {
		transform.position = transform.position + new Vector3(-5 * Time.deltaTime, 0, 0);
	}
}
