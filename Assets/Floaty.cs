using DG.Tweening;
using UnityEngine;

public class Floaty : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float initY = transform.position.y;
        transform.DOMoveY(initY + .15f, 1)
            .SetLoops(-1, LoopType.Yoyo) // Loop the movement indefinitely
            .SetEase(Ease.InOutSine); // Smooth easing for the movement
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
