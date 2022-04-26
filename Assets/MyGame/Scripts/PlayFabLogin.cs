using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField]
    GameObject obgForTextComponent;

    private void OnLoginSuccess(LoginResult result)
    { Debug.Log("Congratulation, you made successful API call!");
        obgForTextComponent.GetComponent<TMP_Text>().text = "Connected";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(8, 130, 0, 255); 
    }
    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
        obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255); 
    }

    public void ConnectPlayFab()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            // if not we need to assign it to the appropriate variable manually
            //otherwise we can just remove this if statement at all
            PlayFabSettings.staticSettings.TitleId = "2BDAF";
        }

        var request = new LoginWithCustomIDRequest { CustomId = "GB//Lesson3", CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    public void DisconnectPlayFab()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        Debug.LogError("PlayFabClientAPI.ForgetAllCredentials();");
        obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
    }
}
