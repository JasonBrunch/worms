using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public TMP_InputField chatInputField;
    public RectTransform chatContent;
    public GameObject chatMessagePrefab;
    public Button sendButton;

    private ChatClient chatClient;
    private string chatChannel = "LobbyChannel";

    void Start()
    {
        sendButton.onClick.AddListener(OnSendMessage); // Hook up the button click event

        chatClient = new ChatClient(this);
        chatClient.UseBackgroundWorkerForSending = true; // Ensures chat works in WebGL
        ConnectToChat();
    }

    void Update()
    {
        chatClient.Service();
    }

    private void ConnectToChat()
    {
        chatClient.ChatRegion = "US"; // Set your region
        PhotonNetwork.PhotonServerSettings.AppSettings.Protocol = ExitGames.Client.Photon.ConnectionProtocol.WebSocketSecure;
        AuthenticationValues authValues = new AuthenticationValues(PhotonNetwork.NickName);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", authValues);
    }

    private void OnSendMessage()
    {
        if (!string.IsNullOrEmpty(chatInputField.text))
        {
            chatClient.PublishMessage(chatChannel, chatInputField.text);
            chatInputField.text = "";
            chatInputField.ActivateInputField(); // Keep the input field focused
        }
    }

    public void OnConnected()
    {
        chatClient.Subscribe(chatChannel);
    }

    public void OnDisconnected()
    {
        Debug.Log("Chat disconnected.");
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            string sender = senders[i];
            string message = messages[i].ToString();
            DisplayMessage(sender, message);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        // Handle private messages if needed
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subscribed to channel: " + string.Join(", ", channels));
    }

    public void OnUnsubscribed(string[] channels)
    {
        Debug.Log("Unsubscribed from channel: " + string.Join(", ", channels));
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        // Handle status updates if needed
    }

    public void OnUserSubscribed(string channel, string user)
    {
        // Handle user subscriptions if needed
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        // Handle user unsubscriptions if needed
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log($"DebugReturn: {message}");
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log($"OnChatStateChange: {state}");
    }

    private void DisplayMessage(string sender, string message)
    {
        GameObject chatMessage = Instantiate(chatMessagePrefab, chatContent);
        chatMessage.GetComponentInChildren<TMP_Text>().text = $"{sender}: {message}";
    }
}