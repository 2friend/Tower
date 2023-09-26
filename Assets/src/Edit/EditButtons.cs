using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditButtons : MonoBehaviour
{
    public GameObject gridCreator;
    public CameraMover cameraMover;
    public WaveController waves;
    public Tutorials tutors;

    public GameObject tutorialsButtons;
    public GameObject pathButtons;

    public List<GameObject> disableButtons = new List<GameObject>();

    public List<GameObject> editorsObject = new List<GameObject>();

    public TextMeshProUGUI editText;

    public int editorTypeOpened = 0; // 0 - Disabled, 1 - PathEditor, 2 - TutorialsEditor

    public bool inEditorMode;

    private void Start()
    {
        cameraMover = GameObject.Find("CameraController").GetComponent<CameraMover>();
        editText.text = "";
        AddComponentToButtons();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.F1) && !inEditorMode && editorTypeOpened == 0)
        {
            Debug.Log("[%] [Editor] Started Path Creator Editor!");
            ActivatePathCreatorMode();
        }
        else if(Input.GetKeyUp(KeyCode.F1) && inEditorMode && editorTypeOpened == 1)
        {
            Debug.Log("[%] [Editor] Stoped Path Creator Editor!");
            DisablePathCreatorMode();
        }

        if (Input.GetKeyUp(KeyCode.F2) && !inEditorMode && editorTypeOpened == 0)
        {
            Debug.Log("[%] [Editor] Started Tutorials Creator Editor!");
            ActivateTutorialsCreatorMode();
        }
        else if (Input.GetKeyUp(KeyCode.F2) && inEditorMode && editorTypeOpened==2)
        {
            Debug.Log("[%] [Editor] Stoped Tutorials Creator Editor!");
            DisableTutorialsCreatorMode();
        }

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.T))
        {
            Debug.Log("[%] [Editor] Stoped All Tutorials!");
            tutors.ForceStopAllTutorials();
        }
    }

    void ActivatePathCreatorMode()
    {
        inEditorMode = true;
        editorTypeOpened = 1;
        pathButtons.SetActive(true);
        editText.text = "PathCreator";
        gridCreator.SetActive(true);
        DeactivateAllGameObjects();
    }

    void DisablePathCreatorMode()
    {
        inEditorMode = false;
        editorTypeOpened = 0;
        pathButtons.SetActive(false);
        editText.text = "";
        gridCreator.SetActive(false);
        ActivateAllGameObjects();
    }

    void ActivateTutorialsCreatorMode()
    {
        inEditorMode = true;
        editorTypeOpened = 2;
        tutorialsButtons.SetActive(true);
        editText.text = "TutorialCreator";
        DeactivateAllGameObjects();
    }

    void DisableTutorialsCreatorMode()
    {
        inEditorMode = false;
        editorTypeOpened = 0;
        tutorialsButtons.SetActive(false);
        editText.text = "";
        ActivateAllGameObjects();
    }

    void ActivateAllGameObjects()
    {
        foreach (GameObject obj in disableButtons)
        {
            obj.SetActive(true);
        }
    }

    void DeactivateAllGameObjects()
    {
        foreach (GameObject obj in disableButtons)
        {
            obj.SetActive(false);
        }
    }

    private void AddComponentToButtons()
    {
        foreach(GameObject obj in editorsObject)
        {
            obj.AddComponent<EditButton>();
        }
    }
}

class EditButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private EditButtons editButtons;

    private void Start()
    {
        editButtons = GameObject.Find("EditorButtons").GetComponent<EditButtons>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (editButtons.inEditorMode)
            editButtons.gridCreator.GetComponent<GridCreator>().canBePressed = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (editButtons.inEditorMode)
            editButtons.gridCreator.GetComponent<GridCreator>().canBePressed = true;
    }
}
