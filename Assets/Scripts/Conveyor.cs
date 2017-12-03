using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour {

	public float m_Speed = 1.0f;

	private void OnCollisionStay2D(Collision2D collision) {
		collision.transform.GetComponent<Rigidbody2D>().AddForce(Vector2.right * m_Speed);
	}

}
