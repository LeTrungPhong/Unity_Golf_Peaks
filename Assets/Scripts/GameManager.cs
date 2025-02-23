using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stateGame;
    private GameObject player;
    public bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            checkState();
        }
    }

    void checkState()
    {
        if (player.transform.position.y < -5)
        {
            GameOver();
            isGameOver = true;
        }
    }

    void GameOver()
    {
        stateGame.text = "Game Over";
        stateGame.gameObject.SetActive(true);
    }

    void GameWin()
    {
        stateGame.text = "Game Win";
        stateGame.gameObject.SetActive(true);
    }
}
