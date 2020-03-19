//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;

//public class GameControllerOld : MonoBehaviourPunCallbacks,IPunObservable
//{
//    public static GameControllerOld Instance;
//    public GameObject[] SpawnPoints;
//    public bool[] usedGuns = new bool[3];
//    public GunSpawnerOld gunSpawner;
//    public GunsBehaviourOld.Guns activeGun;
//    public delegate void ChangeActiveWeaponEvent();
//    public static event ChangeActiveWeaponEvent ChangeWeaponEvent;
//    public GunsBehaviourOld.Guns ActiveGun {
//        get {
//            return activeGun;
//        }
//        set
//        {
//            if(activeGun != value)
//            {
//                usedGuns[(int)activeGun] = false;
//                usedGuns[(int)value] = true;
//                activeGun = value;
//                PhotonNetwork.LocalPlayer.CustomProperties["usedGun"] = value;
//            }
//            ChangeWeaponEvent?.Invoke();
//        }
//    }
    
//    private void Awake()
//    {
//        Instance = this;
//        Transform SpawnPoint = GetSpawnPosition();
//        PhotonNetwork.Instantiate("Player", SpawnPoint.position, Quaternion.identity);
//    }
//    public Transform GetSpawnPosition()
//    {
//        return SpawnPoints[Random.Range(0, SpawnPoints.Length - 1)].transform;
//    }

//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.IsWriting)
//        {
//            stream.SendNext(usedGuns);
//        }
//        else
//        {
//            usedGuns = (bool[])stream.ReceiveNext();
//        }
//    }

//}
