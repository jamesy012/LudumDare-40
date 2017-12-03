using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public float m_Strength;
	public float m_ShakeLength;
	private float m_StartTime;
	private bool m_DoingShake;

	private Vector3 m_StartPos;

	private void Awake() {
		m_StartPos = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (m_DoingShake) {
			if(Time.time - m_StartTime > m_ShakeLength) {
				m_DoingShake = false;
			}
			transform.position = m_StartPos + UnityEngine.Random.insideUnitSphere * m_Strength;
		} else {
			transform.position = Vector3.Lerp(transform.position, m_StartPos, 1*Time.deltaTime);
		}
	}

	public void startShake() {
		m_DoingShake = true;
		m_StartTime = Time.time;
	}
}
