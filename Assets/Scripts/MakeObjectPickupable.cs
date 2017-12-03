using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjectPickupable : MonoBehaviour {

	public string m_LookFor;
	public string m_ChangeTo;
	private LayerMask m_LmLookFor;
	private LayerMask m_LmChangeTo;

	// Use this for initialization
	void Awake () {
		m_LmLookFor = LayerMask.NameToLayer(m_LookFor);
		m_LmChangeTo = LayerMask.NameToLayer(m_ChangeTo);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		entered(collision.gameObject);
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		entered(collision.gameObject);
	}

	private void entered(GameObject a_Go) {
		if (a_Go.layer == m_LmLookFor) {
			Draggable dragScript = a_Go.GetComponent<Draggable>();
			if (dragScript.m_IsBeingDragged){
				return;
			}
			a_Go.layer = m_LmChangeTo;
			Destroy(dragScript);
			a_Go.AddComponent<Pickupable>();
		}
	} 


}
