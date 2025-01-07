using System;
using System.Collections;
using Game_Designer_Online.Scripts.Main_Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Designer_Online.Scripts.Do_Not_Destroy_Scripts
{
    /// <summary>
    /// This is the script that will handle the boosters. This script will be attached to the
    /// BoosterHandler GameObject and will persist through all the scenes. Use this script if you wish
    /// to activate or deactivate the boosters that will be in the game. Since all boosters will last for
    /// 5 minutes, we will store the time inside of a variable. We will also make use of PlayerPrefs and update
    /// it to make sure that the booster stays active or not between scenes. We also want to keep the booster
    /// active even if the player closes the game and opens it again. We will use the System.DateTime class to
    /// get the current time and then use another variable to get the time 5 minutes from now. We will then
    /// compare the current time with the time 10 minutes from now to see if the booster is still active or not.
    /// </summary>
    public class BoosterHandler : MonoBehaviour
    {
        #region Booster Activation Functions

        /// <summary>
        /// This boolean will activate the booster
        /// </summary>
        [SerializeField] private bool activate2XBooster, activate3XBooster;
        
        /// <summary>
        /// This function will activate the 2x jewel booster
        /// </summary>
        public void Activate2XBooster()
        {
            //We will only activate the booster if a booster is not already activated
            if (PlayerPrefs.GetInt(BoosterActiveKey) != 0)
            {
                print("A booster is already active");
                return;
            }
            
            print("Activating the 2x jewel booster");
            
            //Setting the key value to 1 to indicate that the 2x jewel booster is active
            PlayerPrefs.SetInt(BoosterActiveKey, 1);
            
            /*//Setting the time at which the booster was activated
            PlayerPrefs.SetString(LastBoosterActivationTimeKey, DateTime.Now.ToString());
            
            //Setting the time at which the booster will be deactivated
            PlayerPrefs.SetString(CurrentBoosterDeactivationTimeKey, DateTime.Now.AddMinutes(1).ToString());*/
            
            //Setting the seconds remaining
            PlayerPrefs.SetInt(CurrentActivatedBoosterSecondsRemainingKey, 180);
            
            //Starting the coroutine that will deplete the booster
            StartCoroutine(Routine_StartDepletingSecondsFromActiveBooster());
        }
        
        /// <summary>
        /// A function that will activate the 3x jewel booster
        /// </summary>
        public void Activate3XBooster()
        {
            //We will only activate the booster if a booster is not already activated
            if (PlayerPrefs.GetInt(BoosterActiveKey) != 0)
            {
                print("A booster is already active");
                return;
            }
            
            print("Activating the 3x jewel booster");
            
            //Setting the key value to 2 to indicate that the 3x jewel booster is active
            PlayerPrefs.SetInt(BoosterActiveKey, 2);
            
            /*//Setting the time at which the booster was activated
            PlayerPrefs.SetString(LastBoosterActivationTimeKey, DateTime.Now.ToString());
            
            //Setting the time at which the booster will be deactivated
            PlayerPrefs.SetString(CurrentBoosterDeactivationTimeKey, DateTime.Now.AddMinutes(1).ToString());*/
            
            //Setting the seconds remaining
            PlayerPrefs.SetInt(CurrentActivatedBoosterSecondsRemainingKey, 120);
            
            //Starting the coroutine that will deplete the booster
            StartCoroutine(Routine_StartDepletingSecondsFromActiveBooster());
        }

        #endregion

        #region Functions to deplete the booster values

        /// <summary>
        /// This function will start depleting the booster that is active right now.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_StartDepletingTheCurrentlyActiveBooster()
        {
            //Running this while one of the boosters is active
            while (PlayerPrefs.GetInt(BoosterActiveKey) != 0)
            {
                //Getting the second remaining by comparing the current time with the time at which the booster
                //will deactivate
                var secondsRemaining = (DateTime.Parse(PlayerPrefs.GetString(CurrentBoosterDeactivationTimeKey)) -
                                        DateTime.Now).TotalSeconds;
                
                //Rounding off the seconds
                secondsRemaining = Math.Round(secondsRemaining, 0);
            
                //If the seconds remaining is zero, we will deactivate the booster
                if (secondsRemaining <= 0)
                {
                    DeactivateBooster();
                    yield break;
                }
                
                print("Seconds remaining: " + secondsRemaining);

                yield return new WaitForSeconds(1.0f);
            }
        }

        /// <summary>
        /// This function will start depleting the booster that is activate right now
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_StartDepletingSecondsFromActiveBooster()
        {
            //Running this while one of the boosters is active
            while (PlayerPrefs.GetInt(BoosterActiveKey) != 0)
            {
                //Getting the second remaining by comparing the current time with the time at which the booster
                //will deactivate
                int secondsRemaining = PlayerPrefs.GetInt(CurrentActivatedBoosterSecondsRemainingKey);

                //If the seconds remaining is zero, we will deactivate the booster
                if (secondsRemaining <= 0)
                {
                    DeactivateBooster();
                    yield break;
                }
                
                print("Seconds remaining: " + secondsRemaining);

                yield return new WaitForSeconds(1.0f);
                
                //Decreasing the seconds remaining
                secondsRemaining--;
                
                //Setting the seconds remaining
                PlayerPrefs.SetInt(CurrentActivatedBoosterSecondsRemainingKey, secondsRemaining);
            }
        }

        /// <summary>
        /// A function that will deactivate the booster by resetting all the values of the keys and making sure
        /// that the booster is not active anymore.
        /// </summary>
        private void DeactivateBooster()
        {
            print("A booster was depleted");
            
            //Setting the key value to 0 to indicate that the booster is not active
            PlayerPrefs.SetInt(BoosterActiveKey, 0);
            
            //Setting the time at which the booster was activated
            PlayerPrefs.SetString(LastBoosterActivationTimeKey, "");
            
            //Setting the time at which the booster will be deactivated
            PlayerPrefs.SetString(CurrentBoosterDeactivationTimeKey, "");
            
            //Setting the seconds remaining to 0
            PlayerPrefs.SetInt(CurrentActivatedBoosterSecondsRemainingKey, 0);
            
            //Checking if we are in the Main Menu Scene
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                //We will try to get the StoreMenuCanvas
                var storeMenuCanvas = FindObjectOfType<StoreMenuCanvas>();
                
                //We will now try to disable the booster buttons if required
                if (storeMenuCanvas != null)
                {
                    print("Since we have found the storeMenuCanvas, we will now disable " +
                          "the booster buttons");
                    
                    storeMenuCanvas.EnableOrDisableBoosterButtonsBasedOnAFewConditions();
                }
            }
        }

        #endregion

        #region Booster Key Handling

        /// <summary>
        /// When the value of the key is 0, no booster is active.
        /// If the value of the key is 1, the 2x jewel booster will be active.
        /// If the value of the key is 2, the 3x jewel booster will be active.
        /// </summary>
        public const string BoosterActiveKey = "BoosterActiveKey";
        
        /// <summary>
        /// This will store the time at which the booster was activated
        /// </summary>
        public const string LastBoosterActivationTimeKey = "LastBoosterActivationTimeKey";
        
        /// <summary>
        /// This will tell the game the time at which the booster has to be deactivated
        /// </summary>
        public const string CurrentBoosterDeactivationTimeKey = "CurrentBoosterDeactivationTimeKey";
        
        /// <summary>
        /// This tells the game how many seconds are remaining for the booster to be deactivated
        /// </summary>
        public const string CurrentActivatedBoosterSecondsRemainingKey 
            = "CurrentActivatedBoosterSecondsRemainingKey";

        /// <summary>
        /// This function will only be called in the HandleBoosterKey() function if a booster is already
        /// active according to the key value.
        /// </summary>
        private void PerformAllFunctionsRequiredToHandleAnAlreadyActiveBooster()
        {
            //Starting the co-routine that will deplete the booster
            StartCoroutine(Routine_StartDepletingSecondsFromActiveBooster());
        }
        
        /// <summary>
        /// This function will be called in the Start() function to check if the key exists or not. If it does exit
        /// we will check if the booster is active or not, and update the time values for that particular booster.
        /// </summary>
        private void HandleTheBoosterKey()
        {
            //If the key exists, we will call a different function to check the current time
            //remaining for the booster and set the time values accordingly. However, the Booster is only active
            //if the key value is 1 or 2. If the key value is 0, the booster is not active.
            if (PlayerPrefs.HasKey(BoosterActiveKey) == true && PlayerPrefs.GetInt(BoosterActiveKey) != 0)
            {
                PerformAllFunctionsRequiredToHandleAnAlreadyActiveBooster();
                return;
            }

            //If the key does not exist, create it and set the value to 0
            if (!PlayerPrefs.HasKey(BoosterActiveKey))
            {
                PlayerPrefs.SetInt(BoosterActiveKey, 0);
                PlayerPrefs.SetInt(CurrentActivatedBoosterSecondsRemainingKey, 0);
            }
        }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            CreateSingleton();
        }

        private void Start()
        {
            HandleTheBoosterKey();
        }

        /*private void Update()
        {
            if (activate2XBooster == true)
            {
                Activate2XBooster();
                activate2XBooster = false;
            }
            
            if (activate3XBooster == true)
            {
                Activate3XBooster();
                activate3XBooster = false;
            }
        }*/

        #endregion

        #region Singleton and Singleton Creation with Don't Destroy On Load

        //Singleton
        public static BoosterHandler Instance { get; private set; }
        
        /// <summary>
        /// A function tht will create the singleton and also apply a DontDestroyOnLoad to the object
        /// </summary>
        private void CreateSingleton()
        {
            //If the instance is not null and is not this, destroy this
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            //Set the instance
            Instance = this;
            
            //Don't destroy this object when loading a new scene
            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}