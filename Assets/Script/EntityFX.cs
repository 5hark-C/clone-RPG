using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



//颜色特效脚本
public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;


    [Header("异常状态颜色")]
    [SerializeField] private Color[] chillColor;
    [SerializeField] private Color[] igniteColor;
    [SerializeField] private Color[] shockColor;

    private void Start()
    {
        sr=GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    //透明化函数
    public void MakeTransprent(bool _transprent)
    {
        if (_transprent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }


    private IEnumerator FlashFX()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;

        sr.color = Color.white;
        yield return new WaitForSeconds(flashDuration);

        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }
    }

    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void IgniteFxFor(float _second)
    {
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    public void ChillFxFor(float _second)
    {
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }


    public void ShockFxFor(float _second)
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColor[0])
            sr.color = igniteColor[0];
        else
            sr.color = igniteColor[1];

    }

    private void ChillColorFx()
    {
        if (sr.color != chillColor[0])
            sr.color = chillColor[0];
        else
            sr.color = chillColor[1];
    }

    private void ShockColorFx()
    {
        if (sr.color != shockColor[0])
            sr.color = shockColor[0];
        else
            sr.color = shockColor[1];
    }
 
}
