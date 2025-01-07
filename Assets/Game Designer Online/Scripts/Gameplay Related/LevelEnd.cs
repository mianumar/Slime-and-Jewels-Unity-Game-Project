using System;
using System.Collections;
using Game_Designer_Online.Scripts.Main_Menu;
using Game_Designer_Online.Scripts.Misc;
using Game_Designer_Online.Scripts.Playfab_Related_Scripts;
using UnityEngine;

namespace Game_Designer_Online.Scripts.Gameplay_Related
{
    /// <summary>
    /// This script is attached to the LevelEnd prefab. This will be used to tell the game that the player has reached
    /// the end of the game. The programmer will need to enter the build index of the next level to load when
    /// in the inspector. It is expected that this script runs when the 'Player' collides with the
    /// 'LevelEnd' GameObject.
    /// </summary>
    public class LevelEnd : MonoBehaviour
    {
        /// <summary>
        /// When this is true, it will tell the game that the upload was complete
        /// </summary>
        private bool waitUntilUploadIsComplete = false;
        
        /// <summary>
        /// Runs when the player hits the level end object
        /// </summary>
        private IEnumerator Routine_LoadNextLevel()
        {
            // Creating a wait for seconds real-time
            WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(0.2f);
            
            // Displaying the ad here
            AdsManagement.Instance.DisplayInterstitialAds();

            // This will freeze the game
            Time.timeScale = 0.0f;
            
            // This will play the level complete sound
            SoundManager.Instance.PlayLevelCompleteSound();
            
            // While loop that will run as long as the level complete sound is playing
            while (SoundManager.Instance.InGameSoundsAudioSource.isPlaying)
            {
                yield return waitForSecondsRealtime;
            }

            // This will make the time scale be 1.0f so that the game can play
            Time.timeScale = 1.0f;

            // This is a boolean that will be set. When true, it will tell the game
            // that the upload is complete
            waitUntilUploadIsComplete = UploadPlayerDataToPlayFabServer();

            // We will wait until the upload is complete
            while (waitUntilUploadIsComplete == true)
            {
                yield return new WaitForSeconds(0.2f);
            }

            // This if statement will tell us that the upload was successful
            if (waitUntilUploadIsComplete == true)
            {
                print("Data was successfully uploaded to the server");
            }

            print("Loading level: " + (buildIndexOfNextLevel - 1));
            UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndexOfNextLevel);

            yield break;
        }
        
        /// <summary>
        /// This will upload the data to the PlayFab server when the player completes a level.
        /// Since we wish to upload all the data to the server at the very end of the level, we will
        /// call this function right after the player has reached the end of the level.
        /// </summary>
        private bool UploadPlayerDataToPlayFabServer()
        {
            // Getting the tokens in total and adding the prestige level to it
            int tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
            
            // Getting the prestige level
            int prestigeLevel = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);
            
            print("We are about to add " + prestigeLevel + " to the tokens in total that is: " +
                  "" + tokensInTotal);
            
            // Adding the prestige level to the tokens in total
            tokensInTotal += prestigeLevel;

            // Setting the tokens in total
            PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
            
            // Get the current build index
            int currentBuildIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
            
            // If we are on the final level, then we will add on prestige level
            if (currentBuildIndex == 6)
            {
                // Getting the current prestige level
                int currentPrestigeLevel = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);
                
                // Adding on the prestige level
                currentPrestigeLevel++;
                
                // Setting the prestige level
                PlayerPrefs.SetInt(StoreMenuCanvas.PrestigeKey, currentPrestigeLevel);
                
                Debug.Log("Prestige level is now: " + currentPrestigeLevel);
            }

            //If we are running this on the windows editor or windows player,
            //then we will upload the data to the server
            if (Application.platform == RuntimePlatform.WindowsEditor
                || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                // Getting all the values required to upload the data to the server
                int tokens = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                int shield = PlayerPrefs.GetInt(StoreMenuCanvas.ShieldKey);
                int extraHealth = PlayerPrefs.GetInt(StoreMenuCanvas.HealthKey);
                int chests = PlayerPrefs.GetInt(StoreMenuCanvas.ChestKey);
                int prestige = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);

                //Updating the keys on the local dictionary before uploading to the server
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                    = tokens;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["Shield"] 
                    = shield;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["ExtraHealth"] 
                    = extraHealth;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["ChestsInTotal"]
                    = chests;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["PrestigeLevel"]
                    = prestige;
            
                //This will upload the data to the server
                bool resultSuccess = HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                // We will change the value of the boolean here
                waitUntilUploadIsComplete = resultSuccess;

                return resultSuccess;
            }
            
            //If we are running on the android platform, then we will upload the data to the server
            //using the LocalUserData class
            if (Application.platform == RuntimePlatform.Android)
            {
                // Getting all the values required to upload the data to the server
                int tokens = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                int shield = PlayerPrefs.GetInt(StoreMenuCanvas.ShieldKey);
                int extraHealth = PlayerPrefs.GetInt(StoreMenuCanvas.HealthKey);
                int chests = PlayerPrefs.GetInt(StoreMenuCanvas.ChestKey);
                int prestige = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);

                //Updating the keys on the local dictionary before uploading to the server
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                    = tokens;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["Shield"] 
                    = shield;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["ExtraHealth"] 
                    = extraHealth;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["ChestsInTotal"]
                    = chests;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["PrestigeLevel"]
                    = prestige;

                //This will upload the data to the server
                bool resultSuccess = HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                // We will change the value of the boolean here
                waitUntilUploadIsComplete = resultSuccess;
                
                return resultSuccess;
            }
            
            // If we are running on the iPhone platform, then we will upload the data on the server
            // using the LocalUserDataClass
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // Getting all the values required to upload the data to the server
                int tokens = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                int shield = PlayerPrefs.GetInt(StoreMenuCanvas.ShieldKey);
                int extraHealth = PlayerPrefs.GetInt(StoreMenuCanvas.HealthKey);
                int chests = PlayerPrefs.GetInt(StoreMenuCanvas.ChestKey);
                int prestige = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);

                //Updating the keys on the local dictionary before uploading to the server
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                    = tokens;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["Shield"] 
                    = shield;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["ExtraHealth"] 
                    = extraHealth;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["ChestsInTotal"]
                    = chests;
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["PrestigeLevel"]
                    = prestige;

                //This will upload the data to the server
                bool resultSuccess = HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                // We will change the value of the boolean here
                waitUntilUploadIsComplete = resultSuccess;
                
                return resultSuccess;
            }

            return false;
        }

        /// <summary>
        /// This will setup the next level key that will be used to set the load game function of the game
        /// </summary>
        private void SetupNextLevelKey()
        {
            //This will setup the next level key that will be used to set the load game function of the game
            string nextLevelKey = "Level " + (buildIndexOfNextLevel - 1);
            
            //If we have reached the end of the game, then we will set the next level key to the Level 1 value
            if (buildIndexOfNextLevel == 7)
            {
                nextLevelKey = "Level 1";
            }
            
            //Setting the next level key
            PlayerPrefs.SetString(MainMenuCanvas.LoadGameKey, nextLevelKey);
        }
        
        /// <summary>
        /// The value of this variable has to be sent in the inspector. This will be the build index of the next level
        /// </summary>
        [SerializeField, Range(2,7)] 
        private int buildIndexOfNextLevel;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Routine_LoadNextLevel());
                SetupNextLevelKey();
            }
        }
    }
}