using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;

public class createFullScreenQuad : MonoBehaviour
{
    public Material renderMat;
    public bool turnOnOverlayQuad = true;
    private CommandBuffer cb = null;
    private Camera cam;
    private CameraEvent cacheCamEvent;
    private int cacheDownsampleLevel;
    private enum FrameRate : byte
    {
        none = 0,
        half = 30,
        full = 60
    };

    [Range(-1, -16)]
    [SerializeField] private int DownsampleLevel = -1;

    [SerializeField] private CameraEvent m_CameraEvent = CameraEvent.BeforeSkybox;
    [SerializeField] private FrameRate m_frameRate = FrameRate.full;

    private void Awake()
    {
        Application.targetFrameRate = (byte)m_frameRate;
    }

    private void OnEnable()
    {
        cam = GetComponent<Camera>();

        if (cam.renderingPath == RenderingPath.Forward)
            cam.forceIntoRenderTexture = true;

        else cam.forceIntoRenderTexture = false;
        
        cacheCamEvent = m_CameraEvent;
        cacheDownsampleLevel = DownsampleLevel;

        CreateCB();

    }

    private void OnDisable()
    {
        if (cam)
        {
            cb.Clear();
            cam.RemoveCommandBuffer(m_CameraEvent, cb);
            cb = null;
        }
    }

    private void CreateCB()
    {
        if (cam && cb == null)
        {
            cb = new CommandBuffer();
            cb.name = "PREVIOUS FRAME";
            int texID = Shader.PropertyToID("_TexID");
            cb.GetTemporaryRT(texID, DownsampleLevel, DownsampleLevel, 24, FilterMode.Bilinear);
            cb.Blit(BuiltinRenderTextureType.CameraTarget, texID);
            cb.ReleaseTemporaryRT(texID);

            cam.AddCommandBuffer(m_CameraEvent, cb);
        }
    }

    private void RefreshCB()
    {

        if (cb != null)
        {
            if (cam.renderingPath == RenderingPath.Forward)
                cam.forceIntoRenderTexture = true;

            else cam.forceIntoRenderTexture = false;

            cb.Clear();
            cam.RemoveCommandBuffer(cacheCamEvent, cb);
            cb = null;

            cacheCamEvent = m_CameraEvent;
            cacheDownsampleLevel = DownsampleLevel;

            CreateCB();

            Debug.Log("Called RefreshCB");
        }
    }

    private void RefreshFrameRate()
    {
        Application.targetFrameRate = (byte)m_frameRate;
        Debug.Log(Application.targetFrameRate);
    }

    void OnRenderObject()
    {
            
        if (Application.targetFrameRate != (byte)m_frameRate)
        {
            RefreshFrameRate();
        }

        if (cacheCamEvent != m_CameraEvent)
        {
            RefreshCB();
        }

        if (cacheDownsampleLevel != DownsampleLevel)
        {
            RefreshCB();
        }

        if (Camera.current != Camera.main)
            return;



    }
}
