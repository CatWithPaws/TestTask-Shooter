using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AmmoSpawner : MonoBehaviour, IPunObservable
{
    [SerializeField] private GameObject ammoBox;
    [SerializeField] private float calldownTime = 20;
    private bool canPick = true;
    private PhotonView photonView;

    [PunRPC]
    public void ThrowInCalldown() => StartCoroutine(CallDown());

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

    private void Start() => photonView = GetComponent<PhotonView>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerController>().photonView.IsMine && canPick)
        {
            other.gameObject.GetComponentInParent<PlayerController>().GunsBehave.CurrGunData.SetMaxBullets();
            EventManager.UpdateUIEvent.Invoke();
            photonView.RPC(nameof(ThrowInCalldown), RpcTarget.All);
        }

    }

    IEnumerator CallDown()
    {
        canPick = false;
        ammoBox.SetActive(false);
        yield return new WaitForSeconds(calldownTime);
        ammoBox.SetActive(true);
        canPick = true;
    }
    


}
