//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;

//public class CurrGunOld : MonoBehaviour,IPunObservable
//{
//    public GunsBehaviourOld.Guns _currGun;
//    [SerializeField] private GunsBehaviourOld gunsBehaviour;
//    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
//    {
//        if (stream.IsWriting)
//        {
//            stream.SendNext((int)_currGun);
//        }
//        else
//        {
//            _currGun = (GunsBehaviourOld.Guns)(int)stream.ReceiveNext();
//        }
//    }
//}
