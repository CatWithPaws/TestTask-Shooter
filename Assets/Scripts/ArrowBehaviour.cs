using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PhotonView))]
public class ArrowBehaviour : MonoBehaviour
{
    [SerializeField] PhotonView photonView;
    private Vector3 arrowDirection;
    private float damage = 15;
    private Photon.Realtime.Player owner;
    private void Start()
    {
        arrowDirection = transform.forward * 12;
        photonView = GetComponent<PhotonView>();
        owner = photonView.Owner;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.localRotation = Quaternion.LookRotation(arrowDirection);
        transform.Translate(transform.forward * Time.deltaTime * 40,Space.World) ;
        arrowDirection.y -= Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        PlayerController _playerController = collision.gameObject.GetComponentInParent<PlayerController>();

        if (_playerController & !photonView.IsMine) _playerController.TakeDamage(damage, owner);

        PhotonNetwork.Destroy(gameObject);
    }
}
