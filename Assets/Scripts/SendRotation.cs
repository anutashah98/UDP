using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

public class SendRotation : MonoBehaviour
{
    [SerializeField] private int _port = 25550;
    [SerializeField] private int _targetPort = 24000;
    [SerializeField] private float _turnSpeed = 10f;

    [NotNull] private string _prev = null!;

    private Rigidbody rb;
    private UdpClient _UdpClient;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        _UdpClient = new UdpClient(_port);
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        var turn = Input.GetAxis("Horizontal");
        Vector3 tr = new Vector3(0f, turn * _turnSpeed * Time.deltaTime, 0f);
        transform.Rotate(tr);

        var rot = transform.rotation.eulerAngles;
        var message = $"{rot.x} | {rot.y} | {rot.z}";
        
        
        if (_prev == message)
        {
            return;
        }
        
        
        var array = Encoding.ASCII.GetBytes(message);
        _UdpClient!.Send(array, array.Length, "localhost", _targetPort);
        _prev = message;

        //Debug.Log(transform.rotation);
    }
    
    

}