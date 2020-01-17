using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CBVisualizer : MonoBehaviour
{
    CommandBuffer cb = null;

    private Light m_light;

    void OnEnable()
    {
        m_light = GetComponent<Light>();
    }
    
    void CreateCB()
    {
        if (m_light && cb == null)
        {
            cb = new CommandBuffer();
            cb.name = "GrabScrnSpcShadows";
            cb.SetGlobalTexture("_mScreenSpaceShadows", BuiltinRenderTextureType.CurrentActive);
            m_light.AddCommandBuffer(UnityEngine.Rendering.LightEvent.AfterScreenspaceMask, cb);
        }
    }

    private void OnRenderObject()
    {
        if (Camera.current != Camera.main)
            return;

        CreateCB();
    }

    void OnDisable()
    {
        if (m_light)
        {
            cb.Clear();
            cb.SetGlobalTexture("_mScreenSpaceShadows", BuiltinRenderTextureType.None);
            cb.ClearRenderTarget(true, true, Color.black);
            m_light.RemoveCommandBuffer(UnityEngine.Rendering.LightEvent.AfterScreenspaceMask, cb);
            Debug.Log("removed CB");
        }
    }
}
