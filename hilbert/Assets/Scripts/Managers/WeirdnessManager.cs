using Core;
using Scripts.Core;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Manages the weirdness level of the game. If a Elevator is used, the weirdness level increases.
    /// </summary>
    public class WeirdnessManager : MonoBehaviour
    {
        // get only static property for weirdness
        // must be kept between 1-3 -> only have 3 weirdness levels
        private static int _weirdnessLevel = 1;
        public static int WeirdnessLevel 
        { 
            get => _weirdnessLevel;
            private set 
            {
                if (_weirdnessLevel != value)
                {
                    int curWeirdnessLevel = _weirdnessLevel;
                    _weirdnessLevel = Mathf.Clamp(value, 1, 3);
                    if (curWeirdnessLevel != _weirdnessLevel)
                    {
                        // Trigger the event when weirdness level changes
                        Events.TriggerWeirdnessChangeEvent(_weirdnessLevel);
                    }
                }
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
            if (interactionType == ClassesAndEnums.InteractionType.ELEVATOR)
            {
                IncrementWeirdness();
            }
        }
        
        public static void IncrementWeirdness()
        {
            SetWeirdnessLevel(WeirdnessLevel + 1);
        }
        public static void DecrementWeirdness()
        {
            SetWeirdnessLevel(WeirdnessLevel - 1);
        }

        public static void SetWeirdnessLevel(int level)
        {
            WeirdnessLevel = level;
            // Ensure weirdness level is between 1 and 3
            WeirdnessLevel = Mathf.Clamp(WeirdnessLevel, 1, 3);
        }
        
    }
}