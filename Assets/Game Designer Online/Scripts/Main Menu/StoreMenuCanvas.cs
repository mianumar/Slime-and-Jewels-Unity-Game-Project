using System;
using System.Collections;
using Game_Designer_Online.Scripts.Do_Not_Destroy_Scripts;
using Game_Designer_Online.Scripts.Misc;
using Game_Designer_Online.Scripts.Playfab_Related;
using Game_Designer_Online.Scripts.Playfab_Related_Scripts;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game_Designer_Online.Scripts.Main_Menu
{
    /// <summary>
    /// This script is attached to the StoreMenuCanvas prefab. It contains all of the functions required
    /// to make the store menu work properly in the game.
    /// </summary>
    public class StoreMenuCanvas : MonoBehaviour, IStoreListener
    {
        #region Misc Buttons

        /// <summary>
        /// Runs when the back button is clicked
        /// </summary>
        public void OnBackButtonClicked()
        {
            print("Back Button Clicked");

            mainMenuCanvas.SetActive(true);
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

        #endregion

        #region Shop Buttons

        /// <summary>
        /// All of the strings that are require to make the in app purchases work
        /// </summary>
        private const string FiveShieldProductId = "fiveshield",
        FiveHealthProductId = "fivehealth",
        SeventyFiveTokensProductId = "seventyfivertokens",
        OneHundredAndFiftyTokensProductId = "onehundredandfiftytokens",
        ThreeHundredTokensProductId = "threehundredtokens",
        FiveHundredTokensProductId = "fivehundredtokens";

        [Header("Shop Buttons Related")]
        // Reference to the five shield button
        [SerializeField]
        private Button fiveShieldButton;
        
        // Reference to the five health button
        [SerializeField] private Button fiveHealthButton;
        
        // Reference to the seventy five player tokens button
        [SerializeField] private Button seventyFivePlayerTokensButton;
        
        // Reference to the one hundred and fifty player tokens button
        [SerializeField] private Button oneHundredAndFiftyTokensButton;
        
        // Reference to the three hundred player tokens button
        [SerializeField] private Button threeHundredTokensButton;
        
        // Reference to the five hundred player tokens button
        [SerializeField] private Button fiveHundredTokensButton;

        /// <summary>
        /// This will run when the player clicks on the activate 2x booster button
        /// </summary>
        public void On2XBoosterButtonClicked()
        {
            print("The 2X Booster Button Was Clicked");

            //Activating the 2x booster
            BoosterHandler.Instance.Activate2XBooster();

            //We will deduct 1 from the player's 2x booster inventory
            PlayerPrefs.SetInt(TwoXBoosterKey, PlayerPrefs.GetInt(TwoXBoosterKey) - 1);

            //Getting the local user data dictionary from the PlayFab server
            var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

            //Checking if the player has the 2x booster key in the local dictionary
            if (localDictionary.ContainsKey("2xTokenBooster"))
            {
                //If the player has the 2x booster key in the local dictionary, then we will update the value
                //of the 2x booster key in the local dictionary
                localDictionary["2xTokenBooster"] = PlayerPrefs.GetInt(TwoXBoosterKey);

                //Upload the local dictionary to the server
                HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

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
            else
            {
                Debug.LogError("The player does not have the 2XBooster key in the local dictionary");

                try
                {
                    // Playing the button click sound
                    SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            //Checking if we need to disable the booster buttons
            EnableOrDisableBoosterButtonsBasedOnAFewConditions();
        }

        /// <summary>
        /// This will run when the player clicks on the activate 3x booster button
        /// </summary>
        public void On3XBoosterButtonClicked()
        {
            print("The 3X Booster Button Was Clicked");

            //Activating the 3x booster
            BoosterHandler.Instance.Activate3XBooster();

            //We will deduct 1 from the player's 3x booster inventory
            PlayerPrefs.SetInt(ThreeXBoosterKey, PlayerPrefs.GetInt(ThreeXBoosterKey) - 1);

            //Getting the local user data dictionary from the PlayFab server
            var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

            //Checking if the player has the 3x booster key in the local dictionary
            if (localDictionary.ContainsKey("3xTokenBooster"))
            {
                //If the player has the 3x booster key in the local dictionary, then we will update the value
                //of the 3x booster key in the local dictionary
                localDictionary["3xTokenBooster"] = PlayerPrefs.GetInt(ThreeXBoosterKey);

                //Upload the local dictionary to the server
                HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

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
            else
            {
                Debug.LogError("The player does not have the 3XBooster key in the local dictionary");

                try
                {
                    // Playing the button click sound
                    SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            //Checking if we need to disable the booster buttons
            EnableOrDisableBoosterButtonsBasedOnAFewConditions();
        }
        
        /// <summary>
        /// This will add the listeners to all of the store buttons
        /// </summary>
        private void AddListenersToAllStoreButtons()
        {
            //Adding the listener to the five shield button
            fiveShieldButton.onClick.AddListener(OnFiveShieldButtonClicked);
            
            //Adding the listener to the five health button
            fiveHealthButton.onClick.AddListener(OnFiveHealthButtonClicked);
            
            //Adding the listener to the seventy five player tokens button
            seventyFivePlayerTokensButton.onClick.AddListener(OnSeventyFiveTokensButtonClicked);
            
            //Adding the listener to the one hundred and fifty player tokens button
            oneHundredAndFiftyTokensButton.onClick.AddListener(OnOneHundredAndFiftyTokensButtonClicked);
            
            //Adding the listener to the three hundred player tokens button
            threeHundredTokensButton.onClick.AddListener(OnThreeHundredTokensButtonClicked);
            
            //Adding the listener to the five hundred player tokens button
            fiveHundredTokensButton.onClick.AddListener(OnFiveHundredTokensButtonClicked);
        }
        
        /// <summary>
        /// This will run when the player attempts to buy the five shield
        /// </summary>
        private void OnFiveShieldButtonClicked()
        {
            print("Five Shield Purchase Button was clicked");

            // We will return if the store controller is null
            if (_storeController == null)
            {
                return;
            }

            // Initializing the purchase process for the remove ads for one month
            _storeController.InitiatePurchase(FiveShieldProductId);

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
        /// This will run when the player attempts to buy the five health
        /// </summary>
        private void OnFiveHealthButtonClicked()
        {
            print("Five Health Purchase Button was clicked");

            // We will return if the store controller is null
            if (_storeController == null)
            {
                return;
            }

            // Initializing the purchase process for the remove ads for one month
            _storeController.InitiatePurchase(FiveHealthProductId);

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
        /// This will run when the player attempts to buy the 75 tokens product
        /// </summary>
        private void OnSeventyFiveTokensButtonClicked()
        {
            print("Fifty Player Tokens Purchase Button was clicked");

            // We will return if the store controller is null
            if (_storeController == null)
            {
                return;
            }

            // Initializing the purchase process for the remove ads for one month
            _storeController.InitiatePurchase(SeventyFiveTokensProductId);

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
        /// This will run when the player attempts to buy the 150 tokens product
        /// </summary>
        private void OnOneHundredAndFiftyTokensButtonClicked()
        {
            print("One Hundred and Fifty Player Tokens Purchase Button was clicked");

            // We will return if the store controller is null
            if (_storeController == null)
            {
                return;
            }

            // Initializing the purchase process for the remove ads for one month
            _storeController.InitiatePurchase(OneHundredAndFiftyTokensProductId);

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
        /// This will run when the player attempts to buy the 300 tokens product
        /// </summary>
        private void OnThreeHundredTokensButtonClicked()
        {
            print("Three Hundred Player Tokens Purchase Button was clicked");

            // We will return if the store controller is null
            if (_storeController == null)
            {
                return;
            }

            // Initializing the purchase process for the remove ads for one month
            _storeController.InitiatePurchase(ThreeHundredTokensProductId);

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
        /// This will run when the player attempts to buy the 500 tokens product
        /// </summary>
        private void OnFiveHundredTokensButtonClicked()
        {
            print("Five Hundred Player Tokens Purchase Button was clicked");

            // We will return if the store controller is null
            if (_storeController == null)
            {
                return;
            }

            // Initializing the purchase process for the remove ads for one month
            _storeController.InitiatePurchase(FiveHundredTokensProductId);

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
        
        #endregion

        #region Constants for Key Values

        /// <summary>
        /// These are some of the constant key values that will be used to store values of the items
        /// that are purchased in the store.
        /// </summary>
        public const string ShieldKey = "Shield",
            HealthKey = "Health",
            PlayerTokensKeyReference = "PlayerTokens",
            ChestKey = "ChestKey",
            TwoXBoosterKey = "TwoXBooster",
            ThreeXBoosterKey = "ThreeXBooster",
            PrestigeKey = "Prestige",
            RemoveAdsKey = "RemoveAds";

        /// <summary>
        /// This will setup the player store key values
        /// </summary>
        private void SetupPlayerStoreKeyValues()
        {
            //Checking if the player has the shield
            if (!PlayerPrefs.HasKey(ShieldKey))
            {
                //If the player does not have the shield, then we will set the shield key value to 0
                PlayerPrefs.SetInt(ShieldKey, 0);
            }

            //Checking if the player has the health
            if (!PlayerPrefs.HasKey(HealthKey))
            {
                //If the player does not have the health, then we will set the health key value to 3.
                //We might change this in the future. 3 is only for testing
                PlayerPrefs.SetInt(HealthKey, 3);
            }

            //Checking if the player has the player tokens
            if (!PlayerPrefs.HasKey(PlayerTokensKeyReference))
            {
                //If the player does not have the player tokens, then we will set the player tokens key value to 0
                PlayerPrefs.SetInt(PlayerTokensKeyReference, 0);
            }

            //Checking if the player has the chest key
            if (!PlayerPrefs.HasKey(ChestKey))
            {
                //If the player does not have the chest key, then we will set the chest key value to 0
                PlayerPrefs.SetInt(ChestKey, 0);
            }

            //Checking if the player has the two x booster key
            if (!PlayerPrefs.HasKey(TwoXBoosterKey))
            {
                //If the player does not have the two x booster key, then we will set the two x booster key value to 0
                PlayerPrefs.SetInt(TwoXBoosterKey, 0);
            }

            //Checking if the player has the three x booster key
            if (!PlayerPrefs.HasKey(ThreeXBoosterKey))
            {
                //If the player does not have the three x booster key, then we will set the three x booster key value to 0
                PlayerPrefs.SetInt(ThreeXBoosterKey, 0);
            }

            // Checking if the player has the prestige key
            if (!PlayerPrefs.HasKey(PrestigeKey))
            {
                //If the player does not have the prestige key, then we will set the prestige key value to 0
                PlayerPrefs.SetInt(PrestigeKey, 0);
            }

            // Checking if the player has the remove ads key
            if (!PlayerPrefs.HasKey(RemoveAdsKey))
            {
                //If the player does not have the remove ads key, then we will set the remove ads key value to 0
                PlayerPrefs.SetInt(RemoveAdsKey, 0);
            }
        }

        /// <summary>
        /// This function will just setup the values of the player's inventory based on the key values
        /// that have been loaded
        /// </summary>
        private void SetupPlayerInventoryValuesBasedOnKeyValues()
        {
            //Setup string for the player's shield inventory
            string shieldInventory = "Shield: " + PlayerPrefs.GetInt(ShieldKey);

            //Setup string for the player's health inventory
            string healthInventory = "Health: " + PlayerPrefs.GetInt(HealthKey);

            //Setup string for the player's player tokens inventory
            string playerTokensInventory = "Player Tokens: " + PlayerPrefs.GetInt(PlayerTokensKeyReference);

            //Setup string for the player's chest inventory
            string chestInventory = "Chest: " + PlayerPrefs.GetInt(ChestKey);

            //Setup string for the player's two x booster inventory
            string twoXBoosterInventory = "2X Booster: " + PlayerPrefs.GetInt(TwoXBoosterKey);

            //Setup string for the player's three x booster inventory
            string threeXBoosterInventory = "3X Booster: " + PlayerPrefs.GetInt(ThreeXBoosterKey);

            //Setting the text of the shield inventory text component
            shieldInventoryTextComponent.text = shieldInventory;

            //Setting the text of the health inventory text component
            healthInventoryTextComponent.text = healthInventory;

            //Setting the text of the player tokens inventory text component
            playerTokensInventoryTextComponent.text = playerTokensInventory;

            //Setting the text of the chest inventory text component
            chestInventoryTextComponent.text = chestInventory;

            //Setting the text of the 2x booster inventory text component
            twoXBoosterInventoryTextComponent.text = twoXBoosterInventory;

            //Setting the text of the 3x booster inventory text component
            threeXBoosterInventoryTextComponent.text = threeXBoosterInventory;
        }

        #endregion

        #region Chest Related Functions

        [Header("Chest Related Functions")]
        //Chest Button
        [SerializeField]
        private Button chestButton;

        /// <summary>
        /// This is the GameObject that will be used to display the chest opening warning container
        /// </summary>
        [SerializeField] private GameObject chestOpeningWarningContainer;

        /// <summary>
        /// This is the GameObject that is called CoveringPanelForChestOpening. It is one of the children
        /// </summary>
        [SerializeField] private GameObject coveringPanelForChestOpening;

        /// <summary>
        /// This is the text component gameObject that is in the CoveringPanelForChestOpening
        /// </summary>
        [SerializeField] private TextMeshProUGUI chestOpeningText;

        /// <summary>
        /// This runs when the best button is clicked
        /// </summary>
        public void OnChestButtonClicked()
        {
            chestOpeningWarningContainer.SetActive(true);

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
        /// This runs when the ReturnButton on the Chest Opening Warning Container is clicked
        /// </summary>
        public void OnChestReturnButtonClicked()
        {
            chestOpeningWarningContainer.SetActive(false);

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
        /// This will run when the OpenAChestButton is clicked. The player can either earn a shield
        /// or they can earn a health. The player has a 50% chance of earning a shield and a 50% chance
        /// of earning a health when they open a chest.
        /// </summary>
        public void OnOpenAChestButtonClicked()
        {
            print("Open A Chest Button Clicked");

            //Turning the chest opening warning container off
            chestOpeningWarningContainer.SetActive(false);

            //Turning the opening chest covering panel on
            coveringPanelForChestOpening.SetActive(true);

            //Getting a random number between 1 and 100
            int randomNumber = Random.Range(1, 101);

            //If the random number is below 50, we will add 1 token to the player's inventory
            if (randomNumber <= 50)
            {
                //Adding 1 to the player's token inventory
                PlayerPrefs.SetInt(PlayerTokensKeyReference, PlayerPrefs.GetInt(PlayerTokensKeyReference) + 1);

                //Deducting 1 from the player's chest inventory
                PlayerPrefs.SetInt(ChestKey, PlayerPrefs.GetInt(ChestKey) - 1);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the TokensInTotal in the local dictionary
                if (localDictionary.ContainsKey("TokensInTotal"))
                {
                    //If the player has the TokensInTotal in the local dictionary, then we will update the value
                    //of the TokensInTotal in the local dictionary
                    localDictionary["TokensInTotal"] = PlayerPrefs.GetInt(PlayerTokensKeyReference);

                    //If the player has the chest key in the local dictionary, then we will update the value
                    //of the chest key in the local dictionary
                    localDictionary["ChestsInTotal"] = PlayerPrefs.GetInt(ChestKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                }
                else
                {
                    Debug.LogError("The player does not have the TokensInTotal key in the local dictionary");
                }

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

                //Setting the text of the chest opening text component
                chestOpeningText.text = "You got 1 token!";
            }

            //If the random number is above 50, but less than or equal to 70, we will add
            //3 tokens to the player's inventory
            if (randomNumber > 50 && randomNumber <= 70)
            {
                //Adding 3 to the player's token inventory
                PlayerPrefs.SetInt(PlayerTokensKeyReference, PlayerPrefs.GetInt(PlayerTokensKeyReference) + 3);

                //Deducting 1 from the player's chest inventory
                PlayerPrefs.SetInt(ChestKey, PlayerPrefs.GetInt(ChestKey) - 1);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the TokensInTotal in the local dictionary
                if (localDictionary.ContainsKey("TokensInTotal"))
                {
                    //If the player has the TokensInTotal in the local dictionary, then we will update the value
                    //of the TokensInTotal in the local dictionary
                    localDictionary["TokensInTotal"] = PlayerPrefs.GetInt(PlayerTokensKeyReference);

                    //If the player has the chest key in the local dictionary, then we will update the value
                    //of the chest key in the local dictionary
                    localDictionary["ChestsInTotal"] = PlayerPrefs.GetInt(ChestKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                }
                else
                {
                    Debug.LogError("The player does not have the TokensInTotal key in the local dictionary");
                }

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

                //Setting the text of the chest opening text component
                chestOpeningText.text = "You got 3 tokens!";
            }

            //If the random number is above 70, but less than or equal to 80, we will add
            //5 tokens to the player's inventory
            if (randomNumber > 70 && randomNumber <= 80)
            {
                //Adding 5 to the player's token inventory
                PlayerPrefs.SetInt(PlayerTokensKeyReference, PlayerPrefs.GetInt(PlayerTokensKeyReference) + 5);

                //Deducting 1 from the player's chest inventory
                PlayerPrefs.SetInt(ChestKey, PlayerPrefs.GetInt(ChestKey) - 1);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the TokensInTotal in the local dictionary
                if (localDictionary.ContainsKey("TokensInTotal"))
                {
                    //If the player has the TokensInTotal in the local dictionary, then we will update the value
                    //of the TokensInTotal in the local dictionary
                    localDictionary["TokensInTotal"] = PlayerPrefs.GetInt(PlayerTokensKeyReference);

                    //If the player has the chest key in the local dictionary, then we will update the value
                    //of the chest key in the local dictionary
                    localDictionary["ChestsInTotal"] = PlayerPrefs.GetInt(ChestKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                }
                else
                {
                    Debug.LogError("The player does not have the TokensInTotal key in the local dictionary");
                }

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

                //Setting the text of the chest opening text component
                chestOpeningText.text = "You got 5 tokens!";
            }

            //If the random number is above 80, but less than or equal to 88, we will add
            //7 tokens to the player's inventory
            if (randomNumber > 80 && randomNumber <= 88)
            {
                //Adding 7 to the player's token inventory
                PlayerPrefs.SetInt(PlayerTokensKeyReference, PlayerPrefs.GetInt(PlayerTokensKeyReference) + 7);

                //Deducting 1 from the player's chest inventory
                PlayerPrefs.SetInt(ChestKey, PlayerPrefs.GetInt(ChestKey) - 1);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the TokensInTotal in the local dictionary
                if (localDictionary.ContainsKey("TokensInTotal"))
                {
                    //If the player has the TokensInTotal in the local dictionary, then we will update the value
                    //of the TokensInTotal in the local dictionary
                    localDictionary["TokensInTotal"] = PlayerPrefs.GetInt(PlayerTokensKeyReference);

                    //If the player has the chest key in the local dictionary, then we will update the value
                    //of the chest key in the local dictionary
                    localDictionary["ChestsInTotal"] = PlayerPrefs.GetInt(ChestKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                }
                else
                {
                    Debug.LogError("The player does not have the TokensInTotal key in the local dictionary");
                }

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

                //Setting the text of the chest opening text component
                chestOpeningText.text = "You got 7 tokens!";
            }

            //If the random number is above 88, but less than or equal to 92, we will add
            //1 shield to the player's inventory
            if (randomNumber > 88 && randomNumber <= 92)
            {
                //Adding 1 to the player's shield inventory
                PlayerPrefs.SetInt(ShieldKey, PlayerPrefs.GetInt(ShieldKey) + 1);

                //Deducting 1 from the player's chest inventory
                PlayerPrefs.SetInt(ChestKey, PlayerPrefs.GetInt(ChestKey) - 1);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the Shield in the local dictionary
                if (localDictionary.ContainsKey("Shield"))
                {
                    //If the player has the Shield in the local dictionary, then we will update the value
                    //of the Shield in the local dictionary
                    localDictionary["Shield"] = PlayerPrefs.GetInt(ShieldKey);

                    //If the player has the chest key in the local dictionary, then we will update the value
                    //of the chest key in the local dictionary
                    localDictionary["ChestsInTotal"] = PlayerPrefs.GetInt(ChestKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                }
                else
                {
                    Debug.LogError("The player does not have the Shield key in the local dictionary");
                }

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

                //Setting the text of the chest opening text component
                chestOpeningText.text = "You got 1 shield!";
            }

            //If the random number is above 92, but less than or equal to 96, we will add
            //1 health to the player's inventory
            if (randomNumber > 92 && randomNumber <= 96)
            {
                //Adding 1 to the player's health inventory
                PlayerPrefs.SetInt(HealthKey, PlayerPrefs.GetInt(HealthKey) + 1);

                //Deducting 1 from the player's chest inventory
                PlayerPrefs.SetInt(ChestKey, PlayerPrefs.GetInt(ChestKey) - 1);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the ExtraHealth in the local dictionary
                if (localDictionary.ContainsKey("ExtraHealth"))
                {
                    //If the player has the ExtraHealth in the local dictionary, then we will update the value
                    //of the ExtraHealth in the local dictionary
                    localDictionary["ExtraHealth"] = PlayerPrefs.GetInt(HealthKey);

                    //If the player has the chest key in the local dictionary, then we will update the value
                    //of the chest key in the local dictionary
                    localDictionary["ChestsInTotal"] = PlayerPrefs.GetInt(ChestKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                }
                else
                {
                    Debug.LogError("The player does not have the ExtraHealth key in the local dictionary");
                }

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

                //Setting the text of the chest opening text component
                chestOpeningText.text = "You got 1 health!";
            }

            //If the random number is above 96, but less than or equal to 100, we will add
            //2x booster to the player's inventory
            if (randomNumber > 96 && randomNumber <= 98)
            {
                //Adding 1 to the player's 2x booster inventory
                PlayerPrefs.SetInt(TwoXBoosterKey, PlayerPrefs.GetInt(TwoXBoosterKey) + 1);

                //Deducting 1 from the player's chest inventory
                PlayerPrefs.SetInt(ChestKey, PlayerPrefs.GetInt(ChestKey) - 1);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the 2xTokenBooster in the local dictionary
                if (localDictionary.ContainsKey("2xTokenBooster"))
                {
                    //If the player has the 2xTokenBooster in the local dictionary, then we will update the value
                    //of the 2xTokenBooster in the local dictionary
                    localDictionary["2xTokenBooster"] = PlayerPrefs.GetInt(TwoXBoosterKey);

                    //If the player has the chest key in the local dictionary, then we will update the value
                    //of the chest key in the local dictionary
                    localDictionary["ChestsInTotal"] = PlayerPrefs.GetInt(ChestKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                }
                else
                {
                    Debug.LogError("The player does not have the 2xTokenBooster key in the local dictionary");
                }

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

                //Setting the text of the chest opening text component
                chestOpeningText.text = "You got 2x booster!";
            }

            //If the random number is above 96, but less than or equal to 100, we will add
            //3x booster to the player's inventory
            if (randomNumber > 98 && randomNumber <= 100)
            {
                //Adding 1 to the player's 3x booster inventory
                PlayerPrefs.SetInt(ThreeXBoosterKey, PlayerPrefs.GetInt(ThreeXBoosterKey) + 1);

                //Deducting 1 from the player's chest inventory
                PlayerPrefs.SetInt(ChestKey, PlayerPrefs.GetInt(ChestKey) - 1);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the 3xTokenBooster in the local dictionary
                if (localDictionary.ContainsKey("3xTokenBooster"))
                {
                    //If the player has the 3xTokenBooster in the local dictionary, then we will update the value
                    //of the 3xTokenBooster in the local dictionary
                    localDictionary["3xTokenBooster"] = PlayerPrefs.GetInt(ThreeXBoosterKey);

                    //If the player has the chest key in the local dictionary, then we will update the value
                    //of the chest key in the local dictionary
                    localDictionary["ChestsInTotal"] = PlayerPrefs.GetInt(ChestKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                }
                else
                {
                    Debug.LogError("The player does not have the 3xTokenBooster key in the local dictionary");
                }

                //Updating the player's inventory
                SetupPlayerInventoryValuesBasedOnKeyValues();

                //Setting the text of the chest opening text component
                chestOpeningText.text = "You got 3x booster!";
            }

            //Activate or deactivate the chest button based on the chest inventory
            ActivateOrDeactivateChestButtonBasedOnChestInventory();

            //Enabling or disabling the booster buttons based on a few conditions
            EnableOrDisableBoosterButtonsBasedOnAFewConditions();

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
        /// This will run when the ReturnButton on the Chest Opening Container is clicked
        /// </summary>
        public void OnChestOpeningReturnButtonClicked()
        {
            //Turning the chest opening warning container off
            chestOpeningWarningContainer.SetActive(false);

            //Turning the opening chest covering panel off
            coveringPanelForChestOpening.SetActive(false);

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
        /// This function should run in Start(). This will activate or deactivate the chest button based on
        /// how many chests the player currently has
        /// </summary>
        private void ActivateOrDeactivateChestButtonBasedOnChestInventory()
        {
            //Storing the number of chest the player has in the chest inventory
            int chestInInventory = PlayerPrefs.GetInt(ChestKey);
            //chestInInventory = 0;

            //If we have more than 0 chest in the player's inventory, then we will activate the chest button
            if (chestInInventory > 0)
            {
                chestButton.gameObject.SetActive(true);
                print("The player has a chest in the inventory so the chest button will be activated");
            }
            else
            {
                chestButton.gameObject.SetActive(false);
                print("The player does not have a chest in the inventory so the chest button" +
                      " will be deactivated");
            }
        }

        #endregion

        #region Booster Buttons Enable/Disable Functions

        /// <summary>
        /// This will enable or disable the booster buttons based on a few conditions
        /// </summary>
        public void EnableOrDisableBoosterButtonsBasedOnAFewConditions()
        {
            //Getting the value of the booster active key
            int boosterActiveKey = PlayerPrefs.GetInt(BoosterHandler.BoosterActiveKey);

            //If the booster active key value is not zero, it means that a booster is active and
            //we have to disable the booster buttons
            if (boosterActiveKey != 0)
            {
                //Disabling the booster buttons
                activate2XBoosterButton.interactable = false;
                activate3XBoosterButton.interactable = false;

                print("Disabling the booster button as one of the booster was active");
            }
            else
            {
                //Enabling the booster buttons
                activate2XBoosterButton.interactable = true;
                activate3XBoosterButton.interactable = true;
            }

            //Checking the current player inventory for the 2x booster
            int twoXBoosterInventory = PlayerPrefs.GetInt(TwoXBoosterKey);

            //If the player has 0 2x booster in the inventory, then we will disable the 2x booster button
            if (twoXBoosterInventory == 0)
            {
                activate2XBoosterButton.interactable = false;
                print("Disabling the 2x booster button as the player has 0 2x booster in the inventory");
            }

            //Checking the current player inventory for the 3x booster
            int threeXBoosterInventory = PlayerPrefs.GetInt(ThreeXBoosterKey);

            //If the player has 0 3x booster in the inventory, then we will disable the 3x booster button
            if (threeXBoosterInventory == 0)
            {
                activate3XBoosterButton.interactable = false;
                print("Disabling the 3x booster button as the player has 0 3x booster in the inventory");
            }
        }

        #endregion

        #region Remove Ads Related Functions

        /// <summary>
        /// This will run when the player clicks on the remove ads button for one month
        /// </summary>
        private void OnRemoveAdsOneMonthSubscriptionButtonClicked()
        {
            print("Remove Ads One Month Subscription Button Clicked");

            // We will return if the store controller is null
            if (_storeController == null)
            {
                return;
            }

            // Making all other buttons in the remove ads button container not interactable
            removeAdsForOneMonthButton.interactable = false;
            removeAdsForFourMonthsButton.interactable = false;
            removeAdsButtonContainerBackButton.interactable = false;

            // Initializing the purchase process for the remove ads for one month
            _storeController.InitiatePurchase(RemoveAdsPerMonthProductId);

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
        /// This will run when the player clicks on the remove ads button for four months
        /// </summary>
        public void OnRemoveAdsFourMonthsSubscriptionButtonClicked()
        {
            print("Remove Ads Four Months Subscription Button Clicked");

            // We will return if the store controller is null
            if (_storeController == null)
            {
                return;
            }

            // Making all other buttons in the remove ads button container not interactable
            removeAdsForOneMonthButton.interactable = false;
            removeAdsForFourMonthsButton.interactable = false;
            removeAdsButtonContainerBackButton.interactable = false;

            // Initializing the purchase process for the remove ads for four months
            _storeController.InitiatePurchase(RemoveAdsPerFourMonthsProductId);

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
        /// This routine should be called when the player purchases the remove ads button
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_WaitToDisableRemoveAdsContainerUntilPurchaseIsComplete()
        {
            yield return new WaitForSeconds(2f);

            // Disabling the remove ads button interactable
            removeAdsButton.interactable = false;

            //Deactivating the remove ads button container
            removeAdsButtonContainer.SetActive(false);
        }

        [Header("Functions Related to Remove Ads Button")]
        // This is a reference to the remove ads button
        [SerializeField]
        private Button removeAdsButton;

        /// <summary>
        /// This is a reference to the remove ads button container
        /// </summary>
        [SerializeField] private GameObject removeAdsButtonContainer;

        /// <summary>
        /// This is a reference to the remove ads button container back button is clicked
        /// </summary>
        [SerializeField] private Button removeAdsButtonContainerBackButton;

        /// <summary>
        /// These are references to the remove ads for one month and remove ads for four months buttons
        /// </summary>
        [SerializeField] private Button removeAdsForOneMonthButton, removeAdsForFourMonthsButton;

        /// <summary>
        /// This is a class for the subscription item, that will be used to store the subscription item
        /// </summary>
        [Serializable]
        public class SubscriptionItem
        {
            public string productName;
            public string productId;
            public string productDescription;
            public float productPrice;
            public int timeDurationForSubscription;
        }

        /// <summary>
        /// Remove Ads ids for per month product
        /// </summary>
        private const string RemoveAdsPerMonthProductId = "removeadspermonth_2",
            RemoveAdsPerFourMonthsProductId = "removeadsforfourmonths_2";

         /// <summary>
        /// This function should be called in the Awake Method and should be used to setup the store builder
        /// </summary>
        public void StoreBuilderSetup()
        {
            // Telling the game that this will be the fake store
            StandardPurchasingModule.Instance().useFakeStoreAlways = false;

            // Creating a builder
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            /*// Adding remove ads for one month subscription
            _removeAdsOneMonthSubscription = new SubscriptionItem
            {
                productName = "Remove Ads for One Month",
                productId = RemoveAdsPerMonthProductId,
                productDescription = "Remove ads for one month",
                productPrice = 4.99f,
                timeDurationForSubscription = 30
            };

            // Adding remove ads for four months subscription
            _removeAdsFourMonthsSubscription = new SubscriptionItem
            {
                productName = "Remove Ads for Four Months",
                productId = RemoveAdsPerFourMonthsProductId,
                productDescription = "Remove ads for four months",
                productPrice = 8.99f,
                timeDurationForSubscription = 120
            };*/

            // Adding the remove ads for one month subscription to the builder with new ids
            builder.AddProduct(RemoveAdsPerMonthProductId, ProductType.Subscription, new IDs
            {
                { RemoveAdsPerMonthProductId, GooglePlay.Name },
                { RemoveAdsPerMonthProductId, AppleAppStore.Name }
            });

            // Adding the remove ads for four months subscription to the builder
            builder.AddProduct(RemoveAdsPerFourMonthsProductId, ProductType.Subscription, new IDs
            {
                { RemoveAdsPerFourMonthsProductId, GooglePlay.Name },
                { RemoveAdsPerFourMonthsProductId, AppleAppStore.Name }
            });
            
            // Adding the five shield product to the builder
            builder.AddProduct(FiveShieldProductId, ProductType.Consumable, new IDs
            {
                { FiveShieldProductId, GooglePlay.Name },
                { FiveShieldProductId, AppleAppStore.Name }
            });
            
            // Adding the five health product to the builder
            builder.AddProduct(FiveHealthProductId, ProductType.Consumable, new IDs
            {
                { FiveHealthProductId, GooglePlay.Name },
                { FiveHealthProductId, AppleAppStore.Name }
            });
            
            // Adding the seventy five tokens product to the builder
            builder.AddProduct(SeventyFiveTokensProductId, ProductType.Consumable, new IDs
            {
                { SeventyFiveTokensProductId, GooglePlay.Name },
                { SeventyFiveTokensProductId, AppleAppStore.Name }
            });
            
            // Adding the one hundred and fifty tokens product to the builder
            builder.AddProduct(OneHundredAndFiftyTokensProductId, ProductType.Consumable, new IDs
            {
                { OneHundredAndFiftyTokensProductId, GooglePlay.Name },
                { OneHundredAndFiftyTokensProductId, AppleAppStore.Name }
            });
            
            // Adding the three hundred tokens product to the builder
            builder.AddProduct(ThreeHundredTokensProductId, ProductType.Consumable, new IDs
            {
                { ThreeHundredTokensProductId, GooglePlay.Name },
                { ThreeHundredTokensProductId, AppleAppStore.Name }
            });
            
            // Adding the five hundred tokens product to the builder
            builder.AddProduct(FiveHundredTokensProductId, ProductType.Consumable, new IDs
            {
                { FiveHundredTokensProductId, GooglePlay.Name },
                { FiveHundredTokensProductId, AppleAppStore.Name }
            });

            UnityPurchasing.Initialize(this, builder);
        }
        
        /// <summary>
        /// This will run to check if the remove ads button should be intractable or not.
        /// We will check if the user has a receipt. If they do, then it means that the subscription
        /// is still valid.
        /// </summary>
        public void CheckingRemoveAdsReceipt()
        {
            // If the store controller is null, then we will return
            if (_storeController == null)
            {
                return;
            }

            // Getting the products from the store controller
            var removeAdsOneMonthProduct =
                _storeController.products.WithID(RemoveAdsPerMonthProductId);
            var removeAdsFourMonthsProduct =
                _storeController.products.WithID(RemoveAdsPerFourMonthsProductId);

            // If the remove ads one month product is not null
            if (removeAdsOneMonthProduct != null && removeAdsFourMonthsProduct != null)
            {
                try
                {
                    // Checking if we have a receipt for the remove ads one month product
                    if (removeAdsOneMonthProduct.hasReceipt == true)
                    {
                        // Creating a new subscription manager
                        SubscriptionManager subscriptionManager =
                            new SubscriptionManager(removeAdsOneMonthProduct, null);

                        // Getting the subscription info
                        SubscriptionInfo subscriptionInfo = subscriptionManager.getSubscriptionInfo();

                        if (subscriptionInfo.isSubscribed() == Result.True)
                        {
                            print("The player is subscribed to the remove ads for one month");

                            // Making the remove ads button not intractable
                            removeAdsButton.interactable = false;

                            // Setting the value of the remove ads key to 1
                            PlayerPrefs.SetInt(RemoveAdsKey, 1);

                            return;
                        }
                        else
                        {
                            print("The player is not subscribed to the remove ads for one month");

                            // Making the remove ads button interactable
                            removeAdsButton.interactable = true;

                            // Setting the value of the remove ads key to 0
                            PlayerPrefs.SetInt(RemoveAdsKey, 0);
                        }
                    }
                    else
                    {
                        print("The player is not subscribed to the remove ads for one month");

                        // Making the remove ads button interactable
                        removeAdsButton.interactable = true;

                        // Setting the value of the remove ads key to 0
                        PlayerPrefs.SetInt(RemoveAdsKey, 0);
                    }

                    // Checking if we have a receipt for the remove ads four months product
                    if (removeAdsFourMonthsProduct.hasReceipt == true)
                    {
                        // Creating a new subscription manager
                        SubscriptionManager subscriptionManager =
                            new SubscriptionManager(removeAdsFourMonthsProduct, null);

                        // Getting the subscription info
                        SubscriptionInfo subscriptionInfo = subscriptionManager.getSubscriptionInfo();

                        if (subscriptionInfo.isSubscribed() == Result.True)
                        {
                            print("The player is subscribed to the remove ads for four months");

                            // Making the remove ads button not interactable
                            removeAdsButton.interactable = false;

                            // Setting the value of the remove ads key to 1
                            PlayerPrefs.SetInt(RemoveAdsKey, 1);

                            return;
                        }
                        else
                        {
                            print("The player is not subscribed to the remove ads for four months");

                            // Making the remove ads button interactable
                            removeAdsButton.interactable = true;

                            // Setting the value of the remove ads key to 0
                            PlayerPrefs.SetInt(RemoveAdsKey, 0);
                        }
                    }
                    else
                    {
                        print("The player is not subscribed to the remove ads for four months");

                        // Making the remove ads button interactable
                        removeAdsButton.interactable = true;

                        // Setting the value of the remove ads key to 0
                        PlayerPrefs.SetInt(RemoveAdsKey, 0);
                    }
                }
                catch (Exception e)
                {
                    print("One of the remove ads product could not be retrieved so we will " +
                          "make the remove ads button interactable");

                    // Making the remove ads button interactable
                    removeAdsButton.interactable = true;

                    // Setting the value of the remove ads key to 0
                    PlayerPrefs.SetInt(RemoveAdsKey, 0);

                    print("The error that was thrown is " + e);
                }
            }
            else
            {
                print("The remove ads product could not be retrieved so we will " +
                      "make the remove ads button interactable");

                // If the remove ads one month product is null, then we will make the remove ads button interactable
                removeAdsButton.interactable = true;

                // Setting the value of the remove ads key to 0
                PlayerPrefs.SetInt(RemoveAdsKey, 0);
            }
        }
        
        /// <summary>
        /// This function will run when the player clicks on the remove ads button is clicked
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnRemoveAdsButtonClicked()
        {
            print("Remove ads button was clicked");

            // Activate the remove ads button container
            removeAdsButtonContainer.SetActive(true);

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
        /// This function will run when the player clicks on the remove ads button container back button
        /// </summary>
        private void OnRemoveAdsButtonContainerBackButtonClicked()
        {
            removeAdsButtonContainer.SetActive(false);
        }

        /// <summary>
        /// This function will setup the button listeners for all of the buttons in the remove ads button container
        /// </summary>
        private void AddListenersToAllRemoveAdsButtonContainerButtons()
        {
            // These are the listeners for navigation
            removeAdsButton.onClick.AddListener(OnRemoveAdsButtonClicked);
            removeAdsButtonContainerBackButton.onClick.AddListener(OnRemoveAdsButtonContainerBackButtonClicked);

            // These are the listeners for the remove ads for one month and remove ads for four months buttons
            removeAdsForOneMonthButton.onClick.AddListener(OnRemoveAdsOneMonthSubscriptionButtonClicked);
            removeAdsForFourMonthsButton.onClick.AddListener(OnRemoveAdsFourMonthsSubscriptionButtonClicked);
        }

        #endregion

        #region Guest Warning Related Function
        
        [Header("Guest Warning Related Functions")]
        // This is a gameObject that will activate when the user is a guest and tries to access the store
        [SerializeField] private GameObject guestWarningMessage;

        /// <summary>
        /// This function will check if the user is a guest and enable the required menu which will tell the user
        /// that they are a guest and they will not be able to save their progress properly
        /// </summary>
        private void EnableGuestWarningMessageIfTheUserIsAGuest()
        {
            // Getting the value of the guest key
            int guestKey = PlayerPrefs.GetInt(LoginAndRegistrationCanvas.IsGuestKey);
            
            // If the guest key is 1, then we will enable the guest warning message
            if (guestKey == 1)
            {
                guestWarningMessage.SetActive(true);
            }
            else
            {
                guestWarningMessage.SetActive(false);
            }
        }

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            SetupPlayerStoreKeyValues();
            SetupPlayerInventoryValuesBasedOnKeyValues();
            ActivateOrDeactivateChestButtonBasedOnChestInventory();
            EnableOrDisableBoosterButtonsBasedOnAFewConditions();
            EnableGuestWarningMessageIfTheUserIsAGuest();
        }

        private void Start()
        {
            //StoreBuilderSetup();
            AddListenersToAllRemoveAdsButtonContainerButtons();
            AddListenersToAllStoreButtons();
        }

        #endregion

        #region References to Objects, Scripts, and Components

        [Header("References to Objects, Scripts, and Components")]
        //Reference to the MainMenuCanvas
        [SerializeField]
        private GameObject mainMenuCanvas;

        /// <summary>
        /// These are the references to the text components that will display the player's inventory
        /// and should be updated when the player purchases an item from the store.
        /// </summary>
        [SerializeField] private TextMeshProUGUI shieldInventoryTextComponent,
            healthInventoryTextComponent,
            playerTokensInventoryTextComponent,
            chestInventoryTextComponent,
            twoXBoosterInventoryTextComponent,
            threeXBoosterInventoryTextComponent;

        /// <summary>
        /// These are the buttons that will be used to activate or deactivate the 2x and 3x booster buttons
        /// </summary>
        [SerializeField] private Button activate2XBoosterButton, activate3XBoosterButton;

        #endregion

        #region IStoreListener Implementation

        /// <summary>
        /// This is a reference to the store controller
        /// </summary>
        private IStoreController _storeController;

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            print("Failed to initialize the store due to " + error);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            print("Failed to initialize the store due to " + error + " and the message is " + message);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            print("Processing purchases result");

            // Purchasing the remove ads for one month subscription
            if (purchaseEvent.purchasedProduct.definition.id == RemoveAdsPerMonthProductId)
            {
                print("The player has purchased the remove ads for one month subscription");

                // Changing the value of the remove ads key to 1
                PlayerPrefs.SetInt(RemoveAdsKey, 1);

                // Starting the coroutine to disable the remove ads button container until the purchase is complete
                StartCoroutine(Routine_WaitToDisableRemoveAdsContainerUntilPurchaseIsComplete());
            }

            // Purchasing the remove ads for four months subscription
            if (purchaseEvent.purchasedProduct.definition.id == RemoveAdsPerFourMonthsProductId)
            {
                print("The player has purchased the remove ads for four months subscription");

                // Changing the value of the remove ads key to 1
                PlayerPrefs.SetInt(RemoveAdsKey, 1);

                // Starting the coroutine to disable the remove ads button container until the purchase is complete
                StartCoroutine(Routine_WaitToDisableRemoveAdsContainerUntilPurchaseIsComplete());
            }
            
            // Purchasing the five shield product
            if (purchaseEvent.purchasedProduct.definition.id == FiveShieldProductId)
            {
                print("The player has purchased the five shield product");

                //Adding 5 to the player's shield inventory
                PlayerPrefs.SetInt(ShieldKey, PlayerPrefs.GetInt(ShieldKey) + 5);

                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;

                //Check if the player has the shield key in the local dictionary
                if (localDictionary.ContainsKey("Shield"))
                {
                    //If the player has the shield key in the local dictionary, then we will update the value
                    //of the shield key in the local dictionary
                    localDictionary["Shield"] = PlayerPrefs.GetInt(ShieldKey);

                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();

                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                
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
                else
                {
                    Debug.LogError("The player does not have the Shield key in the local dictionary");
                
                    try
                    {
                        // Playing the button click sound
                        SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            
            // Purchasing the five health product
            if (purchaseEvent.purchasedProduct.definition.id == FiveHealthProductId)
            {
                print("The player has purchased the five health product");
                
                //Adding 5 to the player's health inventory
                PlayerPrefs.SetInt(HealthKey, PlayerPrefs.GetInt(HealthKey) + 5);
                
                //Get the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
                
                //Check if the player has the ExtraHealth in the local dictionary
                if (localDictionary.ContainsKey("ExtraHealth"))
                {
                    //If the player has the ExtraHealth in the local dictionary, then we will update the value
                    //of the ExtraHealth in the local dictionary
                    localDictionary["ExtraHealth"] = PlayerPrefs.GetInt(HealthKey);
                    
                    //Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
                    //Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                    
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
                else
                {
                    Debug.LogError("The player does not have the ExtraHealth key in the local dictionary");
                    
                    try
                    {
                        // Playing the button click sound
                        SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            
            // Purchasing the seventy five tokens product
            if (purchaseEvent.purchasedProduct.definition.id == SeventyFiveTokensProductId)
            {
                print("The player has purchased the seventy five tokens product");
                
                //Adding 75 to the player's token inventory
                PlayerPrefs.SetInt(PlayerTokensKeyReference, PlayerPrefs.GetInt(PlayerTokensKeyReference) + 75);
                
                // Getting the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
                
                // Check if the player has the TokensInTotal in the local dictionary
                if (localDictionary.ContainsKey("TokensInTotal"))
                {
                    // If the player has the TokensInTotal in the local dictionary, then we will update the value
                    // of the TokensInTotal in the local dictionary
                    localDictionary["TokensInTotal"] = PlayerPrefs.GetInt(PlayerTokensKeyReference);
                    
                    // Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
                    // Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                    
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
                else
                {
                    Debug.LogError("The player does not have the TokensInTotal key in the local dictionary");
                    
                    try
                    {
                        // Playing the button click sound
                        SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            
            // Purchasing the one hundred and fifty tokens product
            if (purchaseEvent.purchasedProduct.definition.id == OneHundredAndFiftyTokensProductId)
            {
                print("The player has purchased the one hundred and fifty tokens product");
                
                // Adding 150 to the player's token inventory
                PlayerPrefs.SetInt(PlayerTokensKeyReference, PlayerPrefs.GetInt(PlayerTokensKeyReference) + 150);
                
                // Getting the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
                
                // Check if the player has the TokensInTotal in the local dictionary
                if (localDictionary.ContainsKey("TokensInTotal"))
                {
                    // If the player has the TokensInTotal in the local dictionary, then we will update the value
                    // of the TokensInTotal in the local dictionary
                    localDictionary["TokensInTotal"] = PlayerPrefs.GetInt(PlayerTokensKeyReference);
                    
                    // Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
                    // Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                    
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
                else
                {
                    Debug.LogError("The player does not have the TokensInTotal key in the local dictionary");
                    
                    try
                    {
                        // Playing the button click sound
                        SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            
            // Purchasing the three hundred tokens product
            if (purchaseEvent.purchasedProduct.definition.id == ThreeHundredTokensProductId)
            {
                print("The player has purchased the three hundred tokens product");
                
                // Adding 300 to the player's token inventory
                PlayerPrefs.SetInt(PlayerTokensKeyReference, PlayerPrefs.GetInt(PlayerTokensKeyReference) + 300);
                
                // Getting the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
                
                // Check if the player has the TokensInTotal in the local dictionary
                if (localDictionary.ContainsKey("TokensInTotal"))
                {
                    // If the player has the TokensInTotal in the local dictionary, then we will update the value
                    // of the TokensInTotal in the local dictionary
                    localDictionary["TokensInTotal"] = PlayerPrefs.GetInt(PlayerTokensKeyReference);
                    
                    // Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
                    // Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                    
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
                else
                {
                    Debug.LogError("The player does not have the TokensInTotal key in the local dictionary");
                    
                    try
                    {
                        // Playing the button click sound
                        SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }
            
            // Purchasing the five hundred tokens product
            if (purchaseEvent.purchasedProduct.definition.id == FiveHundredTokensProductId)
            {
                print("The player has purchased the five hundred tokens product");
                
                // Adding 500 to the player's token inventory
                PlayerPrefs.SetInt(PlayerTokensKeyReference, PlayerPrefs.GetInt(PlayerTokensKeyReference) + 500);
                
                // Getting the local user data dictionary from the PlayFab server
                var localDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
                
                // Check if the player has the TokensInTotal in the local dictionary
                if (localDictionary.ContainsKey("TokensInTotal"))
                {
                    // If the player has the TokensInTotal in the local dictionary, then we will update the value
                    // of the TokensInTotal in the local dictionary
                    localDictionary["TokensInTotal"] = PlayerPrefs.GetInt(PlayerTokensKeyReference);
                    
                    // Upload the local dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
                    // Updating the player's inventory
                    SetupPlayerInventoryValuesBasedOnKeyValues();
                    
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
                else
                {
                    Debug.LogError("The player does not have the TokensInTotal key in the local dictionary");
                    
                    try
                    {
                        // Playing the button click sound
                        SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            }

            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            print("The purchase has failed due to " + failureReason);

            // Checking if the product id was the remove ads for one month subscription
            if (product.definition.id == RemoveAdsPerMonthProductId)
            {
                print("The purchase has failed for the remove ads for one month subscription");

                // Setting all of the buttons to be interactable again
                removeAdsForOneMonthButton.interactable = true;
                removeAdsForFourMonthsButton.interactable = true;
                removeAdsButtonContainerBackButton.interactable = true;
            }

            // Checking if the product id was the remove ads for four months subscription
            if (product.definition.id == RemoveAdsPerFourMonthsProductId)
            {
                print("The purchase has failed for the remove ads for four months subscription");

                // Setting all of the buttons to be interactable again
                removeAdsForOneMonthButton.interactable = true;
                removeAdsForFourMonthsButton.interactable = true;
                removeAdsButtonContainerBackButton.interactable = true;
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            print("The store has been initialized");
            _storeController = controller;

            // Checking if the user has subscribed to the remove ads
            CheckingRemoveAdsReceipt();
        }

        #endregion
    }
}