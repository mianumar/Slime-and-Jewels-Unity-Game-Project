using Game_Designer_Online.Scripts.Misc;
using UnityEngine;

namespace Game_Designer_Online.Scripts.Gameplay_Related
{
    /// <summary>
    /// At the time of creating this script, the 'Gem' prefab was not created. It is just attached to the 'Gem'
    /// GameObject in Level 1. This script might be modified in the future as we may need to keep track of the Gems
    /// collected by the player.
    /// </summary>
    public class Gem : MonoBehaviour
    {
        /// <summary>
        /// This will get access to the PlayerCharacter.cs script and then add a gem to the player.
        /// </summary>
        /// <param name="playerCharacter"></param>
        private void AddGemToThePlayer(GameObject playerCharacter)
        {
            //Getting access to the PlayerCharacter.cs script
            PlayerCharacter playerCharacterScript = playerCharacter.GetComponent<PlayerCharacter>();
            
            //Adding a gem to the player
            playerCharacterScript.AddJewel();
            
            //Adding a token to the player
            playerCharacterScript.AddToken();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                print("A Gem was collected!");
                AddGemToThePlayer(other.gameObject);
                SoundManager.Instance.PlayCollectionSound();
                gameObject.SetActive(false);
            }
        }
    }
}
