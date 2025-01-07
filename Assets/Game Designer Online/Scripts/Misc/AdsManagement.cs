using System;
using System.Collections;
using Game_Designer_Online.Scripts.Main_Menu;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Game_Designer_Online.Scripts.Misc
{
    /// <summary>
    /// This is the script that will be used to manage the ads in the game.
    /// </summary>
    public class AdsManagement : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener,
        IUnityAdsLoadListener
    {
        #region Interstitial Ads Functions

        /// <summary>
        /// When this turns to 2, it means that we will load the ads or display the ads.
        /// </summary>
        public static int AdsAttempt = 0;
        
        /// <summary>
        /// This is going to be used to display the interstitial ads.
        /// </summary>
        public void DisplayInterstitialAds()
        {
            // We will return if the value of the RemoveAds key is 1. If the value is 1,
            // it means that the player has bought the 'Remove Ads' option
            if (PlayerPrefs.GetInt(StoreMenuCanvas.RemoveAdsKey) == 1)
            {
                Debug.Log("Ads will not be displayed as the player has bought the 'Remove Ads' option.");
                
                return;
            }
            
            // When this is less than 2, we will increase the ads attempt and return.
            /*if (AdsAttempt < 2)
            {
                AdsAttempt++;

                return;
            }*/
            
            // Using a directive to check if we are on android
            #if UNITY_ANDROID
            
            Advertisement.Load("Interstitial_Android", this);
            
            #endif

            #if UNITY_IOS
            
            Advertisement.Load("Interstitial_IOS", this);
            
            #endif
        }

        #endregion
        
        #region Singleton
        
        /// <summary>
        /// This is going to be the singleton for the application.
        /// </summary>
        public static AdsManagement Instance { get; private set; }
        
        /// <summary>
        /// This is going to be the singleton for the AdsManagement.
        /// </summary>
        private void CreateSingleWithDontDestroyOnLoad()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region Unity Ads Initialization

        /// <summary>
        /// This is going to be the game id for the android.
        /// </summary>
        private readonly string _androidGameId = "5545056";
        
        /// <summary>
        /// This is going to be the game id for the ios.
        /// </summary>
        private readonly string _iosGameId = "5545057";

        /// <summary>
        /// This will be used to initialize the ads.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_InitAds()
        {
            // We will run this as long as the ads are not initialized.
            while (Advertisement.isInitialized == false)
            {
                // Using a directive to check if the platform is android.
                #if UNITY_ANDROID
                Advertisement.Initialize(_androidGameId, false, this);
                #endif
                
                // Using a directive to check if the platform is ios.
                #if UNITY_IOS
                Advertisement.Initialize(_iosGameId, true, this);
                #endif
                
                // Waiting for 1 second.
                yield return new WaitForSeconds(1);
            }
        }
        
        public void OnInitializationComplete()
        {
            print("Ads are initialized.");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            print("Ads failed due to " + error + " with message " + message);
        }

        #endregion

        #region Ads Display Listners

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            print("Ads failed to show due to " + error + " with message " + message);
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            print("Ads are starting to show.");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
            print("Ads are clicked.");
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            print("Ads are completed.");
        }

        #endregion

        #region Ads Load Listners

        public void OnUnityAdsAdLoaded(string placementId)
        {
            print("Ads are loaded. Ad loaded was " + placementId);
            
            if (placementId is "Interstitial_Android" or "Interstitial_IOS")
            {
                Advertisement.Show(placementId, this);
                
                // Resetting the ads attempt.
                AdsAttempt = 0;
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            print("Ads failed to load due to " + error + " with message " + message);
        }

        #endregion
        
        #region Unity Functions

        private void Awake()
        {
            CreateSingleWithDontDestroyOnLoad();
            StartCoroutine(Routine_InitAds());
        }

        #endregion
    }
}
