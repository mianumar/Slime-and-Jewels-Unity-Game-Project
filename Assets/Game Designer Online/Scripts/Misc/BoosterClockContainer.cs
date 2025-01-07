using System;
using System.Collections;
using Game_Designer_Online.Scripts.Do_Not_Destroy_Scripts;
using TMPro;
using UnityEngine;

namespace Game_Designer_Online.Scripts.Misc
{
    /// <summary>
    /// This script is attached to the Prefab called BoosterClockHandler. It will just tell the user
    /// How much booster they have left, if they have activated one
    /// </summary>
    public class BoosterClockContainer : MonoBehaviour
    {
        /// <summary>
        /// This is a reference to the boosterClockText object which is a child of this gameObject
        /// </summary>
        [SerializeField] private TextMeshProUGUI boosterClockText;

        /// <summary>
        /// When true, it will tell the game that the coroutine is running
        /// </summary>
        private bool _routineRunning = false;

        private IEnumerator Routine_UpdateBoosterClockValues()
        {
            //We will return if the routine is already running
            if (_routineRunning == true) yield break;
            
            //Setting the routine to true
            _routineRunning = true;
            
            WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

            //This will run as long as the gameObject is active
            while (gameObject.activeSelf == true)
            {
                if (PlayerPrefs.GetInt(BoosterHandler.BoosterActiveKey) == 0)
                {
                    //Turning off the boosterClockText object
                    boosterClockText.gameObject.SetActive(false);
                    
                    yield return waitForOneSecond;
                    continue;
                }
                
                //Turning on the boosterClockText object
                boosterClockText.gameObject.SetActive(true);

                //Getting the booster that was activated and storing it in a string
                string boosterActivated = PlayerPrefs.GetInt(BoosterHandler.BoosterActiveKey) == 1
                    ? "2X Booster Remaining: "
                    : "3x Booster Remaining: ";
                
                //Getting the time remaining from PlayerPrefs and storing it in a string
                string timeRemaining = TimeSpan.FromSeconds(
                        PlayerPrefs.GetInt(BoosterHandler.CurrentActivatedBoosterSecondsRemainingKey))
                    .ToString(@"hh\:mm\:ss");
                
                //Joining the two strings together
                string boosterClockTextString = boosterActivated + timeRemaining;
                
                //Setting the text of the boosterClockText object
                boosterClockText.text = boosterClockTextString;
                
                yield return waitForOneSecond;
            }
        }

        private void OnEnable()
        {
            StartCoroutine(Routine_UpdateBoosterClockValues());
        }

        private void OnDisable()
        {
            //We will tell the routine to false
            _routineRunning = false;
        }
    }
}