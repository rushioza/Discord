using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.NickName = "User_" + Random.Range(1000, 9999);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom(
            "PrivateRoom",
            new RoomOptions { MaxPlayers = 2 },
            TypedLobby.Default
        );
    }

  /*   public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
    } */
}
