using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UDPServer : MonoBehaviour
{
    UdpClient listener;
    IPEndPoint groupEP;
    Thread receiveThread;
    bool isApplicationQuitting = false;
    int port = 61000;

    void Start()
    {
        groupEP = new IPEndPoint(IPAddress.Any, port);
        listener = new UdpClient(groupEP);
        receiveThread = new Thread(new ThreadStart(ThreadMethod));
        receiveThread.IsBackground = true;
        
        receiveThread.Start();

        Debug.Log("Listening on UDP port " + port);
            
    }

    private void ThreadMethod()
    {
        while (!isApplicationQuitting)
        {
            try
            {
                byte[] bytes = listener.Receive(ref groupEP);
                string receivedData = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
                //Debug.Log("Received: " + receivedData);
                // Assuming SortData is implemented correctly elsewhere in this class
                if (Main.Instance.load)
                {
                    Main.Instance.SortData(receivedData);
       
                }
            }
            catch (Exception e)
            {
                if (!isApplicationQuitting)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join(); // Gracefully end the thread
        }
        listener.Close();
    }

    // Make sure to define SortData method and any other used variables.
}
