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
    [SerializeField] EshesPlayerEye playerEye;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public ScriptableItems coinPurse;
    [SerializeField] public TMP_Text coinPurseText;
    [SerializeField] public GameObject buildMenu;
    [SerializeField] public GameObject confirmMenu;
    [SerializeField] public GameObject NewGameMenu;
    [SerializeField] public GameObject TotalQuitMenu;
    [SerializeField] public GameObject SaveConfirmMenu;
    [SerializeField] public GameObject selectToBuild;
    [SerializeField] public GameObject buildConfirm;
    [SerializeField] public GameObject removeConfirm;
    [SerializeField] public GameObject SpiralConfirmMenu;
    [SerializeField] public Camera OverHeadCamera;
    [SerializeField] public Camera FPCamera;
    [SerializeField] public GameObject eshesPlayer;
    [SerializeField] public GameObject OverHeadToggle;
    public ScriptableItems[] scriptableList;
    //foliage
    [SerializeField] public GameObject foliageTypeSelector;
    //trees, flowers, bushes, grass
    [SerializeField] public GameObject trees;
    //trees
    [SerializeField] public ScriptableItems Tree1;
    [SerializeField] public TMP_Text Tree1AmountHeld;


    [SerializeField] public GameObject flowers;
    //flowers

    [SerializeField] public GameObject bushes;
    //Bushes
    [SerializeField] public ScriptableItems Bush1;
    [SerializeField] public TMP_Text Bush1AmountHeld;


    [SerializeField] public GameObject grass;
    //Grasses

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


    public PrefabList prefabList;
    [SerializeField] SaveLoadManager saveLoadManager;

    public static EshesGameManager Instance;
    public GameObject activeMenu;
    public GameObject mainMenuActiveMenu;
    public GameObject activeBuildSelection;


    public bool isPaused;
    public bool inSelectError;
    public bool inConfirmBuild;
    public bool inConfrimRemove;
    public bool FPActive;
    public bool buildON;

    int scene;

    #endregion

    #region Processes
    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "GameStart") //have scene manager check scene
        {
            Cursor.lockState = CursorLockMode.Confined; //lock cursor
            scene = 0; //assign 0 to GameStart
        }
        if (SceneManager.GetActiveScene().name == "Eshes")//have scene manager check scene
        {
            Cursor.lockState = CursorLockMode.Confined; //lock cursor
            scene = 1; //assign 1 to Eterius
        }
        if (SceneManager.GetActiveScene().name == "Spiral")//have scene manager check scene
        {
            scene = 2; //assign two to Spiral Floors
        }
    }
    void Start()
    {
        if(scene == 1) //if eterius
        {
            OverHeadCamera.enabled = true; //overhead cam on
            FPCamera.enabled = false; //FpCam disabled
            FPActive = false; //Fp bool false
        }
        activeBuildSelection = null; //active build set to null
    }


    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameStart") //if gamestart
        {
            scene = 0; //scene 0
        }
        if (SceneManager.GetActiveScene().name == "Eshes") //if eterius
        {
            BuildMenu(); 
            Pause();
            UpdateItemCounts();
            ChangeView();
            scene = 1;

        }
        if (SceneManager.GetActiveScene().name == "Spiral")
        {
            scene = 2; //asign 2
        }

    }

    #endregion

    #region Game States
    public void LoadGame()
    {
        SceneManager.LoadScene("Eshes"); //load eterius
        WaitTimer(); //wait for spawn
        saveLoadManager.Load(); //load
        WaitTimer(); //wait for load
        Time.timeScale = 1; //time start
    }
    public void NewGame() 
    {
        PlayerPrefs.DeleteAll(); //delete data in prefs
        SceneManager.LoadScene("Eshes"); //load eterius
        Time.timeScale = 1; //time start
    }
    public void statePaused()
    {
        isPaused = true; //paused on
        buildON = false; //build set to off
        Cursor.lockState = CursorLockMode.Confined; //keep cursor in the window
        Cursor.visible = true; //show cursor
        Time.timeScale = 0; //reset time passed to zero
    }
    public void stateUnPaused()
    {
        isPaused = false; //paused off
        Cursor.visible = true; //visible cursor
        Cursor.lockState = CursorLockMode.Confined; //keep cursor in the window
        Time.timeScale = 1; //allow time to pass again
        if (activeMenu != null && activeMenu != pauseMenu) 
        {
            activeMenu.SetActive(false); //what ever menu is active turned off
            activeMenu = pauseMenu; //pause to active
            activeMenu.SetActive(true); //pause to on
        }
        else
        {
            activeMenu.SetActive(isPaused); //what ever menu active off
            activeMenu = null; //active menu emptied
        }
    }
    void ShutOffOverHeadCam()
    {
        OverHeadToggle.SetActive(false); //shut off overhead cam
    }
    void TurnOnOverHeadCam()
    {
        OverHeadToggle.SetActive(true); //turn on overhead cam
    }
    void BuildMenu()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (activeMenu == null && !FPActive) //no meanu active and First person is off
            {
                activeMenu = buildMenu; //active is buildMenu
                buildMenu.SetActive(true); //active on
                buildON = true; //bildOn flag on
            }
            else if (activeMenu == buildMenu) //if build active
            {
                buildON = false; //build flag off
                activeMenu = null; //active null
                buildMenu.SetActive(false); //biuld off
                Debug.Log("Off");
            }
        }
    }
    void ChangeView()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if(buildON)
            {
                buildON = false; //build flag off
                activeMenu = null; //active emptied
                buildMenu.SetActive(false); //build off
            }
            if (OverHeadCamera.isActiveAndEnabled)
            {
                Cursor.lockState = CursorLockMode.Confined;//keep cursor in the window
                Cursor.visible = false;//hide cursor
                OverHeadCamera.enabled = false; //OverHead off
                ShutOffOverHeadCam(); //Shut off Function
                eshesPlayer.SetActive(true); //turn on character in eterius
                FPCamera.enabled = true; //FP on
                FPActive = true; //FP flag on
            }
            else
            {
                Cursor.lockState = CursorLockMode.Confined;//keep cursor in the window
                Cursor.visible = true; //hide cursor
                eshesPlayer.SetActive(false); //eterius player off
                playerEye.ResetPosition(); //reset overhead tools
                TurnOnOverHeadCam(); //turn on overhead
                OverHeadCamera.enabled = true; //enable overhead
                FPCamera.enabled = false; //fp off
                FPActive = false; //fp flag off
            }
        }
    }
    void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (activeMenu == null)
            {
                playerEye.RemovePreview(); // turn off build preview
                buildON = false; //build flag off
                statePaused(); //pause
                activeMenu = pauseMenu; //active is pauseMenu
                pauseMenu.SetActive(true); //turn on pause

            }
            else if (activeMenu != null)
            {
                playerEye.RemovePreview(); //remove build preview
                buildON = false; //build flag off
                stateUnPaused(); //unpause
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
            SetAllInactive(); //turn off all menus
            activeBuildSelection = foliageTypeSelector; //active to foliage
            foliageTypeSelector.SetActive(true); //turn it on
            Debug.Log("Foliage Selection");
        }
    }
    public void FoliageTrees()
    {
        SetAllInactive();//turn off all build menus
        Tree1AmountHeld.text = Tree1.amountHeld.ToString(); //Item Counts



        //turn on this selection menu
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        trees.SetActive(true);
        Debug.Log("Trees Selected");
    }
    public void TreeSelection1()
    {
        if (Tree1.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Tree1.eshesBuildObject;
        }
    }
    public void FoliageFlowers()
    {
        SetAllInactive();//turn off all build menus
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        flowers.SetActive(true);
        Debug.Log("Flowers Selected");
    }
    public void FoliageBushes()
    {
        //turn everything off to make sure only the following is open
        SetAllInactive();//turn off all build menus
        //Item Counts
        Bush1AmountHeld.text = Bush1.amountHeld.ToString();

        //turn on this selection
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        bushes.SetActive(true);
        Debug.Log("Bushes Selected");
    }
    public void BushSelection1()
    {
        if (Bush1.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Bush1.eshesBuildObject;
        }

    }
    public void FoliageGrass()
    {
        SetAllInactive();//turn off all build menus
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
            SetAllInactive();//turn off all build menus
            activeBuildSelection = buildingsTypeSelector;
            buildingsTypeSelector.SetActive(true);
            Debug.Log("Buildings Selection");
        }

    }
    public void BuildingsBuildings()
    {
        SetAllInactive();//turn off all build menus
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        buildings.SetActive(true);
        Debug.Log("Buildings Selected");
    }
    public void BuildingsShops()
    {
        SetAllInactive();//turn off all build menus
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        shops.SetActive(true);
        Debug.Log("Shops Selected");
    }
    public void BuildingsHouses()
    {
        SetAllInactive();//turn off all build menus
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        houses.SetActive(true);
        Debug.Log("Houses Selected");
    }
    public void BuildingsRoads()
    {
        SetAllInactive();//turn off all build menus
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
            SetAllInactive();//turn off all build menus
            activeBuildSelection = peopleTypeSelector;
            peopleTypeSelector.SetActive(true);
            Debug.Log("People Selection");
        }
    }
    public void PeopleShopOwners()
    {
        //activate People Type Selector
        SetAllInactive();//turn off all build menus
        activeBuildSelection = peopleTypeSelector;
        peopleTypeSelector.SetActive(true);
        shopOwners.SetActive(true);
        Debug.Log("Shop Owners Selected");
    }
    public void PeopleResidents()
    {
        //activate People Type Selector
        SetAllInactive();//turn off all build menus
        activeBuildSelection = peopleTypeSelector;
        peopleTypeSelector.SetActive(true);
        residents.SetActive(true);
        Debug.Log("Residents Selected");
    }
    public void PeoplePets()
    {
        //activate People Type Selector
        SetAllInactive();//turn off all build menus
        activeBuildSelection = peopleTypeSelector;
        peopleTypeSelector.SetActive(true);
        pets.SetActive(true);
        Debug.Log("Pets Selected");
    }
    public void PeopleSpecial()
    {
        //activate People Type Selector
        SetAllInactive();///turn off all build menus
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
            SetAllInactive();//turn off all build menus
            activeBuildSelection = geologicalTypeSelector;
            geologicalTypeSelector.SetActive(true);
            Debug.Log("Geological Selection");
        }
    }
    public void GeologicalRivers()
    {
        //activate Geological Type Selector
        SetAllInactive();//turn off all menus
        activeBuildSelection = geologicalTypeSelector;
        geologicalTypeSelector.SetActive(true);
        rivers.SetActive(true);
        Debug.Log("Rivers Selected");
    }
    public void GeologicalHills()
    {
        //activate Geological Type Selector
        SetAllInactive();//turn off build all menus
        activeBuildSelection = geologicalTypeSelector;
        geologicalTypeSelector.SetActive(true);
        hills.SetActive(true);
        Debug.Log("Hills Selected");
    }
    public void GeologicalDitches()
    {
        //activate Geological Type Selector
        SetAllInactive();//turn off all menus
        activeBuildSelection = geologicalTypeSelector;
        geologicalTypeSelector.SetActive(true);
        ditches.SetActive(true);
        Debug.Log("Ditches Selected");
    }
    public void GeologicalSpecial()
    {
        //activate Geological Type Selector
        SetAllInactive();//turn off all build menus
        activeBuildSelection = geologicalTypeSelector;
        geologicalTypeSelector.SetActive(true);
        geoSpecial.SetActive(true);
        Debug.Log("Special Selected");
    }

    public void UpdateItemCounts()
    {
        if (scene == 1)
        {
            Tree1AmountHeld.text = Tree1.amountHeld.ToString();
            Bush1AmountHeld.text = Bush1.amountHeld.ToString();

            coinPurseText.text = coinPurse.amountHeld.ToString();
        }
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
    public void selectSomethingToBuild()
    {
        if (!inSelectError)
        {
            StartCoroutine(nothingSelected());
        }
    }
    IEnumerator nothingSelected()
    {
        inSelectError = true;
        selectToBuild.SetActive(true);
        yield return new WaitForSeconds(2F);
        selectToBuild.SetActive(false);
        inSelectError = false;
    }
    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(3F);
    }
    public void quitConfirm()
    {
        statePaused();
        confirmMenu.SetActive(true);
        activeMenu = confirmMenu;
    }
    public void resume2()
    {
        stateUnPaused();
        confirmMenu.SetActive(false);
    }
    public void NewGameConfirm()
    {

        NewGameMenu.SetActive(true);
    }
    public void NewGameCancel()
    {

        NewGameMenu.SetActive(false);
    }
    public void TotalGameQuitConfirm()
    {

        TotalQuitMenu.SetActive(true);
    }
    public void TotalGameQuitCancel()
    {

        TotalQuitMenu.SetActive(false);
    }
    public void SaveConfirm()
    {
        statePaused();
        SaveConfirmMenu.SetActive(true);
        activeMenu = SaveConfirmMenu;
    }
    public void SaveCancel()
    {
        stateUnPaused();
        SaveConfirmMenu.SetActive(false);
    }
    public void SpiralConfirm()
    {
        statePaused();
        SpiralConfirmMenu.SetActive(true);
        activeMenu = SpiralConfirmMenu;
    }
    public void SpiralCancel()
    {
        stateUnPaused();
        SpiralConfirmMenu.SetActive(false);
    }
    public void ResetScriptables()
    {
        foreach (var scriptable in scriptableList)
        {
            scriptable.amountHeld = 0;
        }
    }
}
