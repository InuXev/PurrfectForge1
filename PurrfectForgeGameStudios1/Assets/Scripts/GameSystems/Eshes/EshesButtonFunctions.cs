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
    public Button targetButton;

    #endregion

    #region stateButtons
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
    public void FoliageFlowers()
    {
        gameManager.FoliageFlowers();
    }
    public void FoliageBushes()
    {
        gameManager.FoliageBushes();
    }
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