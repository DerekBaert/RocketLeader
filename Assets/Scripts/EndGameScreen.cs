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
        gameObject.SetActive(false);
    }

    public void Setup(int ammoScore, int buildingScore, int finalScore)
    {
        gameObject.SetActive(true);
        _ammoScore.text += ammoScore;
        _buildingsScore.text += buildingScore;
        _finalScore.text += finalScore;
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void RetryButton()
    {
        SceneManager.LoadScene("Level1");
    }
}
