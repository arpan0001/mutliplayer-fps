using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro; // Added using directive for TextMeshProUGUI

public class RoomList : MonoBehaviourPunCallbacks
{
    public static RoomList Instance;

    public GameObject roomManagerGameobject;
    public RoomManager roomManager;

    [Header("UI")] public Transform roomListParent;
    public GameObject roomListItemPrefab;

    private List<RoomInfo> cachedRoomlist = new List<RoomInfo>();

    public void ChangeRoomToCreateName(string _roomName)
    {
        roomManager.roomNameToJoin = _roomName;
    }

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        // Precautions
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }

        yield return new WaitUntil(() => !PhotonNetwork.IsConnected);

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (cachedRoomlist.Count <= 0)
        {
            cachedRoomlist = roomList;
        }
        else
        {
            foreach (var room in roomList)
            {
                for (int i = 0; i < cachedRoomlist.Count; i++)
                {
                    if (cachedRoomlist[i].Name == room.Name)
                    {
                        List<RoomInfo> newList = new List<RoomInfo>(cachedRoomlist); // Create a new list

                        if (room.RemovedFromList)
                        {
                            newList.Remove(newList[i]);
                        }
                        else
                        {
                            // Update the room properties if needed
                            newList[i] = room;
                        }

                        cachedRoomlist = newList;
                    }
                }
            }
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (Transform roomItem in roomListParent)
        {
            Destroy(roomItem.gameObject);
        }

        foreach (var room in cachedRoomlist)
        {
            GameObject roomItem = Instantiate(roomListItemPrefab, roomListParent);

            roomItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = room.Name;
            roomItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = room.PlayerCount + "/16";

            roomItem.GetComponent<RoomItemButton>().RoomName = room.Name;
        }
    }

    public void JoinRoomByName(string _name)
    {
        roomManager.roomNameToJoin = _name;
        roomManagerGameobject.SetActive(true);
        gameObject.SetActive(false);
    }
}
