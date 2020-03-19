//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GunSpawnerOld : MonoBehaviour
//{
//    [SerializeField] GameObject[] Weapon;
//    [SerializeField] GunsBehaviourOld.Guns gun;
//    int CallDownTime = 10;
//    public void ChangeSpawnWeapon()
//    {
//        if(PhotonNetwork.PlayerList.Length > 1)
//        {
//            for (int i = 0; i < Weapon.Length; i++)
//            {
//                Weapon[i].SetActive(!GameControllerOld.Instance.usedGuns[i]);
//                if (!GameControllerOld.Instance.usedGuns[i])
//                {
//                    gun = (GunsBehaviourOld.Guns)i;
//                }
//            }

//        }
//        else
//        {
//            for (int i = 0; i < Weapon.Length; i++)
//            {
//                Weapon[i].SetActive(false);
//            }
//        }
//    }
//    IEnumerator CallDown()
//    {
//        foreach (var weapon in Weapon)
//        {
//            weapon.SetActive(false);
//        }
//        yield return new WaitForSeconds(CallDownTime);
//        ChangeSpawnWeapon();
//    }
//    private void OnTriggerEnter(Collider other)
//    {
//        if (PhotonNetwork.PlayerList.Length <= 1) return;
//        PhotonView otherPhotonView;
//        if (otherPhotonView = other.gameObject.GetComponent<PhotonView>())
//        {
//            if (otherPhotonView.IsMine)
//            {
//                PhotonNetwork.LocalPlayer.CustomProperties[nameof(GameControllerOld.Instance.activeGun)] = gun;
//                StartCoroutine(CallDown());
//            }
//        }
//    }

//}
