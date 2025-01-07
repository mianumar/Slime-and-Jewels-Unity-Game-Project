using System.Collections.Generic;
using Game_Designer_Online.Scripts.Main_Menu;
using UnityEngine;

namespace Game_Designer_Online.Scripts.Misc
{
    public class HandleConstValuesStaticObjectsAndKeys : MonoBehaviour
    {
        #region Static Objects
        
        /// <summary>
        /// This tells the game which color is currently selected and which is not
        /// </summary>
        public static readonly Dictionary<string, int> ColorSelected = new Dictionary<string, int>
        {
            {"Green", 0},
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
        /// This tells the game which accessory is currently selected and which is not
        /// </summary>
        public static readonly Dictionary<string, int> HatsSelected = new Dictionary<string, int>
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
        /// This tells the game which glasses is currently selected and which is not
        /// </summary>
        public static readonly Dictionary<string, int> GlassesSelected = new Dictionary<string, int>
        {
            {"GlassesOne", 0},
            {"GlassesTwo", 0},
            {"GlassesThree", 0},
        };

        /// <summary>
        /// This will setup the initial values of the last selected keys
        /// </summary>
        private void SettingUpLastSelectedKeys()
        {
            // Todo collect all values from the server
            
            //Checking if the player has the last selected color key
            if (PlayerPrefs.HasKey(LastSelectedColorKey) == false)
            {
                // Setting the last selected color key to the last selected color
                PlayerPrefs.SetString(LastSelectedColorKey, "Green");
            }

            //If the string is not empty, we will not try to get the last selected value
            if (string.IsNullOrEmpty(PlayerPrefs.GetString(LastSelectedColorKey)) == false)
            {
                // Get the value of the last selected color key
                var lastSelectedColor = PlayerPrefs.GetString(LastSelectedColorKey);
                
                //Changing the key value to 1 based on the value of the last selected color key
                ColorSelected[lastSelectedColor] = 1;
            }
            
            //Checking if the player has the last selected hat key
            if (PlayerPrefs.HasKey(LastSelectedHatKey) == false)
            {
                PlayerPrefs.SetString(LastSelectedHatKey, "");
            }

            //If the string is not empty, we will not try to get the last selected value
            if (string.IsNullOrEmpty(PlayerPrefs.GetString(LastSelectedHatKey)) == false)
            {
                // Get the value of the last selected hat key
                var lastSelectedHat = PlayerPrefs.GetString(LastSelectedHatKey);

                //Changing the key value to 1 based on the value of the last selected hat key
                HatsSelected[lastSelectedHat] = 1;
            }

            //Checking if the player has the last selected glasses key
            if (PlayerPrefs.HasKey(LastSelectedGlassesKey) == false)
            {
                PlayerPrefs.SetString(LastSelectedGlassesKey, "");
            }

            //If the string is not empty, we will not try to get the last selected value
            if (string.IsNullOrEmpty(PlayerPrefs.GetString(LastSelectedGlassesKey)) == false)
            {
                // Get the value of the last selected glasses key
                var lastSelectedGlasses = PlayerPrefs.GetString(LastSelectedGlassesKey);
                
                //Changing the key value to 1 based on the value of the last selected glasses key
                GlassesSelected[lastSelectedGlasses] = 1;
            }
        }
        
        /// <summary>
        /// This will set the player mobile container key and the controls
        /// </summary>
        private void SetMobileControlTypeKey()
        {
            // Checking if the key exists
            if (PlayerPrefs.HasKey(MobileButtonsContainerKey) == false)
            {
                //If the key does not exist, then we will set the key to the mobile buttons container
                PlayerPrefs.SetInt(MobileButtonsContainerKey, 1);
            }
        }
        
        /// <summary>
        /// These are the constant key values 
        /// </summary>
        public const string LastSelectedColorKey = "LastSelectedColor",
            LastSelectedHatKey = "LastSelectedHat",
            LastSelectedGlassesKey = "LastSelectedGlasses";
        
        /// <summary>
        /// This is a reference to the mobile buttons container key
        /// </summary>
        public const string MobileButtonsContainerKey = "MobileButtonsContainerKey";

        #endregion
        
        #region Store Key Setup

        /// <summary>
        /// This will setup the player store key values
        /// </summary>
        private void SetupPlayerStoreKeyValues()
        {
            //Checking if the player has the shield
            if (!PlayerPrefs.HasKey(StoreMenuCanvas.ShieldKey))
            {
                //If the player does not have the shield, then we will set the shield key value to 0
                PlayerPrefs.SetInt(StoreMenuCanvas.ShieldKey, 0);
            }
            
            //Checking if the player has the health
            if (!PlayerPrefs.HasKey(StoreMenuCanvas.HealthKey))
            {
                //If the player does not have the health, then we will set the health key value to 3.
                //We might change this in the future. 3 is only for testing
                PlayerPrefs.SetInt(StoreMenuCanvas.HealthKey, 3);
            }
            
            //Checking if the player has the player tokens
            if (!PlayerPrefs.HasKey(StoreMenuCanvas.PlayerTokensKeyReference))
            {
                //If the player does not have the player tokens, then we will set the player tokens key value to 0
                PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, 0);
            }
            
            //Checking if the player has the chest key
            if (!PlayerPrefs.HasKey(StoreMenuCanvas.ChestKey))
            {
                //If the player does not have the chest key, then we will set the chest key value to 0
                PlayerPrefs.SetInt(StoreMenuCanvas.ChestKey, 0);
            }
            
            //Checking if the player has the prestige key
            if (!PlayerPrefs.HasKey(StoreMenuCanvas.PrestigeKey))
            {
                //If the player does not have the prestige key, then we will set the prestige key value to 0
                PlayerPrefs.SetInt(StoreMenuCanvas.PrestigeKey, 0);
            }
        }

        #endregion

        #region Unity Functions

        private void Start()
        {
            SetupPlayerStoreKeyValues();
            SettingUpLastSelectedKeys();
            SetMobileControlTypeKey();
        }

        #endregion
    }
}
