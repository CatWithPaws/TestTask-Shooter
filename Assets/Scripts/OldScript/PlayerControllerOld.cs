//using Photon.Pun;
//using Photon.Realtime;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

////Requirements

//[RequireComponent(typeof(Rigidbody))]

//public class PlayerControllerOld : MonoBehaviourPunCallbacks
//{
//    //-----------------------------------------------------------------\\
//    //   PlayerController  \\
//    //    By Valik8bit   \\
//    //-----------------------------------------------------------------\\

//    //Vars

//    private Transform _playeTransform;
//    [SerializeField] private Vector3 _velocity { get; set; }
//    [SerializeField] private float _speed { get; set; }
//    float _multiplier = 3;
//    public float MaxHP { get; } = 100;
//    public float HP = 100;

//    private Rigidbody _playerRB;
//    [SerializeField]
//    public GunsBehaviourOld gunsBehaviour;
    
//    public PhotonView photonView;
//    public UIControlOld uIControl;
//    public bool isDead { get; private set; }
//    // Start is called before the first frame update
//    void Start()
//    {
//        _playeTransform = transform;
//        _playerRB = GetComponent<Rigidbody>();
//        photonView = GetComponent<PhotonView>();
//        _speed = 10;
//        if (!photonView.IsMine)
//        {
//            gameObject.layer = LayerMask.NameToLayer("Default");
//        }
//    }
//    void FixedUpdate()
//    {
        
//        if (!photonView.IsMine) return;
//        if (!uIControl.isInPause && !isDead)
//        {
//            Vector3 horizontalVector = transform.right * Input.GetAxisRaw("Horizontal");
//            Vector3 verticalVector = transform.forward * Input.GetAxisRaw("Vertical");
//            _velocity = (horizontalVector + verticalVector) * _speed * (Input.GetKey(KeyCode.LeftShift) ? _multiplier : 1f);
//        }

//        _playerRB.velocity = _velocity;
//    }

//    public override void OnPlayerLeftRoom(Player otherPlayer)
//    {
//        GameControllerOld.Instance.usedGuns[(int)otherPlayer.CustomProperties["usedGun"]] = false ;
//    }
//    public override void OnPlayerEnteredRoom(Player newPlayer)
//    {
//        GameControllerOld.Instance.gunSpawner.ChangeSpawnWeapon();

//        GameControllerOld.Instance.usedGuns[(int)newPlayer.CustomProperties["usedGun"]] = true;
//    }
//    public void TakeDamage(float damage)
//    {
//        HP -= damage;
        
//        if(HP <= 0)
//        {
//            isDead = true;
//        }
//        uIControl.UpdateUI();
//    }

//    public void OnRessurection()
//    {
//        HP = MaxHP;
//        transform.position = GameControllerOld.Instance.GetSpawnPosition().position;
//        isDead = false;
//        gunsBehaviour._activeGun[(int)gunsBehaviour.currGun._currGun].GetComponent<GunOld>().SetMaxBullets();
//        transform.rotation = Quaternion.identity;
//        uIControl.UpdateUI();
//    }
//    [PunRPC]
//    public void UpdateUsedGunsInMaster(bool[] data)
//    {
//        GameControllerOld.Instance.usedGuns = data;
//    }
//}
