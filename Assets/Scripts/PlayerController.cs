using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public static int RespawnTime = 5;
    public UIController uIController;
    public bool isDead = false;
    public int Score = 0;
    public GunsBehaviour GunsBehave;
    public float MaxHp { get; } = 100;
    public float Hp { get; private set; }
    [SerializeField] private GameObject playerMeshGameObject;
    [SerializeField] private SpawnPlayer spawnPlayer;
    Transform playerTrasform;
    Rigidbody playerRB;
    Vector3 velocity;
    float Speed { get; set; } = 10;
    int Multiplier { get; set; } = 2;

    [PunRPC]
    void EndGame() => PhotonNetwork.LeaveRoom();

    [PunRPC]
    public void UpdateTop() => EventManager.UpdateTopListEvent.Invoke();

    [PunRPC]
    public void AddScore(int value)
    {
        NetworkSyncManager.Instance.AddScore(1);
        CheckWin();
    }
    [PunRPC]
    public void TakeDamage(float _damage, Photon.Realtime.Player _damager)
    {
        Hp -= _damage;
        isDead = Hp <= 0;
        EventManager.UpdateUIEvent.Invoke();
        if (isDead)
        {
            AddScoreToDamager(_damager);
            EventManager.DeadEvent.Invoke();
        }
    }

    public void Heal() => Hp = MaxHp;
    private void StartRespawn() => StartCoroutine(Respawn());
    
    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerTrasform = transform;
        gameObject.layer = photonView.IsMine ? 10 : 0; // 10 - MainPlayer Layer,0 - Default Layer
        playerMeshGameObject.layer = photonView.IsMine ? 8 : 0; // 8 - IgnoreCamera Layer, 0 - Default Layer
        Hp = MaxHp;
        EventManager.DeadEvent.AddListener(StartRespawn);
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (!uIController.IsPaused && !isDead)
        {
            Vector3 horizonVector = playerTrasform.right * Input.GetAxisRaw("Horizontal");
            Vector3 vertVector = playerTrasform.forward * Input.GetAxisRaw("Vertical");
            velocity = (horizonVector + vertVector) * Speed;
            velocity *= Input.GetKey(KeyCode.LeftShift) ? Multiplier : 1;
        }
        else velocity = Vector3.zero;
        playerRB.velocity = velocity;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading) Score = (int)stream.ReceiveNext();
        else stream.SendNext(Score);
    }

    void AddScoreToDamager(Photon.Realtime.Player _damager)
    {
        playerMeshGameObject.SetActive(false);
        EventManager.DeadEvent.Invoke();
        if (photonView.IsMine) photonView.RPC(nameof(AddScore), _damager, 1);
    }

    private void CheckWin()
    {
        Dictionary<string, int> scores = NetworkSyncManager.Instance.GetScoreList();
        foreach (var scoreOne in scores)
            if (scoreOne.Value >= 10)
                photonView.RPC(nameof(EndGame), RpcTarget.All);
    }

    public void OnRespawn()
    {
        Hp = MaxHp;
        isDead = false;
        GunsBehave.CurrGunData.SetMaxBullets();
        EventManager.UpdateUIEvent.Invoke();
        transform.position = SpawnPlayer.Instance.SpawnersTransform[Random.Range(0, SpawnPlayer.Instance.SpawnersTransform.Length - 1)].position;
        transform.rotation = Quaternion.identity;
        playerMeshGameObject.SetActive(true);
    }

    IEnumerator Respawn()
    {
        if (!photonView.IsMine) {
            playerMeshGameObject.SetActive(false);
            yield return new WaitForSeconds(PlayerController.RespawnTime);
            playerMeshGameObject.SetActive(true);
        }
        else yield return null;
    }
    
   

    
    
    
    

    
}
