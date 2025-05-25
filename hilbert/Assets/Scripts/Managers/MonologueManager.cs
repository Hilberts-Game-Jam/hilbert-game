using System.Collections;
using System.Collections.Generic;
using System.IO;
using Scripts.Core;
using TMPro;
using UnityEngine;

namespace Scripts.Managers
{
    [System.Serializable]
    public class MonologueData
    {
        public List<string> DOOR;
        public List<string> START;
        public List<string> FIRST;
        
        public List<string> BROKEN_BED;
        public List<string> BROKEN_CHAIR;
        public List<string> BROKEN_SHELF;
        public List<string> BROKEN_SUITCASE;
        public List<string> BROKEN_TABLE;
        
        public List<string> BURNED_BED;
        public List<string> BURNED_CHAIR;
        public List<string> BURNED_SHELF;
        public List<string> BURNED_SUITCASE;
        public List<string> BURNED_TABLE;
        
        public List<string> MOIST_BED;
        public List<string> MOIST_CHAIR;
        public List<string> MOIST_SHELF;
        public List<string> MOIST_SUITCASE;
        public List<string> MOIST_TABLE;
        
        public List<string> NONE;
        
        public List<string> NORMAL_BED;
        public List<string> NORMAL_CHAIR;
        public List<string> NORMAL_SHELF;
        public List<string> NORMAL_SUITCASE;
        public List<string> NORMAL_TABLE;
    }
    
    // todo: add delay between monologue calls
    public class MonologueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogPrefab;
        [SerializeField] private float dialogDuration = 2f;
        [SerializeField] private GameObject dialogCanvasParent;
        [SerializeField] private string monologueFilePath = "Assets/Monologues/monologues.json";

        
        private GameObject _currentDialogInstance;
        private TextMeshProUGUI _dialogText;
        private TextMeshProUGUI _speakerNameText;
        private Coroutine _autoCloseCoroutine;
        private MonologueData _monologueData;
        
        private static bool _firstStartRoom = true;
        
        

        private void Awake()
        {
            LoadMonologueData();
            DontDestroyOnLoad(dialogCanvasParent);
        }
        
        private void LoadMonologueData()
        {
            try
            {
                Debug.Log("Loading monologue data from: " + monologueFilePath);
                string jsonText = File.ReadAllText(monologueFilePath);
                // print jsonText for debugging
                Debug.Log("Monologue JSON content: " + jsonText);
                
                _monologueData = JsonUtility.FromJson<MonologueData>(jsonText);
                
                // print loaded data for debugging with for loop
                if (_monologueData != null)
                {
                    Debug.Log("Printing monologue data:");
                    foreach (var list in _monologueData.GetType().GetFields())
                    {
                        var fieldValue = list.GetValue(_monologueData) as List<string>;
                        if (fieldValue != null)
                        {
                            Debug.Log($"{list.Name}: {string.Join(", ", fieldValue)}");
                        }
                        else
                        {
                            Debug.Log($"{list.Name} is null or not a List<string>");
                        }
                    }
                }
                
                
                if (_monologueData == null)
                {
                    Debug.LogError("Failed to deserialize monologue data");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading monologues: {e.Message}");
                _monologueData = new MonologueData();
            }
        }

        private void OnEnable()
        {
            Events.OnInteraction += OnInteraction;
            
        }

        private void OnDisable()
        {
            Events.OnInteraction -= OnInteraction;
        }
        
        private void OnInteraction(ClassesAndEnums.InteractionType interactionType)
        {

            if (interactionType == ClassesAndEnums.InteractionType.DOOR)
            {
                CloseDialog();
            }
            else
            {
                DisplayDialog(interactionType);
            }
        }

        private void DisplayDialog(ClassesAndEnums.InteractionType interactionType)
        {
            Debug.Log("Displaying dialog for interaction type: " + interactionType);
            if (interactionType == ClassesAndEnums.InteractionType.DOOR || 
                interactionType == ClassesAndEnums.InteractionType.ELEVATOR)
            {
                // Only handle specific interaction types
                return;
            }
            
            // Timer Coroutine is still running, do not create a new dialog
            if (_autoCloseCoroutine != null) { return; }
            
            Debug.Log("Creating dialog for interaction type: " + interactionType);
            
            

            bool successDialogText = false;
            var text = string.Empty;
            // Set text content based on interaction type
            (successDialogText, text) = GetTextForInteraction(interactionType);
            if (!successDialogText)
            {
                Debug.LogWarning($"No dialog text found for interaction type: {interactionType}");
                _autoCloseCoroutine = StartCoroutine(AutoCloseDialog());
                return; // Exit if no dialog text is available
            }
            
            // Create dialog instance if it doesn't exist
            if (_currentDialogInstance == null)
            {
                _currentDialogInstance = Instantiate(dialogPrefab, dialogCanvasParent.transform);
                
                // Find text components in the instantiated prefab - first is dialog text
                _dialogText = _currentDialogInstance.GetComponentInChildren<TextMeshProUGUI>(true);
                _dialogText.text = text;
                
                // Find speaker name - assuming it's the second TextMeshProUGUI component
                TextMeshProUGUI[] textComponents = _currentDialogInstance.GetComponentsInChildren<TextMeshProUGUI>(true);
                if (textComponents.Length > 1)
                {
                    _speakerNameText = textComponents[1];
                }
            }
            
            Debug.Log("Text for interaction: " + _dialogText.text);

            if (_speakerNameText != null)
            {
                string speaker = GetSpeakerForInteraction(interactionType);
                _speakerNameText.gameObject.SetActive(!string.IsNullOrEmpty(speaker));
                _speakerNameText.text = speaker;
            }
            
            // start coroutine to auto-close the dialog after 'dialogDuration' seconds
            _autoCloseCoroutine = StartCoroutine(AutoCloseDialog());
        }

        private (bool, string) GetTextForInteraction(ClassesAndEnums.InteractionType interactionType)
        {
            if (_monologueData == null)
            {
                Debug.LogWarning("Monologue data not loaded, reloading...");
                LoadMonologueData();
                return (true, "Huh? I don't have anything to say right now.");
            }

            List<string> textOptions = null;

            // todo(opt): could make this more elegant
            switch (interactionType)
            {
                case ClassesAndEnums.InteractionType.NONE:
                    textOptions = _monologueData.NONE;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_BED:
                    textOptions = _monologueData.BROKEN_BED;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_CHAIR:
                    textOptions = _monologueData.BROKEN_CHAIR;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_SHELF:
                    textOptions = _monologueData.BROKEN_SHELF;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_SUITCASE:
                    textOptions = _monologueData.BROKEN_SUITCASE;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_TABLE:
                    textOptions = _monologueData.BROKEN_TABLE;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_BED:
                    textOptions = _monologueData.BURNED_BED;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_CHAIR:
                    textOptions = _monologueData.BURNED_CHAIR;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_SHELF:
                    textOptions = _monologueData.BURNED_SHELF;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_SUITCASE:
                    textOptions = _monologueData.BURNED_SUITCASE;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_TABLE:
                    textOptions = _monologueData.BURNED_TABLE;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_BED:
                    textOptions = _monologueData.MOIST_BED;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_CHAIR:
                    textOptions = _monologueData.MOIST_CHAIR;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_SHELF:
                    textOptions = _monologueData.MOIST_SHELF;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_SUITCASE:
                    textOptions = _monologueData.MOIST_SUITCASE;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_TABLE:
                    textOptions = _monologueData.MOIST_TABLE;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_BED:
                    textOptions = _monologueData.NORMAL_BED;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_CHAIR:
                    textOptions = _monologueData.NORMAL_CHAIR;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_SHELF:
                    textOptions = _monologueData.NORMAL_SHELF;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_SUITCASE:
                    textOptions = _monologueData.NORMAL_SUITCASE;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_TABLE:
                    textOptions = _monologueData.NORMAL_TABLE;
                    break;
                case ClassesAndEnums.InteractionType.DOOR:
                    textOptions = _monologueData.DOOR;
                    break;
                case ClassesAndEnums.InteractionType.START:
                    if (_firstStartRoom)
                    {
                        textOptions = _monologueData.FIRST;
                        _firstStartRoom = false; // Set to false after the first interaction
                    }
                    else
                    {
                        textOptions = _monologueData.START;
                    }
                    break;
                default:
                    Debug.LogWarning($"No dialogue texts defined for interaction type: {interactionType}");
                    return (false, "...");
            }

            // Check if there are any text options available
            if (textOptions == null || textOptions.Count == 0)
            {
                Debug.LogWarning($"No dialogue available for {interactionType}.");
                return (false, "I don't know what to say to that.");
            }

            // Choose a random text option from the list
            int randomIndex = Random.Range(0, textOptions.Count);
            return (true, textOptions[randomIndex]);
        }

        private string GetSpeakerForInteraction(ClassesAndEnums.InteractionType interactionType)
        {
            // we only have monologues currently
            // todo: maybe remove this if not needed
            return "You";
        }
        
        private IEnumerator AutoCloseDialog()
        {
            yield return new WaitForSeconds(dialogDuration);
            CloseDialog();
        }
        

        /// <summary>
        /// Closes the current monologue dialog if it exists.
        /// </summary>
        private void CloseDialog()
        {
            if (_currentDialogInstance != null)
            {
                Destroy(_currentDialogInstance);
                _currentDialogInstance = null;
            }
            
            _autoCloseCoroutine = null;
        }
    }
}