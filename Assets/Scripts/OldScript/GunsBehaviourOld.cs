//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Photon.Pun;
//using UnityEngine.Events;

//public class GunsBehaviourOld : MonoBehaviour
//{
//    delegate void ShootFunction();
//    public enum Guns
//    {
//        Pistol = 0,
//        Bow = 1,
//        Launcher = 2
//    }
//    public float[] _gunDamage = { 10, 20, 40 };
//    float[] _gunReloadTime = { 1, 3, 5 };
//    public float[] _gunMaxBullets = {7,3,3};
//    bool[] _gunCanShoot = { true, true, true };
//    [SerializeField]
//    public GameObject[] _activeGun;
//    int _countGuns;
//    [SerializeField]
//    public GunsBehaviourOld Gun;


//    [SerializeField] private Transform _cameraTransform;
//    [SerializeField] private Transform _ArrowGunShootPos,_launcherShootPos;
//    ShootFunction[] Shoot;

//    private float _pistolRange = 100f;

//    [SerializeField] GameObject _arrowPrefab, _rocketPrefab;

//    [SerializeField] PlayerControllerOld _playerController;
//    [SerializeField] public CurrGunOld currGun;

//    private void Start()
//    {
//        if (!_playerController.photonView.IsMine) ChangeWeapon();
//        _countGuns = 3;

//        Shoot = new ShootFunction[_countGuns];

//        Shoot[(int)Guns.Pistol] = ShootPistol;
//        Shoot[(int)Guns.Bow] = ShootBow;
//        Shoot[(int)Guns.Launcher] = ShootLauncher;
        
        
        
//        StartCoroutine(GetWeapon());
//        GameControllerOld.ChangeWeaponEvent += GameController_ChangeWeaponEvent;
//        if (PhotonNetwork.IsMasterClient)
//        {
//            _playerController.photonView.RPC(nameof(_playerController.UpdateUsedGunsInMaster), RpcTarget.MasterClient);
//        }
//    }

//    private void GameController_ChangeWeaponEvent()
//    {
//        currGun._currGun = GameControllerOld.Instance.ActiveGun;

//        ChangeWeapon();
//    }

//    Guns GetRandomWeapon()
//    {
//        Guns result = Guns.Pistol;
//        int willResult = 0;
//        willResult = Random.Range(0, 3);
//        while (GameControllerOld.Instance.usedGuns[willResult])
//        {
//            willResult = Random.Range(0, 3);
//        }
//        GameControllerOld.Instance.usedGuns[willResult] = true;
//        result = (Guns)willResult;
//        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
//        hash.Add("usedGun", willResult);
//        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
//        GameControllerOld.Instance.gunSpawner.ChangeSpawnWeapon();
//        return result;
//    }
//    IEnumerator GetWeapon()
//    {
//        yield return new WaitForSeconds(1);
//        currGun._currGun = GetRandomWeapon();
//        GameControllerOld.Instance.usedGuns[(int)currGun._currGun] = true;
//        ChangeWeapon();
        
//    }
//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0) && _gunCanShoot[(int)currGun._currGun] && _playerController.photonView.IsMine && !_playerController.uIControl.isInPause)
//        {
//            StartCoroutine(Reloader(_gunReloadTime[(int)currGun._currGun]));
//            int currBullets = _activeGun[(int)currGun._currGun].GetComponent<GunOld>().currBullets;
//            _activeGun[(int)currGun._currGun].GetComponent<GunOld>().SetCurrBullets(currBullets - 1);
//            _playerController.uIControl.UpdateUI();
//            Shoot[(int)currGun._currGun]();
//            ChangeWeapon();
//        }
//        Debug.DrawRay(_cameraTransform.position, transform.forward * _pistolRange);
//    }

//    public void ChangeWeapon()
//    {
//        Guns _currGun;
//        if (_playerController.photonView.IsMine) {
//            _currGun = (Guns)(int)PhotonNetwork.LocalPlayer.CustomProperties["usedGun"]; 
//        }
//        else
//        {
//            _currGun = (Guns)(int)PhotonNetwork.PlayerListOthers[0].CustomProperties["usedGuns"];
//        }
//        _activeGun[(int)Guns.Pistol].SetActive(_currGun == Guns.Pistol);
//        _activeGun[(int)Guns.Bow].SetActive(_currGun == Guns.Bow);
//        _activeGun[(int)Guns.Launcher].SetActive(_currGun == Guns.Launcher);
//        _playerController.uIControl.UpdateUI();
       
//    }

//    void ShootPistol()
//    {
//        GetShootRaycastHit(_cameraTransform.position,_pistolRange).collider.GetComponent<PlayerControllerOld>()?.TakeDamage(_gunDamage[(int)currGun._currGun]);
//        print("bruh...");
//    }
//    void ShootBow()
//    {
//        GameObject arrow = PhotonNetwork.Instantiate(_arrowPrefab.name,
//            _ArrowGunShootPos.position,
//            Quaternion.Euler( _cameraTransform.gameObject.GetComponent<CameraController>().CurrRotation));
//    }
//    void ShootLauncher()
//    {
//        GameObject rocket = PhotonNetwork.Instantiate(_rocketPrefab.name, 
//            _launcherShootPos.position,
//            Quaternion.Euler(_cameraTransform.gameObject.GetComponent<CameraController>().CurrRotation));
//    }

//    RaycastHit GetShootRaycastHit(Vector3 rayStartPos,float rayLenght)
//    {
//        RaycastHit hit;
//        Physics.Raycast(_cameraTransform.position, transform.forward * _pistolRange, out hit);
//        return hit;
//    }

//    IEnumerator Reloader(float reloadTime)
//    {
//        _gunCanShoot[(int)currGun._currGun] = false;
//        yield return new WaitForSeconds(reloadTime);
//        _gunCanShoot[(int)currGun._currGun] = true;
//    }
//}
