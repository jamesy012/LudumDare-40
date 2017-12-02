using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour {

	public Rigidbody2D m_Rigidbody;

	private void Awake() {
		m_Rigidbody = GetComponent<Rigidbody2D>();
	}
}
