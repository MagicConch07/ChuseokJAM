using System.Collections;
using Cinemachine;
using UnityEngine;

public enum ZoomMode
{
    Out,
    In
}

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _followCam;
    [SerializeField] private float _followCamUpSize = 30;
    [SerializeField] private float _duration = 0.3f;
    private float _followOriginalSize = 0;

    private void Awake()
    {
        _followOriginalSize = _followCam.m_Lens.OrthographicSize;
    }

    private void Start()
    {
        _followCam.m_Lens.OrthographicSize = _followCamUpSize;
    }

    /// <summary>
    /// Camera Zoom
    /// </summary>
    /// <param name="zoom">Zoom Mode</param>
    public void Zoom(ZoomMode zoom)
    {
        switch (zoom)
        {
            case ZoomMode.Out:
                StartCoroutine(CamLerp(_followCamUpSize));
                break;
            case ZoomMode.In:
                StartCoroutine(CamLerp(_followOriginalSize));
                break;
        }
    }
    
    /// <summary>
    /// Custom Duration Camera Zoom
    /// </summary>
    /// <param name="zoom">Zoom Mode</param>
    /// <param name="duration">Custom Duration</param>
    public void Zoom(ZoomMode zoom, float duration)
    {
        switch (zoom)
        {
            case ZoomMode.Out:
                StartCoroutine(CamLerp(_followCamUpSize, duration));
                break;
            case ZoomMode.In:
                StartCoroutine(CamLerp(_followOriginalSize, duration));
                break;
        }
    }

    /// <summary>
    /// Cam Zoom Lerp
    /// </summary>
    /// <param name="size">Cam Zoom Size</param>
    /// <returns></returns>
    private IEnumerator CamLerp(float size)
    {
        float time = 0;
        float startValue = _followCam.m_Lens.OrthographicSize;
        
        while (time < _duration)
        {
            _followCam.m_Lens.OrthographicSize = Mathf.Lerp(startValue, size, time / _duration);
            
            time += Time.deltaTime;
            yield return null;
        }
        _followCam.m_Lens.OrthographicSize = size;
    }
    
    /// <summary>
    /// CustomDuration Lerp
    /// </summary>
    /// <param name="size">Cam Zoom Size</param>
    /// <param name="duration">Cam Zoom Duration</param>
    /// <returns></returns>
    private IEnumerator CamLerp(float size, float duration)
    {
        float time = 0;
        float startValue = _followCam.m_Lens.OrthographicSize;
        
        while (time < duration)
        {
            _followCam.m_Lens.OrthographicSize = Mathf.Lerp(startValue, size, time / duration);
            
            time += Time.deltaTime;
            yield return null;
        }
        _followCam.m_Lens.OrthographicSize = size;
    }

    public void FollowTarget(Transform target)
    {
        _followCam.Follow = target;
    }

    public void ZoomAndFollow(ZoomMode zoom, float duration = 0, Transform target = null)
    {
        if (duration <= 0)
            duration = _duration;
        
        switch (zoom)
        {
            case ZoomMode.Out:
                StartCoroutine(CamZoomAndFollowLerp(_followCamUpSize, duration, target));
                break;
            case ZoomMode.In:
                StartCoroutine(CamZoomAndFollowLerp(_followOriginalSize, duration,target));
                break;
        }
    }
    
    private IEnumerator CamZoomAndFollowLerp(float size, float duration, Transform target)
    {
        float time = 0;
        float startValue = _followCam.m_Lens.OrthographicSize;
        
        while (time < duration)
        {
            _followCam.m_Lens.OrthographicSize = Mathf.Lerp(startValue, size, time / duration);
            
            time += Time.deltaTime;
            yield return null;
        }
        _followCam.m_Lens.OrthographicSize = size;
        FollowTarget(target);
    }
}
