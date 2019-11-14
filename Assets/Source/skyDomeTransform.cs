using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyDomeTransform : MonoBehaviour {

    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private float _offsetY;
    private Vector3 _finalTransform;

    private void Awake()
    {
        if (_cameraObject == null)
        {
            _cameraObject = FindObjectOfType<Camera>().gameObject;
        }
    }

    // Update is called once per frame
    void Update () {
        _finalTransform = _cameraObject.transform.position;
        _finalTransform.y += _offsetY;
        gameObject.transform.position = _finalTransform;
    }
}
