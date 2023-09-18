using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GridCreator gridCreator;

    private void Start()
    {
        gridCreator = GameObject.Find("GridCreator").GetComponent<GridCreator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        gridCreator.canBePressed = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gridCreator.canBePressed = true;
    }

    public void OpenEditorMenu()
    {

    }
}
