using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorials : MonoBehaviour
{
    private const string FOLDER_PATH = "Data";
    private const string TUTORIAL_FILE_PATH = "Tutorials";

    private const string TUTORIAL_OBJECT_VAR = "tutorial";
    private const string TUTORIAL_NAME_ATTRIBUTE_VAR = "id";

    private const string STAGE_OBJECT_VAR = "stage";
    private const string STAGE_ID_ATTRIBUTE_VAR = "id";
    private const string STAGE_CAMERA_POS_ATTRIBUTE_VAR = "camera_pos";
    private const string STAGE_HELP_TEXT_ATTRIBUTE_VAR = "help_text";

    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject helperObject;
    [SerializeField] private GameObject towerShop;
    [SerializeField] private WaveController waves;

    [SerializeField] private TextMeshProUGUI  helperText;

    // TODO: Refactor. Change Tutorials Reallisation
    public int maxStages = 0;
    public int currStage = 0;

    private Tutorial currTutor;

    public List<Tutorial> tutors = new List<Tutorial>();

    public void ReadTutorsFile()
    {
        TextAsset binary = Resources.Load<TextAsset>(FOLDER_PATH + "/" + TUTORIAL_FILE_PATH);

        if (binary == null)
        {
            Debug.LogError("[!] [Loading] [Tutorials] No Such File: %" + FOLDER_PATH + "/" + TUTORIAL_FILE_PATH + "% To Read!");
            return;
        }

        XmlTextReader reader = new XmlTextReader(new StringReader(binary.text));
        Debug.Log("[Loading] [Tutorials] Reading Tutorials File: " + FOLDER_PATH + "/" + TUTORIAL_FILE_PATH + ".xml");
        tutors.Clear();

        string currentTutorialId = null;
        Tutorial currentTutorial = null;
        int currentStageIndex = 0;

        while (reader.Read())
        {
            if (reader.IsStartElement(TUTORIAL_OBJECT_VAR))
            {
                currentTutorialId = reader.GetAttribute(TUTORIAL_NAME_ATTRIBUTE_VAR);
                currentTutorial = new Tutorial(currentTutorialId);
                currentTutorial.stages = new Dictionary<int, TutorialStage>();
                currentStageIndex = 0;
            }
            else if (reader.IsStartElement(STAGE_OBJECT_VAR) && currentTutorial != null)
            {
                int _stageId = Convert.ToInt32(reader.GetAttribute(STAGE_ID_ATTRIBUTE_VAR));
                string _stageCameraPos = reader.GetAttribute(STAGE_CAMERA_POS_ATTRIBUTE_VAR);
                string _stageHelpText = reader.GetAttribute(STAGE_HELP_TEXT_ATTRIBUTE_VAR);

                Vector3 cameraPosition = ParseCameraPosition(_stageCameraPos);
                TutorialStage _tutorialStage = new TutorialStage(_stageId, cameraPosition, _stageHelpText);

                currentTutorial.stages[_stageId] = _tutorialStage;
                currentStageIndex++;
            }
            else if (reader.NodeType == XmlNodeType.EndElement && reader.Name == TUTORIAL_OBJECT_VAR && currentTutorial != null)
            {
                Debug.Log("[Loading] [Tutorials] Loaded New Tutorial: " + currentTutorialId + " With Stages: " + currentTutorial.stages.Keys.Count);
                tutors.Add(currentTutorial);
                currentTutorial = null;
            }
        }

        reader.Close();
        Debug.Log("[Loading] [Tutorials] Reading Finished, Loaded: " + tutors.Count + " Tutorials!");
    }

    private Vector3 ParseCameraPosition(string cameraPosAttr)
    {
        string[] posValues = cameraPosAttr.Split(',');
        Vector3 cameraPosition = new Vector3(0f, 0f, 0f);
        if (posValues.Length == 2)
        {
            float x = float.Parse(posValues[0], CultureInfo.InvariantCulture);
            float y = float.Parse(posValues[1], CultureInfo.InvariantCulture);
            cameraPosition = new Vector3(x, y, 0f);
        }
        return cameraPosition;
    }

    public void StartTutorial(Tutorial _tut)
    {
        currTutor = _tut;
        cam.GetComponent<CameraMover>().canMove = false;
        maxStages = _tut.stages.Count-1;

        Debug.Log("[Gameplay] [Tutorials] Started Tutorial: %" + currTutor.tutorial_id + "%");

        StartCoroutine(MoveCamera(_tut.stages[currStage].camPos));
        DeactivateButtons();
    }

    IEnumerator MoveCamera(Vector3 targetPosition)
    {
        float moveSpeed = 5f;
        Vector3 initialPosition = cam.transform.position;
        float journeyLength = Vector3.Distance(initialPosition, targetPosition);
        float startTime = Time.time;

        while (Time.time - startTime < journeyLength / moveSpeed)
        {
            float distanceCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;

            cam.transform.position = Vector3.Lerp(initialPosition, targetPosition, fractionOfJourney);

            yield return null;
        }
        if (currTutor.stages[currStage].helpText!=null)
        {
            helperText.text = currTutor.stages[currStage].helpText;

            StartCoroutine(FadeInText());
        }
        cam.transform.position = targetPosition;

        yield return new WaitForSeconds(2f);
        
        if (currStage<maxStages)
        {
            currStage++;

            StartCoroutine(MoveCamera(currTutor.stages[currStage].camPos));
        }
        else if(currStage == maxStages && currTutor==tutors[0])
        {
            StartCoroutine(waves.StartWaves());

            Debug.Log("[Gameplay] [Tutorials] Finished Tutorial: %" + currTutor.tutorial_id + "%");
        }
        else if(currStage == maxStages)
        {
            Debug.Log("[Gameplay] [Tutorials] Finished Tutorial: %" + currTutor.tutorial_id + "%");

            ActivateButtons();

            currTutor = null;
        }
        
    }

    private IEnumerator FadeInText()
    {
        float fadeInDuration = 1.0f;
        float elapsedTime = 0f;
        Color originalColor = helperText.color;

        while (elapsedTime < fadeInDuration)
        {
            helperText.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);

            elapsedTime += Time.deltaTime;
            yield return null;

        }
        helperText.alpha = 1f;
        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOutText());
    }

    private IEnumerator FadeOutText()
    {
        float fadeOutDuration = 1.0f;
        float elapsedTime = 0f;
        Color originalColor = helperText.color;

        while (elapsedTime < fadeOutDuration)
        {
            helperText.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        helperText.alpha = 0f;
    }

    Button[] GetButtonsInChildrenRecursive(Transform parent)
    {
        List<Button> buttonList = new List<Button>();

        Button[] components = parent.GetComponents<Button>();
        buttonList.AddRange(components);

        foreach (Transform child in parent)
        {
            Button[] childButtons = GetButtonsInChildrenRecursive(child);
            buttonList.AddRange(childButtons);
        }

        return buttonList.ToArray();
    }

    public void ForceStopAllTutorials()
    {
        if (currTutor != null)
        {
            StopAllCoroutines();

            if (currTutor == tutors[0])
            {
                StartCoroutine(waves.StartWaves());
            }

            helperText.text = "";
            currTutor = null;

            ActivateButtons();
        }
    }

    void DeactivateButtons()
    {
        foreach (Button button in GetButtonsInChildrenRecursive(towerShop.gameObject.transform))
        {
            button.interactable = false;
        }
    }

    void ActivateButtons()
    {
        foreach (Button button in GetButtonsInChildrenRecursive(towerShop.gameObject.transform))
        {
            button.interactable = true;
        }
    }
}


public class Tutorial
{
    public string tutorial_id;
    public Dictionary<int, TutorialStage> stages = new Dictionary<int, TutorialStage>();
    public Tutorial(string _id)
    {
        tutorial_id = _id;
    }
}

public class TutorialStage
{
    public int id;
    public Vector3 camPos;
    public string helpText;

    public TutorialStage(int _id, Vector3 _pos, string _text)
    {
        id = _id;
        camPos = _pos;
        helpText = _text;
    }
}