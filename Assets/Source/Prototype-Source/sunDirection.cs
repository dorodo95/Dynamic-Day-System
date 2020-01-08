using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class sunDirection : MonoBehaviour
{
    public Transform m_trans;
    public Transform m_moonPhaseTrans;
    int zAxisID;
    int xAxisID;
    int moonPhaseID;

    private void OnEnable()
    {
        m_trans = GetComponent<Transform>();
        xAxisID = Shader.PropertyToID("_xAxis");
        zAxisID = Shader.PropertyToID("_zAxis");
        moonPhaseID = Shader.PropertyToID("_moonPhase");
    }

    private void Update()
    {
        Shader.SetGlobalVector(zAxisID, m_trans.transform.forward);
        Shader.SetGlobalVector(xAxisID, m_trans.transform.right);
        Shader.SetGlobalVector(moonPhaseID, m_moonPhaseTrans.transform.forward);
    }
}
