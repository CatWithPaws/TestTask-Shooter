using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SuplySpawner : MonoBehaviour,IPunObservable
{
    [SerializeField] private GameObject supplyChrist;

    private PhotonView photonView;

    float calldownTime = 10;
    bool canPick = true;

    [PunRPC]
    public void ThrowInCalldown() => StartCoroutine(CallDown());


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

   
    private void Start() => photonView = GetComponent<PhotonView>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerController>().photonView.IsMine && canPick)
        {
            other.gameObject.GetComponentInParent<PlayerController>().Heal();
            EventManager.UpdateUIEvent.Invoke();
            photonView.RPC(nameof(ThrowInCalldown), RpcTarget.All);
        }

    }

    private IEnumerator CallDown()
    {
        canPick = false;
        supplyChrist.SetActive(false);

        yield return new WaitForSeconds(calldownTime);

        supplyChrist.SetActive(true);
        canPick = true;
    }

    


   


}
