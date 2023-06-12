using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using Application = UnityEngine.Device.Application;

public class PosReceiver : MonoBehaviour
{
    [SerializeField] private int port = 24000;
    [SerializeField] private bool async = true;

    private UdpClient? udpClient;

    private void Start()
    {
        udpClient = new UdpClient(port);
        if (async)
        {
            UpdatePositionAsync();
        }
    }

    private void Update()
    {
        IPEndPoint? endPoint = null;
        while (async == false)
        {
            Apply(udpClient!.Receive(ref endPoint));
        }
    }

    [ContextMenu(nameof(UpdatePositionAsync))]
    private async void UpdatePositionAsync()
    {
        udpClient ??= new UdpClient(port);
        while (Application.isPlaying)
        {
            var data = await udpClient.ReceiveAsync();
            Apply(data.Buffer!);
        }
    }

    private void Apply(byte[] array)
    {
        var message = Encoding.ASCII.GetString(array);
        var split = message.Split('|')!;
        var x = float.Parse(split[0]);
        var y = float.Parse(split[1]);
        var z = float.Parse(split[2]);
        //var rot = new Vector3(x, y, z);
        transform.rotation = Quaternion.Euler(x, y, z);
        print(transform.rotation.eulerAngles);
    }
}