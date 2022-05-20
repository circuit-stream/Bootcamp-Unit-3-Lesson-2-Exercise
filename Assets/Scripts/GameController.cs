using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Pin[] pins;
    [SerializeField] private Ball ball;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject startGameText;

    private int currentScore;

    public void AddScore(Pin pin)
    {
        if (!pin.pinWasHitThisRound)
        {
            pin.pinWasHitThisRound = true;
            currentScore += pin.pinValue;
            SetScoreText();
        }
    }

    public void EndGame()
    {
        StartCoroutine(RestartGame(3));
    }

    private IEnumerator RestartGame(float waitForSeconds)
    {
        // Waiting a few seconds so the player can see the score she got
        yield return new WaitForSeconds(waitForSeconds);

        currentScore = 0;
        SetScoreText();

        startGameText.SetActive(true);

        // By setting the script back to enabled, the Update method starts being called again
        enabled = true;

        ball.Restart();

        // We need to make sure the pins will start scoring again
        foreach (var pin in pins)
            pin.pinWasHitThisRound = false;
    }

    private void SetScoreText()
    {
        scoreText.text = $"Score: {currentScore}";
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ball.ThrowBall();
            startGameText.SetActive(false);
            enabled = false;
        }
    }

    private void Awake()
    {
        StartCoroutine(RestartGame(1));

        SetScoreText();
        startGameText.SetActive(false);

        enabled = false;
    }
}
