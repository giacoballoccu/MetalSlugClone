// This script is a Manager that controls the UI HUD (deaths, health, and score) for the 
// project. All HUD UI commands are issued through the static methods of this class

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //This class holds a static reference to itself to ensure that there will only be
    //one in existence. This is often referred to as a "singleton" design pattern. Other
    //scripts access this one through its public static methods
    static UIManager current;
    public TextMeshProUGUI gameOverText;    //Text element showing the Game Over message
    public Image healthBar;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bombs;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI winText;
    public TextMeshProUGUI winPointsText;

    void Awake()
    {
        if (current)
            Destroy(current);
        current = this;

        // disable game over text
        current.gameOverText.gameObject.SetActive(false);

        current.winText.gameObject.SetActive(false);
        current.winPointsText.gameObject.SetActive(false);

        // set score text to 0
        UpdateScoreUI();
        UpdateBombsUI();
    }

    public static void UpdateScoreUI()
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //Refresh the score
        current.scoreText.SetText(GameManager.GetScore().ToString());
    }

   public static void UpdateBombsUI()
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //Refresh the score
        current.bombs.SetText(GameManager.GetBombs().ToString());
    }

    public static void UpdateAmmoUI()
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //Refresh the score
        if (GameManager.GetHeavyMachineAmmo() == 0)
        {
            current.ammoText.SetText("oo");
        }
        else
        {
            current.ammoText.SetText(GameManager.GetHeavyMachineAmmo().ToString());
        }
    }

    public static void DisplayGameOverText()
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //Show the game over text
        current.gameOverText.gameObject.SetActive(true);
    }

    public static void UpdateHealthUI(float health, float maxHealth)
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //update the player death count element
        current.healthBar.fillAmount = health / maxHealth;
    }

    public static void DisplayWinText()
    {
        //If there is no current UIManager, exit
        if (current == null)
            return;

        //Show the win text and points
        current.winText.gameObject.SetActive(true);

        current.winPointsText.SetText(GameManager.GetScore().ToString());
        current.winPointsText.gameObject.SetActive(true);
    }
}
