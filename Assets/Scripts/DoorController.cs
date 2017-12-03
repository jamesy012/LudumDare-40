using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

	public float m_TimeItTakesToOpen = 4.0f;
	private float m_OpenTime;
	private bool m_IsMoving = true;
	private bool m_IsOpening = false;

	private void Start() {
		runDoorAnimation(false);
	}

	// Update is called once per frame
	void Update () {
		if (m_IsMoving) {
			float percentage = (Time.time - m_OpenTime) / m_TimeItTakesToOpen;

			if(percentage > 1) {
				m_IsMoving = false;
				percentage = 1;
			}

			if (!m_IsOpening) {
				percentage = 1 - percentage;
			}

			float newZRot = Mathf.Lerp(0, 90, percentage);
			Vector3 rot = transform.rotation.eulerAngles;
			rot.z = newZRot;
			transform.rotation = Quaternion.Euler(rot);
		}
	}

	public void runDoorAnimation(bool a_Open) {
		if(a_Open == m_IsOpening) {
			return;
		}
		float percentage = (Time.time - m_OpenTime) / m_TimeItTakesToOpen;
		percentage = Mathf.Clamp01(percentage);
		m_OpenTime = Time.time - (m_TimeItTakesToOpen * (1 - percentage));

		m_IsOpening = a_Open;
		m_IsMoving = true;


	}
}
