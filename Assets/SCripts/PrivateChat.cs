using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrivateChat : MonoBehaviourPunCallbacks
{
    public TMP_InputField messageInput;
    public TextMeshProUGUI chatUserText;
    public ScrollRect chatScrollRect;
    public Transform content;
    public GameObject myBubblePrefab;
    public GameObject otherBubblePrefab;

    void Start()
    {
        UpdateChatHeader();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("OnPlayerEnteredRoom");
        UpdateChatHeader();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        UpdateChatHeader();
    }

    void UpdateChatHeader()
    {
        Debug.Log("UpdateChatHeader");
        foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
        {
            if (!p.IsLocal)
            {
                chatUserText.text = p.NickName;
                return;
            }
        }

        // If alone in room
        chatUserText.text = "Waiting for user...";
    }

    public void SendMessage()
    {
        Debug.Log("SendMessage");
        if (string.IsNullOrEmpty(messageInput.text))
            return;

        Player target = GetOtherPlayer();

        Debug.Log("SendMessage" + target);
        if (target != null)
        {
            photonView.RPC("ReceiveMessage", target, PhotonNetwork.NickName, messageInput.text);

            CreateBubble(messageInput.text, true);
            messageInput.text = "";
        }
    }

    void CreateBubble(string msg, bool isMine)
    {
        GameObject bubble = Instantiate(isMine ? myBubblePrefab : otherBubblePrefab, content);

        bubble.GetComponent<ChatBubble>().SetMessage(msg);

        LayoutRebuilder.ForceRebuildLayoutImmediate(bubble.GetComponent<RectTransform>());
        
        Canvas.ForceUpdateCanvases();
        chatScrollRect.verticalNormalizedPosition = 0f;
    }

    Player GetOtherPlayer()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (!p.IsLocal)
                return p;
        }
        return null;
    }

    [PunRPC]
    void ReceiveMessage(string sender, string msg)
    {
        CreateBubble(msg, false);
    }
}
