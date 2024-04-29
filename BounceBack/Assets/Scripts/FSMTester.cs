using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMTester : MonoBehaviour
{
    public KeyCode Title;
    public KeyCode MainMenu;
    public KeyCode Options;
    public KeyCode Credits;
    public KeyCode Gameplay;
    public KeyCode GameOver;
    public KeyCode Pause;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance == null)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    public void ProcessInput()
    {
        if (Input.GetKey(Title))
        {
            GameManager.Instance.gameState = GameManager.GameState.Title;
        }

        if (Input.GetKey(MainMenu))
        {
            GameManager.Instance.gameState = GameManager.GameState.MainMenu;
        }

        if (Input.GetKey(Options))
        {

            GameManager.Instance.gameState = GameManager.GameState.Options;
        }

        if (Input.GetKey(Credits))
        {
            GameManager.Instance.gameState = GameManager.GameState.Credits;
        }

        if (Input.GetKey(Gameplay))
        {
            GameManager.Instance.gameState = GameManager.GameState.Gameplay;
        }

        if (Input.GetKey(GameOver))
        {
            GameManager.Instance.gameState = GameManager.GameState.GameOver;
        }

        if (Input.GetKey(Pause))
        {
            GameManager.Instance.gameState = GameManager.GameState.Pause;
        }
    }
}
