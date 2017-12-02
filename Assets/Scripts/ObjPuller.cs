using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPuller : MonoBehaviour {

	private GameplayManager m_Gpm;

	private void Awake() {
		m_Gpm = FindObjectOfType<GameplayManager>();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		//if this object is a pickupable
		if (collision.gameObject.GetComponent<Pickupable>()) {
			m_Gpm.objectSent(collision.gameObject);
			collision.gameObject.AddComponent<ObjPull>();
		}
	}


}
