//using Photon.Pun;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public class UIControlOld : MonoBehaviour
//{
//    [SerializeField]
//    PlayerControllerOld playerController;
//    [SerializeField]
//    GunsBehaviourOld gunsBehaviour;
//    [SerializeField]
//    Text BulletsText,ResurectionText;
//    [SerializeField]
//    GameObject PauseMenu;
//    public bool isInPause = false;
//    [SerializeField] Image HPBar;
//    [SerializeField] GameObject LosePanel;

//    [SerializeField] Text LogText;
//    private void Start()
//    {
//        UpdateUI();
//        Cursor.visible = false;
//    }

//    private void Update()
//    {
//        if (!playerController.photonView.IsMine) return;
//        if (Input.GetKeyDown(KeyCode.Escape)&& !playerController.isDead)
//        {
//            if (Cursor.visible) Cursor.visible = false;
//            else Cursor.visible = true;
//            PauseMenu.SetActive(!PauseMenu.activeSelf);
//            isInPause = !isInPause;
//        }

//        LogText.text = GameControllerOld.Instance.usedGuns[0].ToString() + "\n";
//        LogText.text += GameControllerOld.Instance.usedGuns[1].ToString() + "\n";
//        LogText.text += GameControllerOld.Instance.usedGuns[2].ToString();
//    }

//    public void UpdateUI()
//    {
//        GunOld gun = gunsBehaviour._activeGun[(int)gunsBehaviour.currGun._currGun].GetComponent<GunOld>();
//        BulletsText.text = gun.currBullets.ToString() + "/" + gun.currStoredBullets.ToString();
//        HPBar.fillAmount = playerController.HP / playerController.MaxHP;
//        if (playerController.isDead && !LosePanel.activeSelf)
//            StartCoroutine(Ressurection());
//        LosePanel.SetActive(playerController.isDead);
//    }
//    public void LeaveRoom()
//    {
//        PhotonNetwork.LeaveRoom();
//        SceneManager.LoadScene("Menu");
//    }

//    IEnumerator Ressurection()
//    {
//        int time = 5;
//        while (time > 0)
//        {
//            ResurectionText.text = "You will ressurected by " + time + "seconds";
//            time--;
//            yield return new WaitForSeconds(1);

//        }
//        playerController.OnRessurection();
//    }

//}
