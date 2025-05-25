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
        public List<string> none;
        public List<string> door;
        public List<string> start;
        public List<string> first;
        
        public List<string> broken_bed;
        public List<string> broken_chair;
        public List<string> broken_suitcase;
        public List<string> broken_table;
        
        public List<string> burned_bed;
        public List<string> burned_chair;
        public List<string> burned_shelf;
        public List<string> burned_suitcase;
        public List<string> burned_table;
        
        public List<string> moist_bed;
        public List<string> moist_chair;
        public List<string> moist_shelf;
        public List<string> moist_suitcase;
        public List<string> moist_table;
        
        public List<string> normal_bed;
        public List<string> normal_chair;
        public List<string> normal_shelf;
        public List<string> normal_suitcase;
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
        }
        
        private void LoadMonologueData()
        {
            try
            {
                string jsonText = File.ReadAllText(monologueFilePath);
                _monologueData = JsonUtility.FromJson<MonologueData>(jsonText);
                
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
            Events.OnInteraction += DisplayDialog;
        }

        private void OnDisable()
        {
            Events.OnInteraction -= DisplayDialog;
        }

        private void DisplayDialog(ClassesAndEnums.InteractionType interactionType)
        {
            if (interactionType == ClassesAndEnums.InteractionType.DOOR || 
                interactionType == ClassesAndEnums.InteractionType.ELEVATOR)
            {
                // Only handle specific interaction types
                return;
            }
            
            // Timer Coroutine is still running, do not create a new dialog
            if (_autoCloseCoroutine != null) { return; }
            
            // Create dialog instance if it doesn't exist
            if (_currentDialogInstance == null)
            {
                _currentDialogInstance = Instantiate(dialogPrefab, dialogCanvasParent.transform);
                
                // Find text components in the instantiated prefab - first is dialog text
                _dialogText = _currentDialogInstance.GetComponentInChildren<TextMeshProUGUI>(true);
                
                // Find speaker name - assuming it's the second TextMeshProUGUI component
                TextMeshProUGUI[] textComponents = _currentDialogInstance.GetComponentsInChildren<TextMeshProUGUI>(true);
                if (textComponents.Length > 1)
                {
                    _speakerNameText = textComponents[1];
                }
            }

            bool success_dialogText = false;
            // Set text content based on interaction type
            if (_dialogText != null)
            {
                (success_dialogText, _dialogText.text) = GetTextForInteraction(interactionType);
            }
            if (!success_dialogText)
            {
                Debug.LogWarning($"No dialog text found for interaction type: {interactionType}");
                return; // Exit if no dialog text is available
            }

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
                    textOptions = _monologueData.none;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_BED:
                    textOptions = _monologueData.broken_bed;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_CHAIR:
                    textOptions = _monologueData.broken_chair;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_SUITCASE:
                    textOptions = _monologueData.broken_suitcase;
                    break;
                case ClassesAndEnums.InteractionType.BROKEN_TABLE:
                    textOptions = _monologueData.broken_table;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_BED:
                    textOptions = _monologueData.burned_bed;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_CHAIR:
                    textOptions = _monologueData.burned_chair;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_SHELF:
                    textOptions = _monologueData.burned_shelf;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_SUITCASE:
                    textOptions = _monologueData.burned_suitcase;
                    break;
                case ClassesAndEnums.InteractionType.BURNED_TABLE:
                    textOptions = _monologueData.burned_table;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_BED:
                    textOptions = _monologueData.moist_bed;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_CHAIR:
                    textOptions = _monologueData.moist_chair;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_SHELF:
                    textOptions = _monologueData.moist_shelf;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_SUITCASE:
                    textOptions = _monologueData.moist_suitcase;
                    break;
                case ClassesAndEnums.InteractionType.MOIST_TABLE:
                    textOptions = _monologueData.moist_table;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_BED:
                    textOptions = _monologueData.normal_bed;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_CHAIR:
                    textOptions = _monologueData.normal_chair;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_SHELF:
                    textOptions = _monologueData.normal_shelf;
                    break;
                case ClassesAndEnums.InteractionType.NORMAL_SUITCASE:
                    textOptions = _monologueData.normal_suitcase;
                    break;
                case ClassesAndEnums.InteractionType.DOOR:
                    textOptions = _monologueData.door;
                    break;
                case ClassesAndEnums.InteractionType.START:
                    if (_firstStartRoom)
                    {
                        textOptions = _monologueData.first;
                        _firstStartRoom = false; // Set to false after the first interaction
                    }
                    else
                    {
                        textOptions = _monologueData.start;
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
        public void CloseDialog()
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