using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Designer_Online.Scripts.Misc
{
    /// <summary>
    /// This script will handle all of the sound effects and music that will be in the game
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        #region Singleton Setup

        /// <summary>
        /// Singleton
        /// </summary>
        public static SoundManager Instance;
        
        /// <summary>
        /// This will setup the singleton for the sound manager
        /// </summary>
        private void SetUpSingleton()
        {
            // Singleton
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            // Make sure that the sound manager is not destroyed when a new scene is loaded
            DontDestroyOnLoad(gameObject);
        }

        #endregion
        
        #region Scene Loading Functions

        /// <summary>
        /// This function will run when the scene is loaded
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            print("Sound Manager is processing sounds when scene is loaded");
            
            // Storing the build index of the scene that was loaded
            int sceneIndex = SceneManager.GetActiveScene().buildIndex;

            // Switch case to handle the various scenes
            switch (sceneIndex)
            {
                case 0: // Login Scene
                    PlayLoginSceneBackgroundMusic();
                    break;
                case 1: // Main Menu Scene
                    PlayMainMenuSceneBackgroundMusic();
                    break;
                case 2: // Level 1
                    backgroundMusicForTheGameScenesAudioSource.Stop();
                    backgroundMusicForTheGameScenesAudioSource.clip = level1Clip;
                    backgroundMusicForTheGameScenesAudioSource.Play();
                    break;
                case 3: // Level 2
                    backgroundMusicForTheGameScenesAudioSource.Stop();
                    backgroundMusicForTheGameScenesAudioSource.clip = level2Clip;
                    backgroundMusicForTheGameScenesAudioSource.Play();
                    break;
                case 4: // Level 3
                    backgroundMusicForTheGameScenesAudioSource.Stop();
                    backgroundMusicForTheGameScenesAudioSource.clip = level3Clip;
                    backgroundMusicForTheGameScenesAudioSource.Play();
                    break;
                case 5: // Level 4
                    backgroundMusicForTheGameScenesAudioSource.Stop();
                    backgroundMusicForTheGameScenesAudioSource.clip = level4Clip;
                    backgroundMusicForTheGameScenesAudioSource.Play();
                    break;
                case 6: // Level 5
                    backgroundMusicForTheGameScenesAudioSource.Stop();
                    backgroundMusicForTheGameScenesAudioSource.clip = level5Clip;
                    backgroundMusicForTheGameScenesAudioSource.Play();
                    break;
            }
        }

        #endregion

        #region Menu and In-Game Store Navigation Sounds

        [Header("Menu and In-Game Store Navigation Sounds"), Space(5)]
        // Audio Source for Menu and In-Game Store Navigation Sounds
        [SerializeField] private AudioSource menuAndInGameStoreNavigationSoundsAudioSource;

        /// <summary>
        /// This is the click sound that we will use when we click the menus in the menus
        /// </summary>
        [SerializeField] private AudioClip menuAndInGameStoreClickSounds;
        
        /// <summary>
        /// This is the sound that will play when we have a negative message to give to the player
        /// </summary>
        [SerializeField] private AudioClip menuAndInGameNegativeClickSounds;
        
        /// <summary>
        /// This function should be called whenever we click on a button in the menu or in-game store
        /// </summary>
        public void PlayMenuAndInGameStoreClickSounds()
        {
            menuAndInGameStoreNavigationSoundsAudioSource.PlayOneShot(menuAndInGameStoreClickSounds);
        }
        
        /// <summary>
        /// This function should be called whenever we need to send a negative message to the player
        /// </summary>
        public void PlayMenuAndInGameStoreNegativeClickSounds()
        {
            menuAndInGameStoreNavigationSoundsAudioSource.PlayOneShot(menuAndInGameNegativeClickSounds);
        }

        #endregion

        #region Background Music For the Game Scenes

        [Header("Background Music For the Game Scenes"), Space(5)]
        // Audio Source for Background Music For the Game Scenes
        [SerializeField] private AudioSource backgroundMusicForTheGameScenesAudioSource;
        
        /// <summary>
        /// This will play the background music for the game scenes
        /// </summary>
        [SerializeField] private AudioClip loginSceneBackgroundMusic;

        /// <summary>
        /// This is the background music for the main menu scene
        /// </summary>
        [SerializeField] private AudioClip mainMenuSceneBackgroundMusic;
        
        /// <summary>
        /// This will allow us to play the background music for the game scenes
        /// </summary>
        private void PlayLoginSceneBackgroundMusic()
        {
            backgroundMusicForTheGameScenesAudioSource.Stop();
            backgroundMusicForTheGameScenesAudioSource.clip = loginSceneBackgroundMusic;
            backgroundMusicForTheGameScenesAudioSource.Play();
        }
        
        /// <summary>
        /// This will allow us to play the background music for the main menu
        /// </summary>
        private void PlayMainMenuSceneBackgroundMusic()
        {
            backgroundMusicForTheGameScenesAudioSource.Stop();
            backgroundMusicForTheGameScenesAudioSource.clip = mainMenuSceneBackgroundMusic;
            backgroundMusicForTheGameScenesAudioSource.Play();
        }

        #endregion

        #region In-Game Sounds

        [Header("In-Game Sounds")]
        // Audio Source for In-Game Sounds
        [SerializeField] private AudioSource inGameSoundsAudioSource;
        
        /// <summary>
        /// Return the audio source for the in-game sounds
        /// </summary>
        public AudioSource InGameSoundsAudioSource => inGameSoundsAudioSource;

        /// <summary>
        /// This will be the jump sound that we will use when the player jumps
        /// </summary>
        [SerializeField] private AudioClip jumpAudioClip;

        /// <summary>
        /// This will be the reset level sound. Should be played when the player dies
        /// </summary>
        [SerializeField] private AudioClip resetLevelSound;

        /// <summary>
        /// This will be the sound that will play when the player completes a level
        /// </summary>
        [SerializeField] private AudioClip levelCompleteSound;

        /// <summary>
        /// This will be the sounds that will play whenever the player clicks on the in-game UI
        /// </summary>
        [SerializeField] private AudioClip inGameUIClicksSound;
        
        /// <summary>
        /// These are the audio clips that will play when the player is inside a certain level
        /// </summary>
        [SerializeField] private AudioClip level1Clip, level2Clip,
            level3Clip, level4Clip, level5Clip;

        /// <summary>
        /// This is going to be the collection sound
        /// </summary>
        [SerializeField] private AudioClip collectionSound;

        /// <summary>
        /// This is a reference to the hurt sound
        /// </summary>
        [SerializeField] private AudioClip hurtSound;
        
        /// <summary>
        /// This should play whenever the player jumps
        /// </summary>
        public void PlayJumpSound()
        {
            inGameSoundsAudioSource.PlayOneShot(jumpAudioClip);
        }

        /// <summary>
        /// This should play whenever the player dies
        /// </summary>
        public void PlayResetLevelSound()
        {
            inGameSoundsAudioSource.PlayOneShot(resetLevelSound);
        }
        
        /// <summary>
        /// This will be the level complete sound
        /// </summary>
        public void PlayLevelCompleteSound()
        {
            inGameSoundsAudioSource.PlayOneShot(levelCompleteSound);
        }
        
        /// <summary>
        /// This should play whenever the player clicks on the in-game UI
        /// </summary>
        public void PlayInGameUIClicksSound()
        {
            inGameSoundsAudioSource.PlayOneShot(inGameUIClicksSound);
        }
        
        /// <summary>
        /// This is going to be the hurt sound
        /// </summary>
        public void PlayHurtSound()
        {
            inGameSoundsAudioSource.PlayOneShot(hurtSound);
        }

        /// <summary>
        /// This is going to play the collection sound
        /// </summary>
        public void PlayCollectionSound()
        {
            inGameSoundsAudioSource.PlayOneShot(collectionSound);
        }

        #endregion
        
        #region Unity Functions

        private void OnEnable()
        {
            // Function to Run when Scene is Loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            // Function to Run when Scene is Loaded
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Awake()
        {
            SetUpSingleton();
        }

        #endregion
    }
}