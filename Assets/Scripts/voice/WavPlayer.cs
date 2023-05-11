﻿using System.Collections;
using System.Collections.Generic;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.MessageTypes.Std;
using UnityEngine;

public class WavPlayer : MonoBehaviour
{
    RosSocket rosSocket;
    public string WebSocketIP; //IP address
    private string RosBridgeServerUrl; //IP address

    public string topic_name;
    private string audio_data;

    private float[] LeftChannel = new float[16000];
    // private float[] StereoChannel = new float[88200];

    private int pos = 0, SampleCount = 0;
    private bool isMessageReceived = false;
    // Start is called before the first frame update
    void Start()
    {
        RosBridgeServerUrl = WebSocketIP;
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol(RosBridgeServerUrl));
        Debug.Log("Established connection with ros(WAV PLAYER)");

        audio_data = rosSocket.Subscribe<std_msgs.UInt8MultiArray>(topic_name, data_process);
        // audio_data = rosSocket.Subscribe<std_msgs.Int16MultiArray>(topic_name, data_process);
        // WavPlay();
    }
    // Update is called once per frame
    void Update()
    {
        if (isMessageReceived)
        {
            AudioClip audioClip = AudioClip.Create("WavFileSound", 16000, 1, 16000, false);
            audioClip.SetData(LeftChannel, 0);
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioClip;
            audio.Play();
            // Debug.Log(isMessageReceived);
            isMessageReceived = false;
        }

        /*if (isMessageReceived)
        {
            AudioClip audioClip = AudioClip.Create("WavFileSound", 44100, 2, 44100, false);
            audioClip.SetData(StereoChannel, 0);
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = audioClip;
            audio.Play();
            // Debug.Log(isMessageReceived);
            isMessageReceived = false;
        }*/
    }
    public void WavPlay()
    {
        WWUtils.Audio.WAV wav = new WWUtils.Audio.WAV("rode3.wav");
        Debug.Log(wav);
        AudioClip audioClip = AudioClip.Create("WavFileSound", wav.SampleCount, 2, wav.Frequency, false);
        audioClip.SetData(wav.StereoChannel, 0);
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = audioClip;
        audio.Play();
    }
    private void data_process(std_msgs.UInt8MultiArray message)
    {
        // Debug.Log(message.data[0]);
        while (pos < 320)
        {
            LeftChannel[SampleCount] = bytesToFloat(message.data[pos], message.data[pos + 1]);
            SampleCount++;
            pos += 2;
        }
        if (SampleCount == 16000)
        {
            SampleCount = 0;
            isMessageReceived = true;
        }
        pos = 0;
    }
    /*private void data_process(std_msgs.Int16MultiArray message)
    {
        // Debug.Log(message.data[0]);
        while (pos < 3528)
        {
            StereoChannel[SampleCount] = int16ToFloat(message.data[pos], message.data[pos + 1]);
            SampleCount++;
            pos += 2;
            StereoChannel[SampleCount] = int16ToFloat(message.data[pos], message.data[pos + 1]);
            SampleCount++;
            pos += 2;
        }
        if (SampleCount == 88200)
        {
            SampleCount = 0;
            isMessageReceived = true;
        }
        pos = 0;
    }*/
    // convert two bytes to one float in the range -1 to 1
    static float bytesToFloat(byte firstByte, byte secondByte)
    {
        // convert two bytes to one short (little endian)
        short s = (short)((secondByte << 8) | firstByte);
        // Debug.Log(s / 32768.0F);
        // convert to range from -1 to (just below) 1
        return s / 32768.0F;
    }
    // convert two int16 to one float in the range -1 to 1
    static float int16ToFloat(short firstByte, short secondByte)
    {
        // convert two bytes to one short (little endian)
        int s = (int)((secondByte << 16) | firstByte);
        // convert to range from -1 to (just below) 1
        return s / 2147483648.0F;
    }
}
