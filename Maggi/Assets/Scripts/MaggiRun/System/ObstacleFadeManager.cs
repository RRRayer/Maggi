using UnityEngine;
using System.Collections.Generic;

public class ObstacleFadeManager : MonoBehaviour
{
    public Transform player;
    public float fadeDistance = 5f;
    public float targetAlphaNear = 1f;
    public float targetAlphaFar = 0.2f;
    public float fadeSpeed = 3f; // 알파 변화 속도

    private class FadeData
    {
        public Renderer renderer;
        public float currentAlpha = 1f;
    }

    private List<FadeData> fadeTargets = new();

    private void Start()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (var obj in obstacles)
        {
            var r = obj.GetComponent<Renderer>();
            if (r != null)
            {
                fadeTargets.Add(new FadeData { renderer = r });
            }
        }
    }

    private void Update()
    {
        foreach (var fade in fadeTargets)
        {
            if (fade.renderer == null) continue;

            Vector3 toPlayer = player.position - fade.renderer.transform.position;
            float dot = Vector3.Dot(toPlayer, player.forward);

            float targetAlpha = dot > fadeDistance ? targetAlphaFar : targetAlphaNear;

            // 알파값 부드럽게 변화 (Lerp)
            fade.currentAlpha = Mathf.Lerp(fade.currentAlpha, targetAlpha, Time.deltaTime * fadeSpeed);
            ApplyAlpha(fade.renderer, fade.currentAlpha);
        }
    }

    private void ApplyAlpha(Renderer rend, float alpha)
    {
        if (rend == null || rend.material == null) return;

        Color color = rend.material.color;
        color.a = alpha;
        rend.material.color = color;
    }

}