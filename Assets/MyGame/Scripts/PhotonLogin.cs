using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLogin : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";
    [SerializeField]
    GameObject obgForTextComponent;
    [SerializeField]
    GameObject _loaderWrapper;

    public TMP_InputField PlayerNameInput;

    [SerializeField]
    GameObject _selectionWrapper;
    [SerializeField]
    GameObject _insideRoomWrapper;


    private Dictionary<string, RoomInfo> cachedRoomList;
    private Dictionary<string, GameObject> roomListEntries;
    private Dictionary<int, GameObject> playerListEntries;

    public GameObject RoomListContent;
    public GameObject RoomListEntryPrefab;



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
            Debug.LogError("Вы уже подключенны так что вызвана команда PhotonNetwork.JoinRandomRoom(); " + "Статус соеденения: " + PhotonNetwork.IsConnected);
        }
        else
        {
            string playerName = PlayerNameInput.text;

            if (!playerName.Equals(""))
            {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
                _loaderWrapper.SetActive(true);
            }
            else
            {
                Debug.LogError("Player Name is invalid.");
            }          
        }
    }
    public void OnJoinRandomRoomButtonClicked()
    {
        _selectionWrapper.SetActive(false);
        _insideRoomWrapper.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
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
        _loaderWrapper.SetActive(false);
        obgForTextComponent.GetComponent<TMP_Text>().text = "Connected";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(8, 130, 0, 255);
        _selectionWrapper.SetActive(true);
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            // Remove room from cached room list if it got closed, became invisible or was marked as removed
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            // Update cached room info
            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }
            // Add new room info to cache
            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private string _roomName;
    public void UpdateRoomName(string roomName)
    {
        _roomName = roomName; 
    }
    public void OnCreateRoomButtonClicked()
    {
                PhotonNetwork.CreateRoom(_roomName);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("blabla");
        /*
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();*/
    }
    private void ClearRoomListView()
    {
        foreach (GameObject entry in roomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        roomListEntries.Clear();
    }
    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomListEntryPrefab);
            entry.transform.SetParent(RoomListContent.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);

            roomListEntries.Add(info.Name, entry);
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogError($"Something went wrong: {cause}");
        _loaderWrapper.SetActive(false);
        obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
    }

    public override void OnCustomAuthenticationFailed(string messageDebug)
    {
        Debug.LogError($"Something went wrong: {messageDebug}");
        _loaderWrapper.SetActive(false);
        obgForTextComponent.GetComponent<TMP_Text>().text = "fail to connect";
        obgForTextComponent.GetComponent<TMP_Text>().color = new Color32(147, 23, 27, 255);
    }
    
}
