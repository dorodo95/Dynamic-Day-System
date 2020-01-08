using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DebugMoveCameraWithEditor : MonoBehaviour {
#if UNITY_EDITOR
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(SceneView.lastActiveSceneView.camera != null)
        {
            gameObject.transform.position = SceneView.lastActiveSceneView.camera.gameObject.transform.position;
            gameObject.transform.rotation = SceneView.lastActiveSceneView.camera.gameObject.transform.rotation;
        }

    }

#endif
}

