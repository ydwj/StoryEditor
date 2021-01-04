using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.Playables;

public class SwitchScene : MonoBehaviour
{

   
    public TimelineManagner timelineManagner;

    private AsyncOperation asyncOperation;
    
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    
    }
    public void GoNextScene()
    {
        asyncOperation= SceneManager.LoadSceneAsync("Chapter2", LoadSceneMode.Single);

        timelineManagner.playableDirector = null;
        EventManager.DispatchEvent<int>(NodeEvent.ChangeGraph.ToString(), 0);
        StartCoroutine(ChangeDir());

    }

    IEnumerator ChangeDir()
    {
        while (!asyncOperation.isDone)
        {
            yield return 0;
        }
        PlayableDirector director = GameObject.Find("Timeline2").GetComponent<PlayableDirector>();
        Debug.Log("播放");
        timelineManagner.playableDirector = director;
        director.Play();

    }
   

}
