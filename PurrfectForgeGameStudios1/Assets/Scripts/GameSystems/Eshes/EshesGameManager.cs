using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
//using UnityEditor.Experimental.GraphView;
using System.Linq;

public class EshesGameManager : MonoBehaviour
{

    #region Fields/Objects

    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject buildMenu;
    //foliage
    [SerializeField] public GameObject foliageTypeSelector;
    //trees, flowers, bushes, grass
    [SerializeField] public GameObject trees;
    [SerializeField] public GameObject flowers;
    [SerializeField] public GameObject bushes;
    [SerializeField] public GameObject grass;


    //buildings
    [SerializeField] public GameObject buildingsTypeSelector;
    //buildings, shops, houses, roads
    [SerializeField] public GameObject buildings;
    [SerializeField] public GameObject shops;
    [SerializeField] public GameObject houses;
    [SerializeField] public GameObject roads;


    //people
    [SerializeField] public GameObject peopleTypeSelector;
    //shop owners, residents, pets, special
    [SerializeField] public GameObject shopOwners;
    [SerializeField] public GameObject residents;
    [SerializeField] public GameObject pets;
    [SerializeField] public GameObject special;


    //geological
    [SerializeField] public GameObject geologicalTypeSelector;
    //rivers, hills, ditches, special
    [SerializeField] public GameObject rivers;
    [SerializeField] public GameObject hills;
    [SerializeField] public GameObject ditches;
    [SerializeField] public GameObject geoSpecial;



    public static EshesGameManager Instance;
    public GameObject activeMenu;
    public GameObject activeBuildSelection;

    public bool isPaused;

    #endregion

    #region Processes
    void Start()
    {
        activeBuildSelection = null;
    }


    void Update()
    {

        BuildMenu();
        Pause();
    }

    #endregion

    #region Game States

    public void statePaused()
    {
        isPaused = true;
        //keep cursor in the window
        Cursor.lockState = CursorLockMode.Confined;
        //hide cursor
        Cursor.visible = true;
        //reset time passed to zero
        Time.timeScale = 0;
    }
    public void stateUnPaused()
    {
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //allow time to pass again
        Time.timeScale = 1;
        activeMenu.SetActive(isPaused);
        activeMenu = null;
    }

    void BuildMenu()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (activeMenu == null)
            {
                statePaused();
                activeMenu = buildMenu;
                buildMenu.SetActive(true);

            }
            else if (activeMenu == buildMenu)
            {
                activeMenu = null;
                buildMenu.SetActive(false);
            }
        }
    }

    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeMenu == null)
            {

                statePaused();
                activeMenu = pauseMenu;
                pauseMenu.SetActive(true);

            }
            else if (activeMenu != null)
            {
                stateUnPaused();
            }
        }
    }
    #endregion 

    #region Foliage
    public void FoliageTop()
    {
        //activate Foliage Type Selector
        if (activeBuildSelection == null || activeBuildSelection != null)
        {
            SetAllInactive();
            activeBuildSelection = foliageTypeSelector;
            foliageTypeSelector.SetActive(true);
            Debug.Log("Foliage Selection");
        }

    }
    public void FoliageTrees()
    {
        SetAllInactive();
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        trees.SetActive(true);
        Debug.Log("Trees Selected");
    }
    public void FoliageFlowers()
    {
        SetAllInactive();
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        flowers.SetActive(true);
        Debug.Log("Flowers Selected");
    }
    public void FoliageBushes()
    {
        SetAllInactive();
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        bushes.SetActive(true);
        Debug.Log("Bushes Selected");

    }
    public void FoliageGrass()
    {
        SetAllInactive();
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        grass.SetActive(true);
        Debug.Log("Grass Selected");

    }
    #endregion

    #region Buildings
    public void BuildingsTop()
    {
        //activate Building Type Selector
        if (activeBuildSelection == null || activeBuildSelection != null)
        {
            SetAllInactive();
            activeBuildSelection = buildingsTypeSelector;
            buildingsTypeSelector.SetActive(true);
            Debug.Log("Buildings Selection");
        }

    }
    public void BuildingsBuildings()
    {
        SetAllInactive();
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        buildings.SetActive(true);
        Debug.Log("Buildings Selected");
    }
    public void BuildingsShops()
    {
        SetAllInactive();
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        shops.SetActive(true);
        Debug.Log("Shops Selected");
    }
    public void BuildingsHouses()
    {
        SetAllInactive();
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        houses.SetActive(true);
        Debug.Log("Houses Selected");
    }
    public void BuildingsRoads()
    {
        SetAllInactive();
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        roads.SetActive(true);
        Debug.Log("Roads Selected");
    }

    #endregion

    #region People

    public void PeopleTop()
    {
        //activate People Type Selector
        if (activeBuildSelection == null || activeBuildSelection != null)
        {
            SetAllInactive();
            activeBuildSelection = peopleTypeSelector;
            peopleTypeSelector.SetActive(true);
            Debug.Log("People Selection");
        }
    }
    public void PeopleShopOwners()
    {
        //activate People Type Selector
        SetAllInactive();
        activeBuildSelection = peopleTypeSelector;
        peopleTypeSelector.SetActive(true);
        shopOwners.SetActive(true);
        Debug.Log("Shop Owners Selected");
    }
    public void PeopleResidents()
    {
        //activate People Type Selector
        SetAllInactive();
        activeBuildSelection = peopleTypeSelector;
        peopleTypeSelector.SetActive(true);
        residents.SetActive(true);
        Debug.Log("Residents Selected");
    }
    public void PeoplePets()
    {
        //activate People Type Selector
        SetAllInactive();
        activeBuildSelection = peopleTypeSelector;
        peopleTypeSelector.SetActive(true);
        pets.SetActive(true);
        Debug.Log("Pets Selected");
    }
    public void PeopleSpecial()
    {
        //activate People Type Selector
        SetAllInactive();
        activeBuildSelection = peopleTypeSelector;
        peopleTypeSelector.SetActive(true);
        special.SetActive(true);
        Debug.Log("Special People Selected");
    }

    #endregion



    #region Geographical
    public void GeologicalTop()
    {
        //activate Geological Type Selector
        if (activeBuildSelection == null || activeBuildSelection != null)
        {
            SetAllInactive();
            activeBuildSelection = geologicalTypeSelector;
            geologicalTypeSelector.SetActive(true);
            Debug.Log("Geological Selection");
        }
    }
    public void GeologicalRivers()
    {
        //activate Geological Type Selector
        SetAllInactive();
        activeBuildSelection = geologicalTypeSelector;
        geologicalTypeSelector.SetActive(true);
        rivers.SetActive(true);
        Debug.Log("Rivers Selected");
    }
    public void GeologicalHills()
    {
        //activate Geological Type Selector
        SetAllInactive();
        activeBuildSelection = geologicalTypeSelector;
        geologicalTypeSelector.SetActive(true);
        hills.SetActive(true);
        Debug.Log("Hills Selected");
    }
    public void GeologicalDitches()
    {
        //activate Geological Type Selector
        SetAllInactive();
        activeBuildSelection = geologicalTypeSelector;
        geologicalTypeSelector.SetActive(true);
        ditches.SetActive(true);
        Debug.Log("Ditches Selected");
    }
    public void GeologicalSpecial()
    {
        //activate Geological Type Selector
        SetAllInactive();
        activeBuildSelection = geologicalTypeSelector;
        geologicalTypeSelector.SetActive(true);
        geoSpecial.SetActive(true);
        Debug.Log("Special Selected");
    }


    #endregion
    public void SetAllInactive()
    {
        activeBuildSelection = null;

        //foliage
        foliageTypeSelector.SetActive(false);
        //trees, flowers, bushes, grass
        trees.SetActive(false);
        flowers.SetActive(false);
        bushes.SetActive(false);
        grass.SetActive(false);

        //buildings
        buildingsTypeSelector.SetActive(false);
        //buildings, shops, houses, roads
        buildings.SetActive(false);
        shops.SetActive(false);
        houses.SetActive(false);
        roads.SetActive(false);

        //people
        peopleTypeSelector.SetActive(false);
        //shop owners, residents, pets, special
        shopOwners.SetActive(false);
        residents.SetActive(false);
        pets.SetActive(false);
        special.SetActive(false);

        //geological
        geologicalTypeSelector.SetActive(false);
        //rivers, hills, ditches, special
        rivers.SetActive(false);
        hills.SetActive(false);
        ditches.SetActive(false);
        geoSpecial.SetActive(false);

    }

}