using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game_Designer_Online.Scripts.Misc;
using Game_Designer_Online.Scripts.Playfab_Related_Scripts;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Game_Designer_Online.Scripts.Main_Menu
{
    /// <summary>
    /// This script is attached to the CustomizationMenuCanvas. This will allow the player
    /// to customize their character and will later be used to save the player's customization.
    /// This has to be uploaded to the server so that the player can access their customization
    /// </summary>
    public class CustomizationMenuCanvas : MonoBehaviour
    {
        #region Buttons Related to Customization Menu (Misc)

        [Header("Customization Menu Buttons")]
        //Back Button
        [SerializeField] private Button backButton;

        /// <summary>
        /// Runs when the back button is clicked
        /// </summary>
        private void OnBackButtonClicked()
        {
            print("Back button clicked");
            
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

        #region Buttons Related to Customization Menu (Colors)

        /// <summary>
        /// Unlocks the green color
        /// </summary>
        public void UnlockGreenColor()
        {
            print("Green Color was Unlocked");
            
            //Getting the local user dictionary
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);

                if (getSpriteColorsDictionary.ContainsKey("ColorOne"))
                {
                    //Changing the value of the key ColorOne to 1
                    getSpriteColorsDictionary["ColorOne"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Green color was unlocked");
                    
                    //Setting the value of the green color to 1
                    colorsUnlocked["Green"] = 1;
                    
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[0].priceText.text = "Unlocked";
                    
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[0].customizationButtonTemplate.
                        DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorOne in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }

        /// <summary>
        /// Unlocks the green color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockGreenColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Green"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[0].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[0].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the green color
        /// </summary>
        public void SelectGreenColor()
        {
            print("Green color was selected");
            
            //Unselecting all the colors, hats and glasses
            ChangeAllColorsSelectedValueToZero();

            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Green"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Green");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the bull color
        /// </summary>
        public void UnlockBullColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorTwo"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 450)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 450;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();

                    //Changing the value of the key ColorTwo to 1
                    getSpriteColorsDictionary["ColorTwo"] = 1;

                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Bull color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Bull"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[1].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[1].customizationButtonTemplate.
                        DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorTwo in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockBullColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Bull"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[1].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[1].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the bull color
        /// </summary>
        public void SelectBullColor()
        {
            print("Bull color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Bull"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Bull");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the cactus color is unlocked
        /// </summary>
        public void UnlockCactusColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorThree"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 350)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 350;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorThree to 1
                    getSpriteColorsDictionary["ColorThree"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Cactus color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Cactus"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[2].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[2].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorThree in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockCactusColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Cactus"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[2].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[2].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the cactus color is selected
        /// </summary>
        public void SelectCactusColor()
        {
            print("Cactus color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Cactus"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Cactus");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the crystal color is unlocked
        /// </summary>
        public void UnlockCrystalColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorFour"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 350)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 350;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorFour to 1
                    getSpriteColorsDictionary["ColorFour"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Crystal color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Crystal"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[3].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[3].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorFour in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockCrystalColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Crystal"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[3].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[3].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the crystal color is selected
        /// </summary>
        public void SelectCrystalColor()
        {
            print("Crystal color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Crystal"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Crystal");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the dark color is unlocked
        /// </summary>
        public void UnlockDarkColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorFive"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 650)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 650;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorFive to 1
                    getSpriteColorsDictionary["ColorFive"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Dark color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Dark"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[4].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[4].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorFive in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockDarkColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Dark"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[4].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[4].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the dark color is selected
        /// </summary>
        public void SelectDarkColor()
        {
            print("Dark color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Dark"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Dark");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the electric color is unlocked
        /// </summary>
        public void UnlockElectricColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorSix"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 300)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 300;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorSix to 1
                    getSpriteColorsDictionary["ColorSix"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Electric color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Electric"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[5].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[5].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorSix in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockElectricColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Electric"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[5].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[5].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the electric color is selected
        /// </summary>
        public void SelectElectricColor()
        {
            print("Electric color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();

            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Electric"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Electric");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the fire color is unlocked
        /// </summary>
        public void UnlockFireColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorSeven"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 250)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 250;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorSeven to 1
                    getSpriteColorsDictionary["ColorSeven"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Fire color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Fire"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[6].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[6].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorSeven in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockFireColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Fire"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[6].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[6].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the fire color is selected
        /// </summary>
        public void SelectFireColor()
        {
            print("Fire color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Fire"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Fire");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the nyan color is unlocked
        /// </summary>
        public void UnlockNyanColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorEight"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 500)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 500;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorEight to 1
                    getSpriteColorsDictionary["ColorEight"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Nyan color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Nyan"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[7].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[7].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorEight in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockNyanColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Nyan"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[7].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[7].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the nyan color is selected
        /// </summary>
        public void SelectNyanColor()
        {
            print("Nyan color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Nyan"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Nyan");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the poop color is unlocked
        /// </summary>
        public void UnlockPoopColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorNine"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 400)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 400;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorNine to 1
                    getSpriteColorsDictionary["ColorNine"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Poop color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Poop"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[8].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[8].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorNine in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockPoopColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Poop"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[8].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[8].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the poop color is selected
        /// </summary>
        public void SelectPoopColor()
        {
            print("Poop color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Poop"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Poop");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the rock color is unlocked
        /// </summary>
        public void UnlockRockColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorTen"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 150)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 150;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorNine to 1
                    getSpriteColorsDictionary["ColorTen"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Rock color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Rock"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[9].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[9].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorTen in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockRockColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Rock"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[9].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[9].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the rock color is selected
        /// </summary>
        public void SelectRockColor()
        {
            print("Rock color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Rock"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Rock");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the skull color is unlocked
        /// </summary>
        public void UnlockSkullColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorEleven"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 550)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 550;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorNine to 1
                    getSpriteColorsDictionary["ColorEleven"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Skull color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Skull"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[10].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[10].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorNine in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }

        /// <summary>
        /// Unlocks this color on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockSkullColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Skull"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[10].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[10].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the skull color is selected
        /// </summary>
        public void SelectSkullColor()
        {
            print("Skull color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Skull"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Skull");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Runs when the water color is unlocked
        /// </summary>
        public void UnlockWaterColor()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteColors key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                var serializeSpriteColors = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteColors);
                
                if (getSpriteColorsDictionary.ContainsKey("ColorTwelve"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 200)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 200;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key ColorNine to 1
                    getSpriteColorsDictionary["ColorTwelve"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteColors"] 
                        = getSpriteColorsDictionary;
                    
                    print("Water color was unlocked");
            
                    //Setting the value of the green color to 1
                    colorsUnlocked["Water"] = 1;
            
                    //Changing the text of the green color
                    colorCustomizationTemplateItems[11].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[11].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key ColorNine in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }
        }
        
        private void UnlockWaterColorOnEnable()
        {
            //Setting the value of the green color to 1
            colorsUnlocked["Water"] = 1;
                    
            //Changing the text of the green color
            colorCustomizationTemplateItems[11].priceText.text = "Unlocked";
                    
            //Deactivating the buy button
            colorCustomizationTemplateItems[11].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Runs when the water color is selected
        /// </summary>
        public void SelectWaterColor()
        {
            print("Water color was selected");
            
            //Unselecting all the colors
            ChangeAllColorsSelectedValueToZero();
            
            //Setting the value of the green color to 1
            HandleConstValuesStaticObjectsAndKeys.ColorSelected["Water"] = 1;
            
            //Select this color to the previously selected color
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, "Water");
            
            //Changing the text of the green color
            ChangeTextOfAColorThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        [Header("Buttons Related to Customization Menu (Colors)")]
        //This is the dictionary that will be used to store the colors and also tell the game which colors
        //are unlocked and which ones are locked
        public Dictionary<string, int> colorsUnlocked = new Dictionary<string, int>
        {
            {"Green", 1},
            {"Bull", 0},
            {"Cactus", 0},
            {"Crystal", 0},
            {"Dark", 0},
            {"Electric", 0},
            {"Fire", 0},
            {"Nyan", 0},
            {"Poop", 0},
            {"Rock", 0},
            {"Skull", 0},
            {"Water", 0},
        };
        
        /// <summary>
        /// This is a reference to the CustomizationTemplateItems array that will be used to store the references
        /// of the buttons with their text
        /// </summary>
        [SerializeField] private CustomizationTemplateItems[] colorCustomizationTemplateItems;

        /// <summary>
        /// This will change the text properties of the colors that are unlocked according to the dictionary
        /// </summary>
        private void ChangeTextOfColorsThatAreUnlocked()
        {
            //Using a for loop to iterate through the colorsUnlocked dictionary
            for (int i = 0; i < colorsUnlocked.Count; i++)
            {
                //Getting the Key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = colorsUnlocked.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    colorCustomizationTemplateItems[i].priceText.text = "Unlocked";
                    
                    //Deactivating the buy button
                    colorCustomizationTemplateItems[i].customizationButtonTemplate.
                        DeactivateTheBuyButtonIfItemIsUnlocked();
                }
            }
        }

        /// <summary>
        /// This will change the text properties of the color that is selected according to the dictionary
        /// </summary>
        private void ChangeTextOfAColorThatIsSelected()
        {
            //Using a for loop to iterate through the colorSelected dictionary
            for (int i = 0; i < HandleConstValuesStaticObjectsAndKeys.ColorSelected.Count; i++)
            {
                //Getting the Key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = 
                    HandleConstValuesStaticObjectsAndKeys.ColorSelected.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    colorCustomizationTemplateItems[i].priceText.text = "Selected";
                }
            }
        }
        
        /// <summary>
        /// This should be called before selecting a color. This will change the value of all the colors
        /// to zero so that only one color is selected at one time
        /// </summary>
        private void ChangeAllColorsSelectedValueToZero()
        {
            //Changing all of the hats and glasses selected values to zero
            ChangeAllHatsSelectedValueToZero();
            ChangeAllGlassesSelectedValueToZero();
            
            //Change the text back to unlocked as well for glasses and hats
            ChangeTextOfHatsThatAreUnlocked();
            ChangeTextOfGlassesThatAreUnlocked();

            //Using a for loop to iterate through the colorSelected dictionary
            for (int i = 0; i < HandleConstValuesStaticObjectsAndKeys.ColorSelected.Count; i++)
            {
                //Getting the Key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = HandleConstValuesStaticObjectsAndKeys.
                    ColorSelected.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    HandleConstValuesStaticObjectsAndKeys.ColorSelected[keyValuePair.Key] = 0;
                    
                    //Changing the text of the button
                    colorCustomizationTemplateItems[i].priceText.text = "Select";
                }
            }
        }

        #endregion

        #region Buttons Related to Customization Menu (Hats)

        /// <summary>
        /// Unlocks the Hat One Accessories
        /// </summary>
        public void UnlockHatOne()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryOne"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 400)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 400;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryOne to 1
                    getSpriteAccessoriesDictionary["AccessoryOne"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat One was unlocked");
            
                    //Setting the value of the HatOne to 1
                    hatsUnlocked["HatOne"] = 1;
            
                    //Changing the text of the HatOne
                    hatCustomizationTemplateItems[0].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[0].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryOne in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatOneOnEnable()
        {
            //Setting the value of the HatOne to 1
            hatsUnlocked["HatOne"] = 1;
            
            //Changing the text of the HatOne
            hatCustomizationTemplateItems[0].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[0].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }

        /// <summary>
        /// Selects the Hat One
        /// </summary>
        public void SelectHatOne()
        {
            print("Hat One was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatOne to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatOne"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatOne");

            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatOne
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the Hat One
        /// </summary>
        public void UnlockHatTwo()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryTwo"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 450)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 450;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryTwo to 1
                    getSpriteAccessoriesDictionary["AccessoryTwo"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Two was unlocked");
            
                    //Setting the value of the HatTwo to 1
                    hatsUnlocked["HatTwo"] = 1;
            
                    //Changing the text of the HatTwo
                    hatCustomizationTemplateItems[1].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[1].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryTwo in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatTwoOnEnable()
        {
            //Setting the value of the HatTwo to 1
            hatsUnlocked["HatTwo"] = 1;
            
            //Changing the text of the HatTwo
            hatCustomizationTemplateItems[1].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[1].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the Hat Two
        /// </summary>
        public void SelectHatTwo()
        {
            print("Hat Two was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatTwo to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatTwo"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatTwo");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatTwo
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the Hat Three
        /// </summary>
        public void UnlockHatThree()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryThree"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 300)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 300;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryThree to 1
                    getSpriteAccessoriesDictionary["AccessoryThree"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Three was unlocked");
            
                    //Setting the value of the HatThree to 1
                    hatsUnlocked["HatThree"] = 1;
            
                    //Changing the text of the HatThree
                    hatCustomizationTemplateItems[2].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[2].customizationButtonTemplate.
                        DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryThree in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatThreeOnEnable()
        {
            //Setting the value of the HatThree to 1
            hatsUnlocked["HatThree"] = 1;
            
            //Changing the text of the HatThree
            hatCustomizationTemplateItems[2].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[2].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the Hat Three
        /// </summary>
        public void SelectHatThree()
        {
            print("Hat Three was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatThree to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatThree"] = 1;

            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatThree");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatThree
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the Hat Four
        /// </summary>
        public void UnlockHatFour()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryFour"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 350)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 350;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryFour to 1
                    getSpriteAccessoriesDictionary["AccessoryFour"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Four was unlocked");
            
                    //Setting the value of the HatFour to 1
                    hatsUnlocked["HatFour"] = 1;
            
                    //Changing the text of the HatFour
                    hatCustomizationTemplateItems[3].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[3].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryFour in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatFourOnEnable()
        {
            //Setting the value of the HatFour to 1
            hatsUnlocked["HatFour"] = 1;
            
            //Changing the text of the HatFour
            hatCustomizationTemplateItems[3].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[3].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the Hat Four
        /// </summary>
        public void SelectHatFour()
        {
            print("Hat Four was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatFour to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatFour"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatFour");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatFour
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks Hat Five
        /// </summary>
        public void UnlockHatFive()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryFive"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 600)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 600;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryFive to 1
                    getSpriteAccessoriesDictionary["AccessoryFive"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Five was unlocked");
            
                    //Setting the value of the HatFive to 1
                    hatsUnlocked["HatFive"] = 1;
            
                    //Changing the text of the HatFive
                    hatCustomizationTemplateItems[4].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[4].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryFive in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatFiveOnEnable()
        {
            //Setting the value of the HatFive to 1
            hatsUnlocked["HatFive"] = 1;
            
            //Changing the text of the HatFive
            hatCustomizationTemplateItems[4].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[4].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects Hat Five
        /// </summary>
        public void SelectHatFive()
        {
            print("Hat Five was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatFive to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatFive"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatFive");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatFive
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks Hat Six
        /// </summary>
        public void UnlockHatSix()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessorySix"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 250)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 250;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessorySix to 1
                    getSpriteAccessoriesDictionary["AccessorySix"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Six was unlocked");
            
                    //Setting the value of the HatSix to 1
                    hatsUnlocked["HatSix"] = 1;
            
                    //Changing the text of the HatSix
                    hatCustomizationTemplateItems[5].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[5].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessorySix in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatSixOnEnable()
        {
            //Setting the value of the HatSix to 1
            hatsUnlocked["HatSix"] = 1;
            
            //Changing the text of the HatSix
            hatCustomizationTemplateItems[5].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[5].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects Hat Six
        /// </summary>
        public void SelectHatSix()
        {
            print("Hat Six was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatSix to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatSix"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatSix");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatSix
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            // Uploading the new data to the server, by updating the user dictionary
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks Hat Seven
        /// </summary>
        public void UnlockHatSeven()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessorySeven"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 500)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 500;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessorySeven to 1
                    getSpriteAccessoriesDictionary["AccessorySeven"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Seven was unlocked");
            
                    //Setting the value of the HatSeven to 1
                    hatsUnlocked["HatSeven"] = 1;
            
                    //Changing the text of the HatSeven
                    hatCustomizationTemplateItems[6].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[6].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessorySeven in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// This will unlock the hat seven and enable it
        /// </summary>
        private void UnlockHatSevenOnEnable()
        {
            //Setting the value of the HatSeven to 1
            hatsUnlocked["HatSeven"] = 1;
            
            //Changing the text of the HatSeven
            hatCustomizationTemplateItems[6].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[6].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects Hat Seven
        /// </summary>
        public void SelectHatSeven()
        {
            print("Hat Seven was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatSeven to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatSeven"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatSeven");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatSeven
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the Hat Eight
        /// </summary>
        public void UnlockHatEight()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryEight"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 550)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 550;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryEight to 1
                    getSpriteAccessoriesDictionary["AccessoryEight"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Eight was unlocked");
            
                    //Setting the value of the HatEight to 1
                    hatsUnlocked["HatEight"] = 1;
            
                    //Changing the text of the HatEight
                    hatCustomizationTemplateItems[7].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[7].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryEight in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatEightOnEnable()
        {
            //Setting the value of the HatEight to 1
            hatsUnlocked["HatEight"] = 1;
            
            //Changing the text of the HatEight
            hatCustomizationTemplateItems[7].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[7].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the Hat Eight
        /// </summary>
        public void SelectHatEight()
        {
            print("Hat Eight was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatEight to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatEight"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatEight");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatEight
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the Hat Nine
        /// </summary>
        public void UnlockHatNine()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryNine"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 800)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 800;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryNine to 1
                    getSpriteAccessoriesDictionary["AccessoryNine"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Nine was unlocked");
            
                    //Setting the value of the HatNine to 1
                    hatsUnlocked["HatNine"] = 1;
            
                    //Changing the text of the HatNine
                    hatCustomizationTemplateItems[8].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[8].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryNine in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatNineOnEnable()
        {
            //Setting the value of the HatNine to 1
            hatsUnlocked["HatNine"] = 1;
            
            //Changing the text of the HatNine
            hatCustomizationTemplateItems[8].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[8].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the Hat Nine
        /// </summary>
        public void SelectHatNine()
        {
            print("Hat Nine was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatNine to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatNine"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatNine");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatNine
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the Hat Ten
        /// </summary>
        public void UnlockHatTen()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryTen"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 750)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 750;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryTen to 1
                    getSpriteAccessoriesDictionary["AccessoryTen"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Ten was unlocked");
            
                    //Setting the value of the HatTen to 1
                    hatsUnlocked["HatTen"] = 1;
            
                    //Changing the text of the HatTen
                    hatCustomizationTemplateItems[9].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[9].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryTen in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatTenOnEnable()
        {
            //Setting the value of the HatTen to 1
            hatsUnlocked["HatTen"] = 1;
            
            //Changing the text of the HatTen
            hatCustomizationTemplateItems[9].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[9].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the Hat Ten
        /// </summary>
        public void SelectHatTen()
        {
            print("Hat Ten was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatTen to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatTen"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatTen");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatTen
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlock Hat Eleven
        /// </summary>
        public void UnlockHatEleven()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);

                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryEleven"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 750)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 750;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryEleven to 1
                    getSpriteAccessoriesDictionary["AccessoryEleven"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Eleven was unlocked");
            
                    //Setting the value of the HatEleven to 1
                    hatsUnlocked["HatEleven"] = 1;
            
                    //Changing the text of the HatEleven
                    hatCustomizationTemplateItems[10].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[10].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryEleven in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatElevenOnEnable()
        {
            //Setting the value of the HatEleven to 1
            hatsUnlocked["HatEleven"] = 1;
            
            //Changing the text of the HatEleven
            hatCustomizationTemplateItems[10].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[10].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects Hat Eleven
        /// </summary>
        public void SelectHatEleven()
        {
            print("Hat Eleven was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatEleven to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatEleven"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatEleven");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatEleven
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks Hat Twelve
        /// </summary>
        public void UnlockHatTwelve()
        {
            //Getting the dictionary from the server
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories")) 
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);

                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryTwelve"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 200)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 200;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key AccessoryTwelve to 1
                    getSpriteAccessoriesDictionary["AccessoryTwelve"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Hat Twelve was unlocked");
            
                    //Setting the value of the HatTwelve to 1
                    hatsUnlocked["HatTwelve"] = 1;
            
                    //Changing the text of the HatTwelve
                    hatCustomizationTemplateItems[11].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[11].customizationButtonTemplate.
                        DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key AccessoryTwelve in the dictionary");
                }
            } 
            else 
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these hats on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockHatTwelveOnEnable()
        {
            //Setting the value of the HatTwelve to 1
            hatsUnlocked["HatTwelve"] = 1;
            
            //Changing the text of the HatTwelve
            hatCustomizationTemplateItems[11].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            hatCustomizationTemplateItems[11].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects Hat Twelve
        /// </summary>
        public void SelectHatTwelve()
        {
            print("Hat Twelve was selected");
            
            //Unselecting all the hats
            ChangeAllHatsSelectedValueToZero();
            
            //Setting the value of the HatTwelve to 1
            HandleConstValuesStaticObjectsAndKeys.HatsSelected["HatTwelve"] = 1;
            
            //Setting the value of the last selected hat
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "HatTwelve");
            
            //Making sure that the user does not end up selecting glasses in the main game if
            //hats were selected
            ChangeAllGlassesSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "");
            
            //Changing the text of the HatTwelve
            ChangeTextOfAHatThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }

        [Header("Buttons Related to Customization Menu (Hats)")]
        //This is the dictionary that will be used to store the hats and also tell the game which hats
        //are unlocked and which ones are locked
        public Dictionary<string, int> hatsUnlocked = new Dictionary<string, int>
        {
            {"HatOne", 0},
            {"HatTwo", 0},
            {"HatThree", 0},
            {"HatFour", 0},
            {"HatFive", 0},
            {"HatSix", 0},
            {"HatSeven", 0},
            {"HatEight", 0},
            {"HatNine", 0},
            {"HatTen", 0},
            {"HatEleven", 0},
            {"HatTwelve", 0},
            {"HatThirteen", 0},
        };
        
        /// <summary>
        /// This is a reference to the CustomizationTemplateItems array that will be used to store the references
        /// for hat items
        /// </summary>
        [SerializeField] private CustomizationTemplateItems[] hatCustomizationTemplateItems;
        
        /// <summary>
        /// This will change the text properties of the hats that are unlocked according to the dictionary
        /// </summary>
        private void ChangeTextOfHatsThatAreUnlocked()
        {
            //Using a for loop to iterate through the hatsUnlocked dictionary
            for (int i = 0; i < hatsUnlocked.Count; i++)
            {
                //Getting the key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = hatsUnlocked.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    hatCustomizationTemplateItems[i].priceText.text = "Unlocked";
                    
                    //Deactivating the buy button
                    hatCustomizationTemplateItems[i].customizationButtonTemplate.
                        DeactivateTheBuyButtonIfItemIsUnlocked();
                }
            }
        }

        /// <summary>
        /// This will change the text properties of the hat that is selected according to the dictionary
        /// </summary>
        private void ChangeTextOfAHatThatIsSelected()
        {
            //Using a for loop to iterate through the hatsSelected dictionary
            for (int i = 0; i < HandleConstValuesStaticObjectsAndKeys.HatsSelected.Count; i++)
            {
                //Getting the key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = HandleConstValuesStaticObjectsAndKeys.HatsSelected.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    hatCustomizationTemplateItems[i].priceText.text = "Selected";
                }
            }
        }

        /// <summary>
        /// This should be called before selecting a hat. This will change the value of all the colors
        /// to zero so that only one hat is selected at one time
        /// </summary>
        private void ChangeAllHatsSelectedValueToZero()
        {
            //Using a for loop to iterate through the hatsSelected dictionary
            for (int i = 0; i < HandleConstValuesStaticObjectsAndKeys.HatsSelected.Count; i++)
            {
                //Getting the key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = HandleConstValuesStaticObjectsAndKeys.HatsSelected.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    HandleConstValuesStaticObjectsAndKeys.HatsSelected[keyValuePair.Key] = 0;
                    
                    //Changing the text of the button
                    hatCustomizationTemplateItems[i].priceText.text = "Select";
                }
            }
        }

        #endregion

        #region Buttons Related to Customization Menu (Glasses)

        /// <summary>
        /// Unlocks the Glasses One Accessories
        /// </summary>
        public void UnlockGlassesOne()
        {
            //Getting the local user dictionary
            var localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (localUserDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = localUserDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("GlassesOne"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 350)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 350;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key GlassesOne to 1
                    getSpriteAccessoriesDictionary["GlassesOne"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Glasses One was unlocked");
            
                    //Setting the value of the GlassesOne to 1
                    glassesUnlocked["GlassesOne"] = 1;
            
                    //Changing the text of the GlassesOne
                    glassesCustomizationTemplateItems[0].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    glassesCustomizationTemplateItems[0].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key GlassesOne in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these glasses on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockGlassesOneOnEnable()
        {
            //Setting the value of the GlassesOne to 1
            glassesUnlocked["GlassesOne"] = 1;
            
            //Changing the text of the GlassesOne
            glassesCustomizationTemplateItems[0].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            glassesCustomizationTemplateItems[0].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the Glasses One
        /// </summary>
        public void SelectGlassesOne()
        {
            print("Glasses One was selected");
            
            //Unselecting all the glasses
            ChangeAllGlassesSelectedValueToZero();

            //Setting the value of the GlassesOne to 1
            HandleConstValuesStaticObjectsAndKeys.GlassesSelected["GlassesOne"] = 1;
            
            //Setting the value of the last selected glasses
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "GlassesOne");
            
            //Making sure that the user does not end up selecting a hat in the main game if
            //glasses were selected
            ChangeAllHatsSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "");

            //Changing the text of the GlassesOne
            ChangeTextOfAGlassesThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the Glasses Two Accessories
        /// </summary>
        public void UnlockGlassesTwo()
        {
            //Getting the local user dictionary
            var localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (localUserDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = localUserDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("GlassesTwo"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 700)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 700;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key GlassesTwo to 1
                    getSpriteAccessoriesDictionary["GlassesTwo"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Glasses Two was unlocked");
            
                    //Setting the value of the GlassesTwo to 1
                    glassesUnlocked["GlassesTwo"] = 1;
            
                    //Changing the text of the GlassesTwo
                    glassesCustomizationTemplateItems[1].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    glassesCustomizationTemplateItems[1].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key GlassesTwo in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these glasses on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockGlassesTwoOnEnable()
        {
            //Setting the value of the GlassesTwo to 1
            glassesUnlocked["GlassesTwo"] = 1;
            
            //Changing the text of the GlassesTwo
            glassesCustomizationTemplateItems[1].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            glassesCustomizationTemplateItems[1].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects the Glasses Two
        /// </summary>
        public void SelectGlassesTwo()
        {
            print("Glasses Two was selected");
            
            //Unselecting all the glasses
            ChangeAllGlassesSelectedValueToZero();
            
            //Setting the value of the GlassesTwo to 1
            HandleConstValuesStaticObjectsAndKeys.GlassesSelected["GlassesTwo"] = 1;

            //Setting the value of the last selected glasses
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "GlassesTwo");
            
            //Making sure that the user does not end up selecting a hat in the main game if
            //glasses were selected
            ChangeAllHatsSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "");
            
            //Changing the text of the GlassesTwo
            ChangeTextOfAGlassesThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        /// <summary>
        /// Unlocks the Glasses Three Accessories
        /// </summary>
        public void UnlockGlassesThree()
        {
            //Getting the local user dictionary
            var localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Checking if we have the SpriteAccessories key in the dictionary
            if (localUserDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = localUserDictionary["SpriteAccessories"];
                
                //Serializing the value of the key SpriteAccessories
                var serializeSpriteAccessories = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary 
                    = JsonConvert.DeserializeObject<Dictionary<string, int>>(serializeSpriteAccessories);
                
                if (getSpriteAccessoriesDictionary.ContainsKey("GlassesThree"))
                {
                    //Getting the tokens in total
                    var tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
                    
                    //Making sure that the player has the required amount of tokens
                    if (tokensInTotal < 150)
                    {
                        print("The user doesn't have enough tokens to unlock this color");
                        
                        //Flashing the tokens label
                        StartCoroutine(Routine_FlashTokensLabelRed());
                        
                        try
                        {
                            // Playing the button click sound
                            SoundManager.Instance.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        return;
                    }

                    //Removing the tokens from the player
                    tokensInTotal -= 150;
                        
                    //Setting the tokens in total
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);
                        
                    //Updating the LocalUserDataDictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["TokensInTotal"] 
                        = tokensInTotal;
                        
                    //Updating the tokens label
                    UpdateTokensLabelText();
                    
                    //Changing the value of the key GlassesThree to 1
                    getSpriteAccessoriesDictionary["GlassesThree"] = 1;
                    
                    //Putting the dictionary back into the local user data dictionary
                    HandleUserDataFromPlayFabServer.LocalUserDataDictionary["SpriteAccessories"] 
                        = getSpriteAccessoriesDictionary;
                    
                    print("Glasses Three was unlocked");
            
                    //Setting the value of the GlassesThree to 1
                    glassesUnlocked["GlassesThree"] = 1;
            
                    //Changing the text of the GlassesThree
                    glassesCustomizationTemplateItems[2].priceText.text = "Unlocked";
            
                    //Deactivating the buy button
                    glassesCustomizationTemplateItems[2].customizationButtonTemplate.DeactivateTheBuyButtonIfItemIsUnlocked();
                    
                    //Saving the dictionary to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    
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
                    Debug.Log("Could not find the key GlassesThree in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }
        
        /// <summary>
        /// Unlocks these glasses on enable. This is similar to the function above it. However, it just
        /// doesn't try to upload data to the server. This is because we already have the server data
        /// and we only have to load it
        /// </summary>
        private void UnlockGlassesThreeOnEnable()
        {
            //Setting the value of the GlassesThree to 1
            glassesUnlocked["GlassesThree"] = 1;
            
            //Changing the text of the GlassesThree
            glassesCustomizationTemplateItems[2].priceText.text = "Unlocked";
            
            //Deactivating the buy button
            glassesCustomizationTemplateItems[2].customizationButtonTemplate.
                DeactivateTheBuyButtonIfItemIsUnlocked();
        }
        
        /// <summary>
        /// Selects The Glasses Three
        /// </summary>
        public void SelectGlassesThree()
        {
            print("Glasses Three was selected");
            
            //Unselecting all the glasses
            ChangeAllGlassesSelectedValueToZero();
            
            //Setting the value of the GlassesThree to 1
            HandleConstValuesStaticObjectsAndKeys.GlassesSelected["GlassesThree"] = 1;

            //Setting the value of the last selected glasses
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, "GlassesThree");
            
            //Making sure that the user does not end up selecting a hat in the main game if
            //glasses were selected
            ChangeAllHatsSelectedValueToZero();
            PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, "");
            
            //Changing the text of the GlassesThree
            ChangeTextOfAGlassesThatIsSelected();
            
            try
            {
                // Playing the button click sound
                SoundManager.Instance.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            HandleUserDataFromPlayFabServer.Instance.UpdateUserDictionaryOnServerWhenClothesItemWasChanged();
        }
        
        [Header("Buttons Related to Customization Menu (Glasses)")]
        //This is the dictionary that will be used to store the glasses and also tell the game which glasses
        //are unlocked and which ones are locked
        public Dictionary<string, int> glassesUnlocked = new Dictionary<string, int>
        {
            {"GlassesOne", 0},
            {"GlassesTwo", 0},
            {"GlassesThree", 0},
        };
        
        /// <summary>
        /// This is a reference to the CustomizationTemplateItems array that will be used to store the references
        /// for glasses items
        /// </summary>
        [SerializeField] private CustomizationTemplateItems[] glassesCustomizationTemplateItems;
        
        /// <summary>
        /// This will change the text properties of the glasses that are unlocked according to the dictionary
        /// </summary>
        private void ChangeTextOfGlassesThatAreUnlocked()
        {
            //Using a for loop to iterate through the glassesUnlocked dictionary
            for (int i = 0; i < glassesUnlocked.Count; i++)
            {
                //Getting the key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = glassesUnlocked.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    glassesCustomizationTemplateItems[i].priceText.text = "Unlocked";
                    
                    //Deactivating the buy button
                    glassesCustomizationTemplateItems[i].customizationButtonTemplate.
                        DeactivateTheBuyButtonIfItemIsUnlocked();
                }
            }
        }
        
        /// <summary>
        /// This will change the text properties of the glasses that is selected according to the dictionary
        /// </summary>
        private void ChangeTextOfAGlassesThatIsSelected()
        {
            //Using a for loop to iterate through the glassesSelected dictionary
            for (int i = 0; i < HandleConstValuesStaticObjectsAndKeys.GlassesSelected.Count; i++)
            {
                //Getting the key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = HandleConstValuesStaticObjectsAndKeys.
                    GlassesSelected.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    glassesCustomizationTemplateItems[i].priceText.text = "Selected";
                }
            }
        }
        
        /// <summary>
        /// This should be called before selected a glasses. This will change the value of all the glasses
        /// to zero so that only one hat is selected at one time
        /// </summary>
        private void ChangeAllGlassesSelectedValueToZero()
        {
            //Using a for loop to iterate through the glassesSelected dictionary
            for (int i = 0; i < HandleConstValuesStaticObjectsAndKeys.GlassesSelected.Count; i++)
            {
                //Getting the key value pair of the dictionary
                KeyValuePair<string, int> keyValuePair = HandleConstValuesStaticObjectsAndKeys.GlassesSelected.ElementAt(i);
                
                //Checking if the value of the key value pair is 1
                if (keyValuePair.Value == 1)
                {
                    //If the value of the key value pair is 1, then we will change the text of the button
                    HandleConstValuesStaticObjectsAndKeys.GlassesSelected[keyValuePair.Key] = 0;
                    
                    //Changing the text of the button
                    glassesCustomizationTemplateItems[i].priceText.text = "Select";
                }
            }
        }

        #endregion

        #region Related to reading values from dictionary, and unlocking items

        /// <summary>
        /// This function will unlock the items based on the values from the dictionary
        /// </summary>
        private void UnlockItemsBasedOnValuesFromLocalDictionary()
        {
            //Storing the value of the local dictionary here
            var getLocalDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //The following code will unlock all of the SpriteColors that the player has bought
            //and are also part of the local dictionary
            if (getLocalDictionary.ContainsKey("SpriteColors"))
            {
                //Getting the value of the key SpriteColors
                object getSpriteColors = 
                    getLocalDictionary["SpriteColors"];
                
                //Serializing the value of the key SpriteColors
                string getSpriteColorsString = JsonConvert.SerializeObject(getSpriteColors);
                
                //Cast the value of the key SpriteColors to a dictionary
                var getSpriteColorsDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, int>>(getSpriteColorsString);
                
                //Checking if we have the key ColorOne in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorOne"))
                {
                    //Checking if the value of the key ColorOne is 1
                    if (getSpriteColorsDictionary["ColorOne"] == 1)
                    {
                        //If the value of the key ColorOne is 1, then we will unlock the item
                        UnlockGreenColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorOne in the dictionary");
                }
                
                //Checking if we have the key ColorTwo in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorTwo"))
                {
                    //Checking if the value of the key ColorTwo is 1
                    if (getSpriteColorsDictionary["ColorTwo"] == 1)
                    {
                        //If the value of the key ColorTwo is 1, then we will unlock the item
                        UnlockBullColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorTwo in the dictionary");
                }
                
                //Checking if we have the key ColorThree in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorThree"))
                {
                    //Checking if the value of the key ColorThree is 1
                    if (getSpriteColorsDictionary["ColorThree"] == 1)
                    {
                        //If the value of the key ColorThree is 1, then we will unlock the item
                        UnlockCactusColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorThree in the dictionary");
                }
                
                //Checking if we have the key ColorFour in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorFour"))
                {
                    //Checking if the value of the key ColorFour is 1
                    if (getSpriteColorsDictionary["ColorFour"] == 1)
                    {
                        //If the value of the key ColorFour is 1, then we will unlock the item
                        UnlockCrystalColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorFour in the dictionary");
                }
                
                //Checking if we have the key ColorFive in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorFive"))
                {
                    //Checking if the value of the key ColorFive is 1
                    if (getSpriteColorsDictionary["ColorFive"] == 1)
                    {
                        //If the value of the key ColorFive is 1, then we will unlock the item
                        UnlockDarkColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorFive in the dictionary");
                }
                
                //Checking if we have the key ColorSix in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorSix"))
                {
                    //Checking if the value of the key ColorSix is 1
                    if (getSpriteColorsDictionary["ColorSix"] == 1)
                    {
                        //If the value of the key ColorSix is 1, then we will unlock the item
                        UnlockElectricColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorSix in the dictionary");
                }
                
                //Checking if we have the key ColorSeven in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorSeven"))
                {
                    //Checking if the value of the key ColorSeven is 1
                    if (getSpriteColorsDictionary["ColorSeven"] == 1)
                    {
                        //If the value of the key ColorSeven is 1, then we will unlock the item
                        UnlockFireColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorSeven in the dictionary");
                }
                
                //Checking if we have the key ColorEight in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorEight"))
                {
                    //Checking if the value of the key ColorEight is 1
                    if (getSpriteColorsDictionary["ColorEight"] == 1)
                    {
                        //If the value of the key ColorEight is 1, then we will unlock the item
                        UnlockNyanColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorEight in the dictionary");
                }
                
                //Checking if we have the key ColorNine in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorNine"))
                {
                    //Checking if the value of the key ColorNine is 1
                    if (getSpriteColorsDictionary["ColorNine"] == 1)
                    {
                        //If the value of the key ColorNine is 1, then we will unlock the item
                        UnlockPoopColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorNine in the dictionary");
                }
                
                //Checking if we have the key ColorTen in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorTen"))
                {
                    //Checking if the value of the key ColorTen is 1
                    if (getSpriteColorsDictionary["ColorTen"] == 1)
                    {
                        //If the value of the key ColorTen is 1, then we will unlock the item
                        UnlockRockColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorTen in the dictionary");
                }
                
                //Checking if we have the key ColorEleven in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorEleven"))
                {
                    //Checking if the value of the key ColorEleven is 1
                    if (getSpriteColorsDictionary["ColorEleven"] == 1)
                    {
                        //If the value of the key ColorEleven is 1, then we will unlock the item
                        UnlockSkullColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorEleven in the dictionary");
                }
                
                //Checking if we have the key ColorTwelve in the dictionary and unlocking
                //if required
                if (getSpriteColorsDictionary.ContainsKey("ColorTwelve"))
                {
                    //Checking if the value of the key ColorTwelve is 1
                    if (getSpriteColorsDictionary["ColorTwelve"] == 1)
                    {
                        //If the value of the key ColorTwelve is 1, then we will unlock the item
                        UnlockWaterColorOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key ColorTwelve in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteColors in the dictionary");
            }

            //The following code will unlock all of the Sprite accessories that the player has bought
            //and are also part of the local dictionary
            if (getLocalDictionary.ContainsKey("SpriteAccessories"))
            {
                //Getting the value of the key SpriteAccessories
                object getSpriteAccessories = 
                    getLocalDictionary["SpriteAccessories"];

                //Serializing the value of the key SpriteAccessories
                string getSpriteAccessoriesString = JsonConvert.SerializeObject(getSpriteAccessories);
                
                //Cast the value of the key SpriteAccessories to a dictionary
                var getSpriteAccessoriesDictionary = 
                    JsonConvert.DeserializeObject<Dictionary<string, int>>(getSpriteAccessoriesString);
                
                //Checking if we have the key AccessoryOne in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryOne"))
                {
                    //Checking if the value of the key AccessoryOne is 1
                    if (getSpriteAccessoriesDictionary["AccessoryOne"] == 1)
                    {
                        //If the value of the key AccessoryOne is 1, then we will unlock the item
                        UnlockHatOneOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryOne in the dictionary");
                }
                
                //Checking if we have the key AccessoryTwo in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryTwo"))
                {
                    //Checking if the value of the key AccessoryTwo is 1
                    if (getSpriteAccessoriesDictionary["AccessoryTwo"] == 1)
                    {
                        //If the value of the key AccessoryTwo is 1, then we will unlock the item
                        UnlockHatTwoOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryTwo in the dictionary");
                }
                
                //Checking if we have the key AccessoryThree in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryThree"))
                {
                    //Checking if the value of the key AccessoryThree is 1
                    if (getSpriteAccessoriesDictionary["AccessoryThree"] == 1)
                    {
                        //If the value of the key AccessoryThree is 1, then we will unlock the item
                        UnlockHatThreeOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryThree in the dictionary");
                }
                
                //Checking if we have the key AccessoryFour in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryFour"))
                {
                    //Checking if the value of the key AccessoryFour is 1
                    if (getSpriteAccessoriesDictionary["AccessoryFour"] == 1)
                    {
                        //If the value of the key AccessoryFour is 1, then we will unlock the item
                        UnlockHatFourOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryFour in the dictionary");
                }
                
                //Checking if we have the key AccessoryFive in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryFive"))
                {
                    //Checking if the value of the key AccessoryFive is 1
                    if (getSpriteAccessoriesDictionary["AccessoryFive"] == 1)
                    {
                        //If the value of the key AccessoryFive is 1, then we will unlock the item
                        UnlockHatFiveOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryFive in the dictionary");
                }
                
                //Checking if we have the key AccessorySix in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessorySix"))
                {
                    //Checking if the value of the key AccessorySix is 1
                    if (getSpriteAccessoriesDictionary["AccessorySix"] == 1)
                    {
                        //If the value of the key AccessorySix is 1, then we will unlock the item
                        UnlockHatSixOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessorySix in the dictionary");
                }
                
                //Checking if we have the key AccessorySeven in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessorySeven"))
                {
                    //Checking if the value of the key AccessorySeven is 1
                    if (getSpriteAccessoriesDictionary["AccessorySeven"] == 1)
                    {
                        //If the value of the key AccessorySeven is 1, then we will unlock the item
                        UnlockHatSevenOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessorySeven in the dictionary");
                }
                
                //Checking if we have the key AccessoryEight in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryEight"))
                {
                    //Checking if the value of the key AccessoryEight is 1
                    if (getSpriteAccessoriesDictionary["AccessoryEight"] == 1)
                    {
                        //If the value of the key AccessoryEight is 1, then we will unlock the item
                        UnlockHatEightOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryEight in the dictionary");
                }
                
                //Checking if we have the key AccessoryNine in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryNine"))
                {
                    //Checking if the value of the key AccessoryNine is 1
                    if (getSpriteAccessoriesDictionary["AccessoryNine"] == 1)
                    {
                        //If the value of the key AccessoryNine is 1, then we will unlock the item
                        UnlockHatNineOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryNine in the dictionary");
                }
                
                //Checking if we have the key AccessoryTen in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryTen"))
                {
                    //Checking if the value of the key AccessoryTen is 1
                    if (getSpriteAccessoriesDictionary["AccessoryTen"] == 1)
                    {
                        //If the value of the key AccessoryTen is 1, then we will unlock the item
                        UnlockHatTenOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryTen in the dictionary");
                }
                
                //Checking if we have the key AccessoryEleven in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryEleven"))
                {
                    //Checking if the value of the key AccessoryEleven is 1
                    if (getSpriteAccessoriesDictionary["AccessoryEleven"] == 1)
                    {
                        //If the value of the key AccessoryEleven is 1, then we will unlock the item
                        UnlockHatElevenOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryEleven in the dictionary");
                }
                
                //Checking if we have the key AccessoryTwelve in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("AccessoryTwelve"))
                {
                    //Checking if the value of the key AccessoryTwelve is 1
                    if (getSpriteAccessoriesDictionary["AccessoryTwelve"] == 1)
                    {
                        //If the value of the key AccessoryTwelve is 1, then we will unlock the item
                        UnlockHatTwelveOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key AccessoryTwelve in the dictionary");
                }
                
                //Checking if we have the key GlassesOne in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("GlassesOne"))
                {
                    //Checking if the value of the key GlassesOne is 1
                    if (getSpriteAccessoriesDictionary["GlassesOne"] == 1)
                    {
                        //If the value of the key GlassesOne is 1, then we will unlock the item
                        UnlockGlassesOneOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key GlassesOne in the dictionary");
                }
                
                //Checking if we have the key GlassesTwo in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("GlassesTwo"))
                {
                    //Checking if the value of the key GlassesTwo is 1
                    if (getSpriteAccessoriesDictionary["GlassesTwo"] == 1)
                    {
                        //If the value of the key GlassesTwo is 1, then we will unlock the item
                        UnlockGlassesTwoOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key GlassesTwo in the dictionary");
                }
                
                //Checking if we have the key GlassesThree in the dictionary and unlocking
                //if required
                if (getSpriteAccessoriesDictionary.ContainsKey("GlassesThree"))
                {
                    //Checking if the value of the key GlassesThree is 1
                    if (getSpriteAccessoriesDictionary["GlassesThree"] == 1)
                    {
                        //If the value of the key GlassesThree is 1, then we will unlock the item
                        UnlockGlassesThreeOnEnable();
                    }
                }
                else
                {
                    Debug.Log("Could not find the key GlassesThree in the dictionary");
                }
            }
            else
            {
                Debug.Log("Could not find the key SpriteAccessories in the dictionary");
            }
        }

        #endregion

        #region Tokens Label Related

        [Header("Tokens Label Related")]
        //This is the reference to the tokens label text
        [SerializeField] private TextMeshProUGUI tokensLabelText;

        /// <summary>
        /// This is a reference to the tokens label image
        /// </summary>
        [SerializeField] private Image tokensLabelImage;

        /// <summary>
        /// When this is true, it tells the game that the flash routine is running
        /// </summary>
        private bool _isFlashRoutineRunning = false;
        
        /// <summary>
        /// This will run whenever we need to tell the user that they do not have enough tokens
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_FlashTokensLabelRed()
        {
            if (_isFlashRoutineRunning == true) yield break;

            //Tells that the routine is running
            _isFlashRoutineRunning = true;
            
            //Setting the color of the tokens label text to red
            tokensLabelImage.color = Color.red;
            
            //Waiting for 0.5 seconds
            yield return new WaitForSeconds(0.5f);
            
            //Setting the color of the tokens label text to white
            tokensLabelImage.color = Color.white;
            
            //Tells that the routine is not running
            _isFlashRoutineRunning = false;
        }
        
        /// <summary>
        /// This function will update the tokens label text
        /// </summary>
        private void UpdateTokensLabelText()
        {
            //Getting the value of the tokens from the local dictionary
            object getTokensFromLocalDictionary = HandleUserDataFromPlayFabServer.
                LocalUserDataDictionary["TokensInTotal"];
            
            //Setting the text of the tokens label text
            tokensLabelText.text = getTokensFromLocalDictionary.ToString();
        }

        #endregion
        
        #region Button Listners

        /// <summary>
        /// This function will setup the button listeners in the Start() function
        /// </summary>
        private void SetupAllButtonListeners()
        {
            backButton.onClick.AddListener(OnBackButtonClicked);
        }

        #endregion

        #region Unity Functions

        private void OnEnable()
        {
            //This will unlock the items based on the values that we have from the server. By default
            //the green color is always unlocked
            UnlockItemsBasedOnValuesFromLocalDictionary();

            //Setting up the CustomizationMenuCanvas Colors section
            ChangeTextOfColorsThatAreUnlocked();
            ChangeTextOfHatsThatAreUnlocked();
            ChangeTextOfGlassesThatAreUnlocked();
            
            //Changing the text to selected based on the playerPrefs key
            ChangeTextOfAColorThatIsSelected();
            ChangeTextOfAHatThatIsSelected();
            ChangeTextOfAGlassesThatIsSelected();
            
            //Updating the tokens label text
            UpdateTokensLabelText();
        }

        private void Start()
        {
            //Setting up the button listeners which can be set as they are small in number
            SetupAllButtonListeners();
        }

        #endregion
        
        #region References to Objects, Scripts and Components

        [Header("References to Objects, Scripts and Components")]
        //Main Menu Canvas Reference
        [SerializeField] private GameObject mainMenuCanvas;

        /// <summary>
        /// This class is used to store the references to the customization template items
        /// </summary>
        [Serializable]
        internal class CustomizationTemplateItems
        {
            public Button priceButton;
            public TextMeshProUGUI priceText;
            public CustomizationButtonTemplate customizationButtonTemplate;
        }

        #endregion
    }
}