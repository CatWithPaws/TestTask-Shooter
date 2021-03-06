﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public Transform[] SpawnersTransform;
    public static SpawnPlayer Instance;

    private object Player;
    
    private void Start()
    {
        Instance = this;
        Vector3 _spawnPos = SpawnersTransform[Random.Range(0, SpawnersTransform.Length)].position;
        PhotonNetwork.Instantiate(nameof(Player), _spawnPos, Quaternion.identity);
    }
    
}
