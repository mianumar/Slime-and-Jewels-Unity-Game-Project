using Game_Designer_Online.Scripts.Misc;
using UnityEngine;

namespace Game_Designer_Online.Scripts.Gameplay_Related
{
    /// <summary>
    /// This is attached to the Shield_X gameObject and it is responsible for adding a shield to the player
    /// </summary>
    public class Shield : MonoBehaviour
    {
        /// <summary>
        /// This will get access to the PlayerCharacter.cs script and then add a shield to the player.
        /// </summary>
        /// <param name="playerCharacter"></param>
        private void AddShieldToThePlayer(GameObject playerCharacter)
        {
            //Getting access to the PlayerCharacter.cs script
            PlayerCharacter playerCharacterScript = playerCharacter.GetComponent<PlayerCharacter>();
            
            //Adding a gem to the player
            playerCharacterScript.AddShield();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                print("A Shield item was collected!");
                AddShieldToThePlayer(other.gameObject);
                SoundManager.Instance.PlayCollectionSound();
                gameObject.SetActive(false);
            }
        }
    }
}