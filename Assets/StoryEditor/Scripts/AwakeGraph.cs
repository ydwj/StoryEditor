using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeGraph:MonoBehaviour
{
    PlayDiaLogueGraph aaa;
    private void Start()
    {
        aaa = GameObject.Find("PlayDialogueGraph").GetComponent<PlayDiaLogueGraph>();
    }
    public void AwakeDia()
    {
    
        if (aaa)
        {
         
            aaa.AwakePlayDialogeGraph();
        }
    }

    public void GoNextChapter()
    {
    
        EventManager.DispatchEvent(NodeEvent.ChangeGraph.ToString());
        
    }

  
 
}
