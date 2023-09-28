using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] TMP_Text _ammoScore;
    [SerializeField] private TMP_Text _buildingsScore;
    [SerializeField] private TMP_Text _finalScore;

    void Start()
    {
        // Hide screen at the start of the game.
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets up the values needed to be displayed.
    /// </summary>
    /// <param name="ammoScore">Score bonus from ammo remaining.</param>
    /// <param name="buildingScore">Score bonus from buildings remaining.</param>
    /// <param name="finalScore">Total score.</param>
    public void Setup(int ammoScore, int buildingScore, int finalScore)
    {
        gameObject.SetActive(true);
        _ammoScore.text += ammoScore;
        _buildingsScore.text += buildingScore;
        _finalScore.text += finalScore;
    }

    /// <summary>
    /// Handles when the player presses the quit button.
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// Handles when the player presses the retry button.
    /// </summary>
    public void RetryButton()
    {
        SceneManager.LoadScene("Level1");
    }
}
