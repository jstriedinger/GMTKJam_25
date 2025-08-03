using System;
using DG.Tweening;
using UnityEngine;

public class LigthFlicker : MonoBehaviour
{
    private Light _lightSource;
    Tween _tween;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        _lightSource = GetComponent<Light>();
    }

    void Start()
    {
        if (_lightSource)
        {
            float finalIntensity = _lightSource.intensity + 10;
            _tween = _lightSource.DOIntensity(finalIntensity, 1)
                .SetLoops(-1, LoopType.Yoyo) 
                .SetEase(Ease.InOutSine); 
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
