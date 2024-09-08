using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    #region Fields/Objects

    [SerializeField] GameManager gameManager;
    public Button targetButton;

    #endregion

    #region Buttons
    public void resume() //unpause game from main menu
    {
        gameManager.stateUnPaused(); //call from GM
    }
    public void restart() //on death
    {
        //reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name.ToString()); //grab surrent scene
        gameManager.stateUnPaused();//call from GM
    }
    public void EteriusPort() //port the Eterius
    {
        PlayerManager.Instance.HasFloorKey = false;
        gameManager.SaveGame();//call from GM
        SceneManager.LoadScene("Eshes"); //load eshes
        gameManager.stateUnPaused();//call from GM
    }
    public void FrontQuit() //main quit button
    {
        gameManager.quitConfirm();//call from GM
    }
    public void DeathQuit() //on death quit
    {
        gameManager.DeathQuitConfirm();//call from GM
    }
    public void DeathQuitCancel() //cancel death quit
    {
        gameManager.DeathQuitCancel();//call from GM
    }
    public void completeQuit() //after quit confirmation in pause menu
    {
        SceneManager.LoadScene("Eshes"); //load Eterius
    }
    public void EteriusPortConfirmCancel() //eterius port confirm cancel
    {
        gameManager.stateUnPaused();//call from GM
        gameManager.confirmMenu.SetActive(false);//call from GM turn off confirm menu
    }

    #endregion

    #region Skill Buttons


    #region Fire
    public void Fire1()
    {
        if (gameManager.TierOneCheck() != 2 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Ice" && gameManager.AssignedElementCheck() != "Lightning")
        {
            gameManager.AssignTierOne(1);
            gameManager.AssignElement("Fire");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }

        }
    }
    public void Fire2()
    {
        if (gameManager.TierOneCheck() != 1 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Ice" && gameManager.AssignedElementCheck() != "Lightning")
        {
            gameManager.AssignTierOne(2);
            gameManager.AssignElement("Fire");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Fire3()
    {
        if (gameManager.SkillPointCheck() >= 1 && gameManager.TierTwoUnlocked() && gameManager.AssignedElementCheck() != "Ice" && gameManager.AssignedElementCheck() == "Fire")
        {
            switch (gameManager.SkillTierTwoLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Fire4()
    {
        if (gameManager.TierThreeCheck() != 2 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Fire")
        {
            gameManager.AssignTierThree(1);
            gameManager.SkillPointUse();
        }
    }
    public void Fire5()
    {
        if (gameManager.TierThreeCheck() != 1 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Fire")
        {
            gameManager.AssignTierThree(2);
            gameManager.SkillPointUse();
        }
    }
    #endregion

    #region Fire
    public void Ice1()
    {
        if (gameManager.TierOneCheck() != 2 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Fire" && gameManager.AssignedElementCheck() != "Lightning")
        {
            gameManager.AssignTierOne(1);
            gameManager.AssignElement("Ice");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }

        }
    }
    public void Ice2()
    {
        if (gameManager.TierOneCheck() != 1 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Fire" && gameManager.AssignedElementCheck() != "Lightning")
        {
            gameManager.AssignTierOne(2);
            gameManager.AssignElement("Ice");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Ice3()
    {
        if (gameManager.SkillPointCheck() >= 1 && gameManager.TierTwoUnlocked() && gameManager.AssignedElementCheck() == "Ice")
        {
            switch (gameManager.SkillTierTwoLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Ice4()
    {
        if (gameManager.TierThreeCheck() != 2 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Ice")
        {
            gameManager.AssignTierThree(1);
            gameManager.SkillPointUse();
        }
    }
    public void Ice5()
    {
        if (gameManager.TierThreeCheck() != 1 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Ice")
        {
            gameManager.AssignTierThree(2);
            gameManager.SkillPointUse();
        }
    }
    #endregion
    #region Fire
    public void Lightning1()
    {
        if (gameManager.TierOneCheck() != 2 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Fire" && gameManager.AssignedElementCheck() != "Ice")
        {
            gameManager.AssignTierOne(1);
            gameManager.AssignElement("Lightning");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }

        }
    }
    public void Lightning2()
    {
        if (gameManager.TierOneCheck() != 1 && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() != "Fire" && gameManager.AssignedElementCheck() != "Ice")
        {
            gameManager.AssignTierOne(2);
            gameManager.AssignElement("Lighning");
            switch (gameManager.SkillTierOneLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 2:
                    //level up to three
                    gameManager.SkillOneLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Lightning3()
    {
        if (gameManager.SkillPointCheck() >= 1 && gameManager.TierTwoUnlocked() && gameManager.AssignedElementCheck() == "Lightning")
        {
            switch (gameManager.SkillTierTwoLevelCheck())
            {
                case 0:
                    //unlock level to one
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                case 1:
                    //level up to two
                    gameManager.SkillTwoLevelUp();
                    gameManager.SkillPointUse();
                    break;
                default:
                    break;
            }
        }
    }
    public void Lightning4()
    {
        if (gameManager.TierThreeCheck() != 2 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Lightning")
        {
            gameManager.AssignTierThree(1);
            gameManager.SkillPointUse();
        }
    }
    public void Lightning5()
    {
        if (gameManager.TierThreeCheck() != 1 && gameManager.TierThreeUnlocked() && gameManager.SkillPointCheck() >= 1 && gameManager.AssignedElementCheck() == "Lightning")
        {
            gameManager.AssignTierThree(2);
            gameManager.SkillPointUse();
        }
    }
    #endregion
    #endregion
}
