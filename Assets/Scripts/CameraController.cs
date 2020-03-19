using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]

public class CameraController : MonoBehaviour
{
    //-----------------------------------------------------------------\\
                        //   CameraController  \\
    //-----------------------------------------------------------------\\
    
    public Quaternion CurrRotation;


    private Transform cameraTransform;
    [SerializeField] private Transform playerTransform;
    private float sensivity = 5;
    float rotX = 0,rotY = 0;
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private Transform cameraPos;
    [SerializeField] private GameObject PlayerGraphics;
    [SerializeField] private UIController uIController;
    [SerializeField] private PlayerController playerController;


    private void Start()
    {
        cameraTransform = transform;
        if (!playerController.photonView.IsMine) {
            cameraTransform.gameObject.GetComponent<Camera>().enabled = false;
            GetComponent<AudioListener>().enabled = false;
        }

    }

    private void Update()
    {
        if (uIController.IsPaused || !playerController.photonView.IsMine || playerController.isDead) return;

        rotX += Input.GetAxis("Mouse X") * sensivity;
        rotY -= Input.GetAxis("Mouse Y") * sensivity;
        rotY = Mathf.Clamp(rotY, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(rotY, 0, 0);
        playerTransform.localRotation = Quaternion.Euler(0, rotX, 0);
        cameraTransform.position = cameraPos.position;
        CurrRotation = Quaternion.Euler(rotY, rotX, 0);
    }
}
