using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RenderParticlesToRT : MonoBehaviour
{
    private int camPosition = Shader.PropertyToID("_camPosition");
    private int camSize = Shader.PropertyToID("_orthoCamSize");
    private Camera m_camera;
    public Transform target;

    void Start()
    {
        m_camera = gameObject.GetComponent<Camera>();
    }

    void Update()
    {
        transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 10f, target.transform.position.z);
        Shader.SetGlobalFloat(camSize, m_camera.orthographicSize);
        Shader.SetGlobalVector(camPosition, transform.position);
    }
}
