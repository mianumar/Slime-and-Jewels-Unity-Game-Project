using UnityEngine;

namespace Game_Designer_Online.Scripts.Application_Settings
{
    /// <summary>
    /// This should be used to store application settings.
    /// </summary>
    public class ApplicationSettings : MonoBehaviour
    {
        /// <summary>
        /// Stores the screen width and the screen height
        /// </summary>
        [SerializeField] private int screenWidth = 1920,
            screenHeight = 1080;
        
        /// <summary>
        /// Stores the target frame rate
        /// </summary>
        [SerializeField] private int targetFrameRate = 60;
        
        // Start is called before the first frame update
        void Start()
        {
            // Setting the screen resolution and the target frame rate for windows
            if (Application.platform == RuntimePlatform.WindowsPlayer 
                || Application.platform == RuntimePlatform.WindowsEditor)
            {
                Screen.SetResolution(screenWidth, screenHeight, FullScreenMode.Windowed);
                Application.targetFrameRate = targetFrameRate;
            }
            
            // Setting the screen resolution and the target frame rate for android
            if (Application.platform == RuntimePlatform.Android)
            {
                Screen.SetResolution(1280, 720, true);
                Application.targetFrameRate = targetFrameRate;
            }
            
            // Setting the screen resolution and the target frame rate for iOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                Screen.SetResolution(1280, 720, true);
                Application.targetFrameRate = targetFrameRate;
            }
        }
    }
}
