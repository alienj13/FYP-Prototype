using UnityEngine;
using NativeWebSocket;
using System.Text;

public class WebSocketConnection : MonoBehaviour
{
    WebSocket websocket;

    async void Start()
    {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnMessage += (bytes) =>
        {
            Debug.Log("Received OnMessage!");
            string receivedData = Encoding.UTF8.GetString(bytes);
            if(Main.Instance.play){
            Main.Instance.SortData(receivedData);
            }
        };

        websocket.OnError += (string errMsg) =>
        {
            Debug.LogError("Error! " + errMsg);
        };

        websocket.OnClose += (WebSocketCloseCode code) =>
        {
            Debug.Log("Connection closed with code: " + code.ToString());
        };

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (websocket != null)
            websocket.DispatchMessageQueue();
#endif
    }

    async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}

