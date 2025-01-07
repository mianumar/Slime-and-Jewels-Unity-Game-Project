using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game_Designer_Online.Scripts.Main_Menu
{
    /// <summary>
    /// This script is attached to the CustomizationButtonTemplate prefab. This script will basically be used
    /// to activate or deactivate the ButButton that is a child of the object, as we don't want the player to
    /// be able to buy an item that they already own.
    /// </summary>
    public class CustomizationButtonTemplate : MonoBehaviour
    {
        /// <summary>
        /// This needs to be set in the inspector, as we have to tell the game which item this prefab is for.
        /// </summary>
        [SerializeField] private string itemKeyName;

        /// <summary>
        /// This is a reference to the buttons that are in the CustomizationButtonTemplate prefab. We will use
        /// these to activate or deactivate the buy button if the player already owns the item.
        /// </summary>
        [SerializeField] private GameObject buyButton, selectButton;

        /// <summary>
        /// This is a reference to the price text that is in the CustomizationButtonTemplate prefab. We will use
        /// this to tell the game the price of this item
        /// </summary>
        [SerializeField] private TextMeshProUGUI itemPriceText;

        /// <summary>
        /// This needs to be set in the inspector, as we have to tell the game how much the item costs in tokens.
        /// </summary>
        [SerializeField] private int itemPriceInTokens;

        /// <summary>
        /// This will return the Price of the item in tokens
        /// </summary>
        /// <returns></returns>
        public int GetItemPriceTextValue()
        {
            return itemPriceInTokens;
        }

        /// <summary>
        /// This function will deactivate the item if it is already unlocked by the player. This function will
        /// be called from the CustomizationMenuCanvas script.
        /// </summary>
        public void DeactivateTheBuyButtonIfItemIsUnlocked()
        {
            //Getting the Image component of the buy button
            var buyButtonImage = buyButton.GetComponent<Image>();
            //Turning the raycast target off
            buyButtonImage.raycastTarget = false;
            //Reducing the alpha value of the button to zero
            buyButtonImage.color = new Color(buyButtonImage.color.r, buyButtonImage.color.g,
                buyButtonImage.color.b, 0);

            //Getting the button component of the buy button
            var buyButtonButton = buyButton.GetComponent<Button>();
            //Making sure that the button is not interactable
            buyButtonButton.interactable = false;

            //Activating the select button
            selectButton.SetActive(true);
        }

        /// <summary>
        /// This function will try to unlock the items if the player has the necessary amount of tokens
        /// required to unlock it. It will try to access the CustomizationMenuCanvas.cs script
        /// and call a function based on the itemKeyName variable
        /// </summary>
        public void UnlockThisItemIfPlayerHasRequiredNumberOfTokens()
        {
            //Getting the CustomizationMenuCanvas script
            var customizationMenuCanvas = FindObjectOfType<CustomizationMenuCanvas>();

            //Getting the player tokens from PlayerPrefs
            var playerTokens = PlayerPrefs.GetInt(StoreMenuCanvas.PlayerTokensKeyReference);

            /*//Checking if the player has enough tokens to unlock the item
            if (playerTokens < itemPriceInTokens)
            {
                print("Player does not have enough tokens to unlock this item");

                //Returning out of the function
                return;
            }*/

            //Checking if the customizationMenuCanvas variable is null
            if (customizationMenuCanvas == null)
            {
                //If the customizationMenuCanvas variable is null, then we will print an error message
                //and return out of the function
                Debug.LogError("The customizationMenuCanvas variable is null");
                return;
            }

            //A boolean that will turn true if the player has successfully unlocked an item
            var playerHasUnlockedAnItem = false;

            //Switch case to check which item the player is trying to unlock, for color
            switch (itemKeyName)
            {
                case "GreenColor":
                    customizationMenuCanvas.UnlockGreenColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "BullColor":
                    customizationMenuCanvas.UnlockBullColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "CactusColor":
                    customizationMenuCanvas.UnlockCactusColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "CrystalColor":
                    customizationMenuCanvas.UnlockCrystalColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "DarkColor":
                    customizationMenuCanvas.UnlockDarkColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "ElectricColor":
                    customizationMenuCanvas.UnlockElectricColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "FireColor":
                    customizationMenuCanvas.UnlockFireColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "NyanColor":
                    customizationMenuCanvas.UnlockNyanColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "PoopColor":
                    customizationMenuCanvas.UnlockPoopColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "RockColor":
                    customizationMenuCanvas.UnlockRockColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "SkullColor":
                    customizationMenuCanvas.UnlockSkullColor();
                    playerHasUnlockedAnItem = true;
                    break;
                case "WaterColor":
                    customizationMenuCanvas.UnlockWaterColor();
                    playerHasUnlockedAnItem = true;
                    break;
                default:
                    Debug.LogError("The itemKeyName variable is not a color. This is not an error!");
                    break;
            }

            //Running this only if the player has not unlocked an item yet
            if (playerHasUnlockedAnItem == false)
            {
                //Switch case to check which item the player is trying to unlock, for hats
                switch (itemKeyName)
                {
                    case "Hat_1":
                        customizationMenuCanvas.UnlockHatOne();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_2":
                        customizationMenuCanvas.UnlockHatTwo();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_3":
                        customizationMenuCanvas.UnlockHatThree();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_4":
                        customizationMenuCanvas.UnlockHatFour();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_5":
                        customizationMenuCanvas.UnlockHatFive();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_6":
                        customizationMenuCanvas.UnlockHatSix();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_7":
                        customizationMenuCanvas.UnlockHatSeven();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_8":
                        customizationMenuCanvas.UnlockHatEight();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_9":
                        customizationMenuCanvas.UnlockHatNine();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_10":
                        customizationMenuCanvas.UnlockHatTen();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_11":
                        customizationMenuCanvas.UnlockHatEleven();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Hat_12":
                        customizationMenuCanvas.UnlockHatTwelve();
                        playerHasUnlockedAnItem = true;
                        break;
                    default:
                        Debug.LogError("The itemKeyName variable is not a hat. This is not an error!");
                        break;
                }
            }

            //Running this only if the player has not unlocked an item yet
            if (playerHasUnlockedAnItem == false)
            {
                //Switch case to check which item the player is trying to unlock, for glasses
                switch (itemKeyName)
                {
                    case "Glasses_1":
                        customizationMenuCanvas.UnlockGlassesOne();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Glasses_2":
                        customizationMenuCanvas.UnlockGlassesTwo();
                        playerHasUnlockedAnItem = true;
                        break;
                    case "Glasses_3":
                        customizationMenuCanvas.UnlockGlassesThree();
                        playerHasUnlockedAnItem = true;
                        break;
                    default:
                        Debug.LogError("The itemKeyName variable is not a hat. This is not an error! " +
                                       "We will return and not deduct the tokens");
                        return;
                }
            }
            
            /*
            //Running this only if a player has unlocked an item
            if (playerHasUnlockedAnItem == true)
            {
                //Deduct the tokens and update the player prefs
                playerTokens -= itemPriceInTokens;

                //Setting the player tokens in PlayerPrefs
                PlayerPrefs.SetInt(StoreMenuCanvas.PlayerTokensKeyReference, playerTokens);
            }*/
        }

        /// <summary>
        /// This will set the price text value to the value of the itemPriceInTokens variable
        /// </summary>
        private void SetPriceTextValue()
        {
            //Setting the price text value to the value of the itemPriceInTokens variable
            itemPriceText.text = itemPriceInTokens.ToString();
        }

        private void OnEnable()
        {
            //Setting the item key value as that is the gameObject name
            itemKeyName = gameObject.name;

            SetPriceTextValue();
        }
    }
}