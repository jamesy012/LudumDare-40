using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour {

	private AudioSource m_As;
	static bool m_IsMuted = false;
	private const string MUTED_KEY = "AUDIO_MUTE";

	public UnityEngine.UI.Text m_MutedText;

	public AudioClip[] m_AudioClips;
	private static AudioClip m_CurrentClip = null;

	private void Awake() {
		int muted = PlayerPrefs.GetInt(MUTED_KEY, 0);
		m_IsMuted = muted == 1;

		m_As = GetComponent<AudioSource>();


		setCurrentClip();
		updateAudioSource();
		updateMutedText();
	}

	public void muteMusic() {
		m_IsMuted = !m_IsMuted;
		PlayerPrefs.SetInt(MUTED_KEY, m_IsMuted ? 1 : 0);

		updateAudioSource();
		updateMutedText();
	}

	private void setCurrentClip() {
		AudioClip nextClip = null;
		while(nextClip == m_CurrentClip || nextClip == null) {
			nextClip = m_AudioClips[Random.Range(0, m_AudioClips.Length)];
		}
		m_As.clip = nextClip;
		m_CurrentClip = nextClip;
		m_As.Play();
	}

	private void updateMutedText() {
		if (m_MutedText != null) {
			m_MutedText.text = m_IsMuted ? "Unmute" : "Mute";
		}
	}

	private void updateAudioSource() {
		if (m_IsMuted) {
			m_As.Pause();
			m_As.volume = 0;
		} else {
			m_As.volume = 1;
			m_As.UnPause();
		}
	}
}
