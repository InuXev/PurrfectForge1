using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Debug.Log("SaveGame method called.");

        // Ensure SaveLoadManager is correctly assigned
        if (saveLoadManager == null)
        {
            saveLoadManager = FindObjectOfType<SaveLoadManager>();
            if (saveLoadManager == null)
            {
                Debug.LogError("Save operation failed: SaveLoadManager is missing in the scene.");
                return;
            }
        }

        // Check if the GameManager's prefabList is null
        if (gameManager.prefabList == null)
        {
            Debug.LogError("GameManager's prefabList is null.");
            return;
        }

        // Log the prefabList state before saving
        Debug.Log("PrefabList state before saving:");
        if (gameManager.prefabList.items == null || gameManager.prefabList.items.Count == 0)
        {
            Debug.Log("PrefabList.items is empty.");
        }
        else
        {
            foreach (var item in gameManager.prefabList.items)
            {
                Debug.Log($"Item: {item.name}, Position: {item.position}, Rotation: {item.rotation}, GameObjectName: {item.eshesBuildObjectName}");
            }
        }

        // Proceed to save
        saveLoadManager.Save(gameManager.prefabList);
        Debug.Log("Game saved successfully.");
    }
    public void LoadGame()
    {
        gameManager.LoadGame();
    }
    public void NewGame()
    {
        gameManager.NewGame();
    }
    public void spiralPort()
    {
        //reload scene

        SceneManager.LoadScene("Spiral");
        gameManager.stateUnPaused();

    }
    public void quit2()
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
}
