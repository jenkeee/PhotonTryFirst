using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    [SerializeField]
    GameObject obgForTextComponent;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;    
    }
     void Start()
    {
       // Connect();
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }

    }
        public void DisConnect()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        else
        {
            Debug.LogError($"Photon is alredy disconnected: {PhotonNetwork.IsConnected}");
            obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
            obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
        }

    }

    public override void OnConnected()
    {
        Debug.Log($"Photon callbacks | Congratulation, you made successful API call! {PhotonNetwork.CloudRegion }, ping: {PhotonNetwork.GetPing()}");
        obgForTextComponent.GetComponent<TMP_Text>().text = "Connected";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(8, 130, 0, 255);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Something went wrong: {cause}");
        obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
    }

    public override void OnCustomAuthenticationFailed(string messageDebug)
    {
        Debug.LogError($"Something went wrong: {messageDebug}");
        obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
    }
    
}