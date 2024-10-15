using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookBlock : MonoBehaviour
{
    [SerializeField] private float _destroyTime = 3f;
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Hooking()
    {
        StartCoroutine(InvisibleAlpha());
    }

    private IEnumerator InvisibleAlpha()
    {
        float time = 0;
        Color newColor = _spriteRenderer.color;

        while (time < _destroyTime)
        {
            newColor.a = Mathf.Lerp(1, 0, time / _destroyTime);
            _spriteRenderer.color = newColor;

            time += Time.deltaTime;
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
}
