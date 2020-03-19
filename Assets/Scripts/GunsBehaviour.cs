using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GunsBehaviour : MonoBehaviour,IPunObservable
{
    public enum TypeGuns
    {
        Pistol = 0,
        Bow = 1,
        Launcher = 2,
    }

    public TypeGuns CurrGun;
    public Gun CurrGunData;
    public GameObject[] Guns;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject arrowPrefab, rocketPrefab;
    [SerializeField] private Transform arrowGunShootPos, launcherShootPos;
    [SerializeField] private PlayerController playerController;

    private delegate void ShootFunction();
    private bool canShoot = true;
    private float pistolRange = 100, pistolDamage = 10;
    private ShootFunction[] Shoot = new ShootFunction[3]; // 3 is a count of guns

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext((int)CurrGun);
        }
        else
        {
            CurrGun = (TypeGuns)(int)stream.ReceiveNext();
        }
    }
    public void UpdateCurrWeapon()
    {
        if (playerController.photonView.IsMine)
        {
            CurrGun = (TypeGuns)PhotonNetwork.LocalPlayer.CustomProperties["currGun"];
            CurrGunData = Guns[(int)CurrGun]?.GetComponent<Gun>();
            canShoot = true;
            EventManager.UpdateUIEvent.Invoke();
        }
        else
        {
            CurrGun = (TypeGuns)PhotonNetwork.PlayerListOthers[0].CustomProperties["currGun"];
        }

    }
    private void Start()
    {
        DisableGuns();
        if (playerController.photonView.IsMine) StartCoroutine(DeclareOriginal());
        else SetFromProperties();
        EventManager.UsedGunsChangeEvent.AddListener(UpdateCurrWeapon);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && playerController.photonView.IsMine && canShoot && !playerController.isDead && !playerController.uIController.IsPaused && CurrGunData.currBullets > 0)
        {
            CurrGunData.ConsumeAmmo(1);
            Shoot[(int)CurrGun]();
            StartCoroutine(ShootDelay(CurrGunData.ShootDelay));
            EventManager.UpdateUIEvent.Invoke();
        }
        else if (Input.GetMouseButtonDown(0) && CurrGunData.currBullets <= 0)
        {
            StartCoroutine(Reload());
        }
        if (Input.GetKeyDown(KeyCode.R) && CurrGunData.GetMagazineSize() != CurrGunData.currBullets)
        {
            StopCoroutine(nameof(ShootDelay));
            StartCoroutine(Reload());
        }
        UpdateActiveWeapon();
    }


    void DisableGuns()
    {
        foreach (var gun in Guns) 
            gun.SetActive(false);
    }
    void SetFromProperties()
    {
        CurrGun = (TypeGuns)PhotonNetwork.LocalPlayer.CustomProperties["currGun"];
        UpdateActiveWeapon();
    }
    void UpdateActiveWeapon()
    {
        foreach (GameObject gun in Guns)
            gun.SetActive(false);

        Guns[(int)CurrGun].SetActive(true);
    }
    
    IEnumerator DeclareOriginal()
    {
        DisableGuns();

        yield return new WaitForSeconds(.5f);

        Shoot[(int)TypeGuns.Pistol] = ShootPistol;
        Shoot[(int)TypeGuns.Bow] = ShootBow;
        Shoot[(int)TypeGuns.Launcher] = ShootLauncher;

        CurrGun = GetWeapon();

        EventManager.UsedGunsChangeEvent.Invoke();
    }

    TypeGuns GetWeapon()
    {
        bool[] usedGuns = NetworkSyncManager.Instance.GetUsedGuns();
        TypeGuns _result;

        do _result = (TypeGuns)Random.Range(0, 3);
        while (usedGuns[(int)_result]);

        NetworkSyncManager.Instance.SetCurrGun(_result);
        EventManager.UsedGunsChangeEvent.Invoke();

        return _result;
    }

    IEnumerator Reload()
    {
        playerController.uIController.SetAmmoText("Reloading...");
        canShoot = false;

        yield return new WaitForSeconds(CurrGunData.ReloadTime);

        playerController.GunsBehave.CurrGunData.Reload();
        EventManager.UpdateUIEvent.Invoke();
        canShoot = true;
    }

    void ShootPistol()
    {
        Ray _ray = cameraTransform.gameObject.GetComponent<Camera>().ViewportPointToRay(new Vector3(.5f, .5f, 0));
        RaycastHit _hit;
        if(Physics.Raycast(_ray,out _hit, Mathf.Infinity))
        {
            PlayerController _playerController = _hit.collider.gameObject.GetComponentInParent<PlayerController>();
            _playerController?.photonView.RPC(nameof(playerController.TakeDamage),_playerController.photonView.Owner,pistolDamage,playerController.photonView.Owner);
        }   
    }
    void ShootBow()
    {
        GameObject arrow = PhotonNetwork.Instantiate(
            arrowPrefab.name,
            arrowGunShootPos.position,
            cameraTransform.gameObject.GetComponent<CameraController>().CurrRotation);
    }
    void ShootLauncher()
    {
        GameObject rocket = PhotonNetwork.Instantiate(rocketPrefab.name,
            launcherShootPos.position,
            cameraTransform.gameObject.GetComponent<CameraController>().CurrRotation);
    }

    IEnumerator ShootDelay(int _seconds) {

        canShoot = false;

        yield return new WaitForSeconds(_seconds);

        canShoot = true;

    }

}
