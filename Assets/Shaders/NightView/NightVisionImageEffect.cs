using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class NightVisionImageEffect : MonoBehaviour
{
    public Shader shader;
    [Range(0f, 1f)]
    public float luminance = 0.44f;

    private float lensRadius = 1f;

    private Material material;
    private float targetLensRadius;
    private float transitionDuration = 2.0f;

    public bool isTransitioning = false;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (shader != null)
        {
            if (!material)
            {
                material = new Material(shader);
            }
            material.SetVector("_Luminance", new Vector4(luminance, luminance, luminance, luminance));
            material.SetFloat("_LensRadius", lensRadius);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private IEnumerator TransitionLensRadius(float target)
    {
        isTransitioning = true;
        material.SetInt("_IsTransitioning", 1);
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            lensRadius = Mathf.Lerp(0.25f, target, elapsedTime / transitionDuration);
            elapsedTime = Time.time - startTime;
            yield return null;
        }

        lensRadius = target;
        isTransitioning = false;
        material.SetInt("_IsTransitioning", 0);
    }

    // Butona tıklandığında bu fonksiyon çağrılır.
    public void StartLensRadiusTransition()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionLensRadius(1.0f));
        }
    }
}
