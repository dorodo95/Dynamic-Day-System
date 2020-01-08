using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMoveObject : MonoBehaviour
{
    private Transform m_Transform;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Transform.position = new Vector3(Mathf.Sin(Time.time * 2) * 8.4f, Mathf.Sin(Time.time * 8) * 2, -2.17f);
    }
}
