using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIfWebBuild : MonoBehaviour {
#if UNITY_WEBGL
	private void Awake() {
		Destroy(gameObject);
	}
#endif
}
