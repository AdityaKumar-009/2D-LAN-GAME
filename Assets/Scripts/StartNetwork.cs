using Unity.Netcode;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.Netcode.Transports.UTP;
using System;

public class StartNetwork : MonoBehaviour
{
    public TextMeshProUGUI Host_IP;
    public TMP_InputField IP_Field;
    public Button Join_Btn;

    private void Start()
    {
        if (SceneManage.instance.OnBtnClicked == 0)
        {
            string ip = GetLocalIPAddress();
            Host_IP.text = (ip!=null)?ip:"IP Not Found!";
            StartAsHost(ip);
        }
        else
        {
            IP_Field.gameObject.SetActive(true);
            Join_Btn.gameObject.SetActive(true);
        }
    }

    public void StartAsHost(string ip)
    {
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            ip,  // The IP address is a string
            12321 // The port number is an unsigned short
        );
        NetworkManager.Singleton.StartHost();
    }

    public void JoinAsClient()
    {
        try
        {
            string ip = IP_Field.text;
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
                ip,  // The IP address is a string
                12321 // The port number is an unsigned short
            );
            NetworkManager.Singleton.StartClient();
            IP_Field.gameObject.SetActive(false);
            Join_Btn.gameObject.SetActive(false);
        }
        catch(Exception e)
        {
            Debug.LogException(e);
        }
    }

    public static string GetLocalIPAddress()
    {
        var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        return null;
    }

}
