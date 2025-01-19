using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using System.Text;
using AppleAuth.Native;
using AppleAuth.Extensions;
using UnityEngine.UI;
using System.Security.Cryptography;
using System;


public class AppleSignInManager : MonoBehaviour
{
    private IAppleAuthManager appleAuthManager;

    public static AppleSignInManager instance;

    [SerializeField] private Button appleLogin;
    //[SerializeField] private Button googleLogin;

    private string AppleUserIdKey = "AppleUserId";


    // Start is called before the first frame update
    void Start()
    {

        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            this.appleAuthManager = new AppleAuthManager(deserializer);
            appleLogin.gameObject.SetActive(true);
            //googleLogin.gameObject.SetActive(false);
        }

    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
        }
    }

    public void LoginWithApple(Action<IAppleIDCredential, string , bool> playfabCallback)
    {
        var rawNonce = GenerateRandomString(32);
        var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName, nonce);


        this.appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {

                    var userId = appleIdCredential.User;
                    PlayerPrefs.SetString(AppleUserIdKey, userId);

                    var email = appleIdCredential.Email;

                    var fullName = appleIdCredential.FullName;

                    var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken,
                                0,
                                appleIdCredential.IdentityToken.Length);

                    var authorizationCode = Encoding.UTF8.GetString(
                                appleIdCredential.AuthorizationCode,
                                0,
                                appleIdCredential.AuthorizationCode.Length);
                    if (playfabCallback != null)  //integration of apple login with firebase
                    {
                        Debug.Log("playfabCallback CALL" +  appleIdCredential.Email);
                        //AlertManager.ShowAlert($"Firebase callback successfully.");
                        playfabCallback(appleIdCredential, rawNonce ,false);
                    }
                    else
                    {
                        Debug.Log("playfabCallback IS NULL" +  appleIdCredential.Email);
                    }

                    Debug.Log($"Sign-in successful! User ID: {userId} , {email}");
                    //AlertManager.ShowAlert($"Sign-in successful! User ID: {userId}");
                    //FirebaseManager.LogEvent("Login_Apple", "Login_User", appleIdCredential.User.ToString());

                }
            },
            error =>
            {
                // Something went wrong
                var authorizationErrorCode = error.GetAuthorizationErrorCode();

            });
    }


    public void QuickLoginApple()
    {
        var quickLoginArgs = new AppleAuthQuickLoginArgs();

        this.appleAuthManager.QuickLogin(
            quickLoginArgs,
            credential =>
            {
                // Received a valid credential!
                // Try casting to IAppleIDCredential or IPasswordCredential

                // Previous Apple sign in credential
                var appleIdCredential = credential as IAppleIDCredential;

                // Saved Keychain credential (read about Keychain Items)
                var passwordCredential = credential as IPasswordCredential;
            },
            error =>
            {
                // Quick login failed. The user has never used Sign in With Apple on your app. Go to login screen
                Debug.LogError("Apple Quick Login Failed");
                //AlertManager.ShowAlert("ERROR: " + error.GetAuthorizationErrorCode().ToString());
            });
    }

    private string GenerateRandomString(int length)
    {
        if (length <= 0)
        {
            throw new Exception("Expected nonce to have positive length");
        }

        const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
        var cryptographicallySecureRandomNumberGenerator = new RNGCryptoServiceProvider();
        var result = string.Empty;
        var remainingLength = length;

        var randomNumberHolder = new byte[1];
        while (remainingLength > 0)
        {
            var randomNumbers = new List<int>(16);
            for (var randomNumberCount = 0; randomNumberCount < 16; randomNumberCount++)
            {
                cryptographicallySecureRandomNumberGenerator.GetBytes(randomNumberHolder);
                randomNumbers.Add(randomNumberHolder[0]);
            }

            for (var randomNumberIndex = 0; randomNumberIndex < randomNumbers.Count; randomNumberIndex++)
            {
                if (remainingLength == 0)
                {
                    break;
                }

                var randomNumber = randomNumbers[randomNumberIndex];
                if (randomNumber < charset.Length)
                {
                    result += charset[randomNumber];
                    remainingLength--;
                }
            }
        }

        return result;
    }

    private string GenerateSHA256NonceFromRawNonce(string rawNonce)
    {
        var sha = new SHA256Managed();
        var utf8RawNonce = Encoding.UTF8.GetBytes(rawNonce);
        var hash = sha.ComputeHash(utf8RawNonce);

        var result = string.Empty;
        for (var i = 0; i < hash.Length; i++)
        {
            result += hash[i].ToString("x2");
        }

        return result;
    }

}
