using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPuller : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision) {
		//if this object is a pickupable
		if (collision.gameObject.GetComponent<Pickupable>()) {
			collision.gameObject.AddComponent<ObjPull>();
		}
	}


}
