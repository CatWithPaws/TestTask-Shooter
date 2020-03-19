using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour,IPunObservable
{
    public PhotonView PhotonView;
    [SerializeField] private GameObject[] Guns;
    private GunsBehaviour.TypeGuns currGun;
    private int calldownTime = 30;
    private bool canPick = true;
    [PunRPC]
    public void ThrowInCalldown()
    {
        StartCoroutine(CallDown());
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) { }

    void Start() {
        EventManager.UsedGunsChangeEvent.AddListener(UpdateSpawner);
        PhotonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (canPick)
            UpdateSpawner();
        else
            HideGuns();
    }
    public void UpdateSpawner() {     
        if(PhotonNetwork.PlayerList.Length > 1)
        {
            bool[] _usedGuns = NetworkSyncManager.Instance.GetUsedGuns();
            for(int i = 0; i < Guns.Length; i++)
            {
                Guns[i].SetActive(!_usedGuns[i]);
                if (Guns[i].activeSelf)  currGun = (GunsBehaviour.TypeGuns)i;
            }
        }
        else
            HideGuns();
    }

    void HideGuns()
    {
        for (int i = 0; i < Guns.Length; i++)
            Guns[i].SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<PlayerController>().photonView.IsMine && PhotonNetwork.PlayerList.Length > 1 && canPick)
        {
            other.gameObject.GetComponentInParent<PlayerController>().GunsBehave.CurrGun = currGun;
            NetworkSyncManager.Instance.SetCurrGun(currGun);
            EventManager.UsedGunsChangeEvent.Invoke();
            PhotonView. RPC(nameof(ThrowInCalldown),RpcTarget.All);
            EventManager.UpdateUIEvent.Invoke();
        }
    }

    IEnumerator CallDown()
    {
        canPick = false;

        yield return new WaitForSeconds(calldownTime);

        canPick = true;
    }
}
