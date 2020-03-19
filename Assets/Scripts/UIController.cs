using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class UIController : MonoBehaviour
{
    public bool IsPaused { get; set; } = false;

    [SerializeField] private Text ammoText;
    [SerializeField] private Image hpImage;
    [SerializeField] private GameObject deathScreen,pauseScreen;
    [SerializeField] private Text deathText;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Text[] _topPlayers;

    public void SetAmmoText(string _text) => ammoText.text = _text;
    public void LeftRoom() => PhotonNetwork.LeaveRoom();
    private void OnDead() => StartCoroutine(Respawn());

    private void Awake()
    {
        EventManager.UpdateUIEvent.AddListener(UpdateUI);
        EventManager.DeadEvent.AddListener(OnDead);
        EventManager.UpdateTopListEvent.AddListener(UpdateTopList);
        CleanTopList();
    }

    private void CleanTopList()
    {
        foreach (Text text in _topPlayers)
        {
            text.text = "";
        }
    }

    private void UpdateUI()
    {
        if (!playerController.photonView.IsMine) return;
        Gun _gunData = playerController.GunsBehave.CurrGunData;
        if (_gunData == null) return;
        ammoText.text = _gunData.currBullets.ToString() + " / " + _gunData.currStoredBullets;
        hpImage.fillAmount = playerController.Hp / playerController.MaxHp;
    }

    private void Update()
    {
        EventManager.UpdateTopListEvent.Invoke();
        if (Input.GetKeyDown(KeyCode.Escape)) pauseScreen.SetActive(!pauseScreen.activeSelf); IsPaused = pauseScreen.activeSelf;
    }

    private void UpdateTopList()
    {
        if (PhotonNetwork.PlayerList.Length < 2) CleanTopList();
        Photon.Realtime.Player[] _players = PhotonNetwork.PlayerList;
        _players.OrderByDescending(p => p.CustomProperties["score"]);
        for(int i = 0; i < _players.Length; i++)
        {
            _topPlayers[i].text = _players[i].NickName + "\t" + ((int)_players[i].CustomProperties["score"]).ToString();
        }
    }

    private IEnumerator Respawn()
    {
        if (playerController.photonView.IsMine)
        {
            deathScreen.SetActive(true);
            for (int i = PlayerController.RespawnTime; i > 0;i--)
            {
                deathText.text = "You will respawned after " + i.ToString();
                yield return new WaitForSeconds(1);
            }
            deathScreen.SetActive(false);
            playerController.OnRespawn();
        }
    }
}
