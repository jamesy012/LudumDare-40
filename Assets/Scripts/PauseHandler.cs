using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour {

	public static bool m_IsPaused = false;

	// Use this for initialization
	void Awake () {
		m_IsPaused = false;
		updateTimeScale();
	}

	public void flipPauseState() {
		m_IsPaused = !m_IsPaused;
		updateTimeScale();
	}


	private void updateTimeScale() {
		if (m_IsPaused) {
			Time.timeScale = 0;
		}else {
			Time.timeScale = 1;
		}
	}
}
