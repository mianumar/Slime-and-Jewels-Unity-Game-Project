using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game_Designer_Online.Scripts.Do_Not_Destroy_Scripts;
using Game_Designer_Online.Scripts.Main_Menu;
using Game_Designer_Online.Scripts.Misc;
using Game_Designer_Online.Scripts.Playfab_Related_Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game_Designer_Online.Scripts.Gameplay_Related
{
    /// <summary>
    /// This script is attached to the PlayerCharacter game object and it will handle the movement of the player
    /// and the rest of the features that are required to make the game work properly
    /// </summary>
    public class PlayerCharacter : MonoBehaviour
    {
        #region Avatar Loading and Related Functions

        [Header("List of Avatar Animator Controllers That Can Be Loaded")]
        //A list of green avatar animator controller
        public List<RuntimeAnimatorController> greenAvatarAnimatorControllers;

        /// <summary>
        /// A list of bull avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> bullAvatarAnimatorControllers;

        /// <summary>
        /// A list of cactus avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> cactusAvatarAnimatorControllers;

        /// <summary>
        /// A list of the crystal animator controller
        /// </summary>
        public List<RuntimeAnimatorController> crystalAvatarAnimatorControllers;

        /// <summary>
        /// A list of the dark avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> darkAvatarAnimatorControllers;

        /// <summary>
        /// A list of the electric avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> electricAvatarAnimatorControllers;

        /// <summary>
        /// A list of the fire avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> fireAvatarAnimatorControllers;

        /// <summary>
        /// A list of the nyan avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> nyanAvatarAnimatorController;

        /// <summary>
        /// A list of the poop avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> poopAvatarAnimatorController;

        /// <summary>
        /// A list of the rock avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> rockAvatarAnimatorController;

        /// <summary>
        /// A list of the skull avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> skullAvatarAnimatorController;

        /// <summary>
        /// A list of the water avatar animator controller
        /// </summary>
        public List<RuntimeAnimatorController> waterAvatarAnimatorController;

        /// <summary>
        ///     This is the function that will load the controller according to the selected key
        ///     value from the HandleConstValuesStaticObjectsAndKeys class
        /// </summary>
        private void LoadAnimatorControllerAccordingToSelectedKey()
        {
            //This string will tell the game which avatar animator must be loaded
            var nameOfAvatarAnimatorToLoad = "";

            //This is the dictionary that will tell which color was selected by the player
            var dictionaryOfSelectedColor = HandleConstValuesStaticObjectsAndKeys.ColorSelected;

            //This is the dictionary that will tell which hat was selected by the player
            var dictionaryOfSelectedAccessory = HandleConstValuesStaticObjectsAndKeys.HatsSelected;
            var indexOfSelectedAccessory = 0;

            //This is the dictionary that will tell which glasses was selected by the player
            var dictionaryOfSelectedGlasses = HandleConstValuesStaticObjectsAndKeys.GlassesSelected;
            var indexOfSelectedGlasses = 0;

            //We will see which color was selected by the player by taking a look at the value. If the value
            //of a particular key is one, it means that that color was selected
            foreach (var key in dictionaryOfSelectedColor.Keys)
            {
                //Checking if the value of the key is one
                if (dictionaryOfSelectedColor[key] == 1)
                {
                    //If the value of the key is one, then we will set the nameOfAvatarAnimatorToLoad variable
                    //to the key
                    nameOfAvatarAnimatorToLoad = key;
                }
            }

            //This switch case will tell the game which color was selected by the player and will load the
            //animator controller accordingly. But before that, this will also check if the player has
            //any accessories equipped as well
            switch (nameOfAvatarAnimatorToLoad)
            {
                case "Green":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = greenAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = greenAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "PlayerCharacterAvatarAnimator";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = greenAvatarAnimatorControllers.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Bull":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = bullAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = bullAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "BullAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = bullAvatarAnimatorControllers.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Cactus":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = cactusAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = cactusAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "CactusAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = cactusAvatarAnimatorControllers.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Crystal":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = crystalAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = crystalAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "CrystalAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = crystalAvatarAnimatorControllers.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Dark":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = darkAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = darkAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "DarkAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = darkAvatarAnimatorControllers.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Electric":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = electricAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = electricAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "ElectricAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = electricAvatarAnimatorControllers.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Fire":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = fireAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = fireAvatarAnimatorControllers.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "FireAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = fireAvatarAnimatorControllers.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Nyan":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = nyanAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = nyanAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "NyanAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = nyanAvatarAnimatorController.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Poop":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = poopAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = poopAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "PoopAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = poopAvatarAnimatorController.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Rock":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = rockAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = rockAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "RockAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = rockAvatarAnimatorController.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Skull":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = skullAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = skullAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "SkullAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = skullAvatarAnimatorController.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                case "Water":
                    //Getting the index of the selected accessory
                    for (int i = 0; i < dictionaryOfSelectedAccessory.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedAccessory.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected accessory
                            //to the index of the key value pair
                            indexOfSelectedAccessory = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedAccessory is not 0, it means that an accessory was selected
                    if (indexOfSelectedAccessory != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorAccessory " + indexOfSelectedAccessory;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = waterAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //Getting the index of the selected glasses
                    for (int i = 0; i < dictionaryOfSelectedGlasses.Count; i++)
                    {
                        //Getting the key value pair of the dictionary
                        var keyValuePair = dictionaryOfSelectedGlasses.ElementAt(i);

                        //Checking if the value of the key is one
                        if (keyValuePair.Value == 1)
                        {
                            //If the value of the key is one, then we will set the index of the selected glasses
                            //to the index of the key value pair
                            indexOfSelectedGlasses = i + 1;
                            break;
                        }
                    }

                    //If indexOfSelectedGlasses is not 0, it means that glasses was selected
                    if (indexOfSelectedGlasses != 0)
                    {
                        nameOfAvatarAnimatorToLoad += "AvatarAnimatorGlasses " + indexOfSelectedGlasses;

                        //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                        avatarAnimator.runtimeAnimatorController = waterAvatarAnimatorController.Find(
                            x => x.name == nameOfAvatarAnimatorToLoad);

                        break;
                    }

                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "WaterAvatarAnimatorAccessory 0";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = waterAvatarAnimatorController.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);

                    break;

                default:
                    //This is the name of the default character animator controller that we can load
                    nameOfAvatarAnimatorToLoad = "PlayerCharacterAvatarAnimator";

                    //Return the animator controller whose name matches the nameOfAvatarAnimatorToLoad variable
                    avatarAnimator.runtimeAnimatorController = greenAvatarAnimatorControllers.Find(
                        x => x.name == nameOfAvatarAnimatorToLoad);
                    break;
            }
        }

        #endregion

        #region Movement and Functions Related to It, Including Jump Function

        //___________________________INPUT FUNCTIONS____________________________//

        /// <summary>
        ///     When this variable is true, it tells the game that the player is allowed to jump
        /// </summary>
        [SerializeField] private bool allowJumpInput = true;

        /// <summary>
        ///     When true, it tells the game that the player should apply a jump force to the player
        /// </summary>
        [SerializeField] private bool applyJumpForce;

        /// <summary>
        /// When true, tells the game that the input has been disabled
        /// </summary>
        private bool _disableInput = false;

        /// <summary>
        /// This will run when the right button on the mobile controls is interacted with
        /// </summary>
        public void SetMoveInputMobileRight(float valueToSet)
        {
            moveInput.x = valueToSet;
        }

        /// <summary>
        /// This will run when the left button on the mobile controls is interacted with
        /// </summary>
        /// <param name="valueToSet"></param>
        public void SetMoveInputMobileLeft(float valueToSet)
        {
            moveInput.x = valueToSet;
        }
        
        /// <summary>
        /// This should only run on the mobile.
        /// </summary>
        public void OnMobileJumpInput()
        {
            //Checking if the player is allowed to jump
            if (allowJumpInput == false)
                //If the player is not allowed to jump, then we will return from the function
                return;
            
            //If the player is allowed to jump, then we will set the _allowJumpInput variable to false
            allowJumpInput = false;

            //We will then set the _applyJumpForce variable to true
            applyJumpForce = true;
            
            // Playing the jump sound
            SoundManager.Instance.PlayJumpSound();

            //We will then call the Routine_ResetAllowJumpInput() coroutine
            StartCoroutine(Routine_ResetJumpRelatedVariables());
        }
        
        /// <summary>
        ///     This function will be used to set the value of the moveInput variable. This should be called in the update
        ///     through the SwitchCaseToRunDifferentFunctionsInUpdate(). This will only run if we are not on mobile
        /// </summary>
        private void SetMoveInput()
        {
            // This will only run if we are not on mobile
            if (Application.platform == RuntimePlatform.WindowsEditor 
                || Application.platform == RuntimePlatform.WindowsPlayer
                || Application.platform == RuntimePlatform.LinuxEditor
                || Application.platform == RuntimePlatform.OSXEditor)
            {
                moveInput.x = Input.GetAxisRaw("Horizontal");
                moveInput.y = Input.GetAxisRaw("Vertical");
            }
            
            // This will only run if we are on mobile
            if (Application.platform == RuntimePlatform.Android)
            {
                // This will run only when the joystick is active
                if (joystickContainer.activeSelf == true)
                {
                    moveInput.x = floatingJoystick.Horizontal;
                    moveInput.y = floatingJoystick.Vertical;
                }
            }
            
            // This will run if we are on an iPhone
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // This will run only when the joystick is active
                if (joystickContainer.activeSelf == true)
                {
                    moveInput.x = floatingJoystick.Horizontal;
                    moveInput.y = floatingJoystick.Vertical;
                }
            }
        }

        /// <summary>
        ///     This function should be called in the Update() function through the
        ///     SwitchCaseToRunDifferentFunctionsInUpdate()
        /// </summary>
        private void CheckForJumpInput()
        {
            //Checking if the player is allowed to jump
            if (allowJumpInput == false)
                //If the player is not allowed to jump, then we will return from the function
                return;

            //Checking if the player has pressed the jump button
            if (Input.GetButtonDown("Jump"))
            {
                //If the player has pressed the jump button, then we will set the _allowJumpInput variable to false
                allowJumpInput = false;

                //We will then set the _applyJumpForce variable to true
                applyJumpForce = true;

                // We will run this only if the SoundManager is not null
                if (SoundManager.Instance != null)
                {
                    // Playing the jump sound
                    SoundManager.Instance.PlayJumpSound();
                }

                //We will then call the Routine_ResetAllowJumpInput() coroutine
                StartCoroutine(Routine_ResetJumpRelatedVariables());
            }
        }

        /// <summary>
        ///     A variable that will be used in the Routine_ResetAllowJumpInput() coroutine to reset the _allowJumpInput
        /// </summary>
        private readonly WaitForSeconds _jumpWaitTime = new(0.1f);

        /// <summary>
        ///     This function will be called when the player presses the jump button. This will be used to reset
        ///     the value of the _allowJumpInput variable and the _applyJumpForce variable
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_ResetJumpRelatedVariables()
        {
            yield return _jumpWaitTime;
            allowJumpInput = true;
            applyJumpForce = false;
        }

        //_____________________________________________________________________________//

        //___________________________MOVEMENT FUNCTIONS________________________________//

        /// <summary>
        ///     This is the function that will be used to move the player. This should be called in the FixedUpdate()
        ///     through the SwitchCaseToRunDifferentFunctionsInFixedUpdate()
        /// </summary>
        private void MovePlayerUsingRigidbodyVelocity()
        {
            //Returns if the input has been disabled
            if (_disableInput == true) return;

            //Setting up velocity for the player
            thisRigidbody2D.velocity = new Vector2(moveInput.x * 5, thisRigidbody2D.velocity.y);

            //Clamp the magnitude of the velocity
            thisRigidbody2D.velocity = Vector2.ClampMagnitude(thisRigidbody2D.velocity, 12.5f);
        }

        /// <summary>
        ///     This is the function that will be used to apply a jump force to the player. This should be called in the
        ///     through the SwitchCaseToRunDifferentFunctionsInFixedUpdate()
        /// </summary>
        private void ApplyJumpForceToThePlayerIfAllowed()
        {
            //Returns if the input has been disabled
            if (_disableInput == true) return;

            //Checking if the player is allowed to jump
            if (applyJumpForce == false)
                //If the player is not allowed to jump, then we will return from the function
                return;

            //If the player is allowed to jump, then we will apply a jump force to the player
            thisRigidbody2D.AddForce(Vector2.up * 15f, ForceMode2D.Impulse);
        }

        //_____________________________________________________________________________________//

        //___________________________GROUND DETECTION FUNCTIONS________________________________//

        /// <summary>
        ///     The layer mask for the ground ray is set here. Please set it through the inspector
        /// </summary>
        [SerializeField] private LayerMask groundLayerMask;

        /// <summary>
        ///     This variable should return something if the player is on the ground, as the function called
        ///     ShootCircleCastToCheckIfPlayerIsOnTheGround() will shoot a circle cast to check if the player
        ///     is on the ground
        /// </summary>
        private RaycastHit2D _groundRayCastHit;

        /// <summary>
        ///     This function will be used to shoot the circle cast on the ground
        /// </summary>
        private void ShootCircleCastToCheckIfPlayerIsOnTheGround()
        {
            //Setting the radius of the circle cast based on the colliders size
            var circleCastRadius = thisCircleCollider2D.radius;

            //Setting the distance of the raycast based on the bounds of the collider
            var circleCastDistance = thisCircleCollider2D.bounds.extents.y + 0.1f;

            //Shooting the circle cast
            _groundRayCastHit = Physics2D.CircleCast(transform.position, circleCastRadius,
                Vector2.down, circleCastDistance, groundLayerMask);
        }

        /// <summary>
        ///     This function will tell the game whether the player is on the ground or not. If the _groundRayCastHit
        ///     hits something, it means that the player is on the ground
        /// </summary>
        private void ChangeValueOfIsOnGroundVariableIfPlayerIsOnTheGround()
        {
            //Checking if the circle cast has hit something
            if (_groundRayCastHit.collider != null)
            {
                //If the circle cast has hit something, then we will set the isOnGround variable to true
                isOnGround = true;
                return;
            }

            //If the circle cast has not hit anything, then we will set the isOnGround variable to false
            isOnGround = false;
        }

        //________________________________________________________________________________________//

        #endregion

        #region Sprite Renderer and Animator Related Functions

        //_____________________________AVATAR DIRECTION_________________________________//

        /// <summary>
        ///     This function will change the direction of the sprite renderer based on the direction of the movement
        /// </summary>
        private void ChangeDirectionOfSpriteRendererBasedOnMovementDirection()
        {
            //This will make the sprite face right
            if (moveInput.x > 0.2f) avatarSpriteRenderer.flipX = true;

            //This will make the sprite face left
            if (moveInput.x < -0.2f) avatarSpriteRenderer.flipX = false;
        }

        //_____________________________AVATAR ANIMATIONS_________________________________//

        /// <summary>
        ///     This function will play the idle animation
        /// </summary>
        private void PlayIdleAnimation()
        {
            avatarAnimator.Play("PlayerCharacterAvatarIdle");
        }

        /// <summary>
        ///     This will play the movement animation
        /// </summary>
        private void PlayMovementAnimation()
        {
            avatarAnimator.Play("PlayerCharacterAvatarMove");
        }

        /// <summary>
        ///     This function will either play the upwards animation or the downwards animation based on the velocity of
        ///     while the player is off the ground, based on the player's velocity
        /// </summary>
        private void PlayOffGroundAnimationBasedOnYVelocity()
        {
            //If the player is moving up, then we will play the jump up animation
            if (thisRigidbody2D.velocity.y > 0) avatarAnimator.Play("PlayerCharacterAvatarUpwards");

            //If the player is moving down, then we will play the jump down animation
            if (thisRigidbody2D.velocity.y < -0.15f) avatarAnimator.Play("PlayerCharacterAvatarDownwards");
        }

        #endregion

        #region Camera Movement Functions

        [Header("Camera Movement Functions")]
        //Camera movement offset. This tells the game after how much movement of the player, the camera should move
        //on the x axis to follow the player
        [SerializeField]
        private float cameraMovementOffset = 0.5f;

        /// <summary>
        ///     This variables need to be set in the inspector. These variables will set the camera
        ///     clamps for a particular level, to make sure that the camera doesn't go out of bounds
        /// </summary>
        [SerializeField] private float xCameraClampMin, xCameraClampMax, yCameraClampMin, yCameraClamp;

        /// <summary>
        ///     This tells the game how fast the camera should follow the player
        /// </summary>
        [SerializeField] private float cameraFollowSpeed;

        /// <summary>
        /// When this is true, it will tell the game to allow the camera to move in the Y axis as well
        /// </summary>
        [SerializeField] private bool moveCameraInY = false;

        private void MoveCameraWithPlayerOnlyIfOffsetFromCenterHasExceeded()
        {
            //Checking if the player has moved more than the camera movement offset
            if (Mathf.Abs(transform.position.x - mainCameraReference.transform.position.x) > cameraMovementOffset)
            {
                //Using Lerp to move the camera to the player
                mainCameraReference.transform.position = Vector3.Lerp(mainCameraReference.transform.position,
                    new Vector3(transform.position.x, mainCameraReference.transform.position.y,
                        mainCameraReference.transform.position.z), cameraFollowSpeed);
            }
            
            //We will first check if we are allowed to move the camera in the Y axis
            if (moveCameraInY == true)
            {
                //Checking if the player has moved more than the camera movement offset
                if (Mathf.Abs(transform.position.y - mainCameraReference.transform.position.y) > cameraMovementOffset)
                {
                    //Using Lerp to move the camera to the player
                    mainCameraReference.transform.position = Vector3.Lerp(mainCameraReference.transform.position,
                        new Vector3(mainCameraReference.transform.position.x, transform.position.y,
                            mainCameraReference.transform.position.z), cameraFollowSpeed);
                }
            }

            //Clamping the camera position
            mainCameraReference.transform.position = new Vector3(
                Mathf.Clamp(mainCameraReference.transform.position.x, xCameraClampMin, xCameraClampMax),
                Mathf.Clamp(mainCameraReference.transform.position.y, yCameraClampMin, yCameraClamp),
                -10);
        }

        #endregion

        #region Functions Related to Adding or Deducting Inventory Items Such as Health

        /// <summary>
        /// This will store the number of tokens that the player has collected in this level
        /// </summary>
        private int _tokensAddedInThisLevel = 0;
        
        /// <summary>
        ///     Function that will add tokens. Usually called when the player collects a token which is a jewel
        /// </summary>
        public void AddToken()
        {
            //Getting the value of the tokens that are currently stored in the player prefs
            var tempTokens = PlayerPrefs.GetInt(_playerTokensKey);

            //We will check if a booster is active. If a booster is active, then we will add tokens based
            //on what type of booster was activated
            var boosterValueToAdd = 0;

            //Booster value to add
            switch (PlayerPrefs.GetInt(BoosterHandler.BoosterActiveKey))
            {
                case 1:
                    boosterValueToAdd = 1;
                    break;
                case 2:
                    boosterValueToAdd = 2;
                    break;
                default:
                    boosterValueToAdd = 0;
                    break;
            }

            //Adding 1 to the tempTokens variable. This will be used to set the value of the player prefs key
            tempTokens = tempTokens + 1 + boosterValueToAdd;
            
            // Tokens added in this level
            _tokensAddedInThisLevel += 1 + boosterValueToAdd;

            //Setting the value of the tempTokens variable to the player prefs
            PlayerPrefs.SetInt(_playerTokensKey, tempTokens);

            //Setting the value of the tokens variable to the tempTokens variable so that we can display
            //the token that the player has collected
            _tokens = tempTokens;

            //We will use the tokensText.text variable to display the tokens that the player has collected
            tokensText.text = "Tokens: " + _tokens;
        }

        /// <summary>
        /// This will store the number of health that the player has collected in this level
        /// </summary>
        private int _healthAddedInThisLevel = 0;
        
        /// <summary>
        ///     Function that will add health. Usually called when the player will
        /// </summary>
        public void AddHealth()
        {
            // Creating the local user dictionary variable. The value of this will be assigned
            // by getting its value from the HandleUserDataFromPlayFabServer.LocalUserDataDictionary
            // and then this will be uploaded to the server
            Dictionary<string, object> localUserDictionary = new();

            // This will store the value of the health with the added health
            int healthValueWithAddedHealth = 0;
            
            //We will return if the health is already maxed out
            if (_health > 3)
            {
                //Just adding +1 to the relevant PlayerHealth
                PlayerPrefs.SetInt(_playerHealthKey, PlayerPrefs.GetInt(_playerHealthKey) + 1);
                
                // Adding 1 to the health added in this level
                _healthAddedInThisLevel++;
                
                /*// Get the current value of the Local User Dictionary
                localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
                // Check if the local user dictionary is not null
                if (localUserDictionary != null)
                {
                    // We will check if the local user dictionary contains the key "ExtraHealth"
                    if (localUserDictionary.ContainsKey("ExtraHealth"))
                    {
                        // Converting the value of the shield to an integer
                        healthValueWithAddedHealth = 
                            Convert.ToInt32(localUserDictionary["ExtraHealth"]) + 1;
                        
                        // If the local user dictionary contains the key "ExtraHealth",
                        // then we will add 1 to the value
                        localUserDictionary["ExtraHealth"] = healthValueWithAddedHealth;
                    
                        // Upload the data to the server
                        HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    }
                }*/

                return;
            }

            //Adding 1 to the tempHealth variable. This will be used to set the value of the player prefs key
            _health += 1;
            
            // Adding 1 to the health added in this level
            _healthAddedInThisLevel++;

            //Setting the health to display the health that the player has collected
            healthText.text = "Health: " + _health;

            //Just adding +1 to the relevant PlayerHealth
            PlayerPrefs.SetInt(_playerHealthKey, PlayerPrefs.GetInt(_playerHealthKey) + 1);
        }

        /// <summary>
        /// This will store the shield that the player has collected in this level
        /// </summary>
        private int _shieldAddedInThisLevel = 0;

        /// <summary>
        ///     A function that will add a shield. Usually called when the player collects a shield
        /// </summary>
        public void AddShield()
        {
            // Creating the local user dictionary variable. The value of this will be assigned
            // by getting its value from the HandleUserDataFromPlayFabServer.LocalUserDataDictionary
            // and then this will be uploaded to the server
            Dictionary<string, object> localUserDictionary = new();

            // This will store the value of the shield with the added shield
            int shieldValueWithAddedShield = 0;
            
            // If we have more than 3 shields, we will just add that shield to the player prefs
            if (_shield >= 3)
            {
                //Just adding +1 to the relevant PlayerShield
                PlayerPrefs.SetInt(_playerShieldKey, PlayerPrefs.GetInt(_playerShieldKey) + 1);
                
                // Adding 1 to the shield added in this level
                _shieldAddedInThisLevel++;

                return;
            }

            //Adding 1 to the tempShield variable. This will be used to set the value of the player prefs key
            _shield += 1;
            
            // Adding 1 to the shield added in this level
            _shieldAddedInThisLevel++;

            //Setting the shield to display the shield that the player has collected
            shieldText.text = "Shield: " + _shield;

            //Just adding +1 to the relevant PlayerShield
            PlayerPrefs.SetInt(_playerShieldKey, PlayerPrefs.GetInt(_playerShieldKey) + 1);
            
            /*// Get the current value of the Local User Dictionary
            localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            // Check if the local user dictionary is not null
            if (localUserDictionary != null)
            {
                // We will check if the local user dictionary contains the key "Shield"
                if (localUserDictionary.ContainsKey("Shield"))
                {
                    // Converting the value of the shield to an integer
                    shieldValueWithAddedShield = 
                        Convert.ToInt32(localUserDictionary["Shield"]) + 1;
                        
                    // If the local user dictionary contains the key "Shield", then we will add 1 to the value
                    localUserDictionary["Shield"] = shieldValueWithAddedShield;
                    
                    // Upload the data to the server
                    HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                }
            }*/
        }

        /// <summary>
        ///     A function that will add a gem. Usually called when the player collects a gem
        /// </summary>
        public void AddJewel()
        {
            _jewels++;
            jewelsText.text = "Jewels: " + _jewels;
        }

        /// <summary>
        /// This will store the amount of chests that were added in this level
        /// </summary>
        private int _chestsAddedInThisLevel = 0;
        
        /// <summary>
        /// A function that will simply add a chest. Usually called when a player collects a chest
        /// that is in level 2 or level 5
        /// </summary>
        public void AddChest()
        {
            //Adding a chest
            _chests++;
            
            // Adding 1 to the chest added in this level
            _chestsAddedInThisLevel++;
            
            //Setting the chest text to display the number of chests that the player has collected
            chestText.text = "Chests: " + _chests;
            
            //Just adding +1 to the relevant PlayerChest
            PlayerPrefs.SetInt(_playerChestKey, PlayerPrefs.GetInt(_playerChestKey) + 1);
        }

        /// <summary>
        /// This function will remove a health or a shield from the player when the player has collided
        /// with an enemy. This is a very standard feature that is present in most single player games.
        /// It will also return true if the player is dead
        /// </summary>
        private bool RemoveHealthOrShieldWhenHasCollidedWithEnemyAndReturnTrueIfDead()
        {
            // Creating the local user dictionary variable. The value of this will be assigned
            // by getting its value from the HandleUserDataFromPlayFabServer.LocalUserDataDictionary
            // and then this will be uploaded to the server
            Dictionary<string, object> localUserDictionary = new();
            
            // Making sure that the shield never drops below 0
            if (_shield < 0)
            {
                _shield = 0;
            }
            
            //Checking if the player has a shield
            if (_shield > 0)
            {
                //If the player has a shield, then we will deduct the shield
                _shield--;
                
                // Making sure that the shield never drops below 0
                if (_shield < 0)
                {
                    _shield = 0;
                }

                //Setting the shield to display the shield that the player has collected
                shieldText.text = "Shield: " + _shield;

                //Just adding -1 to the relevant PlayerShield
                PlayerPrefs.SetInt(_playerShieldKey, PlayerPrefs.GetInt(_playerShieldKey) - 1);

                // This will run only if we have collected a shield in this level
                if (_shieldAddedInThisLevel > 0)
                {
                    // Remove 1 from the shield added in this level
                    _shieldAddedInThisLevel--;
                }
                
                // Get the current value of the Local User Dictionary
                localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
                // Check if the local user dictionary is not null
                if (localUserDictionary != null)
                {
                    // We will check if the local user dictionary contains the key "Shield"
                    if (localUserDictionary.ContainsKey("Shield"))
                    {
                        // Setting hte value of the shield to the value of the local user dictionary
                        localUserDictionary["Shield"] = PlayerPrefs.GetInt(_playerShieldKey);
                    
                        // Upload the data to the server
                        HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                    }
                }
            }
            else
            {
                //If health is equal to 4 we will reduce it from the PlayerHealth
                if (_health == 4)
                {
                    //Just adding -1 to the relevant PlayerHealth
                    PlayerPrefs.SetInt(_playerHealthKey, PlayerPrefs.GetInt(_playerHealthKey) - 1);
                    
                    // Get the current value of the Local User Dictionary
                    localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
                    
                    // Check if the local user dictionary is not null
                    if (localUserDictionary != null)
                    {
                        // We will check if the local user dictionary contains the key "ExtraHealth"
                        if (localUserDictionary.ContainsKey("ExtraHealth"))
                        {
                            // Setting hte value of the shield to the value of the local user dictionary
                            localUserDictionary["ExtraHealth"] = PlayerPrefs.GetInt(_playerHealthKey);
                    
                            // Upload the data to the server
                            HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
                        }
                    }
                }
                
                //If the player does not have a shield, then we will deduct the health
                _health--;
                
                // Making sure that the health never drops below 0
                if (_health < 0)
                {
                    _health = 0;
                }
                
                // If we have collected a health in this level
                if (_healthAddedInThisLevel > 0)
                {
                    _healthAddedInThisLevel--;
                }

                //Check if the health drops below 0
                if (_health < 1)
                {
                    Time.timeScale = 0;
                    
                    //If the health drops below 0, then we will return true
                    return true;
                }
                
                //Setting the health to display the health that the player has collected
                healthText.text = "Health: " + _health;
            }

            //Returning false means that the player is still alive
            return false;
        }
        
        /// <summary>
        /// This should be called upon player death. This will remove all of the items that were collected
        /// when the player dies and will also consume items from the player's inventory if needed, as
        /// some of the items that are used in the level are pulled from the player's inventory
        /// </summary>
        private void RemoveAllItemsThatWereCollectedInThisLevelAndConsumeInventoryItems()
        {
            // Setting up a variable for the localUserDictionary
            Dictionary<string, object> localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Removing all the tokens that were collected in this level
            PlayerPrefs.SetInt(_playerTokensKey, PlayerPrefs.GetInt(_playerTokensKey) - _tokensAddedInThisLevel);
            
            // Printing amount of tokens removed
            print("Amount of tokens removed: " + _tokensAddedInThisLevel);

            //Removing all the health that was collected in this level
            PlayerPrefs.SetInt(_playerHealthKey, PlayerPrefs.GetInt(_playerHealthKey) - _healthAddedInThisLevel);
            
            // Printing amount of health removed
            print("Amount of health removed: " + _healthAddedInThisLevel);
            
            //Removing all the shield that was collected in this level
            PlayerPrefs.SetInt(_playerShieldKey, PlayerPrefs.GetInt(_playerShieldKey) - _shieldAddedInThisLevel);
            
            // Printing amount of shield removed
            print("Amount of shield removed: " + _shieldAddedInThisLevel);
            
            //Removing all the chests that was collected in this level
            PlayerPrefs.SetInt(_playerChestKey, PlayerPrefs.GetInt(_playerChestKey) - _chestsAddedInThisLevel);
            
            // Printing amount of chests removed
            print("Amount of chests removed: " + _chestsAddedInThisLevel);
            
            /*
             * If the health is 4 at the time of death, we will need to deduct the health from the server
             * as well, as the health will be consumed from the player's inventory
             */
            if (_health == 4)
            {
                // Checking if the LocalUserDictionaryValue is not null
                if (localUserDictionary != null)
                {
                    // We will check if the local user dictionary contains the key "ExtraHealth"
                    if (localUserDictionary.ContainsKey("ExtraHealth"))
                    {
                        // Getting the value of the health from the local user dictionary
                        var extraHealthValue = Convert.ToInt32(localUserDictionary["ExtraHealth"]);
                        
                        // Setting hte value of the shield to the value of the local user dictionary
                        localUserDictionary["ExtraHealth"] = extraHealthValue - 1;
                    }
                }
            }
            
            /*
             * If the shield is more than 0 at the time of death, we will need to deduct the shield from the server
             * as well, as the shield will be consumed from the player's inventory
             */
            if (_shield > 0)
            {
                // Checking if the LocalUserDictionaryValue is not null
                if (localUserDictionary != null)
                {
                    // We will check if the local user dictionary contains the key "Shield"
                    if (localUserDictionary.ContainsKey("Shield"))
                    {
                        // Getting the value of the shield from the local user dictionary
                        var shieldValue = Convert.ToInt32(localUserDictionary["Shield"]);
                        
                        // Setting hte value of the shield to the value of the local user dictionary
                        localUserDictionary["Shield"] = shieldValue - 1;
                    }
                }
            }
            
            // Upload the data to the server
            HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
        }

        /// <summary>
        /// This should run when the player dies, and is usually reserved for situations where we need
        /// to restart the level after the upload to the server is fully complete
        /// </summary>
        /// <returns></returns>
        private IEnumerator
            Routine_RemoveAllItemsThatWereCollectedInThisLevelAndConsumeInventoryItemsAndRestartCurrentLevel()
        {
            // Setting up a variable for the localUserDictionary
            Dictionary<string, object> localUserDictionary = HandleUserDataFromPlayFabServer.LocalUserDataDictionary;
            
            //Removing all the tokens that were collected in this level
            PlayerPrefs.SetInt(_playerTokensKey, PlayerPrefs.GetInt(_playerTokensKey) - _tokensAddedInThisLevel);
            
            // Printing amount of tokens removed
            print("Amount of tokens removed: " + _tokensAddedInThisLevel);

            //Removing all the health that was collected in this level
            PlayerPrefs.SetInt(_playerHealthKey, PlayerPrefs.GetInt(_playerHealthKey) - _healthAddedInThisLevel);
            
            // Printing amount of health removed
            print("Amount of health removed: " + _healthAddedInThisLevel);
            
            //Removing all the shield that was collected in this level
            PlayerPrefs.SetInt(_playerShieldKey, PlayerPrefs.GetInt(_playerShieldKey) - _shieldAddedInThisLevel);
            
            // Printing amount of shield removed
            print("Amount of shield removed: " + _shieldAddedInThisLevel);
            
            //Removing all the chests that was collected in this level
            PlayerPrefs.SetInt(_playerChestKey, PlayerPrefs.GetInt(_playerChestKey) - _chestsAddedInThisLevel);
            
            // Printing amount of chests removed
            print("Amount of chests removed: " + _chestsAddedInThisLevel);
            
            /*
             * If the health is 4 at the time of death, we will need to deduct the health from the server
             * as well, as the health will be consumed from the player's inventory
             */
            if (_health == 4)
            {
                // Deducting the health from player prefs
                PlayerPrefs.SetInt(_playerHealthKey, PlayerPrefs.GetInt(_playerHealthKey) - 1);
                
                // If the player prefs value is less than 0, we will change it to zero
                if (PlayerPrefs.GetInt(_playerHealthKey) < 0)
                {
                    PlayerPrefs.SetInt(_playerHealthKey, 0);
                }
                
                // Checking if the LocalUserDictionaryValue is not null
                if (localUserDictionary != null)
                {
                    // We will check if the local user dictionary contains the key "ExtraHealth"
                    if (localUserDictionary.ContainsKey("ExtraHealth"))
                    {
                        // Getting the value of the health from the local user dictionary
                        var extraHealthValue = Convert.ToInt32(localUserDictionary["ExtraHealth"]);

                        // We will only deduct the health if the health value is more than 0
                        if (extraHealthValue > 0)
                        {
                            // Setting hte value of the shield to the value of the local user dictionary
                            localUserDictionary["ExtraHealth"] = extraHealthValue - 1;
                        }
                        else
                        {
                            Debug.LogWarning("The health value is less than 0 in the local user dictionary" +
                                             " so we can't deduct the health from the server");
                        }
                    }
                }
            }
            
            /*
             * If the shield is more than 0 at the time of death, we will need to deduct the shield from the server
             * as well, as the shield will be consumed from the player's inventory
             */
            if (_shield > 0)
            {
                // Deducting the shield from player prefs
                PlayerPrefs.SetInt(_playerShieldKey, PlayerPrefs.GetInt(_playerShieldKey) - 1);
                
                // If the player prefs value is less than 0, we will change it to zero
                if (PlayerPrefs.GetInt(_playerShieldKey) < 0)
                {
                    PlayerPrefs.SetInt(_playerShieldKey, 0);
                }
                
                // Checking if the LocalUserDictionaryValue is not null
                if (localUserDictionary != null)
                {
                    // We will check if the local user dictionary contains the key "Shield"
                    if (localUserDictionary.ContainsKey("Shield"))
                    {
                        // Getting the value of the shield from the local user dictionary
                        var shieldValue = Convert.ToInt32(localUserDictionary["Shield"]);

                        // We will only deduct the shield if the shield value is more than 0
                        if (shieldValue > 0)
                        {
                            // Setting hte value of the shield to the value of the local user dictionary
                            localUserDictionary["Shield"] = shieldValue - 1;
                        }
                        else
                        {
                            Debug.LogWarning("The shield value is less than 0 in the local user dictionary" +
                                             " so we can't deduct the shield from the server");
                        }
                    }
                }
            }
            
            // creating a new wait for seconds real time
            WaitForSecondsRealtime waitingForRealTimeSeconds = new WaitForSecondsRealtime(0.1f);
            
            // We will wait while the sound is playing
            while (SoundManager.Instance.InGameSoundsAudioSource.isPlaying == true)
            {
                yield return waitingForRealTimeSeconds;
            }
            
            // Upload the data to the server
            HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServerAndRestartTheCurrentScene();

            yield break;
        }
        
        /// <summary>
        /// This function will remove the items that were collected in this level. This will be called
        /// whenever the user restarts the level or goes to the main menu. We will have to use this function
        /// whenever we need to deduct the items that were collected in this level, but we don't want to
        /// consume the items from the player's inventory
        /// </summary>
        private void RemoveAllItemsThatWereCollectedInThisLevel()
        {
            //Removing all the tokens that were collected in this level
            PlayerPrefs.SetInt(_playerTokensKey, PlayerPrefs.GetInt(_playerTokensKey) - _tokensAddedInThisLevel);
            
            // Printing amount of tokens removed
            print("Amount of tokens removed: " + _tokensAddedInThisLevel);

            //Removing all the health that was collected in this level
            PlayerPrefs.SetInt(_playerHealthKey, PlayerPrefs.GetInt(_playerHealthKey) - _healthAddedInThisLevel);
            
            // Printing amount of health removed
            print("Amount of health removed: " + _healthAddedInThisLevel);
            
            //Removing all the shield that was collected in this level
            PlayerPrefs.SetInt(_playerShieldKey, PlayerPrefs.GetInt(_playerShieldKey) - _shieldAddedInThisLevel);
            
            // Printing amount of shield removed
            print("Amount of shield removed: " + _shieldAddedInThisLevel);
            
            //Removing all the chests that was collected in this level
            PlayerPrefs.SetInt(_playerChestKey, PlayerPrefs.GetInt(_playerChestKey) - _chestsAddedInThisLevel);
            
            // Printing amount of chests removed
            print("Amount of chests removed: " + _chestsAddedInThisLevel);
        }

        #endregion

        #region Player Canvas Related Functions

        /// <summary>
        ///     This function will start the game by setting the values for all the text items in the player canvas
        /// </summary>
        private void SetValuesForAllTextItemsOnStart()
        {
            //Setting the value for the tokens text
            tokensText.text = "Tokens: " + PlayerPrefs.GetInt(_playerTokensKey);

            //Setting the value for the health text. The values will always be 3 at the start of the game
            _health = 3;
            healthText.text = "Health: " + _health;
            
            // Checking if the _playerHealthKey value is more than 0
            if (PlayerPrefs.GetInt(_playerHealthKey) > 0)
            {
                // We will add +1 to the health
                _health += 1;
                healthText.text = "Health: " + _health;
            }

            //Setting the value for the shield text, using the player prefs
            _shield = PlayerPrefs.GetInt(_playerShieldKey);

            // Setting the value for the shield text, when we have more than a certain number of shield
            if (_shield > 0)
            {
                _shield = 1;
                
                Debug.LogWarning("This is where the shield is either added if no shield is present" +
                                 " or the player starts with no shield. If a shield has to be added when there is" +
                                 " no shield in the inventory, then the shield will be added here");
            }

            shieldText.text = "Shield: " + _shield;

            //Setting the value for the gems text
            jewelsText.text = "Jewels: " + _jewels;
            
            //Setting the value for the chest text
            _chests = PlayerPrefs.GetInt(_playerChestKey);
            chestText.text = "Chests: " + _chests;
            
            //Setting the value of the current level text
            currentLevelText.text = SceneManager.GetActiveScene().name;
        }

        [Header("Player Canvas")]
        //Reference to the player canvas
        [SerializeField]
        private GameObject playerCanvas;

        /// <summary>
        ///     These are the texts that will be in the Player Canvas that will let the user know
        ///     how much items they have currently collected
        /// </summary>
        [SerializeField] private TextMeshProUGUI tokensText,
            healthText,
            shieldText,
            jewelsText,
            chestText;

        /// <summary>
        /// This is a reference to the level text that will be displayed in the player canvas
        /// </summary>
        [SerializeField] private TextMeshProUGUI currentLevelText;

        /// <summary>
        ///     These are the variables that will be used to store the values of the tokens, health and shield.
        /// </summary>
        private int _tokens, _health, _shield, _jewels, _chests;

        #endregion

        #region Player Pause Menu

        [Header("Player Pause Menu Related")]
        //Pause Menu Container
        [SerializeField]
        private GameObject pauseMenuContainer;

        /// <summary>
        /// This is a reference to the pause button
        /// </summary>
        [SerializeField] private Button pauseButton;

        /// <summary>
        /// This is the button that will be used to toggle the controls on the pause menu
        /// </summary>
        [SerializeField] private Button toggleControlsButton;

        /// <summary>
        /// This is a reference to the joysticks and the mobile buttons container
        /// </summary>
        [SerializeField] private GameObject mobileButtonsContainer, joystickContainer;

        /// <summary>
        /// This is a reference to the current controls text, which will tell the user exactly what controls
        /// that user is currently using
        /// </summary>
        [SerializeField] private TextMeshProUGUI currentControlsText;

        /// <summary>
        /// Runs when the pause menu button is clicked
        /// </summary>
        public void OnPauseMenuButtonClicked()
        {
            print("Pause menu button was clicked");
            
            //Time scale will be set to zero to pause the game
            Time.timeScale = 0;
            
            //Activating the pause menu here
            pauseMenuContainer.SetActive(true);
            
            // Deactivate the mobile buttons
            joystickAndButtonContainer.SetActive(false);
            
            // Playing the UI button sound
            SoundManager.Instance.PlayInGameUIClicksSound();
        }

        /// <summary>
        /// Runs when the resume button is clicked in the pause menu
        /// </summary>
        public void OnPauseMenuResumeButtonClicked()
        {
            print("Pause menu resume button was clicked");
            
            //Time scale will be set to 1 to resume the game
            Time.timeScale = 1;
            
            //Deactivating the pause menu here
            pauseMenuContainer.SetActive(false);
            
            // Activating the mobile buttons
            joystickAndButtonContainer.SetActive(true);
            
            // Playing the UI button sound
            SoundManager.Instance.PlayInGameUIClicksSound();
        }
        
        /// <summary>
        /// This will run when the restart button is clicked in the pause menu
        /// </summary>
        public void OnPauseMenuRestartButtonClicked()
        {
            print("Pause menu restart button was clicked");
            
            //Time scale will be set to 1 to resume the game
            Time.timeScale = 1;
            
            //Deactivating the pause menu here
            pauseMenuContainer.SetActive(false);
            
            //Restarting the game
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            
            // Playing the UI button sound
            SoundManager.Instance.PlayInGameUIClicksSound();

            // Resetting all of the items that were collected in this level
            RemoveAllItemsThatWereCollectedInThisLevel();
        }
        
        /// <summary>
        /// This runs when the return to main menu button is clicked in the pause menu
        /// </summary>
        public void OnPauseMenuReturnToMainMenuButtonClicked()
        {
            print("Pause menu return to main menu button was clicked");
            
            //Time scale will be set to 1 to resume the game
            Time.timeScale = 1;
            
            //Deactivating the pause menu here
            pauseMenuContainer.SetActive(false);
            
            //Returning to the main menu
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
            
            // Playing the UI button sound
            SoundManager.Instance.PlayInGameUIClicksSound();
            
            // Resetting all of the items that were collected in this level
            RemoveAllItemsThatWereCollectedInThisLevel();
        }
        
        /// <summary>
        /// This will run whenever the Toggle Controls button is clicked
        /// </summary>
        public void OnToggleControlsButtonClicked()
        {
            // Playing the UI button sound
            SoundManager.Instance.PlayInGameUIClicksSound();
            
            // Getting the value of the key
            int mobileButtonsContainerKeyValue = 
                PlayerPrefs.GetInt(HandleConstValuesStaticObjectsAndKeys.MobileButtonsContainerKey);
            
            // Changing the value of the key based on the current value of the key
            mobileButtonsContainerKeyValue = mobileButtonsContainerKeyValue == 1 ? 0 : 1;
            
            // Setting the value of the key
            PlayerPrefs.SetInt(HandleConstValuesStaticObjectsAndKeys.MobileButtonsContainerKey, 
                mobileButtonsContainerKeyValue);
            
            // Change the value of 'LastSelectedMobileControls' of the local user dictionary to
            // the new value and upload the data to the server
            if (HandleUserDataFromPlayFabServer.LocalUserDataDictionary.ContainsKey("LastSelectedMobileControls"))
            {
                HandleUserDataFromPlayFabServer.LocalUserDataDictionary["LastSelectedMobileControls"] 
                    = mobileButtonsContainerKeyValue;
                HandleUserDataFromPlayFabServer.Instance.UploadLocalDictionaryToServer();
            }

            // Setting the player mobile container key and the controls
            SetPlayerMobileContainerKeyAndControls();
        }
        
        /// <summary>
        /// This will set the player mobile container key and the controls
        /// </summary>
        private void SetPlayerMobileContainerKeyAndControls()
        {
            // Checking if the key exists
            if (PlayerPrefs.HasKey(HandleConstValuesStaticObjectsAndKeys.MobileButtonsContainerKey) == false)
            {
                //If the key does not exist, then we will set the key to the mobile buttons container
                PlayerPrefs.SetInt(HandleConstValuesStaticObjectsAndKeys.MobileButtonsContainerKey, 1);
            }
            
            // Getting the value of the key
            int mobileButtonsContainerKeyValue = 
                PlayerPrefs.GetInt(HandleConstValuesStaticObjectsAndKeys.MobileButtonsContainerKey);
            
            // Activating the mobile buttons container or the joystick based on the value of the key
            if (mobileButtonsContainerKeyValue == 1)
            {
                // Mobile buttons container will be activated
                mobileButtonsContainer.SetActive(true);
                
                //We will then deactivate the joystick container
                joystickContainer.SetActive(false);
                
                //Setting the current controls text to display the current controls
                currentControlsText.text = "Current Controls: Mobile Buttons";
            }
            else
            {
                // Mobile buttons container will be deactivated
                mobileButtonsContainer.SetActive(false);
                
                //We will then activate the joystick container
                joystickContainer.SetActive(true);
                
                //Setting the current controls text to display the current controls
                currentControlsText.text = "Current Controls: Joystick";
            }
        }

        #endregion
        
        #region Player Keys

        /// <summary>
        ///     This will store the value of the key that will be used to modify the PlayerTokens
        /// </summary>
        private string _playerTokensKey = StoreMenuCanvas.PlayerTokensKeyReference;

        /// <summary>
        ///     This will store the value of the key that will be used to modify the PlayerHealth
        /// </summary>
        private string _playerHealthKey = StoreMenuCanvas.HealthKey;

        /// <summary>
        ///     This will store the value of the key that will be used to modify the PlayerShield
        /// </summary>
        private string _playerShieldKey = StoreMenuCanvas.ShieldKey;

        /// <summary>
        /// This will store the value of the key that will be used to store the value of the chest
        /// key
        /// </summary>
        private string _playerChestKey = StoreMenuCanvas.ChestKey;

        /// <summary>
        ///     This will set the value of all the keys that need to be accessed at the very start of the game
        /// </summary>
        private void SetValueForKeyItemsAtStart()
        {
            _playerTokensKey = StoreMenuCanvas.PlayerTokensKeyReference;
            _playerHealthKey = StoreMenuCanvas.HealthKey;
            _playerShieldKey = StoreMenuCanvas.ShieldKey;
            _playerChestKey = StoreMenuCanvas.ChestKey;
        }

        #endregion

        #region Related to Collision with Enemies

        /// <summary>
        /// When true, tells the game to not allow this player to receive any damage as the player
        /// has entered an invulnerability state as the player has collided with an enemy.
        /// </summary>
        private bool _isInvulnerable = false;

        /// <summary>
        /// This function will run when the player has collided with an enemy
        /// </summary>
        private void DeductHealthOrShieldWhenHasCollidedWithEnemy()
        {
            //We will not run the code, if the player is in an invulnerable state
            //as indicated by the blinking of the sprite
            if (_isInvulnerable == true) return;

            //Disabling the velocity
            thisRigidbody2D.velocity = Vector2.zero;

            //Reset the input values
            moveInput = Vector2.zero;

            //Apply jump force on the player based on the direction that the sprite
            //is facing
            thisRigidbody2D.AddForce(avatarSpriteRenderer.flipX == false ? new Vector2(5, 10) : 
                    new Vector2(-5, 10), ForceMode2D.Impulse);

            //Starting the routine to take damage and disable input
            StartCoroutine(Routine_TakeDamageAndDisableInput());
        }

        /// <summary>
        /// This function will disable the input for a while as the player has taken damage
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_TakeDamageAndDisableInput()
        {
            //Remove a health or a shield from the player when the player has collided
            //with an enemy. This is a very standard feature that is present in most single player games
            bool isDead = RemoveHealthOrShieldWhenHasCollidedWithEnemyAndReturnTrueIfDead();
            
            //If the player is dead we will not continue the rest of the function
            if (isDead == true)
            {
                print("Main Character Has Died");
                
                // Playing the death sound
                SoundManager.Instance.PlayResetLevelSound();
                
                //Disabling the pause menu
                pauseButton.gameObject.SetActive(false);
                
                // Remove the items that were collected in this level
                RemoveAllItemsThatWereCollectedInThisLevelAndConsumeInventoryItems();
                
                //This is a wait for seconds real time variable that will be used to wait for 0.2 seconds
                WaitForSecondsRealtime waitForSecondsRealtime = new WaitForSecondsRealtime(0.2f);
                
                //Disabling the input
                _disableInput = true;
                
                //Disabling the sprite renderer
                avatarSpriteRenderer.enabled = false;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Enabling the sprite renderer
                avatarSpriteRenderer.enabled = true;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Disabling the sprite renderer
                avatarSpriteRenderer.enabled = false;
                _disableInput = false;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Enabling the sprite renderer
                avatarSpriteRenderer.enabled = true;
                //Disabling the sprite renderer
                avatarSpriteRenderer.enabled = false;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Enabling the sprite renderer
                avatarSpriteRenderer.enabled = true;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Disabling the sprite renderer
                avatarSpriteRenderer.enabled = false;
                _disableInput = false;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Enabling the sprite renderer
                avatarSpriteRenderer.enabled = true;
                //Disabling the sprite renderer
                avatarSpriteRenderer.enabled = false;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Enabling the sprite renderer
                avatarSpriteRenderer.enabled = true;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Disabling the sprite renderer
                avatarSpriteRenderer.enabled = false;
                _disableInput = false;
                //Waiting for 0.2 seconds
                yield return waitForSecondsRealtime;
                //Enabling the sprite renderer
                avatarSpriteRenderer.enabled = true;
                
                // Try to load an ad here
                AdsManagement.Instance.DisplayInterstitialAds();
                
                //Waiting for 1.5 seconds before restarting the game
                yield return new WaitForSecondsRealtime(1.5f);
                
                //Restarting the game
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                
                yield break;
            }
            
            // Playing the sound for taking damage
            SoundManager.Instance.PlayHurtSound();

            //A new wait for seconds
            WaitForSeconds waitForZeroPointTwoSeconds = new WaitForSeconds(0.2f);

            //Disabling the input
            _disableInput = true;

            //Telling the game that the player is in an invulnerable state
            _isInvulnerable = true;

            //Disabling the sprite renderer
            avatarSpriteRenderer.enabled = false;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Enabling the sprite renderer
            avatarSpriteRenderer.enabled = true;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Disabling the sprite renderer
            avatarSpriteRenderer.enabled = false;
            _disableInput = false;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Enabling the sprite renderer
            avatarSpriteRenderer.enabled = true;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Disabling the sprite renderer
            avatarSpriteRenderer.enabled = false;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Enabling the sprite renderer
            avatarSpriteRenderer.enabled = true;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Disabling the sprite renderer
            avatarSpriteRenderer.enabled = false;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Enabling the sprite renderer
            avatarSpriteRenderer.enabled = true;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Disabling the sprite renderer
            avatarSpriteRenderer.enabled = false;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Enabling the sprite renderer
            avatarSpriteRenderer.enabled = true;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Disabling the sprite renderer
            avatarSpriteRenderer.enabled = false;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Enabling the sprite renderer
            avatarSpriteRenderer.enabled = true;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Disabling the sprite renderer
            avatarSpriteRenderer.enabled = false;
            //Waiting for 0.2 seconds
            yield return waitForZeroPointTwoSeconds;
            //Enabling the sprite renderer
            avatarSpriteRenderer.enabled = true;

            //Telling the game that the player is no longer in an invulnerable state
            _isInvulnerable = false;
        }

        #endregion

        #region Related to Visibility of Joystick and Buttons

        [Header("Related to Visibility of Joystick and Buttons")]
        //Reference to the joystick and button container
        [SerializeField] private GameObject joystickAndButtonContainer;
        
        /// <summary>
        /// This will hide the joystick and button container if the game is not running on mobile
        /// </summary>
        private void HideJoyStickAndButtonIfNotOnMobile()
        {
            //Checking if the game is not running on mobile
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                //If the game is not running on mobile, then we will disable the joystick and button container
                joystickAndButtonContainer.SetActive(false);
            }
        }

        #endregion

        #region Resetting the Level

        /// <summary>
        /// This will run when the player collides with the restart trigger
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_ResetLevelWhenPlayerCollidesWithRestartTrigger()
        {
            // Waiting for realtime seconds
            WaitForSecondsRealtime waitingForRealTimeSeconds = new WaitForSecondsRealtime(0.2f);
            
            // Displaying the ad
            AdsManagement.Instance.DisplayInterstitialAds();

            // We will wait while the sound is playing
            while (SoundManager.Instance.InGameSoundsAudioSource.isPlaying == true)
            {
                yield return waitingForRealTimeSeconds;
            }

            yield break;
        }

        #endregion

        #region Functions Related to Joystick

        [Header("Functions Related to Joystick")]
        // Floating Joystick
        [SerializeField]
        private FloatingJoystick floatingJoystick;

        #endregion
        
        #region Unity Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            //When a collision with an enemy is detected
            if (other.CompareTag("Enemy"))
            {
                DeductHealthOrShieldWhenHasCollidedWithEnemy();
            }
            
            //When a collision with a restart trigger is detected
            if (other.CompareTag("RestartTrigger"))
            {
                // This will freeze the game
                Time.timeScale = 0.0f;

                // Playing the sound for resetting the level
                SoundManager.Instance.PlayResetLevelSound();

                // Starting the co-routine to reset the level
                StartCoroutine(Routine_ResetLevelWhenPlayerCollidesWithRestartTrigger());
                
                // Resetting all of the items that were collected in this level and restarting the level
                StartCoroutine(Routine_RemoveAllItemsThatWereCollectedInThisLevelAndConsumeInventoryItemsAndRestartCurrentLevel());
            }
            
            //When we collide with a chest
            if (other.CompareTag("Chest"))
            {
                //Adding a chest
                AddChest();
                
                //Destroying the chest
                Destroy(other.gameObject);
            }
        }

        private void Awake()
        {
            GetAllReferences();
        }

        private void Start()
        {
            //Turning the time scale to 1 to make sure that the game is running
            Time.timeScale = 1;
            
            //These functions will be the ones that will be called at the start of the game
            SetValueForKeyItemsAtStart();
            SetValuesForAllTextItemsOnStart();
            LoadAnimatorControllerAccordingToSelectedKey();
            HideJoyStickAndButtonIfNotOnMobile();
            SetPlayerMobileContainerKeyAndControls();
        }

        private void Update()
        {
            SwitchCaseToRunDifferentFunctionsInUpdate();
        }

        private void FixedUpdate()
        {
            HandlePlayerStatesBasedOnVariableValues();
            SwitchCaseToRunDifferentFunctionsInFixedUpdate();

            //Handling the camera movement
            MoveCameraWithPlayerOnlyIfOffsetFromCenterHasExceeded();
        }

        #endregion

        #region References to Objects, Scripts and Components

        [Header("Reference to Objects, Scripts and Components")]
        //Reference to rigidbody
        [SerializeField]
        private Rigidbody2D thisRigidbody2D;

        /// <summary>
        ///     Reference to the animator component of the player character
        /// </summary>
        [SerializeField] private Animator avatarAnimator;

        /// <summary>
        ///     Reference to the avatar sprite renderer
        /// </summary>
        [SerializeField] private SpriteRenderer avatarSpriteRenderer;

        /// <summary>
        ///     Reference to the circle collider 2D component of the player character
        /// </summary>
        [SerializeField] private CircleCollider2D thisCircleCollider2D;

        /// <summary>
        ///     This is a reference to the main camera that is in the scene
        /// </summary>
        [SerializeField] private UnityEngine.Camera mainCameraReference;

        /// <summary>
        ///     This function will be used to get the components and set the references to the objects and scripts
        /// </summary>
        private void GetAllReferences()
        {
            mainCameraReference = UnityEngine.Camera.main;
        }

        #endregion

        #region Player States

        /// <summary>
        ///     These are the states that the player can be in
        /// </summary>
        public enum PlayerStates
        {
            Idle,
            Move,
            OffGround
        }

        [Header("Player States Functions")]
        //Player states variable
        public PlayerStates currentPlayerState;

        /// <summary>
        ///     We will use this variable to store the input that the player gives to the game
        /// </summary>
        [SerializeField] private Vector2 moveInput;

        /// <summary>
        ///     When this variable is true, it tells the game that the player is on the ground
        /// </summary>
        [SerializeField] private bool isOnGround = true;

        /// <summary>
        ///     A function that will handle the player states based on the variable values that will be set elsewhere.
        ///     For example, if the _moveInput variable is not equal to 0, then the player state will be set to Move.
        ///     This will be called in the FixedUpdate function
        /// </summary>
        private void HandlePlayerStatesBasedOnVariableValues()
        {
            //This runs when the player is off the ground
            if (isOnGround == false)
            {
                currentPlayerState = PlayerStates.OffGround;
                return;
            }

            //If the moveInput value is not zero, it means that the player is moving
            if (moveInput.x != 0)
            {
                currentPlayerState = PlayerStates.Move;
                return;
            }

            //If the moveInput value is zero, it means that the player is idle
            currentPlayerState = PlayerStates.Idle;
        }

        /// <summary>
        ///     This function will run in the FixedUpdate(). It will tell the game which functions to run based on
        ///     the value of the currentStates variable
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void SwitchCaseToRunDifferentFunctionsInFixedUpdate()
        {
            switch (currentPlayerState)
            {
                case PlayerStates.Idle:
                    ApplyJumpForceToThePlayerIfAllowed();
                    MovePlayerUsingRigidbodyVelocity();
                    ShootCircleCastToCheckIfPlayerIsOnTheGround();
                    ChangeValueOfIsOnGroundVariableIfPlayerIsOnTheGround();
                    PlayIdleAnimation();
                    break;
                case PlayerStates.Move:
                    ApplyJumpForceToThePlayerIfAllowed();
                    MovePlayerUsingRigidbodyVelocity();
                    ChangeDirectionOfSpriteRendererBasedOnMovementDirection();
                    ShootCircleCastToCheckIfPlayerIsOnTheGround();
                    ChangeValueOfIsOnGroundVariableIfPlayerIsOnTheGround();
                    PlayMovementAnimation();
                    break;
                case PlayerStates.OffGround:
                    MovePlayerUsingRigidbodyVelocity();
                    ChangeDirectionOfSpriteRendererBasedOnMovementDirection();
                    ShootCircleCastToCheckIfPlayerIsOnTheGround();
                    ChangeValueOfIsOnGroundVariableIfPlayerIsOnTheGround();
                    PlayOffGroundAnimationBasedOnYVelocity();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        ///     This function will run in the Update() function. It will tell the game which functions to run based on
        ///     the value of the currentStates variable
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void SwitchCaseToRunDifferentFunctionsInUpdate()
        {
            switch (currentPlayerState)
            {
                case PlayerStates.Idle:
                    CheckForJumpInput();
                    SetMoveInput();
                    break;
                case PlayerStates.Move:
                    CheckForJumpInput();
                    SetMoveInput();
                    break;
                case PlayerStates.OffGround:
                    SetMoveInput();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion
    }
}