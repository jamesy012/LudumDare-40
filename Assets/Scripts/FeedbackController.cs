using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackController : MonoBehaviour {

	struct Ani {
		public Animator m_Animation;
		public float m_LastStartTime;
	}

	private Ani[] m_Animations;

	// Use this for initialization
	void Awake () {
		m_Animations = new Ani[transform.childCount];

		for(int i = 0; i < transform.childCount; i++) {
			m_Animations[i].m_Animation = transform.GetChild(i).GetComponent<Animator>();
			m_Animations[i].m_LastStartTime = -9999;
		}
	}

	private void Update() {

	}

	public void addFeedback(Sprite a_Sprite, bool a_Correct) {
		for(int i = 0; i < m_Animations.Length; i++) {
			//0 for no animation
			if (Time.time - m_Animations[i].m_LastStartTime > 3.0f) {
				m_Animations[i].m_LastStartTime = Time.time;
				m_Animations[i].m_Animation.transform.GetChild(1).GetChild(0).gameObject.SetActive(a_Correct);
				m_Animations[i].m_Animation.transform.GetChild(1).GetChild(1).gameObject.SetActive(!a_Correct);
				m_Animations[i].m_Animation.transform.GetChild(2).GetComponent<UnityEngine.UI.Image>().sprite = a_Sprite;
				m_Animations[i].m_Animation.SetTrigger("RunAnimation");
				break;
			}
		}
	}
}
