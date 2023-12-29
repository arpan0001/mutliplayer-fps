using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using Photon.Pun;

public class LeaderBoard : MonoBehaviour
{
    public GameObject playersHolder;

    [Header("Options")]
    public float refreshRate = 1f;

    [Header("UI")]
    public GameObject[] slots;
    [Space]
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI[] nameTexts;
    public TextMeshProUGUI[] kdTexts;

    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 0f, refreshRate);
    }

    public void Refresh()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        var sortedPlayerList = PhotonNetwork.PlayerList.ToList();

        int i = 0;
        foreach (var player in sortedPlayerList)
        {
            slots[i].SetActive(true);

            if (player.NickName == "")
                player.NickName = "unnamed";

            nameTexts[i].text = player.NickName;

            // Replace this with your actual logic to get the player's score
            int playerScore = GetPlayerScore(player);
            scoreTexts[i].text = playerScore.ToString();

            if (player.CustomProperties["kills"] != null)
            {
                kdTexts[i].text = player.CustomProperties["kills"] + "/" + player.CustomProperties["deatns"];

            }
            else
            {
                kdTexts[i].text = "0/0";
            }


                



            i++;
        }
    }

    private int GetPlayerScore(Photon.Realtime.Player player)
    {
        // Replace this with your actual logic to get the player's score
        // For example, you might have a custom property named "Score" in the player's custom properties
        if (player.CustomProperties.TryGetValue("Score", out object scoreObj) && scoreObj is int)
        {
            return (int)scoreObj;
        }

        return 0; // Default score if not found
    }

    private void Update()
    {
        playersHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
