using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;

    private void Start()
    {
        StartCoroutine(StartTimeline());
    }

    private IEnumerator StartTimeline()
    {
        for (int i = 3; i >= 1; --i)
        {
            Debug.Log(i + " !!");
            yield return new WaitForSeconds(1);
        }

        playableDirector.Play();
    }
}
