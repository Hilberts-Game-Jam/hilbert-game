using System.Collections;
using Core;
using Scripts.Core;
using TMPro;
using UnityEngine;

namespace Managers
{
    public class MonologueManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogPrefab;
        [SerializeField] private float dialogDuration = 2f;
        [SerializeField] private GameObject dialogCanvasParent;
        
        private GameObject _currentDialogInstance;
        private TextMeshProUGUI _dialogText;
        private TextMeshProUGUI _speakerNameText;
        private Coroutine _autoCloseCoroutine;

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
            // todo: replace with actual text retrieval logic
            return "some text";
        }

        private string GetSpeakerForInteraction(ClassesAndEnums.InteractionType interactionType)
        {
            // we only have monologues currently
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