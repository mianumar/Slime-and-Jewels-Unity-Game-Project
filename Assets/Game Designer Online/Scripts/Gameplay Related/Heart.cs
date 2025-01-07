using Game_Designer_Online.Scripts.Misc;
using UnityEngine;

namespace Game_Designer_Online.Scripts.Gameplay_Related
{
    /// <summary>
    /// This script is attached to the Heart_x gameObject and it is responsible for adding health to the player
    /// </summary>
    public class Heart : MonoBehaviour
    {
        /// <summary>
        /// This will get access to the PlayerCharacter.cs script and then add a heart to the player.
        /// </summary>
        /// <param name="playerCharacter"></param>
        private void AddHealthToThePlayer(GameObject playerCharacter)
        {
            //Getting access to the PlayerCharacter.cs script
            PlayerCharacter playerCharacterScript = playerCharacter.GetComponent<PlayerCharacter>();
            
            //Adding a gem to the player
            playerCharacterScript.AddHealth();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                print("A Health item was collected!");
                AddHealthToThePlayer(other.gameObject);
                SoundManager.Instance.PlayCollectionSound();
                gameObject.SetActive(false);
            }
        }
    }
}
