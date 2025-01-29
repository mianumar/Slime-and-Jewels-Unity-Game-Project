using System;
using Game_Designer_Online.Scripts.Misc;
using Game_Designer_Online.Scripts.Playfab_Related_Scripts;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab.AdminModels;

namespace Game_Designer_Online.Scripts.Main_Menu
{
    /// <summary>
    /// Contains all the functions required for the main menu to work properly in the game 
    /// </summary>
    public class MainMenuCanvas : MonoBehaviour
    {
        #region Main Menu Landing Buttons and Related Functions

        /// <summary>
        /// This is the text that is on the New Game Button. We may need to change this to 'Continue Loop'
        /// if the player has more than 0 prestige points.
        /// </summary>
        [SerializeField] private TextMeshProUGUI newGameButtonText;

        /// <summary>
        /// This will tell the game which level to load
        /// </summary>
        private int _levelToLoad;

        /// <summary>
        /// This function will help us setup the value of the new game button text. If the next level to load
        /// is 'Level 1', and the prestige level is greater than 0, then we will change the text of the new game button
        /// to 'Continue Loop'.
        /// </summary>
        public void SetupValueOfNewGameButtonText()
        {
            //Getting the value of the prestige level
            int prestigeLevel = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);
            
            // Check the value of the LoadGameKey
            if (PlayerPrefs.GetString(LoadGameKey) == "Level 1" && prestigeLevel == 0)
            {
                //If the value of the LoadGameKey is 'Level 1', then we will change
                //the text of the new game button
                newGameButtonText.text = "New Game";
                
                return;
            }
            
            // If we are at Level 1 and the prestige level is greater than 0, then we will change the text of the new game button
            if (PlayerPrefs.GetString(LoadGameKey) == "Level 1" && prestigeLevel > 0)
            {
                //If the value of the LoadGameKey is 'Level 1', then we will change the text of the new game button
                newGameButtonText.text = "Continue Loop";
            }
        }
        
        /// <summary>
        /// This function will tell the game which level it should load by giving a value fo the _levelToLoad variable.
        /// This can be used for testing. Whatever level we wish to load, we must take into account the index of
        /// the scene that we wish to load. For example, if we wish to load the level 1, we must give a value of 2
        /// as that is the index of the level 1 scene in the build settings.
        /// </summary>
        private void SetLevelToLoad()
        {
            _levelToLoad = 2;
        }

        public void LoadRewardedAd()
        {
            AdsManagement.Instance.DisplayRewardedAds();
        }

        /// <summary>
        /// Runs when the player Game Button is clicked
        /// </summary>
        private void OnPlayGameButtonClicked()
        {
            print("Play Game Button Clicked");
            
            //Checking if the _levelToLoad variable points to a valid scene
            if (_levelToLoad < 0 || _levelToLoad >= SceneManager.sceneCountInBuildSettings)
            {
                //If the _levelToLoad variable does not point to a valid scene, then we will print an error message
                //and return from the function
                Debug.LogError("The level to load is not valid");
                return;
            }
            
            // Checking if the load game button is interactable or not
            if (loadGameButton.interactable == true)
            {
                // If the load game button is interactable, then we will show the new game warning menu
                newGameWarningMenu.SetActive(true);
                
                return;
            }
            
            // Getting the prestige level
            int prestigeLevel = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);

            // We will only run this if the prestige level is 0
            if (prestigeLevel == 0 && PlayerPrefs.GetString(LoadGameKey) == "Level 1")
            {
                // Reset prestige level to 0
                PlayerPrefs.SetInt(StoreMenuCanvas.PrestigeKey, 0);
                print("Prestige level reset to 0");
            
                //We will need to upload this new value immediately to the PlayFab server
                HandleUserDataFromPlayFabServer.Instance.UpdatePlayerDataOnServerImmediately();

                //Loading the scene
                //SceneManager.LoadScene(_levelToLoad, LoadSceneMode.Single);
            
                //Activating the tutorial screen menu
                tutorialScreenMenu.SetActive(true);
            
                //Changing the main menu title text to read 'Loading...'
                mainMenuTitleText.text = "Loading...";
            }
            
            // If our prestige level is greater than 0, then we will load the next level
            if (prestigeLevel > 0 && PlayerPrefs.GetString(LoadGameKey) == "Level 1")
            {
                print("Requirements for Continue Loop are met. Loading the next level...");
                
                //Loading the scene
                SceneManager.LoadScene(_levelToLoad, LoadSceneMode.Single);
                
                //Changing the main menu title text to read 'Loading...'
                mainMenuTitleText.text = "Loading...";
                
                try
                {
                    // Playing the button click sound
                    SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }

                return;
            }
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            // Set the LoadGameKey to 'Level 1' to make sure that the player does not load a saved game
            PlayerPrefs.SetString(LoadGameKey, "Level 1");
        }

        /// <summary>
        /// This will run when the I Understand Button is clicked
        /// </summary>
        private void OnIUnderstandButtonClicked()
        {
            print("I Understand Button Clicked");
            
            //Checking if the _levelToLoad variable points to a valid scene
            if (_levelToLoad < 0 || _levelToLoad >= SceneManager.sceneCountInBuildSettings)
            {
                //If the _levelToLoad variable does not point to a valid scene, then we will print an error message
                //and return from the function
                Debug.LogError("The level to load is not valid");
                return;
            }
            
            // Reset prestige level to 0
            PlayerPrefs.SetInt(StoreMenuCanvas.PrestigeKey, 0);
            print("Prestige level reset to 0");
            
            //We will need to upload this new value immediately to the PlayFab server
            HandleUserDataFromPlayFabServer.Instance.UpdatePlayerDataOnServerImmediately();

            //Loading the scene
            //SceneManager.LoadScene(_levelToLoad, LoadSceneMode.Single);
            
            //Activating the tutorial screen menu
            tutorialScreenMenu.SetActive(true);
            
            //Changing the main menu title text to read 'Loading...'
            mainMenuTitleText.text = "Loading...";

            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Set the LoadGameKey to 'Level 1' to make sure that the player does not load a saved game
            PlayerPrefs.SetString(LoadGameKey, "Level 1");
        }

        /// <summary>
        /// This will run when the back button is pressed on the new game warning menu
        /// </summary>
        private void OnIUnderstandBackButtonClicked()
        {
            newGameWarningMenu.SetActive(false);
        }
        
        /// <summary>
        /// Runs when the tutorial screen play button is clicked
        /// </summary>
        private void OnTutorialScreenPlayButtonClicked()
        {
            print("Tutorial Screen Play Button Clicked");
            
            //Checking if the _levelToLoad variable points to a valid scene
            if (_levelToLoad < 0 || _levelToLoad >= SceneManager.sceneCountInBuildSettings)
            {
                //If the _levelToLoad variable does not point to a valid scene, then we will print an error message
                //and return from the function
                Debug.LogError("The level to load is not valid");
                return;
            }
            
            // Reset prestige level to 0
            PlayerPrefs.SetInt(StoreMenuCanvas.PrestigeKey, 0);
            print("Prestige level reset to 0");
            
            //We will need to upload this new value immediately to the PlayFab server
            HandleUserDataFromPlayFabServer.Instance.UpdatePlayerDataOnServerImmediately();

            //Loading the scene
            SceneManager.LoadScene(_levelToLoad, LoadSceneMode.Single);
            
            //Changing the main menu title text to read 'Loading...'
            mainMenuTitleText.text = "Loading...";

            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Set the LoadGameKey to 'Level 1' to make sure that the player does not load a saved game
            PlayerPrefs.SetString(LoadGameKey, "Level 1");
        }

        /// <summary>
        /// Runs when the load game button is clicked
        /// </summary>
        private void OnLoadGameButtonClicked()
        {
            print("Load Game Button Clicked");
            
            //Getting the value of the LoadGameKey
            string levelToLoad = PlayerPrefs.GetString(LoadGameKey);
            
            //Checking if the levelToLoad variable points to a valid scene
            if (string.IsNullOrEmpty(levelToLoad))
            {
                //If the levelToLoad variable does not point to a valid scene, then we will print an error message
                //and return from the function
                Debug.LogError("The level to load is not valid");
                return;
            }
            
            //Loading the scene
            SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Runs when the quit game button is clicked
        /// </summary>
        private void OnQuitGameButtonClicked()
        {
            print("Quit Game Button Clicked");
            Application.Quit();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Runs when the store button is clicked
        /// </summary>
        private void OnStoreButtonClicked()
        {
            print("The store button was clicked");
            
            storeMenuCanvas.SetActive(true);
            gameObject.SetActive(false);
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Runs when the customization button is clicked
        /// </summary>
        private void OnCustomizationButtonClicked()
        {
            print("The customization button was clicked");
            
            customizationMenuCanvas.SetActive(true);
            gameObject.SetActive(false);
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// This function will run when the delete account button was clicked
        /// </summary>
        private void OnDeleteAccountButtonClicked()
        {
            print("Delete account button was clicked");
            
            // We need to activate the delete account pop-up to make sure that we can allow the user
            // to delete their account if they want to delete it
            deleteAccountWarningMenu.SetActive(true);
            
            // Deactivating the main menu landing menu
            mainMenuLandingMenu.SetActive(false);
            // Deactivate the prestige level text
            prestigeContainerGameObject.SetActive(false);
            
            // Make all the buttons interactable
            deleteAccountWarningMenuDeleteButton.interactable = true;
            deleteAccountWarningMenuBackButton.interactable = true;
        }

        /// <summary>
        /// This is the button that will run when the delete button is clicked on the delete account warning menu.
        /// We should attempt to delete the user's account after this is clicked.
        /// </summary>
        private void OnDeleteButtonClicked()
        {
            print("Attempting to delete the player's account");
            
            // Change the warning text to read 'Deleting Account...'
            deleteAccountWarningMenuText.text = "Deleting Account...";
            
            // Disabling the buttons
            deleteAccountWarningMenuDeleteButton.interactable = false;
            deleteAccountWarningMenuBackButton.interactable = false;
            
            // Sending a request to get the player's PlayFab ID
            var getAccountInfo = new GetAccountInfoRequest();
            
            // Sending the get account info request
            PlayFabClientAPI.GetAccountInfo(
                getAccountInfo,
                result =>
                {
                    // Get the playfab ID from the result
                    string playFabId = result.AccountInfo.PlayFabId;
                    
                    // If we have the playfab ID, then we will attempt to delete the account
                    if (playFabId != null)
                    {
                        print("We have the playfab ID. Attempting to delete the account...");
                        
                        // Attempt to delete the account
                        AttemptToDeleteTheMasterPlayerAccountAfterPlayFabIdIsRetrieved(playFabId);
                    }
                },
                error =>
                {
                    // Print the error message
                    print("Account info could not be retrieved: " + error.ErrorMessage);
                    // Update the warning text to read 'Account could not be deleted'
                    deleteAccountWarningMenuText.text = "Account could not be deleted. Please try again later!";
                    
                    // Enable only the back button
                    deleteAccountWarningMenuBackButton.interactable = true;
                }
            );
        }
        
        /// <summary>
        /// This will run when the back button is clicked on the delete account warning menu
        /// </summary>
        private void OnDeleteAccountWarningBackButtonClicked()
        {
            deleteAccountWarningMenu.SetActive(false);
            mainMenuLandingMenu.SetActive(true);
            prestigeContainerGameObject.SetActive(true);
        }
        
        /// <summary>
        /// This function will run after the PlayFab ID is retrieved. We will attempt to delete the account
        /// We will delete the master player account.
        /// </summary>
        private void AttemptToDeleteTheMasterPlayerAccountAfterPlayFabIdIsRetrieved(string playersPlayFabId)
        {
            // Sending an admin api request to delete the player api
            var deleteMasterPlayerAccount = new DeleteMasterPlayerAccountRequest
            {
                PlayFabId = playersPlayFabId
            };
            
            // Sending the delete master player account request
            PlayFabAdminAPI.DeleteMasterPlayerAccount(
                deleteMasterPlayerAccount,
                result =>
                {
                    // Print the success message
                    print("Account deleted successfully");
                    // Update the warning text to read 'Account deleted successfully'
                    deleteAccountWarningMenuText.text = "Account deleted successfully! Please restart the game manually!";
                },
                error => 
                {
                    // Print the error message
                    print("Account info could not be retrieved: " + error.ErrorMessage);
                    // Update the warning text to read 'Account could not be deleted'
                    deleteAccountWarningMenuText.text = "Account could not be deleted. Please try again later!";
                    
                    // Enable only the back button
                    deleteAccountWarningMenuBackButton.interactable = true;
                }
            );
        }

        [Header("Main Menu Landing Buttons")]
        // This is going to store the key that will be used to check if the player has a saved game or not
        public const string LoadGameKey = "LoadGameKey";
        
        /// <summary>
        /// Reference to the Play Game Button
        /// </summary>
        [SerializeField] private Button playGameButton;
        
        /// <summary>
        /// Load Game Button Reference
        /// </summary>
        [SerializeField] private Button loadGameButton;
        
        /// <summary>
        /// Quit Game Button Reference
        /// </summary>
        [SerializeField] private Button quitGameButton;

        /// <summary>
        /// Store Button Reference
        /// </summary>
        [SerializeField] private Button storeButton;

        /// <summary>
        /// Customization Button Reference
        /// </summary>
        [SerializeField] private Button customizationButton;

        /// <summary>
        /// Button reference that will be on the new game warning menu
        /// </summary>
        [SerializeField] private Button iUnderstandButton;
        
        /// <summary>
        /// This is a back button that will be on the new game warning menu
        /// </summary>
        [SerializeField] private Button iUnderstandBackButton;

        /// <summary>
        /// This is a reference to the tutorial screen play button
        /// </summary>
        [SerializeField] private Button tutorialScreenPlayButton;

        /// <summary>
        /// This is a reference to the delete account button
        /// </summary>
        [SerializeField] private Button deleteAccountButton;

        /// <summary>
        /// This is a reference to the delete account warning menu delete button
        /// </summary>
        [SerializeField] private Button deleteAccountWarningMenuDeleteButton;

        /// <summary>
        /// This is a reference to the delete account back menu delete button
        /// </summary>
        [SerializeField] private Button deleteAccountWarningMenuBackButton;

        /// <summary>
        /// This is a reference to the delete account warning menu text
        /// </summary>
        [SerializeField] private TextMeshProUGUI deleteAccountWarningMenuText;
        
        /// <summary>
        /// This is a reference to the main menu title text that appears on the main menu canvas
        /// </summary>
        [SerializeField] private TextMeshProUGUI mainMenuTitleText;
        
        /// <summary>
        /// This is a reference to the prestige level text that appears on the main menu canvas
        /// </summary>
        [SerializeField] private TextMeshProUGUI prestigeLevelText;
        
        /// <summary>
        /// This function should be called by the HandleUserDataFromPlayFabServer script to set
        /// the prestige level text on the main menu canvas
        /// </summary>
        /// <param name="prestigeLevel"></param>
        public void SetPrestigeLevelText(int prestigeLevel)
        {
            prestigeLevelText.text = "Prestige Level: " + prestigeLevel;
            print("Prestige Level Text Set");
        }
        
        /// <summary>
        /// This will setup the button listeners in the Start() function
        /// </summary>
        private void SetupButtonListeners()
        {
            //Adding the listeners to the buttons
            playGameButton.onClick.AddListener(OnPlayGameButtonClicked);
            iUnderstandButton.onClick.AddListener(OnIUnderstandButtonClicked);
            iUnderstandBackButton.onClick.AddListener(OnIUnderstandBackButtonClicked);
            tutorialScreenPlayButton.onClick.AddListener(OnTutorialScreenPlayButtonClicked);
            loadGameButton.onClick.AddListener(OnLoadGameButtonClicked);
            quitGameButton.onClick.AddListener(OnQuitGameButtonClicked);
            storeButton.onClick.AddListener(OnStoreButtonClicked);
            customizationButton.onClick.AddListener(OnCustomizationButtonClicked);
            deleteAccountButton.onClick.AddListener(OnDeleteAccountButtonClicked);
            deleteAccountWarningMenuDeleteButton.onClick.AddListener(OnDeleteButtonClicked);
            deleteAccountWarningMenuBackButton.onClick.AddListener(OnDeleteAccountWarningBackButtonClicked);
        }

        /// <summary>
        /// This function will activate or deactivate the Load Game Button's interactable property based on whether
        /// the player has a value in the LoadGameKey or not
        /// </summary>
        private void HandleLoadGameButtonInteractFunction()
        {
            //If the key does not exist, then we will setup the key value
            if (PlayerPrefs.HasKey(LoadGameKey) == false)
            {
                PlayerPrefs.SetString(LoadGameKey, "Level 1");
            }
            
            //If the key exists, then we will check if the value is 'Level 1' or not
            if (PlayerPrefs.GetString(LoadGameKey) == "Level 1")
            {
                //If the value is 'Level 1', then we will deactivate the Load Game Button
                loadGameButton.interactable = false;
            }
            else
            {
                //If the value is not 'Level 1', then we will activate the Load Game Button
                loadGameButton.interactable = true;
            }
        }

        #endregion
        
        #region Unity Functions

        private void OnEnable()
        {
            newGameWarningMenu.SetActive(false);
        }

        private void Start()
        {
            SetLevelToLoad();
            SetupButtonListeners();
            HandleLoadGameButtonInteractFunction();
            storeMenuCanvas.GetComponent<StoreMenuCanvas>().StoreBuilderSetup();
        }

        #endregion

        #region References to Objects, Scripts and Components

        [Header("References to Objects, Scripts and Components")]
        //Reference to the StoreMenuCanvas
        [SerializeField] private GameObject storeMenuCanvas;

        /// <summary>
        /// Reference to the CustomizationMenuCanvas
        /// </summary>
        [SerializeField] private GameObject customizationMenuCanvas;

        /// <summary>
        /// Reference to the new game warning menu. This menu is going to appear if the user
        /// presses the new game button while the player has a save game available
        /// </summary>
        [SerializeField] private GameObject newGameWarningMenu;

        /// <summary>
        /// This is a reference to the tutorial screen menu
        /// </summary>
        [SerializeField] private GameObject tutorialScreenMenu;
        
        /// <summary>
        /// This is a reference to the delete account warning menu
        /// </summary>
        [SerializeField] private GameObject deleteAccountWarningMenu;

        /// <summary>
        /// This is a reference to the landing menu that will appear when the player opens the main menu
        /// </summary>
        [SerializeField] private GameObject mainMenuLandingMenu;

        /// <summary>
        /// This is a reference to the prestige container game object
        /// </summary>
        [SerializeField] private GameObject prestigeContainerGameObject;

        #endregion
    }
}