using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPlayerSelect : MonoBehaviour {

	/// <summary>
	/// which item are we looking at picking up
	/// </summary>
	private Pickupable m_LookingAt = null;
	private Pickupable m_CurrentlyPickedUp = null;
	
	public bool isHoldingSomething { get { return m_CurrentlyPickedUp != null; } }

	/// <summary>
	/// at what distance can we pick up the item
	/// </summary>
	public float m_PickupRange = 1.0f;
	/// <summary>
	/// at what distance do we hold the item
	/// </summary>
	public float m_HoldingRange = 1.5f;
	/// <summary>
	/// at what distance do we auto drop the item (it's too far away)
	/// </summary>
	public float m_DropRange = 1.8f;

	private ObjSelectDrag m_Osd;

	private void Awake() {
		m_Osd = FindObjectOfType<ObjSelectDrag>();
	}

	// Update is called once per frame
	void Update() {
		if (m_Osd.isHoldingSomething) {
			pickUpObject(null);
			return;
		}

		Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

		if (m_CurrentlyPickedUp == null) {
			RaycastHit2D simpleCheck = Physics2D.Raycast(transform.position, mouseDir, m_PickupRange, ~(1 << LayerMask.NameToLayer("Player")));

			if (simpleCheck) {
				pickUpObject(simpleCheck.transform.GetComponent<Pickupable>());

				if (Input.GetMouseButtonDown(0)) {
					m_CurrentlyPickedUp = m_LookingAt;
					m_CurrentlyPickedUp.m_Rigidbody.gravityScale = 0;
					m_CurrentlyPickedUp.m_Rigidbody.velocity = Vector2.zero;
				}
			} else {
				pickUpObject(null);
			}

			return;
		}

		bool shouldDrop = false;
		if (Vector3.Distance(m_CurrentlyPickedUp.transform.position,transform.position) > m_DropRange) {
			shouldDrop = true;
		}

		if (Input.GetMouseButtonDown(0)) {
			shouldDrop = true;
		}

		if (shouldDrop) { 
			m_CurrentlyPickedUp.m_Rigidbody.gravityScale = 1;
			m_CurrentlyPickedUp = null;
		}

	}

	private void FixedUpdate() {
		if (m_CurrentlyPickedUp) {
			Vector2 mouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

			//m_CurrentlyPickedUp contains an object
			Rigidbody2D rb = m_CurrentlyPickedUp.m_Rigidbody;
			rb.velocity *= 0.9f;
			rb.angularVelocity *= 0.9f;
			Vector3 dirToMousePos = (transform.position + (new Vector3(mouseDir.x, mouseDir.y, 0).normalized * m_HoldingRange)) - new Vector3(rb.position.x, rb.position.y, 0);
			//rb.position = Vector3.Lerp(rb.position, transform.position + new Vector3(mouseDir.x, mouseDir.y, 0).normalized, 5 * Time.deltaTime);
			rb.AddForce(dirToMousePos * 2, ForceMode2D.Impulse);
			//rb.AddForce(dirToMousePos * 20, ForceMode2D.Force);
		}
	}

	private void pickUpObject(Pickupable a_Object) {
		if (a_Object == m_LookingAt) {
			return;
		}

		if (m_LookingAt != null) {
			m_LookingAt.GetComponent<SpriteRenderer>().color = Color.white;
		}
		m_LookingAt = a_Object;

		if (m_LookingAt != null) {
			m_LookingAt.GetComponent<SpriteRenderer>().color = Color.green;
		}

	}
}
