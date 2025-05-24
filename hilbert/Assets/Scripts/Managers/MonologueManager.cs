using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core;
using Scripts.Core;
using TMPro;
using UnityEngine;

namespace Managers
{
    [System.Serializable]
    public class MonologueData
    {
        public List<string> item;
        public List<string> none;
    }
    
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
            // Reset auto close coroutine
            if (_autoCloseCoroutine != null)
            {
                StopCoroutine(_autoCloseCoroutine);
            }
            
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

            // Set text content based on interaction type
            if (_dialogText != null)
            {
                _dialogText.text = GetTextForInteraction(interactionType);
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

        private string GetTextForInteraction(ClassesAndEnums.InteractionType interactionType)
        {
            if (_monologueData == null)
            {
                Debug.LogWarning("Monologue data not loaded, reloading...");
                LoadMonologueData();
                return "Huh? I don't have anything to say right now.";
            }

            List<string> textOptions = null;

            switch (interactionType)
            {
                case ClassesAndEnums.InteractionType.ITEM:
                    textOptions = _monologueData.item;
                    break;
                case ClassesAndEnums.InteractionType.NONE:
                    textOptions = _monologueData.none;
                    break;
                default:
                    Debug.LogWarning($"No dialogue texts defined for interaction type: {interactionType}");
                    return "...";
            }

            // Check if there are any text options available
            if (textOptions == null || textOptions.Count == 0)
            {
                Debug.LogWarning($"No dialogue available for {interactionType}.");
                return "I don't know what to say to that.";
            }

            // Choose a random text option from the list
            int randomIndex = Random.Range(0, textOptions.Count);
            return textOptions[randomIndex];
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