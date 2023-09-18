using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GridCreator gridCreator;
    public CameraMover cameraMover;

    public bool inEditorMode;

    private void Start()
    {
        inEditorMode = false;
        if (inEditorMode)
            gridCreator = GameObject.Find("GridCreator").GetComponent<GridCreator>();
        cameraMover = GameObject.Find("CameraController").GetComponent<CameraMover>();
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inEditorMode)
        {
            gridCreator.canBePressed = false;
            cameraMover.canMove = false;
        }
        else
            cameraMover.canMove = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inEditorMode)
        {
            gridCreator.canBePressed = true;
            cameraMover.canMove = true;
        }
        else
            cameraMover.canMove = true;
    }

    public void OpenEditorMenu()
    {

    }
}
