using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks
{
    public int health;
    public bool isLocalPlayer;

    public RectTransform healthBar;
    private float originalHealthBarSize;

    [Header("UI")]
    public TextMeshProUGUI healthText;

    private void Start()
    {
        originalHealthBarSize = healthBar.sizeDelta.x;
    }

    [PunRPC]
    public void TakeDamage(int _damage)
    {
        if (!photonView.IsMine) return; // Ensure that only the local player processes damage

        health -= _damage;

        healthBar.sizeDelta = new Vector2(x: originalHealthBarSize * health / 100f, healthBar.sizeDelta.y);

        healthText.text = health.ToString();

        if (health <= 0)
        {
            photonView.RPC("Die", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void Die()
    {
        if (isLocalPlayer)
        {
            RoomManager.instance.SpawnPlayer();
            RoomManager.instance.deaths++;
            RoomManager.instance.SetHashes();
        }

        PhotonNetwork.Destroy(gameObject);
    }
}
