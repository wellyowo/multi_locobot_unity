using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ImageSyn : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    private Texture2D texture2D;
    private PhotonView photonView;
    private byte[] imageData;

    void Start()
    {
        texture2D = new Texture2D(1, 1);
        photonView = GetComponent<PhotonView>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }

    void Update()
    {

    }

    [PunRPC]
    public void getImage(byte[] imageData)
    {
        this.imageData = imageData;
        texture2D.LoadImage(this.imageData);
        texture2D.Apply();
        meshRenderer.material.SetTexture("_MainTex", texture2D);

    }
}

