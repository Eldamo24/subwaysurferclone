using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1,1)] private float curveX;
    [SerializeField, Range(-1,1)] private float curveY;
    [SerializeField] private Material[] materials;
    private float lerpDuration = 2f;

    private void OnEnable()
    {
        StartCoroutine("Curve");
    }

    IEnumerator Curve()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            float randomNumberX = Random.Range(-1.0f, 1.0f);
            float randomNumberY = Random.Range(-1.0f, 1.0f);
            float timeElapsed = 0;
            float startValueX = materials[0].GetFloat("_Curve_X");
            float startValueY = materials[0].GetFloat("_Curve_Y");
            while (timeElapsed < lerpDuration)
            {
                float lerpValueX = Mathf.Lerp(startValueX, randomNumberX, timeElapsed / lerpDuration);
                float lerpValueY = Mathf.Lerp(startValueY, randomNumberY, timeElapsed / lerpDuration);
                foreach (var m in materials)
                {
                    m.SetFloat(Shader.PropertyToID("_Curve_X"), lerpValueX);
                    m.SetFloat(Shader.PropertyToID("_Curve_Y"), lerpValueY);
                }
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            foreach (var m in materials)
            {
                m.SetFloat(Shader.PropertyToID("_Curve_X"), randomNumberX);
                m.SetFloat(Shader.PropertyToID("_Curve_Y"), randomNumberY);
            }
        }
    }

    public void ResetValues()
    {
        foreach (var m in materials)
        {
            m.SetFloat("_Curve_X", 0);
            m.SetFloat("_Curve_Y", 0);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

   




}
