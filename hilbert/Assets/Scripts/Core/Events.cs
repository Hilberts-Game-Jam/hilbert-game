using static Scripts.Core.ClassesAndEnums;


namespace Scripts.Core
{
    public class Events
    {
        
        /// <summary>
        /// Manages player interaction events.
        /// <param name="interactionType">specifies which interaction the player did</param>
        /// </summary>
        public delegate void InteractionEventHandler(InteractionType interactionType = InteractionType.NONE);
        
        public static event InteractionEventHandler OnInteraction;
        
        
        /// <summary>
        /// Trigger an player interaction event.
        /// </summary>
        /// <param name="interactionType">specifies which interaction the player did</param>
        public static void TriggerInteraction(InteractionType interactionType = InteractionType.NONE)
        {
            OnInteraction?.Invoke(interactionType);
        }
        
        // Delegate für den WeirdnessChanged-Event
        public delegate void WeirdnessChangedEventHandler(int newWeirdnessLevel);
        
        // Event, der ausgelöst wird, wenn sich das WeirdnessLevel ändert
        public static event WeirdnessChangedEventHandler OnWeirdnessChanged;
        
        /// <summary>
        /// Trigger when the weirdness level changes.
        /// </summary>
        /// <param name="level">the new weirdness level</param>
        public static void TriggerWeirdnessChangeEvent(int level)
        {
            OnWeirdnessChanged?.Invoke(level);
        }
        
    }
}