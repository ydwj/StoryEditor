using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler ,IPointerClickHandler
{
    public int id;
    public Image image;
    public Text text;
  

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.DispatchEvent<int>(NodeEvent.ChangeSelectID.ToString(), id);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.gameObject.SetActive(true );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.gameObject.SetActive(false);
    }

}
