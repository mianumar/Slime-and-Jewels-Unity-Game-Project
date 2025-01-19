using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using Game_Designer_Online.Scripts.Misc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMPro;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEditor.PackageManager;
using AppleAuth.Interfaces;
using System.Text;

namespace Game_Designer_Online.Scripts.Playfab_Related
{
    /// <summary>
    /// This script will contain all the functions required for the login and registration canvas.
    /// It will use PlayFab features to login and register the user.
    /// </summary>
    public class LoginAndRegistrationCanvas : MonoBehaviour
    {
        #region Unity Functions

        private async void Start()
        {
            //Creating the playFab id if the current one is empty
            if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
            {
                PlayFabSettings.TitleId = "E4A61";
            }
            
            // Try catch to initialize the Unity Services
            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName("production");

                await UnityServices.InitializeAsync(options);
                
                print("Initialized Unity Services.");
            }
            catch (Exception exception)
            {
                // An error occurred during initialization.
                print(exception.Message);
            }

            //Add listeners to the login and register menu buttons
            loginButton.onClick.AddListener(OnLoginButtonClicked);
            registerButton.onClick.AddListener(OnRegisterButtonClicked);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
            loginButtonPasswordVisible.onClick.AddListener(OnLoginButtonPasswordVisibleButtonClicked);
            forgotPasswordButton.onClick.AddListener(OnForgotPasswordButtonClicked);
            guestLoginButton.onClick.AddListener(OnGuestLoginButtonClicked);

            //Add listeners to the login menu buttons
            loginMenuBackButton.onClick.AddListener(OnLoginMenuBackButtonClicked);
            loginMenuLoginButton.onClick.AddListener(OnLoginMenuLoginButtonClicked);
            appleLoginButton.onClick.AddListener(AppleLoginBtn);

            //Add listeners to the register menu buttons
            registerMenuBackButton.onClick.AddListener(OnRegisterMenuBackButtonClicked);
            registerMenuRegisterButton.onClick.AddListener(OnRegisterMenuRegisterButtonClicked);
            registerMenuPasswordVisibleButton.onClick.AddListener(OnRegisterMenuPasswordVisibleButtonClicked);
            
            // Add listeners to the forgot password menu buttons
            forgotPasswordSendEmailButton.onClick.AddListener(OnForgotPasswordSendEmailButtonClicked);
            forgotPasswordBackButton.onClick.AddListener(OnForgotPasswordBackButtonClicked);
            forgotPasswordEmailInputField.onValueChanged.AddListener(OnForgotPasswordEmailInputFieldValueChanged);
            
            // Verification Warning Menu I understand button
            verificationWarningMenuIUnderstandButton.onClick.
                AddListener(OnVerificationWarningMenuIUnderstandButtonClicked);
            
            // Adding all the listeners to the resend verification email menu buttons
            resendVerificationEmailMenuResendEmailButton.onClick.
                AddListener(OnResendVerificationEmailMenuResendEmailButtonClicked);
            resendVerificationEmailMenuBackButton.onClick.AddListener(OnResendVerificationEmailMenuBackButtonClicked);
            resendVerificationEmailMenuEmailInputField.onValueChanged.
                AddListener(OnResendVerificationEmailMenuEmailInputFieldValueChanged);
            
            // Adding all the listeners to the guest login warning menu buttons
            guestLoginWarningMenuIUnderstandButton.onClick.
                AddListener(OnGuestLoginWarningMenuIUnderstandButtonClicked);
            guestLoginWarningMenuBackButton.onClick.AddListener(OnGuestLoginWarningMenuBackButtonClicked);

            //Turning off all containers except the login or register menu container
            loginMenuContainer.SetActive(false);
            registerMenuContainer.SetActive(false);
            loginOrRegisterMenuContainer.SetActive(true);
            
            // Handling the IsGuestKey
            SetIsGuestKeyValue();
        }

        #endregion

        #region Register Features for Playfab

        /// <summary>
        /// When this is true, it will tell the game that the game successfully updated the users contact email
        /// </summary>
        private bool _successfullyUpdatedTheUsersContactEmailWhenRegistrationIsSuccessful = false;
        
        /// <summary>
        /// This will turn true when the player data is successfully setup after registration is successful
        /// </summary>
        private bool _successfullySetupPlayerDataAfterRegistrationIsSuccessful = false;
        
        /// <summary>
        /// This will turn true when the player display is successfully setup after registration is successful
        /// </summary>
        private bool _successfullySetupPlayerDisplayAfterRegistrationIsSuccessful = false;

        /// <summary>
        /// This function will try to register the player to playFab using the stored
        /// username, email and password
        /// </summary>
        private void TryToRegisterPlayerToPlayFab()
        {
            print("Trying to register the player");

            //If the _registerMenuUsernameInputFieldTextValue is empty
            if (string.IsNullOrEmpty(_registerMenuUserNameInputFieldTextValue))
            {
                //Display the message on the register menu message text
                StartCoroutine(Routine_RegisterMenuMessageText("Username cannot be empty"));
                return;
            }

            //If the _registerMenuUsernameInputFieldTextValue is less than 6 characters
            if (_registerMenuUserNameInputFieldTextValue.Length < 6)
            {
                //Display the message on the register menu message text
                StartCoroutine(Routine_RegisterMenuMessageText("Username must be at least 6 characters"));
                return;
            }

            //Checking if an invalid character is entered in _registerMenuUsernameInputFieldTextValue
            if (_registerMenuUserNameInputFieldTextValue.Contains("@") == true)
            {
                //Display the message on the register menu message text
                StartCoroutine(Routine_RegisterMenuMessageText("Username cannot contain @"));
                return;
            }

            //Checking if _registerMenuEmailInputFieldTextValue is empty
            if (string.IsNullOrEmpty(_registerMenuEmailInputFieldTextValue))
            {
                //Display the message on the register menu message text
                StartCoroutine(Routine_RegisterMenuMessageText("Email cannot be empty"));
                return;
            }

            //Checking if an invalid character is entered in _registerMenuEmailInputFieldTextValue
            if (_registerMenuEmailInputFieldTextValue.Contains("@") == false)
            {
                //Display the message on the register menu message text
                StartCoroutine(Routine_RegisterMenuMessageText("Invalid Email"));
                return;
            }

            //Checking if _registerMenuPasswordInputFieldTextValue is empty
            if (string.IsNullOrEmpty(_registerMenuPasswordInputFieldTextValue))
            {
                //Display the message on the register menu message text
                StartCoroutine(Routine_RegisterMenuMessageText("Password cannot be empty"));
                return;
            }

            //If the _registerMenuPasswordInputFieldTextValue is less than 6 characters
            if (_registerMenuPasswordInputFieldTextValue.Length < 6)
            {
                //Display the message on the register menu message text
                StartCoroutine(Routine_RegisterMenuMessageText("Password must be at least 6 characters"));
                return;
            }

            //Turning the Buttons on the register menu off
            registerMenuBackButton.interactable = false;
            registerMenuRegisterButton.interactable = false;

            //Creating a request to register the player
            var newUserRegistrationRequest = new RegisterPlayFabUserRequest
            {
                Username = _registerMenuUserNameInputFieldTextValue,
                Email = _registerMenuEmailInputFieldTextValue,
                Password = _registerMenuPasswordInputFieldTextValue,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    GetUserAccountInfo = true,
                },
            };
            
            PlayFabClientAPI.RegisterPlayFabUser(
                newUserRegistrationRequest,
                result =>
                {
                    print("Registration was successful");

                    //Updating the user's contact email id
                    UpdateUsersContactEmailIdWhenRegistrationIsSuccessful(result.PlayFabId);
                    
                    //Setting the user data when the player registers for the first time
                    SetupPlayerDataAfterRegistrationIsSuccessful(result.PlayFabId);
                    
                    //Setting up the player user name based on the display name that the user entered.
                    //This will be used to display the user name in the back-end so we can search for the player
                    //relatively easily, according to the requirements of the client
                    SetupPlayerDisplayAfterRegistrationIsSuccessful(result);

                    //Display the message on the register menu message text
                    StartCoroutine(Routine_RegisterMenuMessageText("Registration was successful! Please wait!"));

                    StartCoroutine(Routine_WaitForAllProcessesToFinishBeforeDisplayingNextMessage());
                },
                error =>
                {
                    //Display the error message on the register menu message text
                    StartCoroutine(Routine_RegisterMenuMessageText(error.ErrorMessage));

                    //Turning the Buttons on the register menu on
                    registerMenuBackButton.interactable = true;
                    registerMenuRegisterButton.interactable = true;

                    try
                    {
                        // Playing the negative sound
                        SoundManager.Instance!.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            );
        }

        /// <summary>
        /// This function will be called after all of the requests above have been performed properly
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_WaitForAllProcessesToFinishBeforeDisplayingNextMessage()
        {
            // New wait for seconds timer
            WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);

            // This while loop will run until all of these processes are finished
            while (_successfullySetupPlayerDataAfterRegistrationIsSuccessful == false || 
                   _successfullyUpdatedTheUsersContactEmailWhenRegistrationIsSuccessful == false ||
                   _successfullySetupPlayerDisplayAfterRegistrationIsSuccessful == false)
            {
                yield return waitForOneSecond;
            }
            
            print("All registration processes were complete");

            // Turning the verification warning menu on
            verificationWarningMenu.SetActive(true);
                    
            // Turning the register menu off
            registerMenuContainer.SetActive(false);
        }

        #endregion

        #region On Register Contact Email Update Functions + Send Verification Email

        /// <summary>
        /// This function will be called after a user has registered and will be used to verify the user's email
        /// as an event will take place on the PlayFab server which will prompt PlayFab to send a verification
        /// email to the user
        /// </summary>
        private void UpdateUsersContactEmailIdWhenRegistrationIsSuccessful(string userPlayFabId)
        {
            print("Trying to send the user a verification email as the registration was successful");

            //Creating a request to update the user's contact email id
            var updateUserContactEmailIdRequest = new AddOrUpdateContactEmailRequest
            {
                EmailAddress = _registerMenuEmailInputFieldTextValue
            };

            try
            {
                PlayFabClientAPI.AddOrUpdateContactEmail(
                    updateUserContactEmailIdRequest,
                    result =>
                    {
                        print("Successfully sent the user a verification email");
                        _successfullyUpdatedTheUsersContactEmailWhenRegistrationIsSuccessful = true;
                    },
                    error =>
                    {
                        print("Failed to send the user a verification email");
                        UpdateUsersContactEmailIdWhenRegistrationIsSuccessful("");
                    }
                );
            }
            catch (Exception e)
            {
                print(e.Message);
                UpdateUsersContactEmailIdWhenRegistrationIsSuccessful("");
            }
        }

        #endregion

        #region On Register Cloud Script to Setup Player Data

        /// <summary>
        /// This function will setup the user data using the cloud script
        /// </summary>
        /// <param name="userPlayFabId"></param>
        private void SetupPlayerDataAfterRegistrationIsSuccessful(string userPlayFabId)
        {
            print("Trying to setup the player data after registration is successful");

            //This is the execute cloud script request that will setup the user data
            var setupPlayerDataRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "setupUserData",
                FunctionParameter = new { },
                GeneratePlayStreamEvent = true
            };

            try
            {
                //Calling the cloud script to setup the user data
                PlayFabClientAPI.ExecuteCloudScript(
                    setupPlayerDataRequest,
                    result =>
                    {
                        if (result.FunctionResult == null)
                        {
                            print("Failed to setup the player data");
                        
                            // Try to setup the player data again
                            SetupPlayerDataAfterRegistrationIsSuccessful("");
                        
                            return;
                        }
                    
                        //Using Newtonsoft.Json to convert the result into a dictionary
                        var resultDictionary = 
                            JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());
                    
                        //Getting the data from the resultDictionary
                        print(resultDictionary["message"]);
                        
                        // Setting the _successfullySetupPlayerDataAfterRegistrationIsSuccessful to true
                        _successfullySetupPlayerDataAfterRegistrationIsSuccessful = true;
                    },
                    error =>
                    {
                        print("Failed to setup the player data");
                    
                        // Try to setup the player data again
                        SetupPlayerDataAfterRegistrationIsSuccessful("");
                    }
                );
            }
            catch (Exception e)
            {
                print(e.Message);
                SetupPlayerDataAfterRegistrationIsSuccessful("");
            }
        }

        #endregion

        #region On Register Setup Player Display Name

        /// <summary>
        /// This will be called after the player has successfully registered. It will help us
        /// change the display name of the user to the username that was given during registration
        /// </summary>
        /// <param name="result"></param>
        private void SetupPlayerDisplayAfterRegistrationIsSuccessful(RegisterPlayFabUserResult result)
        {
            //This will be the request that will be used to change the display name of the user
            var updatePlayerInformationRequest = new UpdateUserTitleDisplayNameRequest();
            
            //Setting the display name to the username that was given during registration
            updatePlayerInformationRequest.DisplayName = result.Username;

            try
            {
                //Calling the function to update the user's display name
                PlayFabClientAPI.UpdateUserTitleDisplayName(
                    updatePlayerInformationRequest,
                    result =>
                    {
                        _successfullySetupPlayerDisplayAfterRegistrationIsSuccessful = true;
                        print("Successfully updated the user's display name");
                    },
                    error =>
                    {
                        print("Failed to update the user's display name");
                    }
                );
            }
            catch (Exception e)
            {
                print(e.Message);
                SetupPlayerDisplayAfterRegistrationIsSuccessful(result);
            }
        }

        #endregion
        
        #region Login Features for Playfab

        /// <summary>
        /// This function will try to login the player to playFab using the stored email and password
        /// </summary>
        private void TryToLoginPlayerToPlayFab()
        {
            print("Trying to log the player in");

            //If the _loginMenuEmailInputFieldTextValue is empty
            if (string.IsNullOrEmpty(_loginMenuEmailInputFieldTextValue))
            {
                //Display the message on the login menu message text
                StartCoroutine(Routine_LoginMenuMessageText("Email cannot be empty"));
                return;
            }

            //If the _loginMenuPasswordInputFieldTextValue is less than 6 characters
            if (_loginMenuPasswordInputFieldTextValue.Length < 6)
            {
                //Display the message on the login menu message text
                StartCoroutine(Routine_LoginMenuMessageText("Password must be at least 6 characters"));
                return;
            }

            //Checking if an invalid character is entered in _loginMenuEmailInputFieldTextValue
            if (_loginMenuEmailInputFieldTextValue.Contains("@") == false)
            {
                //Display the message on the login menu message text
                StartCoroutine(Routine_LoginMenuMessageText("Invalid Email"));
                return;
            }

            //Checking if _loginMenuPasswordInputFieldTextValue is empty
            if (string.IsNullOrEmpty(_loginMenuPasswordInputFieldTextValue))
            {
                //Display the message on the login menu message text
                StartCoroutine(Routine_LoginMenuMessageText("Password cannot be empty"));
                return;
            }

            //Checking if the password is less than 6 characters
            if (_loginMenuPasswordInputFieldTextValue.Length < 6)
            {
                //Display the message on the login menu message text
                StartCoroutine(Routine_LoginMenuMessageText("Password must be at least 6 characters"));
                return;
            }

            //Turning off the buttons on the login screen
            loginMenuBackButton.interactable = false;
            loginMenuLoginButton.interactable = false;
            forgotPasswordButton.interactable = false;

            //Creating a login with email and password request
            var loginWithEmailAndPasswordRequest = new LoginWithEmailAddressRequest
            {
                Email = _loginMenuEmailInputFieldTextValue,
                Password = _loginMenuPasswordInputFieldTextValue,
                InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                {
                    GetPlayerProfile = true,
                    GetUserAccountInfo = true,
                },
            };


            PlayFabClientAPI.LoginWithEmailAddress(
                loginWithEmailAndPasswordRequest,
                result =>
                {
                    print("Login request sent successfully. Checking verification status!");

                    //Display the message on the login menu message text
                    StartCoroutine(Routine_LoginMenuMessageText("Verifying login details!"));

                    //Loading the next scene here
                    StartCoroutine(Routine_LoadNextSceneOnLoginIfVerified(result));
                },
                error =>
                {
                    //Check which type of error was it
                    var errorType = error.Error;
                    
                    //A string that will be used to display the error message
                    string errorMessageString = "";
                    
                    //Switch case to handle the error type
                    switch (errorType)
                    {
                        case PlayFabErrorCode.UsernameNotAvailable:
                            errorMessageString = "Username is not available";
                            break;
                        case PlayFabErrorCode.InvalidUsername:
                            errorMessageString = "Player With This Username Does Not Exist";
                            break;
                        case PlayFabErrorCode.InvalidPassword:
                            errorMessageString = "You Entered The Incorrect Password";
                            break;
                        case PlayFabErrorCode.UserisNotValid:
                            break;
                        case PlayFabErrorCode.AccountNotFound:
                            errorMessageString = "Username/Account Does Not Exist";
                            break;
                        default:
                            errorMessageString = error.ErrorMessage;
                            break;
                    }

                    //Display the message on the login menu message text
                    StartCoroutine(Routine_LoginMenuMessageText(errorMessageString));

                    //Turning the login buttons on again
                    loginMenuBackButton.interactable = true;
                    loginMenuLoginButton.interactable = true;
                    forgotPasswordButton.interactable = true;
                    
                    try
                    {
                        // Playing the negative sound
                        SoundManager.Instance!.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
            );
        }

        #endregion

        #region On Login with Apple ID 

        private void TryToLoginWithAppleIdToPlayFab(IAppleIDCredential appleIdCredential, string rawNonce , bool craeteAccount)
        {
            Debug.Log("TryToLoginWithAppleIdToPlayFab :: " + appleIdCredential.IdentityToken);
            var loginWithAppleIdRequest = new LoginWithAppleRequest
            {
                IdentityToken = System.Text.Encoding.UTF8.GetString(appleIdCredential.IdentityToken),
                //IdentityToken = Convert.ToBase64String(appleIdCredential.IdentityToken),
                TitleId = "E4A61",
                CreateAccount = craeteAccount,

            };

            PlayFabClientAPI.LoginWithApple(loginWithAppleIdRequest,
                resultCallback =>
                {
                    Debug.Log("Apple Login successfully . ");

                    //Display the message on the login menu message text
                    StartCoroutine(Routine_LoginMenuMessageText("Verifying login details!"));
                    Debug.Log("Verifying login details! ");

                    // Telling the game that this is not a guest login
                    PlayerPrefs.SetInt(IsGuestKey, 0);
                        // Getting the playfab id
                    var playFabId = resultCallback.PlayFabId;

                       // Validating the user data
                    AnonymousLoginDataValidator(playFabId);

                    //Loading the next scene here
                    //StartCoroutine(Routine_LoginMenuMessageText("Login Successful!"));
                    //StartCoroutine(Routine_LoadNextSceneOnLoginOrRegistration());

                },
                error =>
                {
                    //Check which type of error was it
                    var errorType = error.Error;

                    //A string that will be used to display the error message
                    string errorMessageString = "";

                    //Switch case to handle the error type
                    switch (errorType)
                    {
                        case PlayFabErrorCode.UsernameNotAvailable:
                            errorMessageString = "Username is not available";
                            break;
                        case PlayFabErrorCode.InvalidUsername:
                            errorMessageString = "Player With This Username Does Not Exist";
                            break;
                        case PlayFabErrorCode.InvalidPassword:
                            errorMessageString = "You Entered The Incorrect Password";
                            break;
                        case PlayFabErrorCode.UserisNotValid:
                            break;
                        case PlayFabErrorCode.AccountNotFound:
                            errorMessageString = "Username/Account Does Not Exist";
                            {
                                // Create new Playfab account if not found.
                                TryToLoginWithAppleIdToPlayFab(appleIdCredential, rawNonce, true);
                            }
                            break;
                        default:
                            errorMessageString = error.ErrorMessage;
                            break;
                    }
                    Debug.Log("LOGIN ERROR TYPE :: "+ errorType);
                    Debug.Log("LOGIN ERROR MESSAGE :: "+ errorMessageString);

                    //Display the message on the login menu message text
                    StartCoroutine(Routine_LoginMenuMessageText(errorMessageString));

                    //Turning the login buttons on again
                    loginMenuBackButton.interactable = true;
                    loginMenuLoginButton.interactable = true;
                    forgotPasswordButton.interactable = true;

                    try
                    {
                        // Playing the negative sound
                        SoundManager.Instance!.PlayMenuAndInGameStoreNegativeClickSounds();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.StackTrace);

                    }

                });
        }

        #endregion

        #region On Login Setup Player Display Name

        /// <summary>
        /// This will be called after the login details have been verified and the user's email
        /// has been verified as well.
        /// </summary>
        /// <param name="result"></param>
        private IEnumerator SetupPlayerDisplayNameAfterLoginIsSuccessful(LoginResult result)
        {
            //This is the request that will be used to update the user's display name
            var updateDisplayNameRequest = new UpdateUserTitleDisplayNameRequest();
            
            
            //Getting the username from the result
            var username = result.InfoResultPayload.AccountInfo.Username;
            Debug.Log("username  :: "  + username);
            
            //Setting the display name to the username that was given during registration
            updateDisplayNameRequest.DisplayName = username;

            //Calling the function to update the user's display name
            PlayFabClientAPI.UpdateUserTitleDisplayName(
                updateDisplayNameRequest,
                result =>
                {
                    print("Successfully updated the user's display name");
                },
                error =>
                {
                    print("Failed to update the user's display name");
                }
            );
            
            yield break;
        }

        #endregion

        #region Annonymous Login Features for Playfab

        /// <summary>
        /// This will allow us to perform a guest login via playfab's system
        /// </summary>
        private void TryToPerformGuestLoginViaPlayFab()
        {
            Debug.Log("TryToPerformGuestLoginViaPlayFab Application.platform : " + Application.platform);
            // Anonymous login for android
            if (Application.platform == RuntimePlatform.Android)
            {
                // A new login request for android
                var anonymousLoginRequest = new LoginWithAndroidDeviceIDRequest
                {
                    AndroidDeviceId = SystemInfo.deviceUniqueIdentifier,
                    CreateAccount = true,
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                    {
                        GetPlayerProfile = true,
                        GetUserAccountInfo = true,
                    },
                };

                try
                {
                    // Calling the function to login the player
                    PlayFabClientAPI.LoginWithAndroidDeviceID(
                        anonymousLoginRequest,
                        result =>
                        {
                            print("Guest login was successful");

                            // Display the message on the guest login warning menu message text
                            guestLoginWarningMenuMessageText.text = "Validating the user data!";
                        
                            // Getting the playfab id
                            var playFabId = result.PlayFabId;
                        
                            // Validating the user data
                            AnonymousLoginDataValidator(playFabId);
                        
                            // Telling the game that this is a guest login
                            PlayerPrefs.SetInt(IsGuestKey, 1);
                        },
                        error =>
                        {
                            print("Guest login failed");
                        
                            // Display the message on the login menu message text
                            StartCoroutine(Routine_LoginMenuMessageText("Guest Login Failed!"));
                        
                            // Turning the login buttons on again
                            loginMenuBackButton.interactable = true;
                            loginMenuLoginButton.interactable = true;
                        
                            try
                            {
                                // Playing the negative sound
                                SoundManager.Instance!.PlayMenuAndInGameStoreNegativeClickSounds();
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    );
                }
                catch (Exception e)
                {
                    print(e.Message);
                    TryToPerformGuestLoginViaPlayFab();
                }
            }
            
            // Anonymous login for iOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // A new login request for iOS
                var anonymousLoginRequest = new LoginWithIOSDeviceIDRequest
                {
                    DeviceId = SystemInfo.deviceUniqueIdentifier,
                    CreateAccount = true,
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                    {
                        GetPlayerProfile = true,
                        GetUserAccountInfo = true,
                    },
                };

                try
                {
                    // Calling the function to login the player
                    PlayFabClientAPI.LoginWithIOSDeviceID(
                        anonymousLoginRequest,
                        result =>
                        {
                            print("Guest login was successful");
                        
                            // Display the message on the guest login warning menu message text
                            guestLoginWarningMenuMessageText.text = "Validating the user data!";
                        
                            // Getting the playfab id
                            var playFabId = result.PlayFabId;
                        
                            // Validating the user data
                            AnonymousLoginDataValidator(playFabId);
                        
                            // Telling the game that this is a guest login
                            PlayerPrefs.SetInt(IsGuestKey, 1);
                        },
                        error =>
                        {
                            print("Guest login failed");
                        
                            // Display the message on the login menu message text
                            StartCoroutine(Routine_LoginMenuMessageText("Guest Login Failed!"));
                        
                            // Turning the login buttons on again
                            loginMenuBackButton.interactable = true;
                            loginMenuLoginButton.interactable = true;
                        
                            try
                            {
                                // Playing the negative sound
                                SoundManager.Instance!.PlayMenuAndInGameStoreNegativeClickSounds();
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    );
                }
                catch (Exception e)
                {
                    print(e.Message);
                    TryToPerformGuestLoginViaPlayFab();
                }
            }
            
            // Anonymous login for Windows Editor, Windows Player, Linux Player, and Linux Editor
            if (Application.platform == RuntimePlatform.WindowsEditor ||
                Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.LinuxPlayer ||
                Application.platform == RuntimePlatform.LinuxEditor ||
                Application.platform == RuntimePlatform.OSXEditor)
            {
                // A new login request for Windows and Linux
                var anonymousLoginRequest = new LoginWithCustomIDRequest
                {
                    CustomId = SystemInfo.deviceUniqueIdentifier,
                    CreateAccount = true,
                    InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
                    {
                        GetPlayerProfile = true,
                        GetUserAccountInfo = true,
                    },
                };

                try
                {
                    // Calling the function to login the player
                    PlayFabClientAPI.LoginWithCustomID(
                        anonymousLoginRequest,
                        result =>
                        {
                            print("Guest login was successful");
                        
                            // Display the message on the guest login warning menu message text
                            guestLoginWarningMenuMessageText.text = "Validating the user data!";
                        
                            // Getting the playfab id
                            var playFabId = result.PlayFabId;
                        
                            // Validating the user data
                            AnonymousLoginDataValidator(playFabId);
                        
                            // Telling the game that this is a guest login
                            PlayerPrefs.SetInt(IsGuestKey, 1);
                        },
                        error =>
                        {
                            print("Guest login failed");
                        
                            // Display the message on the login menu message text
                            StartCoroutine(Routine_LoginMenuMessageText("Guest Login Failed!"));
                        
                            // Turning the login buttons on again
                            loginMenuBackButton.interactable = true;
                            loginMenuLoginButton.interactable = true;
                        
                            try
                            {
                                // Playing the negative sound
                                SoundManager.Instance!.PlayMenuAndInGameStoreNegativeClickSounds();
                            }
                            catch (Exception e)
                            {
                                Debug.LogException(e);
                            }
                        }
                    );
                }
                catch (Exception e)
                {
                    print(e.Message);
                    TryToPerformGuestLoginViaPlayFab();
                }
            }
        }

        /// <summary>
        /// This will check if the user data already exist on the PlayFab server. If it does not exist, then
        /// we will create the data on the server
        /// </summary>
        private void AnonymousLoginDataValidator(string playFabId)
        {
            print("Trying to get the user data from the server");

            //This request is for executing the cloud script that will get the user data
            var getUserDataFromServer = new ExecuteCloudScriptRequest
            {
                FunctionName = "returnPlayerUserData",
                FunctionParameter = new { },
                GeneratePlayStreamEvent = true
            };

            try
            {
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
                            
                                // If the function result is null, it means we need to create the user data
                                if (result.FunctionResult == null)
                                {
                                    print("User Data does not exist on the server. Creating the user data");
                                
                                    // Creating the user data
                                    CreateUserDataForAnonymousLogin(playFabId);
                                }
                                else
                                {
                                    print("User Data already exists on the server");
                                
                                    // Display the message on the guest login warning menu message text
                                    guestLoginWarningMenuMessageText.text = "Validation Successful! Redirecting!";
                                
                                    // Loading the next scene
                                    StartCoroutine(Routine_LoadNextSceneOnLoginOrRegistration());
                                }
                            }
                            catch (Exception e)
                            {
                                print(e.Message);
                                // Retry the function
                                AnonymousLoginDataValidator(playFabId);
                            }
                        }

                        // This will run for the android platform
                        if (Application.platform == RuntimePlatform.Android)
                        {
                            try
                            {
                                print("User Data was successfully downloaded from the server");
                            
                                // If the function result is null, it means we need to create the user data
                                if (result.FunctionResult == null)
                                {
                                    print("User Data does not exist on the server. Creating the user data");
                                
                                    // Creating the user data
                                    CreateUserDataForAnonymousLogin(playFabId);
                                }
                                else
                                {
                                    print("User Data already exists on the server");
                                
                                    // Display the message on the guest login warning menu message text
                                    guestLoginWarningMenuMessageText.text = "Validation Successful! Redirecting!";
                                
                                    // Loading the next scene
                                    StartCoroutine(Routine_LoadNextSceneOnLoginOrRegistration());
                                }
                            }
                            catch (Exception e)
                            {
                                print(e.Message);
                                // Retry the function
                                AnonymousLoginDataValidator(playFabId);
                            }
                        }
                        
                        // This will run for the iOS platform
                        if (Application.platform == RuntimePlatform.IPhonePlayer)
                        {
                            try
                            {
                                print("User Data was successfully downloaded from the server");
                            
                                // If the function result is null, it means we need to create the user data
                                if (result.FunctionResult == null)
                                {
                                    print("User Data does not exist on the server. Creating the user data");
                                
                                    // Creating the user data
                                    CreateUserDataForAnonymousLogin(playFabId);
                                }
                                else
                                {
                                    print("User Data already exists on the server");
                                
                                    // Display the message on the guest login warning menu message text
                                    guestLoginWarningMenuMessageText.text = "Validation Successful! Redirecting!";
                                
                                    // Loading the next scene
                                    StartCoroutine(Routine_LoadNextSceneOnLoginOrRegistration());
                                }
                            }
                            catch (Exception e)
                            {
                                print(e.Message);
                                // Retry the function
                                AnonymousLoginDataValidator(playFabId);
                            }
                        }
                    },
                    error =>
                    {
                        print("Failed to validate the user's data");
                        AnonymousLoginDataValidator(playFabId);
                    }
                );
            }
            catch (Exception e)
            {
                print(e.Message);
                AnonymousLoginDataValidator(playFabId);
            }
        }

        /// <summary>
        /// This function is going to be used to create the user data if the user data does not exist
        /// on the server. This is to avoid errors.
        /// </summary>
        /// <param name="playFabId"></param>
        private void CreateUserDataForAnonymousLogin(string playFabId)
        {
            print("Trying to setup the player data after anonymous login");
            
            // This is to execute the cloud script that will setup the user data
            var setupPlayerDataRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "setupUserData",
                FunctionParameter = new { },
                GeneratePlayStreamEvent = true
            };

            try
            {
                // Calling the cloud script to setup the user data
                PlayFabClientAPI.ExecuteCloudScript(
                    setupPlayerDataRequest,
                    result =>
                    {
                        if (result.FunctionResult == null)
                        {
                            print("Failed to setup the player data");
                        
                            // Try to setup the player data again
                            CreateUserDataForAnonymousLogin(playFabId);
                        
                            return;
                        }
                    
                        //Using Newtonsoft.Json to convert the result into a dictionary
                        var resultDictionary = 
                            JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());
                    
                        //Getting the data from the resultDictionary
                        print(resultDictionary["message"]);
                    
                        // Display the message on the guest login warning menu message text
                        guestLoginWarningMenuMessageText.text = "Validation Successful! Redirecting!";
                    
                        // Loading the next scene
                        StartCoroutine(Routine_LoadNextSceneOnLoginOrRegistration());
                    },
                    error =>
                    {
                        print("Failed to setup the player data");
                    
                        // Try to setup the player data again
                        CreateUserDataForAnonymousLogin(playFabId);
                    }
                );
            }
            catch (Exception e)
            {
                print(e.Message);
                CreateUserDataForAnonymousLogin(playFabId);
            }
        }

        /// <summary>
        ///  This routine will help us display the message on the guest login warning menu message text
        /// </summary>
        /// <param name="messageToDisplay"></param>
        /// <returns></returns>
        private IEnumerator Routine_GuestLoginWarningMenuTextHandler(string messageToDisplay)
        {
            // Making the guest login warning menu message text active
            guestLoginWarningMenuMessageText.gameObject.SetActive(true);
            // Removing the text from the guest login warning menu message text
            guestLoginWarningMenuMessageText.text = "";
            // Printing the message on the guest login warning menu message text
            guestLoginWarningMenuMessageText.text = messageToDisplay;
            
            yield return new WaitForSeconds(3.0f);
            
            // Deactivating the guest login warning menu message text
            guestLoginWarningMenuMessageText.gameObject.SetActive(false);
        }
        
        #endregion
        
        #region For Loading the Next Scene

        /// <summary>
        /// This loads the next scene when the user logs in or registers. This will be used to load
        /// the main menu scene
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_LoadNextSceneOnLoginOrRegistration()
        {
            yield return new WaitForSeconds(3.5f);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

        #endregion

        #region For Loading Next Scene If The Player Is Verified

        /// <summary>
        /// This will load the next scene on login, only if the player is verified
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_LoadNextSceneOnLoginIfVerified(LoginResult loginResult)
        {
            print("Executing a cloud script to check if the user is verified");
            
            var executeCloudScriptRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "checkIfEmailIsVerified",
                FunctionParameter = new { },
                GeneratePlayStreamEvent = true
            };

            try
            {
                //Calling the cloud script to check if the user is verified
                PlayFabClientAPI.ExecuteCloudScript(
                    executeCloudScriptRequest,
                    result =>
                    {
                        //Using Newtonsoft.Json to convert the result into a dictionary
                        var resultDictionary = 
                            JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());
                        Debug.Log("resultDictionary Count :: "+resultDictionary.Count);
                        //Getting the value of the 'message' key
                        var message = resultDictionary["message"];
                        Debug.Log("resultDictionary MESSAGE" + message);

                        //Converting message into a dictionary
                        var messageDictionary = 
                            JsonConvert.DeserializeObject<Dictionary<string, object>>(message.ToString());
                    
                        //Getting the value of "Created" and converting it into a DateTime object
                        var created = messageDictionary["Created"];
                        var createdDateTime = Convert.ToDateTime(created);

                        //Getting the "ContactEmailAddresses" value and converting it into a dictionary
                        var contactEmailAddresses = messageDictionary["ContactEmailAddresses"];
                    
                        //Converting contact email addresses into a JArray
                        var contactEmailAddressesJArray = JArray.Parse(contactEmailAddresses.ToString());
                    
                        //Converting the first element of the JArray into a dictionary
                        var contactEmailAddressesDictionary = 
                            JsonConvert.DeserializeObject<Dictionary<string, object>>
                                (contactEmailAddressesJArray[0].ToString());
                    
                        //Getting the value of "VerificationStatus"
                        var verificationStatus = contactEmailAddressesDictionary["VerificationStatus"];
                    
                        // This will store the calculation for verification
                        var calculationForVerification = DateTime.UtcNow.Subtract(createdDateTime).TotalDays;
                    
                        //Checking if the account was created more than 1 day ago
                        if (calculationForVerification > 1)
                        {
                            print("Account was created more than 1 days ago. So we will check if the" +
                                  " player is verified or not");
                        
                            //If the player's email is verified
                            if (verificationStatus.ToString() == "Confirmed")
                            {
                                print("Player is verified. Loading the next scene");
                                StartCoroutine(SetupPlayerDisplayNameAfterLoginIsSuccessful(loginResult));
                                StartCoroutine(Routine_LoginMenuMessageText("Login Successful!"));
                                StartCoroutine(Routine_LoadNextSceneOnLoginOrRegistration());
                            }
                            else
                            {
                                print("Player is not verified. We will tell the user that");
                                StartCoroutine(Routine_LoginMenuMessageText("Please verify email! Redirecting!"));
                                StartCoroutine(Routine_EnableAllLoginButtonsAndEnableTheResendVerificationEmailMenu());

                                try
                                {
                                    // Playing the click sound
                                    SoundManager.Instance!.PlayMenuAndInGameStoreClickSounds();
                                }
                                catch (Exception e)
                                {
                                    Debug.LogException(e);
                                }
                            }
                        }
                        else
                        {
                            print("Account was created less than 1 day ago. So we will load the next scene");
                            StartCoroutine(Routine_LoginMenuMessageText("Login Successful!"));
                            StartCoroutine(Routine_LoadNextSceneOnLoginOrRegistration());
                        }
                    },
                    error =>
                    {
                        print("Failed to Verify whether account was verified or not!");
                        StartCoroutine(Routine_LoginMenuMessageText("Failed to retrieve Email Verification Status!"));
                    
                        //Turning the login buttons on again
                        loginMenuBackButton.interactable = true;
                        loginMenuLoginButton.interactable = true;
                    
                        try
                        {
                            // Playing the negative sound
                            SoundManager.Instance!.PlayMenuAndInGameStoreNegativeClickSounds();
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                );
            }
            catch (Exception e)
            {
                print(e.Message);
                // Calling this function again
                StartCoroutine(Routine_LoadNextSceneOnLoginIfVerified(loginResult));
            }
            
            yield break;
        }

        #endregion

        #region Verification Email Resend Menu Functions

         /// <summary>
        /// This runs when the resend verification email menu resend email button is clicked
        /// </summary>
        private void OnResendVerificationEmailMenuResendEmailButtonClicked()
        {
            print("Resend Verification Email Menu Resend Email Button was clicked");
            
            // Check if the string is null or empty
            if (string.IsNullOrEmpty(_resendVerificationEmailMenuEmailInputFieldTextValue))
            {
                // Displaying the message on the forgot password message text
                StartCoroutine(Routine_ResendVerificationEmailMenuMessageText("Email cannot be empty"));
                return;
            }
            
            // Check if the string contains @
            if (_resendVerificationEmailMenuEmailInputFieldTextValue.Contains("@") == false)
            {
                // Displaying the message on the forgot password message text
                StartCoroutine(Routine_ResendVerificationEmailMenuMessageText("Invalid Email"));
                return;
            }
            
            // Running the routine to resend the verification email
            StartCoroutine(Routine_ResendVerificationEmailRoutine());
        }

        /// <summary>
        /// This is going to run when the process to resend the verification email is running. This will wait
        /// for a few APIs to finish before others are run
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_ResendVerificationEmailRoutine()
        {
            // Remove text from the resend verification email menu message text
            resendVerificationEmailMenuMessageText.text = "";
            // Deactivate the resend verification email menu message text
            resendVerificationEmailMenuMessageText.gameObject.SetActive(false);
            
            // When this is true, it tells the game to wait for a previous process to finish
            bool waitForProcess = false;
            
            // Creating a new wait for seconds object
            WaitForSeconds waitForOneSecond = new WaitForSeconds(1f);
            
            // Turning the buttons off
            resendVerificationEmailMenuResendEmailButton.interactable = false;
            resendVerificationEmailMenuBackButton.interactable = false;
            
            // Checking the current email address of the user
            var getUserAccountInfoRequest = new GetAccountInfoRequest();
            
            // This will store the email address of the user
            string currentEmailAddress = "";
            
            // Waiting for the process to finish
            waitForProcess = true;
            
            // Calling the function to get the user's account info
            PlayFabClientAPI.GetAccountInfo(
                getUserAccountInfoRequest,
                result =>
                {
                    // Getting the email address of the user
                    currentEmailAddress = result.AccountInfo.PrivateInfo.Email;
                    
                    // Telling the game that the process has finished
                    waitForProcess = false;
                },
                error =>
                {
                    print("Failed to get the user's account info");
                    
                    // Displaying the message on the resend verification email menu message text
                    StartCoroutine(Routine_ResendVerificationEmailMenuMessageText(error.ErrorMessage));
                    
                    // Turning the buttons on
                    resendVerificationEmailMenuResendEmailButton.interactable = true;
                    resendVerificationEmailMenuBackButton.interactable = true;
                }
            );

            // We will basically wait for the previous process to finish before we run the next one
            while (waitForProcess == true)
            {
                yield return waitForOneSecond;
            }
            
            // We will only run this function if the current email address on back-end is not equal to the one already
            // entered in the resend verification email menu email input field
            if (currentEmailAddress != _resendVerificationEmailMenuEmailInputFieldTextValue)
            {
                print("A new email address was entered. So we will update the user's contact email id");
                
                // Sending a request to update the user's contact email id
                var updateUserContactEmailIdRequest = new AddOrUpdateContactEmailRequest
                {
                    EmailAddress = _resendVerificationEmailMenuEmailInputFieldTextValue
                };
            
                // Calling the function to update the user's contact email id
                PlayFabClientAPI.AddOrUpdateContactEmail(
                    updateUserContactEmailIdRequest,
                    result =>
                    {
                        print("Successfully sent the user a verification email");
                    
                        // Displaying the message on the resend verification email menu message text
                        StartCoroutine(Routine_ResendVerificationEmailMenuMessageText
                            ("Email sent! Please check your email and verify your account!"));
                    
                        // Turning the buttons on
                        resendVerificationEmailMenuResendEmailButton.interactable = true;
                        resendVerificationEmailMenuBackButton.interactable = true;
                    },
                    error =>
                    {
                        print("Failed to send the user a verification email");
                    
                        // Displaying the message on the resend verification email menu message text
                        StartCoroutine(Routine_ResendVerificationEmailMenuMessageText(error.ErrorMessage));
                    
                        // Turning the buttons on
                        resendVerificationEmailMenuResendEmailButton.interactable = true;
                        resendVerificationEmailMenuBackButton.interactable = true;
                    }
                );

                yield break;
            }
            
            print("Since the email address has not changed, we will call the cloud script " +
                  "to resend the verification email");
            
            // Sending a request to execute the cloud script called "resendVerificationEmail"
            var executeCloudScriptRequest = new ExecuteCloudScriptRequest
            {
                FunctionName = "resendVerificationEmail",
                FunctionParameter = new { },
                GeneratePlayStreamEvent = true
            };
            
            // Calling the cloud script to resend the verification email
            PlayFabClientAPI.ExecuteCloudScript(
                executeCloudScriptRequest,
                result =>
                {
                    //Using Newtonsoft.Json to convert the result into a dictionary
                    var resultDictionary = 
                        JsonConvert.DeserializeObject<Dictionary<string, object>>(result.FunctionResult.ToString());
                    
                    //Getting the value of the 'message' key
                    var message = resultDictionary["message"];
                    
                    // Printing the message on the screen
                    print(message);
                    
                    // Displaying the message on the Resend Verification Email Menu Message Text
                    StartCoroutine(Routine_ResendVerificationEmailMenuMessageText(message.ToString()));
                    
                    // Turning the buttons on
                    resendVerificationEmailMenuResendEmailButton.interactable = true;
                    resendVerificationEmailMenuBackButton.interactable = true;
                },
                error =>
                {
                    // Printing the error message
                    print(error.ErrorMessage);
                    
                    // Turning the buttons on
                    resendVerificationEmailMenuResendEmailButton.interactable = true;
                    resendVerificationEmailMenuBackButton.interactable = true;
                } 
            );
            
            yield break;
        }
        
        /// <summary>
        /// This routine will help us display the message on the resend verification email menu message text
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_ResendVerificationEmailMenuMessageText(string messageToDisplay)
        {
            // Activating the object
            resendVerificationEmailMenuMessageText.gameObject.SetActive(true);
            // Setting the text
            resendVerificationEmailMenuMessageText.text = messageToDisplay;
            // Waiting for 5 seconds
            yield return new WaitForSeconds(5f);
            /*// Setting the text to empty
            resendVerificationEmailMenuMessageText.text = "";
            // Deactivating the object
            resendVerificationEmailMenuMessageText.gameObject.SetActive(false);*/
            
            yield break;
        }

        #endregion
        
        #region Button References

        [Header("Login Or Register Menu Button References")]
        //Reference to the login button
        [SerializeField]
        private Button loginButton;

        /// <summary>
        /// Reference to the register button
        /// </summary>
        [SerializeField] private Button registerButton;

        /// <summary>
        /// Reference to the quit button
        /// </summary>
        [SerializeField] private Button quitButton;
        
        /// <summary>
        /// Reference to the login menu password visible button
        /// </summary>
        [SerializeField] private Button loginButtonPasswordVisible;
        
        /// <summary>
        /// This is a reference to the login menu email input field
        /// </summary>
        [SerializeField] private TMP_InputField loginMenuPasswordInputField;

        /// <summary>
        /// Reference to the forgot password button
        /// </summary>
        [SerializeField] private Button forgotPasswordButton;

        /// <summary>
        /// This is a reference to the guest login button that exists in the game
        /// </summary>
        [SerializeField] private Button guestLoginButton;

        /// <summary>
        /// This runs when the login button is clicked
        /// </summary>
        private void OnLoginButtonClicked()
        {
            print("Login Button was clicked");
            loginMenuContainer.SetActive(true);
            loginOrRegisterMenuContainer.SetActive(false);
            
            try
            {
                // Playing the click sound
                SoundManager.Instance!.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void AppleLoginBtn()
        {
            AppleSignInManager.instance.LoginWithApple(TryToLoginWithAppleIdToPlayFab);
            Debug.Log("AppleLoginBtn");
        }

       

        /// <summary>
        /// Runs when the register button is clicked
        /// </summary>
        private void OnRegisterButtonClicked()
        {
            print("Register Button was clicked");
            registerMenuContainer.SetActive(true);
            loginOrRegisterMenuContainer.SetActive(false);
            
            try
            {
                // Playing the click sound
                SoundManager.Instance!.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Runs when the quit button is clicked
        /// </summary>
        private void OnQuitButtonClicked()
        {
            print("Quit Button was clicked");
            Application.Quit();
            
            try
            {
                // Playing the click sound
                SoundManager.Instance!.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        /// <summary>
        /// This will run when the password visible button is clicked on the login menu
        /// </summary>
        private void OnLoginButtonPasswordVisibleButtonClicked()
        {
            print("Login Button Password Visible Button was clicked");
            
            // If the current content is hidden
            if (loginMenuPasswordInputField.contentType == TMP_InputField.ContentType.Password)
            {
                // Changing the content type to standard
                loginMenuPasswordInputField.contentType = TMP_InputField.ContentType.Standard;
                
                // Changing the input type to standard
                loginMenuPasswordInputField.inputType = TMP_InputField.InputType.Standard;
                
                // Changing the character asterisk to none
                loginMenuPasswordInputField.asteriskChar = '\0';
            }
            else
            {
                // Changing the content type to password
                loginMenuPasswordInputField.contentType = TMP_InputField.ContentType.Password;
                
                // Changing the input type to password
                loginMenuPasswordInputField.inputType = TMP_InputField.InputType.Password;
                
                // Changing the character asterisk to *
                loginMenuPasswordInputField.asteriskChar = '*';
            }
        }

        /// <summary>
        /// This will run when the forgot password button is clicked on the login menu
        /// </summary>
        private void OnForgotPasswordButtonClicked()
        {
            print("Forgot Password Button was clicked");
            
            // Activate the forgot password menu
            forgotPasswordMenu.SetActive(true);
            
            // Deactivate the login menu
            loginMenuContainer.SetActive(false);
            
            // Remove the text from the forgotPasswordMessageText
            forgotPasswordMessageText.text = "";
            // Deactivate the forgot password message text
            forgotPasswordMessageText.gameObject.SetActive(false);
        }

        /// <summary>
        /// This will run when the guest login button is clicked on the login menu
        /// </summary>
        private void OnGuestLoginButtonClicked()
        {
            print("Guest login button clicked");
            
            // Activate the guest login warning menu
            guestLoginWarningMenu.SetActive(true);
            // Deactivate the guest login warning message text
            guestLoginWarningMenuMessageText.gameObject.SetActive(false);
            
            // Deactivate the login menu
            loginMenuContainer.SetActive(false);
        }

        [Space(10)]
        [Header("Login Menu Button References")]
        //Reference to the login menu back button
        [SerializeField]
        private Button loginMenuBackButton;

        /// <summary>
        /// Reference to the login menu login button
        /// </summary>
        [SerializeField] private Button loginMenuLoginButton;

        [SerializeField] private Button appleLoginButton;

        /// <summary>
        /// Runs when the login menu back button is clicked
        /// </summary>
        private void OnLoginMenuBackButtonClicked()
        {
            print("Login Menu Back Button was clicked");
            loginMenuContainer.SetActive(false);
            loginOrRegisterMenuContainer.SetActive(true);
            
            try
            {
                // Playing the click sound
                SoundManager.Instance!.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Runs when the login menu login button is clicked
        /// </summary>
        private void OnLoginMenuLoginButtonClicked()
        {
            print("Login Menu Login Button was clicked");
            TryToLoginPlayerToPlayFab();
            
            try
            {
                // Playing the click sound
                SoundManager.Instance!.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        [Space(10)]
        [Header("Register Menu Button References")]
        //Reference to the register menu back button
        [SerializeField]
        private Button registerMenuBackButton;

        /// <summary>
        /// Reference to the register menu register button
        /// </summary>
        [SerializeField] private Button registerMenuRegisterButton;
        
        /// <summary>
        /// Reference to the register menu password visible button
        /// </summary>
        [SerializeField] private Button registerMenuPasswordVisibleButton;
        
        /// <summary>
        /// Reference to the register menu email input field
        /// </summary>
        [SerializeField] private TMP_InputField registerMenuPasswordInputField;

        /// <summary>
        /// Runs when the register menu back button is clicked
        /// </summary>
        private void OnRegisterMenuBackButtonClicked()
        {
            print("Register Menu Back Button was clicked");
            registerMenuContainer.SetActive(false);
            loginOrRegisterMenuContainer.SetActive(true);
            
            try
            {
                // Playing the click sound
                SoundManager.Instance!.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Runs when the register menu register button is clicked
        /// </summary>
        private void OnRegisterMenuRegisterButtonClicked()
        {
            print("Register Menu Register Button was clicked");
            TryToRegisterPlayerToPlayFab();
            
            try
            {
                // Playing the click sound
                SoundManager.Instance!.PlayMenuAndInGameStoreClickSounds();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        /// <summary>
        /// Runs when the password visible button is clicked on the register menu
        /// </summary>
        private void OnRegisterMenuPasswordVisibleButtonClicked()
        {
            print("Register Menu Password Visible Button was clicked");
            
            // If the current content is hidden
            if (registerMenuPasswordInputField.contentType == TMP_InputField.ContentType.Password)
            {
                // Changing the content type to standard
                registerMenuPasswordInputField.contentType = TMP_InputField.ContentType.Standard;
                
                // Changing the input type to standard
                registerMenuPasswordInputField.inputType = TMP_InputField.InputType.Standard;
                
                // Changing the character asterisk to none
                registerMenuPasswordInputField.asteriskChar = '\0';
            }
            else
            {
                // Changing the content type to password
                registerMenuPasswordInputField.contentType = TMP_InputField.ContentType.Password;
                
                // Changing the input type to password
                registerMenuPasswordInputField.inputType = TMP_InputField.InputType.Password;
                
                // Changing the character asterisk to *
                registerMenuPasswordInputField.asteriskChar = '*';
            }
        }
        
        [Header("Forgot Password Menu Button References")]
        // Reference to the Forgot Password Send Email Button
        [SerializeField] private Button forgotPasswordSendEmailButton;

        /// <summary>
        /// Reference to the forgot password back button
        /// </summary>
        [SerializeField] private Button forgotPasswordBackButton;
        
        /// <summary>
        /// This is going to be a reference to the forgot password email input field
        /// </summary>
        [SerializeField] private TMP_InputField forgotPasswordEmailInputField;

        /// <summary>
        /// Reference to the forgot password message text
        /// </summary>
        [SerializeField] private TextMeshProUGUI forgotPasswordMessageText;
        
        /// <summary>
        /// This will store the value of the forgot password email input field
        /// </summary>
        private string _valueOfForgotPasswordEmailInputField;
        
        /// <summary>
        /// This runs when the forgot password send email button is clicked
        /// </summary>
        private void OnForgotPasswordSendEmailButtonClicked()
        {
            print("Forgot Password Send Email Button was clicked");
            
            // Checking if the string is null or empty
            if (string.IsNullOrEmpty(_valueOfForgotPasswordEmailInputField))
            {
                // Displaying the message on the forgot password message text
                StartCoroutine(Routine_ForgotPasswordMessageText("Email cannot be empty"));
                return;
            }
            
            // Checking if the string contains @
            if (_valueOfForgotPasswordEmailInputField.Contains("@") == false)
            {
                // Displaying the message on the forgot password message text
                StartCoroutine(Routine_ForgotPasswordMessageText("Invalid Email"));
                return;
            }
            
            // Turning the buttons off
            forgotPasswordSendEmailButton.interactable = false;
            forgotPasswordBackButton.interactable = false;
            
            // Remove the text from forgotPasswordMessageText
            forgotPasswordMessageText.text = "";
            // Deactivate the forgot password message text
            forgotPasswordMessageText.gameObject.SetActive(false);
            
            // Sending email to the user to reset their password
            var sendAccountRecoveryEmailRequest = new SendAccountRecoveryEmailRequest
            {
                Email = _valueOfForgotPasswordEmailInputField,
                TitleId = PlayFabSettings.TitleId
            };
            
            PlayFabClientAPI.SendAccountRecoveryEmail(
                sendAccountRecoveryEmailRequest,
                result =>
                {
                    print("Successfully sent the user an email to reset their password");
                    
                    // Displaying the message on the forgot password message text
                    StartCoroutine(Routine_ForgotPasswordMessageText
                        ("Successfully sent the email to reset password"));
                    
                    // Turning the buttons on
                    forgotPasswordSendEmailButton.interactable = true;
                    forgotPasswordBackButton.interactable = true;
                },
                error =>
                {
                    print("Failed to send the user an email to reset their password");
                    
                    // Displaying the message on the forgot password message text
                    StartCoroutine(Routine_ForgotPasswordMessageText(error.ErrorMessage));
                    
                    // Turning the buttons on
                    forgotPasswordSendEmailButton.interactable = true;
                    forgotPasswordBackButton.interactable = true;
                }
            );
        }
        
        /// <summary>
        /// This runs when the forgot password back button is clicked
        /// </summary>
        private void OnForgotPasswordBackButtonClicked()
        {
            print("Forgot Password Back Button was clicked");
            
            // Turning off the forgot password menu
            forgotPasswordMenu.SetActive(false);
            // Turning on the login menu
            loginMenuContainer.SetActive(true);
        }
        
        /// <summary>
        /// This is a reference to the forgot password email input field
        /// </summary>
        /// <param name="valueChanged"></param>
        private void OnForgotPasswordEmailInputFieldValueChanged(string valueChanged)
        {
            // Setting the value of the forgot password email input field
            _valueOfForgotPasswordEmailInputField = valueChanged;
        }
        
        /// <summary>
        /// This will be used to send a message to the user whenever required
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private IEnumerator Routine_ForgotPasswordMessageText(string message)
        {
            // We will not run this if the forgot password message text is already active
            if (forgotPasswordMessageText.gameObject.activeSelf == true) yield break;
            
            forgotPasswordMessageText.gameObject.SetActive(true);
            forgotPasswordMessageText.text = message;
            yield return new WaitForSeconds(3.5f);
            // forgotPasswordMessageText.text = "";
            // forgotPasswordMessageText.gameObject.SetActive(false);
        }
        
        [Header("Verification Warning Menu Button References")]
        // Verification Warning Menu I understand button
        [SerializeField] private Button verificationWarningMenuIUnderstandButton;
        
        /// <summary>
        /// This will run when the verification warning menu I understand button is clicked
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void OnVerificationWarningMenuIUnderstandButtonClicked()
        {
            // Turn the IUnderstandButton off
            verificationWarningMenuIUnderstandButton.interactable = false;
            
            // Changing game title to now loading
            gameTitleText.text = "Now Loading...";
            
            //Loading the next scene here
            // StartCoroutine(Routine_LoadNextSceneOnLoginOrRegistration());
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }
        
        [Header("Resend Verification Email Menu Button References")]
        // Resend Verification Email Menu Resend Email Button
        [SerializeField] private Button resendVerificationEmailMenuResendEmailButton;
        
        /// <summary>
        /// Resend verification email back button clicked
        /// </summary>
        [SerializeField] private Button resendVerificationEmailMenuBackButton;
        
        /// <summary>
        /// This is a reference to the input field that is used to enter the email address
        /// </summary>
        [SerializeField] private TMP_InputField resendVerificationEmailMenuEmailInputField;

        /// <summary>
        /// This is a reference to the resend verification email menu message text
        /// </summary>
        [SerializeField] private TextMeshProUGUI resendVerificationEmailMenuMessageText;
        
        /// <summary>
        /// This will store the value of the resend verification email menu email input field
        /// </summary>
        private string _resendVerificationEmailMenuEmailInputFieldTextValue;

        /// <summary>
        /// This runs when the resend verification email menu back button is clicked
        /// </summary>
        private void OnResendVerificationEmailMenuBackButtonClicked()
        {
            print("Resend Verification Email Menu Back Button was clicked");
            
            // Turning off the resend verification email menu
            resendVerificationEmailMenu.SetActive(false);
            // Turning on the login menu
            loginMenuContainer.SetActive(true);
        }
        
        /// <summary>
        /// This will run whenever the value of the resend verification email menu email input field changes
        /// </summary>
        /// <param name="value"></param>
        private void OnResendVerificationEmailMenuEmailInputFieldValueChanged(string value)
        {
            _resendVerificationEmailMenuEmailInputFieldTextValue = value;
        }
        
        [Header("Guest Login Warning Menu Button References")]
        // Guest Login Warning Menu I understand button
        [SerializeField] private Button guestLoginWarningMenuIUnderstandButton;

        /// <summary>
        /// This will be the back button that will take the player back to the login menu
        /// </summary>
        [SerializeField] private Button guestLoginWarningMenuBackButton;

        /// <summary>
        /// This will be used to tell the player that they are about to login as a guest. It can also be used
        /// to display an errors to the user.
        /// </summary>
        [SerializeField] private TextMeshProUGUI guestLoginWarningMenuMessageText;

        /// <summary>
        /// This will run whe the guest login I understand button is clicked
        /// </summary>
        private void OnGuestLoginWarningMenuIUnderstandButtonClicked()
        {
            print("Guest login I understand button is clicked");
            
            // Turn the IUnderstandButton off
            guestLoginWarningMenuIUnderstandButton.interactable = false;
            // Turn the back button off
            guestLoginWarningMenuBackButton.interactable = false;
            
            // Activating the guest login warning menu message text
            guestLoginWarningMenuMessageText.gameObject.SetActive(true);
            // Displaying to the user that we are trying to login as a guest
            guestLoginWarningMenuMessageText.text = "Attempting to login as a guest";
            
            // Login as a guest
            TryToPerformGuestLoginViaPlayFab();
        }
        
        private void OnGuestLoginWarningMenuBackButtonClicked()
        {
            print("Guest login warning menu back button is clicked");
            
            // Turning off the guest login warning menu
            guestLoginWarningMenu.SetActive(false);
            // Turning on the login or register menu
            loginOrRegisterMenuContainer.SetActive(true);
        }

        #endregion

        #region Message Text Reference

        [Header("Message Text Reference")]
        //Reference to login menu message text
        [SerializeField]
        private TextMeshProUGUI loginMenuMessageText;

        /// <summary>
        /// Reference to register message text
        /// </summary>
        [SerializeField] private TextMeshProUGUI registerMenuMessageText;

        /// <summary>
        /// This routine will display the message on the login menu message text for 5 seconds
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private IEnumerator Routine_LoginMenuMessageText(string message)
        {
            loginMenuMessageText.gameObject.SetActive(true);
            loginMenuMessageText.text = message;
            yield return new WaitForSeconds(5f);
            loginMenuMessageText.text = "";
            loginMenuMessageText.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// This routine should only run if the email address is not verified. It will also enable the
        /// resend verification email menu
        /// </summary>
        /// <returns></returns>
        private IEnumerator Routine_EnableAllLoginButtonsAndEnableTheResendVerificationEmailMenu()
        {
            yield return new WaitForSeconds(5f);
            loginMenuBackButton.interactable = true;
            loginMenuLoginButton.interactable = true;
            
            // Activate the resend verification email menu
            resendVerificationEmailMenu.SetActive(true);
            
            // Deactivate the login menu
            loginMenuContainer.SetActive(false);
            
            // Activate all buttons on the resend verification email menu
            resendVerificationEmailMenuResendEmailButton.interactable = true;
            resendVerificationEmailMenuBackButton.interactable = true;
            
            // Remove text from the resend verification email menu message text
            resendVerificationEmailMenuMessageText.text = "";
            // Deactivate the resend verification email menu message text
            resendVerificationEmailMenuMessageText.gameObject.SetActive(false);
        }

        /// <summary>
        /// This routine will display the message on the register menu message text for 5 seconds
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private IEnumerator Routine_RegisterMenuMessageText(string message)
        {
            registerMenuMessageText.gameObject.SetActive(true);
            registerMenuMessageText.text = message;
            yield return new WaitForSeconds(5f);
            registerMenuMessageText.text = "";
            registerMenuMessageText.gameObject.SetActive(false);
        }

        #endregion

        #region Complete Screen References

        [Header("Complete Screen References")]
        //Login Or Register Menu Container
        [SerializeField]
        private GameObject loginOrRegisterMenuContainer;

        /// <summary>
        /// Login Menu Container
        /// </summary>
        [SerializeField] private GameObject loginMenuContainer;

        /// <summary>
        /// Register Menu Container
        /// </summary>
        [SerializeField] private GameObject registerMenuContainer;

        /// <summary>
        /// Reference to the forgot password menu
        /// </summary>
        [SerializeField] private GameObject forgotPasswordMenu;

        /// <summary>
        /// This is a reference to the verification warning menu
        /// </summary>
        [SerializeField] private GameObject verificationWarningMenu;

        /// <summary>
        /// This is a reference to the resent verification email menu
        /// </summary>
        [SerializeField] private GameObject resendVerificationEmailMenu;

        /// <summary>
        /// This is a reference to the guest login warning menu, that should show up when the user
        /// tries to login as a guest
        /// </summary>
        [SerializeField] private GameObject guestLoginWarningMenu;

        #endregion

        #region Input Field References

        [Header("Input Field References")]
        //Login Menu User name Input Field text value
        private string _loginMenuEmailInputFieldTextValue;

        /// <summary>
        /// Login Menu Password Input Field text value
        /// </summary>
        private string _loginMenuPasswordInputFieldTextValue;

        /// <summary>
        /// On login menu user name input field value changed
        /// </summary>
        /// <param name="value"></param>
        public void OnLoginMenuEmailInputFieldValueChanged(string value)
        {
            _loginMenuEmailInputFieldTextValue = value;
        }

        /// <summary>
        /// On login menu password input field value changed
        /// </summary>
        /// <param name="value"></param>
        public void OnLoginMenuPasswordInputFieldValueChanged(string value)
        {
            _loginMenuPasswordInputFieldTextValue = value;
        }

        /// <summary>
        /// Register Menu User name Input Field text value
        /// </summary>
        private string _registerMenuUserNameInputFieldTextValue;

        /// <summary>
        /// Register Menu Password Input Field text value
        /// </summary>
        private string _registerMenuEmailInputFieldTextValue;

        /// <summary>
        /// Register Menu Password Input Field text value
        /// </summary>
        private string _registerMenuPasswordInputFieldTextValue;

        /// <summary>
        /// On register menu user name input field value changed
        /// </summary>
        /// <param name="value"></param>
        public void OnRegisterMenuUserNameInputFieldValueChanged(string value)
        {
            _registerMenuUserNameInputFieldTextValue = value;
        }

        /// <summary>
        /// On register menu password input field value changed
        /// </summary>
        /// <param name="value"></param>
        public void OnRegisterMenuEmailInputFieldValueChanged(string value)
        {
            _registerMenuEmailInputFieldTextValue = value;
        }

        /// <summary>
        /// On register menu password input field value changed
        /// </summary>
        /// <param name="value"></param>
        public void OnRegisterMenuPasswordInputFieldValueChanged(string value)
        {
            _registerMenuPasswordInputFieldTextValue = value;
        }

        #endregion
        
        #region Functions that will tell the game whether this is a guest player or not

        /// <summary>
        /// This key will be used to tell the game whether the player is a guest or not
        /// </summary>
        public const string IsGuestKey = "IsGuestKey"; 

        /// <summary>
        /// This will set the value of the guest key at the very start.
        /// </summary>
        private void SetIsGuestKeyValue()
        {
            PlayerPrefs.SetInt(IsGuestKey, 0);
        }

        #endregion

        #region Misc References

        [Header("Misc References")]
        // Game Title text reference
        [SerializeField] private TextMeshProUGUI gameTitleText;

        #endregion
    }
}