using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePerson : MonoBehaviour
{

    public GameObject personBack;
    public RectTransform dialoguePanel;
    // Start is called before the first frame update
    void Start()
    {
        personBack = GameObject.Find("PersonBack");
        dialoguePanel = GameObject.Find("DialoguePanel").GetComponent<RectTransform>();
    }

    public void Hide()
    {
        personBack.gameObject.SetActive(false);
        dialoguePanel.anchoredPosition3D = new Vector3(190, -298, 0);
    }

    public void NoHide()
    {
        personBack.gameObject.SetActive(true);
        dialoguePanel.anchoredPosition3D = new Vector3(-10,-294,0);
    }
}
