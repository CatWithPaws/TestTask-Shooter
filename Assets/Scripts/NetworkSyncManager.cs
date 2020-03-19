using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkSyncManager : MonoBehaviourPunCallbacks
{
    public static NetworkSyncManager Instance;
    public GunSpawner GunSpawner;

    public override void OnLeftRoom() => PhotonNetwork.LoadLevel("Menu");

    public bool[] GetUsedGuns()
    {
        bool[] _usedGuns = new bool[3];
        foreach (Photon.Realtime.Player _player in PhotonNetwork.PlayerList)
        {
            if (_player == null) continue;

            object _currGun;

            if (_player.CustomProperties.TryGetValue("currGun", out _currGun))
                _usedGuns[(int)(GunsBehaviour.TypeGuns)_currGun] = true;

        }
        return _usedGuns;
    }
    public Dictionary<string, int> GetScoreList()
    {
        Dictionary<string, int> _scoreList = new Dictionary<string, int>();

        foreach (var _player in PhotonNetwork.PlayerList)
        {
            _scoreList[_player.NickName] = (int)_player.CustomProperties["score"];
            print(_player.NickName + ((int)_player.CustomProperties["score"]).ToString());
        }

        return _scoreList;
    }
    public void SetCurrGun(GunsBehaviour.TypeGuns _value)
    {
        Hashtable _hash = new Hashtable();

        _hash["currGun"] = _value;
        _hash["score"] = (int)PhotonNetwork.LocalPlayer.CustomProperties["score"];

        PhotonNetwork.LocalPlayer.SetCustomProperties(_hash);
    }
    public void AddScore(int value)
    {
        Hashtable _hash = new Hashtable();

        _hash["currGun"] = (GunsBehaviour.TypeGuns)PhotonNetwork.LocalPlayer.CustomProperties["currGun"];
        _hash["score"] = (int)PhotonNetwork.LocalPlayer.CustomProperties["score"] + value;

        PhotonNetwork.LocalPlayer.SetCustomProperties(_hash);
    }

    private void Awake()
    {
        Instance = this;
        InitCustomProperties();
    }

    private void InitCustomProperties (){
        Hashtable _hash = new Hashtable();

        _hash["currGun"] = GunsBehaviour.TypeGuns.Pistol;
        _hash["score"] = 0;

        PhotonNetwork.LocalPlayer.SetCustomProperties(_hash);
    }

    

    
}
