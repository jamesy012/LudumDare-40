using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCommon : MonoBehaviour {

	public void restartLevel() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void loadLevel(int a_Index) {
		SceneManager.LoadScene(a_Index);
	}

	public void flipActive(GameObject a_Object) {
		a_Object.SetActive(!a_Object.activeInHierarchy);
	}
}
