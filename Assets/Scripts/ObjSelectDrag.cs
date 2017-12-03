using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSelectDrag : MonoBehaviour {

	private Draggable m_SelectedObject = null;
	private Vector2 m_SelectedOffset = Vector2.zero;

	private Vector2[] m_MouseVelHistory = new Vector2[10];
	private Vector2 m_LastMousePos = Vector2.zero;
	private int m_CurrentLmpIndex = 0;

	public bool isHoldingSomething { get { return m_SelectedObject != null; } }

	private ObjPlayerSelect m_Ops;

	public bool m_IsMainMenu = false;

	private void Awake() {
		m_Ops = FindObjectOfType<ObjPlayerSelect>();
	}

	// Update is called once per frame
	void Update() {
		if (PauseHandler.m_IsPaused) {
			return;
		}
		if (m_Ops != null) {
			if (m_Ops.isHoldingSomething) {
				return;
			}
		}

		if (m_SelectedObject == null) {
			selectObject();
			return;
		}

		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		m_SelectedObject.transform.position = mousePos + m_SelectedOffset;

		addCurrentMouse(mousePos);

		Debug.DrawRay(m_SelectedObject.transform.position, getMouseVel() * 10);

		//removed selected object
		if (Input.GetMouseButtonDown(0) && !m_SelectedObject.isInCollider()) {
			m_SelectedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			m_SelectedObject.GetComponent<Rigidbody2D>().velocity = getMouseVel() * 30;
			if (!m_IsMainMenu) {
				Destroy(m_SelectedObject.GetComponent<BoxCollider2D>());
				m_SelectedObject.gameObject.AddComponent<PolygonCollider2D>();
				m_SelectedObject.gameObject.AddComponent<Pickupable>();
				Destroy(m_SelectedObject);
			}else {
				m_SelectedObject.GetComponent<Collider2D>().isTrigger = false;
				m_SelectedObject = null;
			}
		}
	}

	private void selectObject() {
		//if mb 1 was not pressed that frame, then return
		if (!Input.GetMouseButtonDown(0)) {
			return;
		}
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		//only hit things that are in the default layer
		int layerMask = 1 << LayerMask.NameToLayer("NewObject");
		RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1, layerMask);

		if (hit) {
			Draggable draggableScript = hit.transform.GetComponent<Draggable>();
			if (draggableScript != null) {

				for (int i = 0; i < m_MouseVelHistory.Length; i++) {
					m_MouseVelHistory[i] = Vector2.zero;
				}
				m_CurrentLmpIndex = 0;
				m_LastMousePos = mousePos;

				m_SelectedObject = draggableScript;
				m_SelectedObject.m_IsBeingDragged = true;
				m_SelectedOffset = m_SelectedObject.transform.position - mousePos;
				m_SelectedObject.GetComponent<Collider2D>().isTrigger = true;
				//the rigidbody of the selected object
				Rigidbody2D rb2d = m_SelectedObject.GetComponent<Rigidbody2D>();
				rb2d.angularVelocity = 0;
				rb2d.velocity = Vector2.zero;
				rb2d.bodyType = RigidbodyType2D.Kinematic;
			}
		}
	}

	private void addCurrentMouse(Vector2 a_Pos) {
		Vector2 dist = a_Pos - m_LastMousePos;
		if (dist.magnitude > 0.1f) {
			m_MouseVelHistory[m_CurrentLmpIndex] = a_Pos - m_LastMousePos;
		} else {
			int index = m_CurrentLmpIndex - 1;
			if (index == -1) {
				index = m_MouseVelHistory.Length - 1;
			}
			m_MouseVelHistory[m_CurrentLmpIndex] = m_MouseVelHistory[index] * 0.9f;
		}
		m_LastMousePos = a_Pos;
		m_CurrentLmpIndex = (m_CurrentLmpIndex + 1) % m_MouseVelHistory.Length;
	}

	private Vector2 getMouseVel() {
		Vector2 totalVel = Vector2.zero;
		for (int i = 0; i < m_MouseVelHistory.Length; i++) {
			totalVel += m_MouseVelHistory[i];
		}
		return totalVel / m_MouseVelHistory.Length;
	}
}
