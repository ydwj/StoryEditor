using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using System;

public class TimelineManagner : MonoBehaviour
{

    public  PlayableDirector playableDirector;
    private void Awake()
    {
        playableDirector.Play();
    }
    public void PauseTimeline()
    {
        playableDirector.Pause();
    }


    public void PlayTimeline()
    {
        playableDirector.Play();
    }

    public void PPT2()
    {
        playableDirector.Play();
        StartCoroutine(Wait2Second());

    }

    IEnumerator Wait2Second()
    {
        yield return new WaitForSeconds(9f);
        playableDirector.Pause();
    }
    
    
}

