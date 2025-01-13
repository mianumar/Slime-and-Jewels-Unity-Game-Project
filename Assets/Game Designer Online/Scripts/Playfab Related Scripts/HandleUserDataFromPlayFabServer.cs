using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game_Designer_Online.Scripts.Main_Menu;
using Game_Designer_Online.Scripts.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;
using Object = System.Object;

namespace Game_Designer_Online.Scripts.Playfab_Related_Scripts
{
    /// <summary>
    /// This script will contain the functions required to get the user's data from the PlayFab Server
    /// </summary>
    public class HandleUserDataFromPlayFabServer : MonoBehaviour
    {
        #region Cloud script to download user data and also validate it

        /// <summary>
        /// This will be used to store data locally, before it can be uploaded to the servers
        /// </summary>
        [Preserve]
        public static Dictionary<string, object> LocalUserDataDictionary = new Dictionary<string, object>
        {
            {"TokensInTotal", 0},
            {"2xTokenBooster", 0},
            {"3xTokenBooster", 0},
            {"ExtraHealth", 0},
            {"Shield", 0},
            {"EnemiesKilled", 0},
            {"CurrentLevelPlayerIsOn", 0},
            {"PrestigeLevel", 0},
            {"ChestsInTotal", 0},
            {
                "SpriteColors", new Dictionary<string, int>
                {
                    {"ColorOne", 1},
                    {"ColorTwo", 0},
                    {"ColorThree", 0},
                    {"ColorFour", 0},
                    {"ColorFive", 0},
                    {"ColorSix", 0},
                    {"ColorSeven", 0},
                    {"ColorEight", 0},
                    {"ColorNine", 0},
                    {"ColorTen", 0},
                    {"ColorEleven", 0},
                    {"ColorTwelve", 0}
                }
            },
            {
                "SpriteAccessories", new Dictionary<string, int>
                {
                    {"AccessoryOne", 0},
                    {"AccessoryTwo", 0},
                    {"AccessoryThree", 0},
                    {"AccessoryFour", 0},
                    {"AccessoryFive", 0},
                    {"AccessorySix", 0},
                    {"AccessorySeven", 0},
                    {"AccessoryEight", 0},
                    {"AccessoryNine", 0},
                    {"AccessoryTen", 0},
                    {"AccessoryEleven", 0},
                    {"AccessoryTwelve", 0},
                    {"GlassesOne", 0},
                    {"GlassesTwo", 0},
                    {"GlassesThree", 0}
                }
            },
            {"CurrentHealthOnLevel", 100},
            {"CurrentChestsOnCurrentLevel", 0},
            {"RemoveAds", 0},
            {"LastSelectedColor", "Green"},
            {"LastSelectedHat", ""},
            {"LastSelectedGlasses", ""},
            {"LastSelectedMobileControls", 1},
        };

        /// <summary>
        /// This holds the user data from the server
        /// </summary>
        private object UserDataFromTheServer { get; set; }

        /// <summary>
        /// This will create the local dictionary object, if it is null
        /// </summary>
        private void CreateLocalUserDataDictionaryObject()
        {
            //Setting value of the local dictionary for use
            LocalUserDataDictionary = new Dictionary<string, dynamic>
            {
                {"TokensInTotal", 0},
                {"2xTokenBooster", 0},
                {"3xTokenBooster", 0},
                {"ExtraHealth", 3},
                {"Shield", 0},
                {"EnemiesKilled", 0},
                {"CurrentLevelPlayerIsOn", 0},
                {"PrestigeLevel", 0},
                {"ChestsInTotal", 0},
                {
                    "SpriteColors", new Dictionary<string, int>
                    {
                        {"ColorOne", 1},
                        {"ColorTwo", 0},
                        {"ColorThree", 0},
                        {"ColorFour", 0},
                        {"ColorFive", 0},
                        {"ColorSix", 0},
                        {"ColorSeven", 0},
                        {"ColorEight", 0},
                        {"ColorNine", 0},
                        {"ColorTen", 0},
                        {"ColorEleven", 0},
                        {"ColorTwelve", 0}
                    }
                },
                {
                    "SpriteAccessories", new Dictionary<string, int>
                    {
                        {"AccessoryOne", 0},
                        {"AccessoryTwo", 0},
                        {"AccessoryThree", 0},
                        {"AccessoryFour", 0},
                        {"AccessoryFive", 0},
                        {"AccessorySix", 0},
                        {"AccessorySeven", 0},
                        {"AccessoryEight", 0},
                        {"AccessoryNine", 0},
                        {"AccessoryTen", 0},
                        {"AccessoryEleven", 0},
                        {"AccessoryTwelve", 0},
                        {"GlassesOne", 0},
                        {"GlassesTwo", 0},
                        {"GlassesThree", 0}
                    }
                },
                {"CurrentHealthOnLevel", 100},
                {"CurrentChestsOnCurrentLevel", 0},
                {"RemoveAds", 0},
                {"LastSelectedColor", "Green"},
                {"LastSelectedHat", ""},
                {"LastSelectedGlasses", ""},
                {"LastSelectedMobileControls", 1},
            };
        }

        /// <summary>
        /// This function will get the user data
        /// </summary>
        private void ExecuteCloudScriptToGetUserData()
        {
            print("Trying to get the user data from the server");

            //This request is for executing the cloud script that will get the user data
            var getUserDataFromServer = new ExecuteCloudScriptRequest
            {
                FunctionName = "returnPlayerUserData",
                FunctionParameter = new { },
                GeneratePlayStreamEvent = true
            };

            //Sending the request to execute the cloud script
            PlayFabClientAPI.ExecuteCloudScript(
                getUserDataFromServer,
                result =>
                {
                    // This will run for the windows editor and windows player
                    if (Application.platform == RuntimePlatform.WindowsEditor
                        || Application.platform == RuntimePlatform.WindowsPlayer
                        || Application.platform == RuntimePlatform.LinuxEditor
                        || Application.platform == RuntimePlatform.LinuxPlayer
                        || Application.platform == RuntimePlatform.OSXEditor)
                    {
                        try
                        {
                            
                            //Using Newtonsoft.Json to convert the result into a dictionary
                            var resultDictionary =
                                JsonConvert.DeserializeObject<Dictionary<string, object>>
                                    (result.FunctionResult.ToString());

                            //Storing the "message" key's value in a dictionary
                            var userDataFromMessageResult = resultDictionary["message"];

                            //Setting the value of the userData variable
                            UserDataFromTheServer = userDataFromMessageResult;

                            //Converting the UserDataFromServer variable into a JObject
                            var userDataFromServer =
                                JsonConvert.DeserializeObject<Dictionary<string, object>>(UserDataFromTheServer
                                    .ToString());

                            // We will run this only if the data is not null
                            if (userDataFromServer != null)
                            {
                                // This function will be used to validate the user data keys that have been received
                                // from the server. If the keys are not valid, we will upload a new
                                // dictionary to the server, and then download the data again
                                bool aKeyWasInvalid = ValidateDownloadedDictionaryKeys(userDataFromServer);

                                // If this is false, then we will return
                                if (aKeyWasInvalid == true)
                                {
                                    UploadLocalDictionaryToServerAfterAnInvalidKeyWasFound();
                                
                                    return;
                                }
                            }
                            
                            //Storing the data from the server into the local dictionary
                            LocalUserDataDictionary = userDataFromServer;

                            print("User Data was successfully downloaded from the server");
                            
                            //Setting the local data in the player prefs for quick access
                            SetLocalDataInPlayerPrefsForQuickAccess();
                        }
                        catch (Exception e)
                        {
                            print(e.Message);
                            ExecuteCloudScriptToGetUserData();
                        }
                    }

                    // This will run for the android platform
                    if (Application.platform == RuntimePlatform.Android)
                    {
                        try
                        {
                            //Using Newtonsoft.Json to convert the result into a dictionary
                            var resultDictionary =
                                JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());

                            //Storing the "message" key's value in a dictionary
                            var userDataFromMessageResult = resultDictionary["message"];

                            //Setting the value of the userData variable
                            UserDataFromTheServer = userDataFromMessageResult;

                            //Converting the UserDataFromServer variable into a JObject
                            var userDataFromServer =
                                JsonConvert.DeserializeObject<Dictionary<string, object>>(UserDataFromTheServer
                                    .ToString());
                            
                            // We will run this only if the data is not null
                            if (userDataFromServer != null)
                            {
                                // This function will be used to validate the user data keys that have been received
                                // from the server. If the keys are not valid, we will upload a new
                                // dictionary to the server, and then download the data again
                                bool aKeyWasInvalid = ValidateDownloadedDictionaryKeys(userDataFromServer);

                                // If this is false, then we will return
                                if (aKeyWasInvalid == true)
                                {
                                    UploadLocalDictionaryToServerAfterAnInvalidKeyWasFound();
                                
                                    return;
                                }
                            }
                            
                            //Storing the data from the server into the local dictionary
                            LocalUserDataDictionary = userDataFromServer;
                            
                            print("User Data was successfully downloaded from the server");

                            //Setting the local data in the player prefs for quick access
                            SetLocalDataInPlayerPrefsForQuickAccess();
                        }
                        catch (Exception e)
                        {
                            print(e.Message);
                            ExecuteCloudScriptToGetUserData();
                        }
                    }
                    
                    // This will run for the iOS platform
                    if (Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        try
                        {
                            //Using Newtonsoft.Json to convert the result into a dictionary
                            var resultDictionary =
                                JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());

                            //Storing the "message" key's value in a dictionary
                            var userDataFromMessageResult = resultDictionary["message"];

                            //Setting the value of the userData variable
                            UserDataFromTheServer = userDataFromMessageResult;

                            //Converting the UserDataFromServer variable into a JObject
                            var userDataFromServer =
                                JsonConvert.DeserializeObject<Dictionary<string, object>>(UserDataFromTheServer
                                    .ToString());
                            
                            // We will run this only if the data is not null
                            if (userDataFromServer != null)
                            {
                                // This function will be used to validate the user data keys that have been received
                                // from the server. If the keys are not valid, we will upload a new
                                // dictionary to the server, and then download the data again
                                bool aKeyWasInvalid = ValidateDownloadedDictionaryKeys(userDataFromServer);

                                // If this is false, then we will return
                                if (aKeyWasInvalid == true)
                                {
                                    UploadLocalDictionaryToServerAfterAnInvalidKeyWasFound();
                                
                                    return;
                                }
                            }
                            
                            //Storing the data from the server into the local dictionary
                            LocalUserDataDictionary = userDataFromServer;
                            
                            print("User Data was successfully downloaded from the server");

                            //Setting the local data in the player prefs for quick access
                            SetLocalDataInPlayerPrefsForQuickAccess();
                        }
                        catch (Exception e)
                        {
                            print(e.Message);
                            ExecuteCloudScriptToGetUserData();
                        }
                    }
                },
                error => { print("Failed to setup the player data"); }
            );
        }

        /// <summary>
        /// This function will be used to validate all of the values that are present in the dictionary that
        /// was downloaded from the server. When this game is updated, we may need to put in other keys that
        /// will help us to validate the data that has been downloaded from the server. If the data is not valid, 
        /// </summary>
        /// <returns></returns>
        private bool ValidateDownloadedDictionaryKeys(Dictionary<string, object> userDataFromServer)
        {
            // When this is true, then one of the keys was invalid
            bool aKeyWasInvalid = false;
            
            // Check if the dictionary has the key 'TokensInTotal'
            if (userDataFromServer.ContainsKey("TokensInTotal") == false)
            {
                // Adding a key and a value of zero to the dictionary
                if (LocalUserDataDictionary.ContainsKey("TokensInTotal") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("TokensInTotal", 0);
                }
                else
                {
                    LocalUserDataDictionary["TokensInTotal"] = 0;
                }
                
                print("TokensInTotal key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["TokensInTotal"] = userDataFromServer["TokensInTotal"];
            }

            // Check if the dictionary has the key '2xTokenBooster'
            if (userDataFromServer.ContainsKey("2xTokenBooster") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("2xTokenBooster") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("2xTokenBooster", 0);
                }
                else
                {
                    LocalUserDataDictionary["2xTokenBooster"] = 0;
                }

                print("2xTokenBooster key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["2xTokenBooster"] = userDataFromServer["2xTokenBooster"];
            }
            
            // Check if the dictionary has the key '3xTokenBooster'
            if (userDataFromServer.ContainsKey("3xTokenBooster") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("3xTokenBooster") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("3xTokenBooster", 0);
                }
                else
                {
                    LocalUserDataDictionary["3xTokenBooster"] = 0;
                }
                
                print("3xTokenBooster key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["3xTokenBooster"] = userDataFromServer["3xTokenBooster"];
            }
            
            // Check if the dictionary has the key 'ExtraHealth'
            if (userDataFromServer.ContainsKey("ExtraHealth") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("ExtraHealth") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("ExtraHealth", 0);
                }
                else
                {
                    LocalUserDataDictionary["ExtraHealth"] = 0;
                }

                print("ExtraHealth key was invalid");
                

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["ExtraHealth"] = userDataFromServer["ExtraHealth"];
            }
            
            // Check if the dictionary has the key 'Shield'
            if (userDataFromServer.ContainsKey("Shield") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("Shield") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("Shield", 0);
                }
                else
                {
                    LocalUserDataDictionary["Shield"] = 0;
                }

                print("Shield key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["Shield"] = userDataFromServer["Shield"];
            }
            
            // Check if the dictionary has the key 'EnemiesKilled'
            if (userDataFromServer.ContainsKey("EnemiesKilled") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("EnemiesKilled") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("EnemiesKilled", 0);
                }
                else
                {
                    LocalUserDataDictionary["EnemiesKilled"] = 0;
                }
                
                print("EnemiesKilled key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["EnemiesKilled"] = userDataFromServer["EnemiesKilled"];
            }
            
            // Check if the dictionary has the key 'CurrentLevelPlayerIsOn'
            if (userDataFromServer.ContainsKey("CurrentLevelPlayerIsOn") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("CurrentLevelPlayerIsOn") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("CurrentLevelPlayerIsOn", 0);
                }
                else
                {
                    LocalUserDataDictionary["CurrentLevelPlayerIsOn"] = 0;
                }

                print("CurrentLevelPlayerIsOn key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["CurrentLevelPlayerIsOn"] = userDataFromServer["CurrentLevelPlayerIsOn"];
            }
            
            // Check if the dictionary has the key 'PrestigeLevel'
            if (userDataFromServer.ContainsKey("PrestigeLevel") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("PrestigeLevel") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("PrestigeLevel", 0);
                }
                else
                {
                    LocalUserDataDictionary["PrestigeLevel"] = 0;
                }

                print("PrestigeLevel key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["PrestigeLevel"] = userDataFromServer["PrestigeLevel"];
            }
            
            // Check if the dictionary has the key 'ChestsInTotal'
            if (userDataFromServer.ContainsKey("ChestsInTotal") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("ChestsInTotal") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("ChestsInTotal", 0);
                }
                else
                {
                    LocalUserDataDictionary["ChestsInTotal"] = 0;
                }
                
                print("ChestsInTotal key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["ChestsInTotal"] = userDataFromServer["ChestsInTotal"];
            }
            
            // Check if the dictionary has the key 'SpriteColors'
            if (userDataFromServer.ContainsKey("SpriteColors") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("SpriteColors") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("SpriteColors", new Dictionary<string, object>
                    {
                        {"ColorOne", 1},
                        {"ColorTwo", 0},
                        {"ColorThree", 0},
                        {"ColorFour", 0},
                        {"ColorFive", 0},
                        {"ColorSix", 0},
                        {"ColorSeven", 0},
                        {"ColorEight", 0},
                        {"ColorNine", 0},
                        {"ColorTen", 0},
                        {"ColorEleven", 0},
                        {"ColorTwelve", 0}
                    });
                }
                else
                {
                    LocalUserDataDictionary["SpriteColors"] = new Dictionary<string, object>
                    {
                        {"ColorOne", 1},
                        {"ColorTwo", 0},
                        {"ColorThree", 0},
                        {"ColorFour", 0},
                        {"ColorFive", 0},
                        {"ColorSix", 0},
                        {"ColorSeven", 0},
                        {"ColorEight", 0},
                        {"ColorNine", 0},
                        {"ColorTen", 0},
                        {"ColorEleven", 0},
                        {"ColorTwelve", 0}
                    };
                }

                print("SpriteColors key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["SpriteColors"] = userDataFromServer["SpriteColors"];
            }

            // Checking if the dictionary contains the key 'SpriteColors'
            if (userDataFromServer.ContainsKey("SpriteColors") == true)
            {
                // Converting the value to a dictionary the value to a string
                var spriteColorsAsString = userDataFromServer["SpriteColors"].ToString();
                
                // Get the value of the sprite colors from the local user data dictionary
                var spriteColorsLocalValue = LocalUserDataDictionary["SpriteColors"];
                
                // Converting the local user data dictionary value to a string
                var spriteColorsAsStringLocal = spriteColorsLocalValue.ToString();

                // Converting the value to a dictionary
                var spriteColors = JsonConvert.DeserializeObject<Dictionary<string, object>>(spriteColorsAsString);
                
                // Converting the local user data dictionary value to a dictionary
                var spriteColorsLocal = 
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(spriteColorsAsStringLocal);
                
                // Checking if the dictionary has the key 'ColorOne'
                if (spriteColors.ContainsKey("ColorOne") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorOne", 1);

                    print("ColorOne key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorOne"] = spriteColors["ColorOne"];
                }

                // Checking if the dictionary has the key 'ColorTwo'
                if (spriteColors.ContainsKey("ColorTwo") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorTwo", 0);

                    print("ColorTwo key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorTwo"] = spriteColors["ColorTwo"];
                }

                // Checking if the dictionary has the key 'ColorThree'
                if (spriteColors.ContainsKey("ColorThree") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorThree", 0);

                    print("ColorThree key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorThree"] = spriteColors["ColorThree"];
                }
                
                // Checking if the dictionary has the key 'ColorFour'
                if (spriteColors.ContainsKey("ColorFour") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorFour", 0);

                    print("ColorFour key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorFour"] = spriteColors["ColorFour"];
                }
                
                // Checking if the dictionary has the key 'ColorFive'
                if (spriteColors.ContainsKey("ColorFive") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorFive", 0);

                    print("ColorFive key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorFive"] = spriteColors["ColorFive"];
                }
                
                // Checking if the dictionary has the key 'ColorSix'
                if (spriteColors.ContainsKey("ColorSix") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorSix", 0);

                    print("ColorSix key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorSix"] = spriteColors["ColorSix"];
                }
                
                // Checking if the dictionary has the key 'ColorSeven'
                if (spriteColors.ContainsKey("ColorSeven") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorSeven", 0);

                    print("ColorSeven key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorSeven"] = spriteColors["ColorSeven"];
                }
                
                // Checking if the dictionary has the key 'ColorEight'
                if (spriteColors.ContainsKey("ColorEight") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorEight", 0);

                    print("ColorEight key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorEight"] = spriteColors["ColorEight"];
                }
                
                // Checking if the dictionary has the key 'ColorNine'
                if (spriteColors.ContainsKey("ColorNine") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorNine", 0);

                    print("ColorNine key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorNine"] = spriteColors["ColorNine"];
                }
                
                // Checking if the dictionary has the key 'ColorTen'
                if (spriteColors.ContainsKey("ColorTen") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorTen", 0);

                    print("ColorTen key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorTen"] = spriteColors["ColorTen"];
                }
                
                // Checking if the dictionary has the key 'ColorEleven'
                if (spriteColors.ContainsKey("ColorEleven") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorEleven", 0);

                    print("ColorEleven key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorEleven"] = spriteColors["ColorEleven"];
                }
                
                // Checking if the dictionary has the key 'ColorTwelve'
                if (spriteColors.ContainsKey("ColorTwelve") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteColors.TryAdd("ColorTwelve", 0);

                    print("ColorTwelve key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteColorsLocal["ColorTwelve"] = spriteColors["ColorTwelve"];
                }

                // Adding the sprite colors to the local user data dictionary
                LocalUserDataDictionary["SpriteColors"] = spriteColorsLocal;
            }
            
            // Checking if the dictionary has the key 'SpriteAccessories'
            if (userDataFromServer.ContainsKey("SpriteAccessories") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("SpriteAccessories") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("SpriteAccessories", new Dictionary<string, object>
                    {
                        {"AccessoryOne", 0},
                        {"AccessoryTwo", 0},
                        {"AccessoryThree", 0},
                        {"AccessoryFour", 0},
                        {"AccessoryFive", 0},
                        {"AccessorySix", 0},
                        {"AccessorySeven", 0},
                        {"AccessoryEight", 0},
                        {"AccessoryNine", 0},
                        {"AccessoryTen", 0},
                        {"AccessoryEleven", 0},
                        {"AccessoryTwelve", 0},
                        {"GlassesOne", 0},
                        {"GlassesTwo", 0},
                        {"GlassesThree", 0}
                    });
                }
                else
                {
                    LocalUserDataDictionary["SpriteAccessories"] = new Dictionary<string, object>
                    {
                        {"AccessoryOne", 0},
                        {"AccessoryTwo", 0},
                        {"AccessoryThree", 0},
                        {"AccessoryFour", 0},
                        {"AccessoryFive", 0},
                        {"AccessorySix", 0},
                        {"AccessorySeven", 0},
                        {"AccessoryEight", 0},
                        {"AccessoryNine", 0},
                        {"AccessoryTen", 0},
                        {"AccessoryEleven", 0},
                        {"AccessoryTwelve", 0},
                        {"GlassesOne", 0},
                        {"GlassesTwo", 0},
                        {"GlassesThree", 0}
                    };
                }
                
                print("SpriteAccessories key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["SpriteAccessories"] = userDataFromServer["SpriteAccessories"];
            }
            
            // Checking if the dictionary has the key 'SpriteAccessories'
            if (userDataFromServer.ContainsKey("SpriteAccessories") == true)
            {
                // Converting the value to a string
                var spriteAccessoriesAsString = userDataFromServer["SpriteAccessories"].ToString();
                
                // Get the value of the sprite accessories from the local user data dictionary
                var spriteAccessoriesLocalValue = LocalUserDataDictionary["SpriteAccessories"];

                // Converting the local user data dictionary value to a string
                var spriteAccessoriesAsStringLocal = 
                    spriteAccessoriesLocalValue.ToString();

                // Converting the value to a dictionary
                var spriteAccessories = JsonConvert.
                    DeserializeObject<Dictionary<string, object>>(spriteAccessoriesAsString);
                
                // Converting the local user data dictionary value to a dictionary
                var spriteAccessoriesLocal = 
                    JsonConvert.DeserializeObject<Dictionary<string, object>>(spriteAccessoriesAsStringLocal);

                // Checking if the dictionary has the key 'AccessoryOne'
                if (spriteAccessories.ContainsKey("AccessoryOne") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryOne", 0);

                    print("AccessoryOne key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryOne"] = spriteAccessories["AccessoryOne"];
                }

                // Checking if the dictionary has the key 'AccessoryTwo'
                if (spriteAccessories.ContainsKey("AccessoryTwo") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryTwo", 0);

                    print("AccessoryTwo key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryTwo"] = spriteAccessories["AccessoryTwo"];
                }
                
                // Checking if the dictionary has the key 'AccessoryThree'
                if (spriteAccessories.ContainsKey("AccessoryThree") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryThree", 0);

                    print("AccessoryThree key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryThree"] = spriteAccessories["AccessoryThree"];
                }

                // Checking if the dictionary has the key 'AccessoryFour'
                if (spriteAccessories.ContainsKey("AccessoryFour") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryFour", 0);

                    print("AccessoryFour key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryFour"] = spriteAccessories["AccessoryFour"];
                }
                
                // Checking if the dictionary has the key 'AccessoryFive'
                if (spriteAccessories.ContainsKey("AccessoryFive") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryFive", 0);

                    print("AccessoryFive key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryFive"] = spriteAccessories["AccessoryFive"];
                }
                
                // Checking if the dictionary has the key 'AccessorySix'
                if (spriteAccessories.ContainsKey("AccessorySix") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessorySix", 0);

                    print("AccessorySix key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessorySix"] = spriteAccessories["AccessorySix"];
                }
                
                // Checking if the dictionary has the key 'AccessorySeven'
                if (spriteAccessories.ContainsKey("AccessorySeven") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessorySeven", 0);

                    print("AccessorySeven key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessorySeven"] = spriteAccessories["AccessorySeven"];
                }
                
                // Checking if the dictionary has the key 'AccessoryEight'
                if (spriteAccessories.ContainsKey("AccessoryEight") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryEight", 0);

                    print("AccessoryEight key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryEight"] = spriteAccessories["AccessoryEight"];
                }
                
                // Checking if the dictionary has the key 'AccessoryNine'
                if (spriteAccessories.ContainsKey("AccessoryNine") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryNine", 0);

                    print("AccessoryNine key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryNine"] = spriteAccessories["AccessoryNine"];
                }
                
                // Checking if the dictionary has the key 'AccessoryTen'
                if (spriteAccessories.ContainsKey("AccessoryTen") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryTen", 0);

                    print("AccessoryTen key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryTen"] = spriteAccessories["AccessoryTen"];
                }
                
                // Checking if the dictionary has the key 'AccessoryEleven'
                if (spriteAccessories.ContainsKey("AccessoryEleven") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryEleven", 0);

                    print("AccessoryEleven key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryEleven"] = spriteAccessories["AccessoryEleven"];
                }
                
                // Checking if the dictionary has the key 'AccessoryTwelve'
                if (spriteAccessories.ContainsKey("AccessoryTwelve") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("AccessoryTwelve", 0);

                    print("AccessoryTwelve key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["AccessoryTwelve"] = spriteAccessories["AccessoryTwelve"];
                }
                
                // Checking if the dictionary has the key 'GlassesOne'
                if (spriteAccessories.ContainsKey("GlassesOne") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("GlassesOne", 0);

                    print("GlassesOne key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["GlassesOne"] = spriteAccessories["GlassesOne"];
                }
                
                // Checking if the dictionary has the key 'GlassesTwo'
                if (spriteAccessories.ContainsKey("GlassesTwo") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("GlassesTwo", 0);

                    print("GlassesTwo key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["GlassesTwo"] = spriteAccessories["GlassesTwo"];
                }
                
                // Checking if the dictionary has the key 'GlassesThree'
                if (spriteAccessories.ContainsKey("GlassesThree") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    spriteAccessories.TryAdd("GlassesThree", 0);

                    print("GlassesThree key was invalid");

                    aKeyWasInvalid = true;
                }
                // If we have the key, then we will set the value of the key to the value that is in the
                // dictionary that was downloaded from the server into the local dictionary
                else
                {
                    spriteAccessoriesLocal["GlassesThree"] = spriteAccessories["GlassesThree"];
                }
                
                // Adding the sprite accessories to the local user data dictionary
                LocalUserDataDictionary["SpriteAccessories"] = spriteAccessoriesLocal;
            }
            
            // Checking if the dictionary has the key 'CurrentHealthOnLevel'
            if (userDataFromServer.ContainsKey("CurrentHealthOnLevel") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("CurrentHealthOnLevel") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("CurrentHealthOnLevel", 100);
                }
                else
                {
                    LocalUserDataDictionary["CurrentHealthOnLevel"] = 100;
                }
                
                print("CurrentHealthOnLevel key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["CurrentHealthOnLevel"] = userDataFromServer["CurrentHealthOnLevel"];
            }
            
            // Checking if the dictionary has the key 'CurrentChestsOnCurrentLevel'
            if (userDataFromServer.ContainsKey("CurrentChestsOnCurrentLevel") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("CurrentChestsOnCurrentLevel") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("CurrentChestsOnCurrentLevel", 0);
                }
                else
                {
                    LocalUserDataDictionary["CurrentChestsOnCurrentLevel"] = 0;
                }
                
                print("CurrentChestsOnCurrentLevel key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["CurrentChestsOnCurrentLevel"] = userDataFromServer["CurrentChestsOnCurrentLevel"];
            }
            
            // Checking if the dictionary has the key 'RemoveAds'
            if (userDataFromServer.ContainsKey("RemoveAds") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("RemoveAds") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("RemoveAds", 0);
                }
                else
                {
                    LocalUserDataDictionary["RemoveAds"] = 0;
                }

                print("RemoveAds key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["RemoveAds"] = userDataFromServer["RemoveAds"];
            }
            
            // Checking if the dictionary has the key 'LastSelectedColor'
            if (userDataFromServer.ContainsKey("LastSelectedColor") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("LastSelectedColor") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("LastSelectedColor", "Green");
                }
                else
                {
                    LocalUserDataDictionary["LastSelectedColor"] = "Green";
                }

                print("LastSelectedColor key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["LastSelectedColor"] = userDataFromServer["LastSelectedColor"];
            }
            
            // Checking if the dictionary has the key 'LastSelectedHat'
            if (userDataFromServer.ContainsKey("LastSelectedHat") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("LastSelectedHat") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("LastSelectedHat", "");
                }
                else
                {
                    LocalUserDataDictionary["LastSelectedHat"] = "";
                }
                
                print("LastSelectedHat key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["LastSelectedHat"] = userDataFromServer["LastSelectedHat"];
            }
            
            // Checking if the dictionary has the key 'LastSelectedGlasses'
            if (userDataFromServer.ContainsKey("LastSelectedGlasses") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("LastSelectedGlasses") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("LastSelectedGlasses", "");
                }
                else
                {
                    LocalUserDataDictionary["LastSelectedGlasses"] = "";
                }
                
                print("LastSelectedGlasses key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["LastSelectedGlasses"] = userDataFromServer["LastSelectedGlasses"];
            }
            
            // Check if the dictionary has the key 'LastSelectedMobileControls'
            if (userDataFromServer.ContainsKey("LastSelectedMobileControls") == false)
            {
                if (LocalUserDataDictionary.ContainsKey("LastSelectedMobileControls") == false)
                {
                    // Adding a key and a value of zero to the dictionary
                    LocalUserDataDictionary.TryAdd("LastSelectedMobileControls", 1);
                }
                else
                {
                    LocalUserDataDictionary["LastSelectedMobileControls"] = 1;
                }
                
                print("LastSelectedMobileControls key was invalid");

                aKeyWasInvalid = true;
            }
            // If we have the key, then we will set the value of the key to the value that is in the
            // dictionary that was downloaded from the server
            else
            {
                LocalUserDataDictionary["LastSelectedMobileControls"] = userDataFromServer["LastSelectedMobileControls"];
            }

            // If a key was invalid, then we will upload the new dictionary to the server
            // and then download the data again
            if (aKeyWasInvalid == true)
            {
                return true;
            }

            // If a key is not invalid, we will return false
            return false;
        }

        /// <summary>
        /// This will usually be called after the user data has been downloaded from the server. This will
        /// set some important data in the local storage so that the game can easily access it. It is also
        /// going to check if we are in the main menu, and if we are, we have to show the 'Loading Data from
        /// server' message to stop the player from clicking any menu item as that would create bugs.
        /// </summary>
        private void SetLocalDataInPlayerPrefsForQuickAccess()
        {
            // Todo: We may need to change the way the functions are being handled here as we are loading data from the server now

            // This will run to set the values of the PlayerPrefs after they are loaded
            SetValuesToPlayerPrefsAfterLoadingDataFromServer();

            //This function will check if the loading data from server message has to be displayed
            CheckIfLoadingDataFromServerMessageHasToBeDisplayed();
        }

        /// <summary>
        /// This will run to set the values of the player prefs after the data has been downloaded from the server
        /// </summary>
        private void SetValuesToPlayerPrefsAfterLoadingDataFromServer()
        {
            // If this is running on the windows editor or windows player, then we will set the values
            if (Application.platform == RuntimePlatform.WindowsEditor 
                || Application.platform == RuntimePlatform.WindowsPlayer || 
                Application.platform == RuntimePlatform.LinuxEditor
                || Application.platform == RuntimePlatform.OSXEditor)
            {
                // Object value for storing the value of the key
                object value;

                //Checking if the local dictionary has the key "Shield"
                if (LocalUserDataDictionary.TryGetValue("Shield", out value))
                {
                    //If the local dictionary has the key "Shield", then we will get the value of the key
                    int shieldValue = int.Parse(value.ToString());

                    //Setting the value of the shield key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ShieldKey, shieldValue);
                }

                //Checking if the local dictionary has the key "ExtraHealth"
                if (LocalUserDataDictionary.TryGetValue("ExtraHealth", out value))
                {
                    //If the local dictionary has the key "ExtraHealth", then we will get the value of the key
                    var healthValue = int.Parse(value.ToString());

                    //Setting the value of the health key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.HealthKey, healthValue);
                }

                //Checking if the local dictionary has the key "TokensInTotal"
                if (LocalUserDataDictionary.TryGetValue("TokensInTotal", out value))
                {
                    //If the local dictionary has the key "TokensInTotal", then we will get the value of the key
                    var playerTokensValue = int.Parse(value.ToString());

                    //Setting the value of the player tokens key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, playerTokensValue);
                }

                //Checking if the local dictionary has the key "ChestsInTotal"
                if (LocalUserDataDictionary.TryGetValue("ChestsInTotal", out value))
                {
                    //If the local dictionary has the key "ChestsInTotal", then we will get the value of the key
                    var chestsValue = int.Parse(value.ToString());

                    //Setting the value of the chests key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ChestKey, chestsValue);
                }

                //Checking if the local dictionary has the key "2xTokenBooster"
                if (LocalUserDataDictionary.TryGetValue("2xTokenBooster", out value))
                {
                    //If the local dictionary has the key "2xTokenBooster", then we will get the value of the key
                    var twoXTokenBoosterValue = int.Parse(value.ToString());

                    //Setting the value of the 2x token booster key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.TwoXBoosterKey, twoXTokenBoosterValue);
                }

                //Checking the local user dictionary has the key "3xTokenBooster"
                if (LocalUserDataDictionary.TryGetValue("3xTokenBooster", out value))
                {
                    //If the local dictionary has the key "3xTokenBooster", then we will get the value of the key
                    var threeXTokenBoosterValue = int.Parse(value.ToString());

                    //Setting the value of the 3x token booster key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ThreeXBoosterKey, threeXTokenBoosterValue);
                }

                //Checking if the local user dictionary has the key "Prestige"
                if (LocalUserDataDictionary.TryGetValue("PrestigeLevel", out value))
                {
                    // Converting the value to a string
                    var prestigeValue = int.Parse(value.ToString());

                    //Setting the value of the prestige key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.PrestigeKey, prestigeValue);
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedColor'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedColor", out value))
                {
                    // Converting the value to a string
                    var lastSelectedColor = value.ToString();

                    // Setting the value of the last selected color key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, 
                        lastSelectedColor);

                    // Using a for loop to go through all the colors in the color selected list
                    foreach (var colorBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.ColorSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.ColorSelected[colorBeingProcessed.Key] = 0;
                    }
                    
                    // Setting only the selected color to 1
                    HandleConstValuesStaticObjectsAndKeys.ColorSelected[lastSelectedColor] = 1;
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedHat'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedHat", out value))
                {
                    // Converting the value to a string
                    var lastSelectedHat = value.ToString();

                    // Setting the value of the last selected hat key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, 
                        lastSelectedHat);
                    
                    // For loop to go through all the hats in the hat selected list
                    foreach (var hatBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.HatsSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.HatsSelected[hatBeingProcessed.Key] = 0;
                    }

                    // We will only set the selected hat to 1 if it is not empty
                    if (string.IsNullOrEmpty(lastSelectedHat) == false)
                    {
                        // Setting only the selected hat to 1
                        HandleConstValuesStaticObjectsAndKeys.HatsSelected[lastSelectedHat] = 1;
                    }
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedGlasses'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedGlasses", out value))
                {
                    // Converting the value to a string
                    var lastSelectedGlasses = value.ToString();

                    // Setting the value of the last selected glasses key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, 
                        lastSelectedGlasses);
                    
                    // For loop to go through all the glasses in the glasses selected list
                    foreach (var glassesBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.GlassesSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.GlassesSelected[glassesBeingProcessed.Key] = 0;
                    }

                    // We will only set the selected glasses to 1 if it is not empty
                    if (string.IsNullOrEmpty(lastSelectedGlasses) == false)
                    {
                        // Setting only the selected glasses to 1
                        HandleConstValuesStaticObjectsAndKeys.GlassesSelected[lastSelectedGlasses] = 1;
                    }
                }
                
                // Check if the local user dictionary has the key 'LastSelectedMobileControls'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedMobileControls", out value))
                {
                    // Converting the value to a string
                    var lastSelectedMobileControls = int.Parse(value.ToString());

                    // Setting the value of the last selected mobile controls key in the player prefs
                    PlayerPrefs.SetInt(HandleConstValuesStaticObjectsAndKeys.MobileButtonsContainerKey, 
                        lastSelectedMobileControls);
                }
                
                //This function will check if the prestige has to be displayed
                CheckIfPrestigeHasToBeDisplayed();
                
                print("All key values for quick access have been set");
            }
            
            // Running this if we are on the android platform
            if (Application.platform == RuntimePlatform.Android)
            {
                // Dynamic value for storing the value of the key
                object value;

                //Checking if the local dictionary has the key "Shield"
                if (LocalUserDataDictionary.TryGetValue("Shield", out value))
                {
                    //If the local dictionary has the key "Shield", then we will get the value of the key
                    var shieldValue = int.Parse(value.ToString());

                    //Setting the value of the shield key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ShieldKey, shieldValue);
                }

                //Checking if the local dictionary has the key "ExtraHealth"
                if (LocalUserDataDictionary.TryGetValue("ExtraHealth", out value))
                {
                    //If the local dictionary has the key "ExtraHealth", then we will get the value of the key
                    var healthValue = int.Parse(value.ToString());

                    //Setting the value of the health key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.HealthKey, healthValue);
                }

                //Checking if the local dictionary has the key "TokensInTotal"
                if (LocalUserDataDictionary.TryGetValue("TokensInTotal", out value))
                {
                    //If the local dictionary has the key "TokensInTotal", then we will get the value of the key
                    var playerTokensValue = int.Parse(value.ToString());

                    //Setting the value of the player tokens key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, playerTokensValue);
                }

                //Checking if the local dictionary has the key "ChestsInTotal"
                if (LocalUserDataDictionary.TryGetValue("ChestsInTotal", out value))
                {
                    //If the local dictionary has the key "ChestsInTotal", then we will get the value of the key
                    var chestsValue = int.Parse(value.ToString());

                    //Setting the value of the chests key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ChestKey, chestsValue);
                }

                //Checking if the local dictionary has the key "2xTokenBooster"
                if (LocalUserDataDictionary.TryGetValue("2xTokenBooster", out value))
                {
                    //If the local dictionary has the key "2xTokenBooster", then we will get the value of the key
                    var twoXTokenBoosterValue = int.Parse(value.ToString());

                    //Setting the value of the 2x token booster key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.TwoXBoosterKey, twoXTokenBoosterValue);
                }

                //Checking the local user dictionary has the key "3xTokenBooster"
                if (LocalUserDataDictionary.TryGetValue("3xTokenBooster", out value))
                {
                    //If the local dictionary has the key "3xTokenBooster", then we will get the value of the key
                    var threeXTokenBoosterValue = int.Parse(value.ToString());

                    //Setting the value of the 3x token booster key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ThreeXBoosterKey, threeXTokenBoosterValue);
                }

                //Checking if the local user dictionary has the key "Prestige"
                if (LocalUserDataDictionary.TryGetValue("PrestigeLevel", out value))
                {
                    // Converting the value to a string
                    var prestigeValue = int.Parse(value.ToString());

                    //Setting the value of the prestige key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.PrestigeKey, prestigeValue);
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedColor'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedColor", out value))
                {
                    // Converting the value to a string
                    var lastSelectedColor = value.ToString();

                    // Setting the value of the last selected color key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, 
                        lastSelectedColor);

                    // Using a for loop to go through all the colors in the color selected list
                    foreach (var colorBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.ColorSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.ColorSelected[colorBeingProcessed.Key] = 0;
                    }
                    
                    // Setting only the selected color to 1
                    HandleConstValuesStaticObjectsAndKeys.ColorSelected[lastSelectedColor] = 1;
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedHat'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedHat", out value))
                {
                    // Converting the value to a string
                    var lastSelectedHat = value.ToString();

                    // Setting the value of the last selected hat key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, 
                        lastSelectedHat);
                    
                    // For loop to go through all the hats in the hat selected list
                    foreach (var hatBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.HatsSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.HatsSelected[hatBeingProcessed.Key] = 0;
                    }

                    // We will only set the selected hat to 1 if it is not empty
                    if (string.IsNullOrEmpty(lastSelectedHat) == false)
                    {
                        // Setting only the selected hat to 1
                        HandleConstValuesStaticObjectsAndKeys.HatsSelected[lastSelectedHat] = 1;
                    }
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedGlasses'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedGlasses", out value))
                {
                    // Converting the value to a string
                    var lastSelectedGlasses = value.ToString();

                    // Setting the value of the last selected glasses key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, 
                        lastSelectedGlasses);
                    
                    // For loop to go through all the glasses in the glasses selected list
                    foreach (var glassesBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.GlassesSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.GlassesSelected[glassesBeingProcessed.Key] = 0;
                    }

                    // We will only set the selected glasses to 1 if it is not empty
                    if (string.IsNullOrEmpty(lastSelectedGlasses) == false)
                    {
                        // Setting only the selected glasses to 1
                        HandleConstValuesStaticObjectsAndKeys.GlassesSelected[lastSelectedGlasses] = 1;
                    }
                }
                
                // Check if the local user dictionary has the key 'LastSelectedMobileControls'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedMobileControls", out value))
                {
                    // Converting the value to a string
                    var lastSelectedMobileControls = int.Parse(value.ToString());

                    // Setting the value of the last selected mobile controls key in the player prefs
                    PlayerPrefs.SetInt(HandleConstValuesStaticObjectsAndKeys.MobileButtonsContainerKey, 
                        lastSelectedMobileControls);
                }
                
                //This function will check if the prestige has to be displayed
                CheckIfPrestigeHasToBeDisplayed();
                
                print("All key values for quick access have been set");
            }
            
            // Running this if we are on the iOS platform
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                 // Dynamic value for storing the value of the key
                object value;

                //Checking if the local dictionary has the key "Shield"
                if (LocalUserDataDictionary.TryGetValue("Shield", out value))
                {
                    //If the local dictionary has the key "Shield", then we will get the value of the key
                    var shieldValue = int.Parse(value.ToString());

                    //Setting the value of the shield key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ShieldKey, shieldValue);
                }

                //Checking if the local dictionary has the key "ExtraHealth"
                if (LocalUserDataDictionary.TryGetValue("ExtraHealth", out value))
                {
                    //If the local dictionary has the key "ExtraHealth", then we will get the value of the key
                    var healthValue = int.Parse(value.ToString());

                    //Setting the value of the health key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.HealthKey, healthValue);
                }

                //Checking if the local dictionary has the key "TokensInTotal"
                if (LocalUserDataDictionary.TryGetValue("TokensInTotal", out value))
                {
                    //If the local dictionary has the key "TokensInTotal", then we will get the value of the key
                    var playerTokensValue = int.Parse(value.ToString());

                    //Setting the value of the player tokens key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, playerTokensValue);
                }

                //Checking if the local dictionary has the key "ChestsInTotal"
                if (LocalUserDataDictionary.TryGetValue("ChestsInTotal", out value))
                {
                    //If the local dictionary has the key "ChestsInTotal", then we will get the value of the key
                    var chestsValue = int.Parse(value.ToString());

                    //Setting the value of the chests key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ChestKey, chestsValue);
                }

                //Checking if the local dictionary has the key "2xTokenBooster"
                if (LocalUserDataDictionary.TryGetValue("2xTokenBooster", out value))
                {
                    //If the local dictionary has the key "2xTokenBooster", then we will get the value of the key
                    var twoXTokenBoosterValue = int.Parse(value.ToString());

                    //Setting the value of the 2x token booster key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.TwoXBoosterKey, twoXTokenBoosterValue);
                }

                //Checking the local user dictionary has the key "3xTokenBooster"
                if (LocalUserDataDictionary.TryGetValue("3xTokenBooster", out value))
                {
                    //If the local dictionary has the key "3xTokenBooster", then we will get the value of the key
                    var threeXTokenBoosterValue = int.Parse(value.ToString());

                    //Setting the value of the 3x token booster key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.ThreeXBoosterKey, threeXTokenBoosterValue);
                }

                //Checking if the local user dictionary has the key "Prestige"
                if (LocalUserDataDictionary.TryGetValue("PrestigeLevel", out value))
                {
                    // Converting the value to a string
                    var prestigeValue = int.Parse(value.ToString());

                    //Setting the value of the prestige key in the player prefs
                    PlayerPrefs.SetInt(StoreMenuCanvas.PrestigeKey, prestigeValue);
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedColor'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedColor", out value))
                {
                    // Converting the value to a string
                    var lastSelectedColor = value.ToString();

                    // Setting the value of the last selected color key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey, 
                        lastSelectedColor);

                    // Using a for loop to go through all the colors in the color selected list
                    foreach (var colorBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.ColorSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.ColorSelected[colorBeingProcessed.Key] = 0;
                    }
                    
                    // Setting only the selected color to 1
                    HandleConstValuesStaticObjectsAndKeys.ColorSelected[lastSelectedColor] = 1;
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedHat'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedHat", out value))
                {
                    // Converting the value to a string
                    var lastSelectedHat = value.ToString();

                    // Setting the value of the last selected hat key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey, 
                        lastSelectedHat);
                    
                    // For loop to go through all the hats in the hat selected list
                    foreach (var hatBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.HatsSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.HatsSelected[hatBeingProcessed.Key] = 0;
                    }

                    // We will only set the selected hat to 1 if it is not empty
                    if (string.IsNullOrEmpty(lastSelectedHat) == false)
                    {
                        // Setting only the selected hat to 1
                        HandleConstValuesStaticObjectsAndKeys.HatsSelected[lastSelectedHat] = 1;
                    }
                }
                
                // Checking if the local user dictionary has the key 'LastSelectedGlasses'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedGlasses", out value))
                {
                    // Converting the value to a string
                    var lastSelectedGlasses = value.ToString();

                    // Setting the value of the last selected glasses key in the player prefs
                    PlayerPrefs.SetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey, 
                        lastSelectedGlasses);
                    
                    // For loop to go through all the glasses in the glasses selected list
                    foreach (var glassesBeingProcessed 
                             in HandleConstValuesStaticObjectsAndKeys.GlassesSelected.ToList())
                    {
                        // Setting the values to zero
                        HandleConstValuesStaticObjectsAndKeys.GlassesSelected[glassesBeingProcessed.Key] = 0;
                    }

                    // We will only set the selected glasses to 1 if it is not empty
                    if (string.IsNullOrEmpty(lastSelectedGlasses) == false)
                    {
                        // Setting only the selected glasses to 1
                        HandleConstValuesStaticObjectsAndKeys.GlassesSelected[lastSelectedGlasses] = 1;
                    }
                }
                
                // Check if the local user dictionary has the key 'LastSelectedMobileControls'
                if (LocalUserDataDictionary.TryGetValue("LastSelectedMobileControls", out value))
                {
                    // Converting the value to a string
                    var lastSelectedMobileControls = int.Parse(value.ToString());

                    // Setting the value of the last selected mobile controls key in the player prefs
                    PlayerPrefs.SetInt(HandleConstValuesStaticObjectsAndKeys.MobileButtonsContainerKey, 
                        lastSelectedMobileControls);
                }
                
                //This function will check if the prestige has to be displayed
                CheckIfPrestigeHasToBeDisplayed();
                
                print("All key values for quick access have been set");
            }
        }

        /// <summary>
        /// Runs when a new scene is loaded and is a gameplay scene
        /// </summary>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SceneManagerOnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            //We will try to get the data from the server if we are on the main menu
            if (arg0.buildIndex == 1)
            {
                //CreateLocalUserDataDictionaryObject();
                ExecuteCloudScriptToGetUserData();

                return;
            }

            print("Loading user data again on new gameplay scene");

            //CreateLocalUserDataDictionaryObject();
            ExecuteCloudScriptToGetUserData();
        }

        #endregion

        #region Upload user data to the server

        /// <summary>
        /// This function will upload the new user data to the server. We will just send the whole dictionary
        /// to the server which will be stored in the user data on the server. The dictionary needs to be updated
        /// before this function is called. Please do not forget this!
        /// </summary>
        public bool UploadLocalDictionaryToServer()
        {
            print("Trying to upload data to the server");

            var serializeLocalDictionary = JsonConvert.SerializeObject(LocalUserDataDictionary,
                Formatting.Indented);

            //This request is for executing the cloud script that will store the local user data to the server
            var uploadUserDataToServer = new ExecuteCloudScriptRequest
            {
                FunctionName = "uploadPlayerUserData",
                FunctionParameter = new {userData = serializeLocalDictionary},
                GeneratePlayStreamEvent = true
            };
            
            // This will be true when the data has been uploaded to the server
            bool isSuccessful = false;

            //Sending the request to execute the cloud script
            PlayFabClientAPI.ExecuteCloudScript(
                uploadUserDataToServer,
                result =>
                {
                    //Using Newtonsoft.Json to convert the result into a dictionary
                    var resultDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());

                    //Storing the "message" key's value in a dictionary
                    var userDataFromMessageResult = resultDictionary["message"];

                    //Printing the message
                    print(userDataFromMessageResult.ToString());

                    // This will only be true if the operation was successful
                    isSuccessful = true;
                },
                error => { print("Failed to setup the player data"); }
            );

            return isSuccessful;
        }
        
        /// <summary>
        /// This should be called when we try to upload the new user dictionary to the server after the user
        /// has died. This will restart the current scene after the data has been uploaded to the server
        /// and is usually called after a death event, such as when the player has hit the restart trigger
        /// </summary>
        public void UploadLocalDictionaryToServerAndRestartTheCurrentScene()
        {
            print("Trying to upload data to the server");

            var serializeLocalDictionary = JsonConvert.SerializeObject(LocalUserDataDictionary,
                Formatting.Indented);

            //This request is for executing the cloud script that will store the local user data to the server
            var uploadUserDataToServer = new ExecuteCloudScriptRequest
            {
                FunctionName = "uploadPlayerUserData",
                FunctionParameter = new {userData = serializeLocalDictionary},
                GeneratePlayStreamEvent = true
            };
            
            // This will be true when the data has been uploaded to the server
            bool isSuccessful = false;

            //Sending the request to execute the cloud script
            PlayFabClientAPI.ExecuteCloudScript(
                uploadUserDataToServer,
                result =>
                {
                    //Using Newtonsoft.Json to convert the result into a dictionary
                    var resultDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());

                    //Storing the "message" key's value in a dictionary
                    var userDataFromMessageResult = resultDictionary["message"];

                    //Printing the message
                    print(userDataFromMessageResult.ToString());

                    // This will only be true if the operation was successful
                    isSuccessful = true;
                    
                    // Setting the time scale to 1
                    Time.timeScale = 1;
                    
                    // Restarting the current scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                },
                error => { print("Failed to setup the player data"); }
            );
        }

        /// <summary>
        /// This function should be called whenever the user changes the clothes item. This will update the
        /// the keys on the server
        /// </summary>
        public void UpdateUserDictionaryOnServerWhenClothesItemWasChanged()
        {
            // Get the value of the last selected color
            string lastSelectedColor = PlayerPrefs.GetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedColorKey);
            // Get the value of the last selected hat
            string lastSelectedHat = PlayerPrefs.GetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedHatKey);
            // Get the value of the last selected glasses
            string lastSelectedGlasses = PlayerPrefs.GetString(HandleConstValuesStaticObjectsAndKeys.LastSelectedGlassesKey);
            
            // Update the local user data dictionary with the new values
            LocalUserDataDictionary["LastSelectedColor"] = lastSelectedColor;
            LocalUserDataDictionary["LastSelectedHat"] = lastSelectedHat;
            LocalUserDataDictionary["LastSelectedGlasses"] = lastSelectedGlasses;
            
            // Upload the new dictionary to the server
            UploadLocalDictionaryToServer();
        }

        /// <summary>
        /// This function should be called when we try to upload the new user dictionary to the server after
        /// an invalid key was found
        /// </summary>
        private void UploadLocalDictionaryToServerAfterAnInvalidKeyWasFound()
        {
            print("Trying to upload data to the server");

            var serializeLocalDictionary = JsonConvert.SerializeObject(LocalUserDataDictionary,
                Formatting.Indented);

            //This request is for executing the cloud script that will store the local user data to the server
            var uploadUserDataToServer = new ExecuteCloudScriptRequest
            {
                FunctionName = "uploadPlayerUserData",
                FunctionParameter = new {userData = serializeLocalDictionary},
                GeneratePlayStreamEvent = true
            };
            
            // This will be true when the data has been uploaded to the server
            bool isSuccessful = false;

            //Sending the request to execute the cloud script
            PlayFabClientAPI.ExecuteCloudScript(
                uploadUserDataToServer,
                result =>
                {
                    //Using Newtonsoft.Json to convert the result into a dictionary
                    var resultDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());

                    //Storing the "message" key's value in a dictionary
                    var userDataFromMessageResult = resultDictionary["message"];

                    //Printing the message
                    print(userDataFromMessageResult.ToString());

                    // This will only be true if the operation was successful
                    isSuccessful = true;
                    
                    // If we are successful we will execute the cloud script to get the user data again
                    ExecuteCloudScriptToGetUserData();
                },
                error =>
                {
                    // We will try to upload the user data again if we received an error
                    UploadLocalDictionaryToServerAfterAnInvalidKeyWasFound();
                    
                    print("Failed to setup the player data! Attempting again");
                }
            );
        }

        /// <summary>
        /// This should be called by any script that wishes to upload the data to the server immediately.
        /// This was originally created to update the Prestige level of the player, when the player clicks
        /// the New Game button
        /// </summary>
        public void UpdatePlayerDataOnServerImmediately()
        {
            // Getting the tokens in total and adding the prestige level to it
            int tokensInTotal = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference)
                                + PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);

            // Setting the tokens in total
            PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, tokensInTotal);

            // Get the current build index
            int currentBuildIndex = SceneManager.GetActiveScene().buildIndex;

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

            //Updating the keys on the local dictionary before uploading to the server
            LocalUserDataDictionary["TokensInTotal"]
                = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);
            LocalUserDataDictionary["Shield"]
                = PlayerPrefs.GetInt(StoreMenuCanvas.ShieldKey);
            LocalUserDataDictionary["ExtraHealth"]
                = PlayerPrefs.GetInt(StoreMenuCanvas.HealthKey);
            LocalUserDataDictionary["ChestsInTotal"]
                = PlayerPrefs.GetInt(StoreMenuCanvas.ChestKey);
            LocalUserDataDictionary["PrestigeLevel"]
                = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);

            //This will upload the data to the server
            Instance.UploadLocalDictionaryToServer();
        }

        #endregion

        #region Loading Data from Server Message

        /// <summary>
        /// This function will check if the loading data from server message has to be displayed. This will
        /// basically check if we are in the main menu scene, and if we are, then we will display the message
        /// to stop the player from clicking any button while the data is being loaded.
        /// </summary>
        private void CheckIfLoadingDataFromServerMessageHasToBeDisplayed()
        {
            //Checking if we are in the main menu scene
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                //We will find the object called LoadingDataFromServer in the hierarchy
                var loadingDataFromServer = GameObject.Find("LoadingDataFromServer");

                //Finding the object called MainMenuCanvas in the hierarchy, that is inactive
                var mainMenuCanvas = GameObject.Find("MainMenuCanvas");

                //Checking if the loadingDataFromServer object is not null
                if (loadingDataFromServer != null)
                {
                    //If the loadingDataFromServer object is not null, then we will set it to inactive
                    loadingDataFromServer.SetActive(false);
                }

                //Checking if the mainMenuCanvas object is not null
                if (mainMenuCanvas != null)
                {
                    //If the mainMenuCanvas object is not null, then we will set it to active
                    mainMenuCanvas.SetActive(true);
                }
            }
        }

        #endregion

        #region Prestige Display on Main Menu

        /// <summary>
        /// This function should run when the cloud script has been downloaded, and this will tell the
        /// game about the prestige level of the player. This will also check if the prestige level has
        /// to basically be displayed on the main menu or not.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void CheckIfPrestigeHasToBeDisplayed()
        {
            // Checking if we are in the main menu scene
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                //Finding object of type MainMenuCanvas in the hierarchy
                var mainMenuCanvas = FindObjectOfType<MainMenuCanvas>();

                //Checking if the mainMenuCanvas object is not null
                if (mainMenuCanvas != null)
                {
                    // Get the current prestige level of the player
                    var prestigeLevel = PlayerPrefs.GetInt(StoreMenuCanvas.PrestigeKey);

                    // We will call the function that will set the prestige level on the main menu
                    mainMenuCanvas.SetPrestigeLevelText(prestigeLevel);
                    
                    // We will see if the text of the new game has to be changed
                    mainMenuCanvas.SetupValueOfNewGameButtonText();
                }
            }
        }

        #endregion

        #region Singleton Setup

        /// <summary>
        /// Single that will be used to access this script from other scripts
        /// </summary>
        public static HandleUserDataFromPlayFabServer Instance;

        private void SetupSingleton()
        {
            //Checking if there is an instance of this script
            if (Instance == null)
            {
                //If there is no instance of this script, then we will set the instance to this script
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                //If there is an instance of this script, then we will destroy this script
                Destroy(gameObject);
            }
        }

        #endregion

        #region AOT helper setup

        /// <summary>
        /// This function is created after reading the documentation on the Unity website. This is required
        /// to make sure that the Dictionary Serialization works on the Android platform with IL2CPP scripting
        /// back-end as it is not supported on the Android platform.
        /// </summary>
        private void SetupAOTHelperForAndroid()
        {
            // Ensuring that all of the dictionaries are created
            AotHelper.EnsureDictionary<string, dynamic>();
            AotHelper.EnsureDictionary<string, int>();
            AotHelper.EnsureDictionary<string, object>();
            AotHelper.EnsureDictionary<string, string>();
            AotHelper.EnsureDictionary<string, bool>();
            AotHelper.EnsureDictionary<string, float>();
            AotHelper.EnsureDictionary<string, double>();
            AotHelper.EnsureDictionary<string, long>();
            AotHelper.EnsureDictionary<string, short>();
            AotHelper.EnsureDictionary<string, byte>();
            AotHelper.EnsureDictionary<string, char>();
            AotHelper.EnsureDictionary<string, uint>();
            AotHelper.EnsureDictionary<string, ushort>();
            AotHelper.EnsureDictionary<string, sbyte>();
            AotHelper.EnsureDictionary<string, ulong>();
            AotHelper.EnsureDictionary<string, decimal>();
            AotHelper.EnsureDictionary<string, DateTime>();
            AotHelper.EnsureDictionary<string, TimeSpan>();
            AotHelper.EnsureDictionary<string, Guid>();
            AotHelper.EnsureDictionary<string, DateTimeOffset>();
            AotHelper.EnsureDictionary<string, Uri>();
            AotHelper.EnsureDictionary<string, Version>();
            AotHelper.EnsureDictionary<string, Type>();
            AotHelper.EnsureDictionary<string, Enum>();
            AotHelper.EnsureDictionary<string, Array>();
            AotHelper.EnsureDictionary<string, List<string>>();
            AotHelper.EnsureDictionary<string, List<int>>();
            AotHelper.EnsureDictionary<string, List<object>>();
            AotHelper.EnsureDictionary<string, List<bool>>();
            AotHelper.EnsureDictionary<string, List<float>>();
            AotHelper.EnsureDictionary<string, List<double>>();
            AotHelper.EnsureDictionary<string, List<long>>();
            AotHelper.EnsureDictionary<string, List<short>>();
            AotHelper.EnsureDictionary<string, List<byte>>();
            AotHelper.EnsureDictionary<string, List<char>>();
            AotHelper.EnsureDictionary<string, List<uint>>();
            AotHelper.EnsureDictionary<string, List<ushort>>();
            AotHelper.EnsureDictionary<string, List<sbyte>>();
            AotHelper.EnsureDictionary<string, List<ulong>>();
            AotHelper.EnsureDictionary<string, List<decimal>>();
            AotHelper.EnsureDictionary<string, List<DateTime>>();
            AotHelper.EnsureDictionary<string, List<TimeSpan>>();
            AotHelper.EnsureDictionary<string, List<Guid>>();
            AotHelper.EnsureDictionary<string, List<DateTimeOffset>>();
            AotHelper.EnsureDictionary<string, List<Uri>>();
            AotHelper.EnsureDictionary<string, List<Version>>();
            AotHelper.EnsureDictionary<string, List<Type>>();
            AotHelper.EnsureDictionary<string, List<Enum>>();
            AotHelper.EnsureDictionary<string, List<Array>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, string>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, int>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, object>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, bool>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, float>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, double>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, long>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, short>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, byte>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, char>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, uint>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, ushort>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, sbyte>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, ulong>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, decimal>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, DateTime>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, TimeSpan>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, Guid>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, DateTimeOffset>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, Uri>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, Version>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, Type>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, Enum>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, Array>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<string>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<int>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<object>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<bool>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<float>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<double>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<long>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<short>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<byte>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<char>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<uint>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<ushort>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<sbyte>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<ulong>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<decimal>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<DateTime>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<TimeSpan>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<Guid>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<DateTimeOffset>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<Uri>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<Version>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<Type>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<Enum>>>();
            AotHelper.EnsureDictionary<string, Dictionary<string, List<Array>>>();
        }

        #endregion
        
        #region Unity Functions

        private void OnEnable()
        {
            //Subscribing to the event
            SceneManager.sceneLoaded += SceneManagerOnSceneLoaded;
        }

        private void OnDisable()
        {
            //Unsubscribing from the event
            SceneManager.sceneLoaded -= SceneManagerOnSceneLoaded;
        }

        private void Awake()
        {
            SetupAOTHelperForAndroid();
            SetupSingleton();
        }

        #endregion
    }
}