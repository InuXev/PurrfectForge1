using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EshesButtonFunctions : MonoBehaviour
{
    // Start is called before the first frame update
    #region Fields/Objects

    [SerializeField] EshesGameManager gameManager;
    [SerializeField] EshesPlayerEye playerEye;
    [SerializeField] SaveLoadManager saveLoadManager;
    public Button targetButton;
    public string clickedButton;
    #endregion
    public void Awake()
    {
        // Populate prefabList with dummy data for testing
        if (gameManager.prefabList.items == null)
        {
            gameManager.prefabList.items = new List<PrefabList.PrefabData>();
        }
        // Load the saved prefab list
        PrefabList loadedPrefabList = saveLoadManager.Load();

        // Replace the prefabs in the scene with the loaded data
        saveLoadManager.ReplacePrefabs(loadedPrefabList);
    }
    #region stateButtons

    public void ClearPreviewButton()
    {
        gameManager.buildON = false;
    }
    public void CloseBuildWindowWindow()
    {
        gameManager.stateUnPaused();
        playerEye.RemovePreview();
    }
    public void resume()
    {
        gameManager.stateUnPaused();
    }
    public void restart()
    {
        //reload scene

        SceneManager.LoadScene("Eshes");
        gameManager.stateUnPaused();

    }
    public void SaveGame()
    {
        // Proceed to save
        gameManager.SaveConfirm();
    }
    public void LoadGame()
    {
        gameManager.LoadGame();
    }
    public void SaveGameConfirmed()
    {
        saveLoadManager.SaveEshesWorld();
        Debug.Log("Game saved successfully.");
        gameManager.SaveCancel();
    }
    public void NewGameConfirmed()
    {
        saveLoadManager.ClearSaveData();
        gameManager.ResetScriptables();
        gameManager.ResetCompleteFloors();
        gameManager.NewGame();
    }
    public void spiralPort()
    {
        //reload scene
        gameManager.SpiralConfirm();

    }
    public void SpiralFloorSelect()
    {
        gameManager.SpiralFloorSelection();
    }

    public void spiralPortConfirmed()
    {
        //reload scene
        saveLoadManager.SaveEshesWorld();
        SceneManager.LoadScene("Spiral");
        gameManager.stateUnPaused();

    }
    public void FrontQuit()
    {
        gameManager.quitConfirm();
    }
    public void NewGameConfirm()
    {
        gameManager.quitConfirm();
    }
    public void completeQuit()
    {
        SceneManager.LoadScene("GameStart");
    }
    public void TotalQuitConfirm()
    {
        gameManager.TotalGameQuitConfirm();
    }
    public void QuitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
    #endregion

    #region Foliage
    public void FoliageTop()
    {
        //activate Foliage Type Selector
        gameManager.FoliageTop();
    }
    public void FoliageTrees()
    {
        gameManager.FoliageTrees();
    }
    //trees
    public void Tree1()
    {
        gameManager.buildON = true;
        gameManager.TreeSelection1();
    }
    //flowers
    public void FoliageFlowers()
    {
        gameManager.FoliageFlowers();
    }
    //Bushes
    public void FoliageBushes()
    {
        gameManager.FoliageBushes();
    }
    public void Bush1()
    {
        gameManager.buildON = true;
        gameManager.BushSelection1();
    }
    //Grass
    public void FoliageGrass()
    {
        gameManager.FoliageGrass();
    }
    #endregion

    #region Buildings
    public void BuildingsTop()
    {
        //activate Building Type Selector
        gameManager.BuildingsTop();
    }
    public void BuildingsBuildings()
    {
        //activate Building Type Selector
        gameManager.BuildingsBuildings();
    }
    public void BuildingsShops()
    {
        //activate Building Type Selector
        gameManager.BuildingsShops();
    }
    public void BuildingsHouses()
    {
        //activate Building Type Selector
        gameManager.BuildingsHouses();
    }
    public void BuildingsRoads()
    {
        //activate Building Type Selector
        gameManager.BuildingsRoads();
    }
    #endregion

    #region People
    public void PeopleTop()
    {
        //activate People Type Selector
        gameManager.PeopleTop();
    }
    public void PeopleShopOwners()
    {
        //activate People Type Selector
        gameManager.PeopleShopOwners();
    }
    public void PeopleResidents()
    {
        //activate People Type Selector
        gameManager.PeopleResidents();
    }
    public void PeoplePets()
    {
        //activate People Type Selector
        gameManager.PeoplePets();
    }
    public void PeopleSpecial()
    {
        //activate People Type Selector
        gameManager.PeopleSpecial();
    }
    #endregion

    #region Geological

    public void GeologicalTop()
    {
        //activate Geological Type Selector
        gameManager.GeologicalTop();
    }
    public void GeologicalRivers()
    {
        //activate Geological Type Selector
        gameManager.GeologicalRivers();
    }
    public void GeologicalHills()
    {
        //activate Geological Type Selector
        gameManager.GeologicalHills();
    }
    public void GeologicalDitches()
    {
        //activate Geological Type Selector
        gameManager.GeologicalDitches();
    }
    public void GeologicalSpecial()
    {
        //activate Geological Type Selector
        gameManager.GeologicalSpecial();
    }

    #endregion

    #region SpiralFloorSelectionButtons

    public void FloorButton()
    {
        GameObject clickButton = EventSystem.current.currentSelectedGameObject;
        clickedButton = clickButton.name;
        Debug.Log(clickedButton);
        gameManager.SpiralFloorButtonClicked(clickedButton);
    }
    public void FloorConfirmed()
    {
        saveLoadManager.SaveEshesWorld();
        SceneManager.LoadScene(clickedButton);
        gameManager.stateUnPaused();
        clickedButton = null;
    }
    public void FloorCancel()
    {
        gameManager.FloorConfirmPort.SetActive(false);
        gameManager.SpiralFloorSelector.SetActive(true);
        gameManager.activeMenu = gameManager.SpiralFloorSelector;
    }
    #endregion 
}
