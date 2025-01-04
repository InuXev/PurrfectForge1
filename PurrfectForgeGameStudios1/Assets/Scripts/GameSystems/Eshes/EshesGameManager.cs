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
using UnityEngine.EventSystems;

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
    public ScriptableSkill[] scriptableSkillList;
    [SerializeField] public GameObject reticle;
    [SerializeField] public GameObject BackPackMenu;
    [SerializeField] public GameObject JournalMenu;


    //FloorSelection
    [SerializeField] public ScriptableLevelCompleted HighestLevel;
    [SerializeField] public GameObject SpiralFloorSelector;
    [SerializeField] public GameObject FloorConfirmPort;

    //foliage
    [SerializeField] public GameObject foliageTypeSelector;
    //trees, flowers, bushes, grass
    [SerializeField] public GameObject trees;
    //trees
    [SerializeField] public ScriptableItems SmallBirch;
    [SerializeField] public TMP_Text SmallBirchAmountHeld;
    [SerializeField] public ScriptableItems MediumBirch;
    [SerializeField] public TMP_Text MediumBirchAmountHeld;
    [SerializeField] public ScriptableItems TallBirch;
    [SerializeField] public TMP_Text TallBirchAmountHeld;
    [SerializeField] public ScriptableItems LargeBirch;
    [SerializeField] public TMP_Text LargeBirchAmountHeld;
    [SerializeField] public ScriptableItems SmallWillow;
    [SerializeField] public TMP_Text SmallWillowAmountHeld;
    [SerializeField] public ScriptableItems MediumWillow;
    [SerializeField] public TMP_Text MediumWillowAmountHeld;
    [SerializeField] public ScriptableItems LargeWillow;
    [SerializeField] public TMP_Text LargeWillowAmountHeld;

    [SerializeField] public GameObject flowers;
    //flowers
    [SerializeField] public ScriptableItems PurpleFlower;
    [SerializeField] public TMP_Text PurpleFlowerAmountHeld;
    [SerializeField] public ScriptableItems SmallFern;
    [SerializeField] public TMP_Text SmallFernAmountHeld;
    [SerializeField] public ScriptableItems LargeFern;
    [SerializeField] public TMP_Text LargeFernAmountHeld;


    [SerializeField] public GameObject bushes;
    //Bushes
    [SerializeField] public ScriptableItems SmallBush;
    [SerializeField] public TMP_Text SmallBushAmountHeld;
    [SerializeField] public ScriptableItems MediumBush;
    [SerializeField] public TMP_Text MediumBushAmountHeld;
    [SerializeField] public ScriptableItems LargeBush;
    [SerializeField] public TMP_Text LargeBushAmountHeld;

    [SerializeField] public GameObject grass;
    //Grasses
    [SerializeField] public ScriptableItems SmallGrass;
    [SerializeField] public TMP_Text SmallGrassAmountHeld;
    [SerializeField] public ScriptableItems MediumGrass;
    [SerializeField] public TMP_Text MediumGrassAmountHeld;
    [SerializeField] public ScriptableItems LargeGrass;
    [SerializeField] public TMP_Text LargeGrassAmountHeld;
    [SerializeField] public ScriptableItems UnderGrowth;
    [SerializeField] public TMP_Text UnderGrowthAmountHeld;


    //buildings
    [SerializeField] public GameObject buildingsTypeSelector;
    //buildings, shops, houses, roads
    [SerializeField] public GameObject buildings;

    [SerializeField] public ScriptableItems Shed;
    [SerializeField] public TMP_Text ShedAmountHeld;
    [SerializeField] public ScriptableItems RedStall;
    [SerializeField] public TMP_Text RedStallAmountHeld;
    [SerializeField] public ScriptableItems GreenStall;
    [SerializeField] public TMP_Text GreenStallAmountHeld;
    [SerializeField] public ScriptableItems OpenStall;
    [SerializeField] public TMP_Text OpenStallAmountHeld;
    [SerializeField] public ScriptableItems YellowStall;
    [SerializeField] public TMP_Text YellowStallAmountHeld;

    [SerializeField] public GameObject shops;

    [SerializeField] public ScriptableItems Apothecary;
    [SerializeField] public TMP_Text ApothecaryAmountHeld;
    [SerializeField] public ScriptableItems Bakery;
    [SerializeField] public TMP_Text BakeryAmountHeld;
    [SerializeField] public ScriptableItems BlackSmith;
    [SerializeField] public TMP_Text BlackSmithAmountHeld;
    [SerializeField] public ScriptableItems Church;
    [SerializeField] public TMP_Text ChurchAmountHeld;
    [SerializeField] public ScriptableItems LeonsInn;
    [SerializeField] public TMP_Text LeonsInnAmountHeld;
    [SerializeField] public ScriptableItems Mayors;
    [SerializeField] public TMP_Text MayorsAmountHeld;

    [SerializeField] public GameObject houses;
    [SerializeField] public GameObject housesPageTwo;

    [SerializeField] public ScriptableItems BaseRedHouse;
    [SerializeField] public TMP_Text BaseRedHouseAmountHeld;
    [SerializeField] public ScriptableItems BaseDarkRedHouse;
    [SerializeField] public TMP_Text BaseDarkRedHouseAmountHeld;
    [SerializeField] public ScriptableItems BaseGreenHouse;
    [SerializeField] public TMP_Text BaseGreenHouseAmountHeld;
    [SerializeField] public ScriptableItems BaseBlueHouse;
    [SerializeField] public TMP_Text BaseBlueHouseAmountHeld;

    [SerializeField] public ScriptableItems MediumRedHouse;
    [SerializeField] public TMP_Text MediumRedHouseAmountHeld;
    [SerializeField] public ScriptableItems MediumDarkRedHouse;
    [SerializeField] public TMP_Text MediumDarkRedHouseAmountHeld;
    [SerializeField] public ScriptableItems MediumGreenHouse;
    [SerializeField] public TMP_Text MediumGreenHouseAmountHeld;
    [SerializeField] public ScriptableItems MediumBlueHouse;
    [SerializeField] public TMP_Text MediumBlueHouseAmountHeld;

    [SerializeField] public ScriptableItems LiftedRedHouse;
    [SerializeField] public TMP_Text LiftedRedHouseAmountHeld;
    [SerializeField] public ScriptableItems LiftedDarkRedHouse;
    [SerializeField] public TMP_Text LiftedDarkRedHouseAmountHeld;
    [SerializeField] public ScriptableItems LiftedGreenHouse;
    [SerializeField] public TMP_Text LiftedGreenHouseAmountHeld;
    [SerializeField] public ScriptableItems LiftedBlueHouse;
    [SerializeField] public TMP_Text LiftedBlueHouseAmountHeld;

    [SerializeField] public ScriptableItems LargeRedHouse;
    [SerializeField] public TMP_Text LargeRedHouseAmountHeld;
    [SerializeField] public ScriptableItems LargeDarkRedHouse;
    [SerializeField] public TMP_Text LargeDarkRedHouseAmountHeld;
    [SerializeField] public ScriptableItems LargeGreenHouse;
    [SerializeField] public TMP_Text LargeGreenHouseAmountHeld;
    [SerializeField] public ScriptableItems LargeBlueHouse;
    [SerializeField] public TMP_Text LargeBlueHouseAmountHeld;


    [SerializeField] public GameObject misc;
    [SerializeField] public GameObject miscPageTwo;

    [SerializeField] public ScriptableItems Brazier;
    [SerializeField] public TMP_Text BrazierAmountHeld;
    [SerializeField] public ScriptableItems Bridge;
    [SerializeField] public TMP_Text BridgeAmountHeld;
    [SerializeField] public ScriptableItems BrokenWall;
    [SerializeField] public TMP_Text BrokenWallAmountHeld;
    [SerializeField] public ScriptableItems Campfire;
    [SerializeField] public TMP_Text CampfireAmountHeld;
    [SerializeField] public ScriptableItems CampfireWithPot;
    [SerializeField] public TMP_Text CampfireWithPotAmountHeld;
    [SerializeField] public ScriptableItems CurvedChest;
    [SerializeField] public TMP_Text CurvedChestAmountHeld;
    [SerializeField] public ScriptableItems DoubleFence;
    [SerializeField] public TMP_Text DoubleFenceAmountHeld;
    [SerializeField] public ScriptableItems FlatChest;
    [SerializeField] public TMP_Text FlatChestAmountHeld;
    [SerializeField] public ScriptableItems LampPost;
    [SerializeField] public TMP_Text LampPostAmountHeld;
    [SerializeField] public ScriptableItems MetalCrate;
    [SerializeField] public TMP_Text MetalCrateAmountHeld;
    [SerializeField] public ScriptableItems OpenCrate;
    [SerializeField] public TMP_Text OpenCrateAmountHeld;
    [SerializeField] public ScriptableItems RoadSign1;
    [SerializeField] public TMP_Text RoadSign1AmountHeld;
    [SerializeField] public ScriptableItems RoadSign2;
    [SerializeField] public TMP_Text RoadSign2AmountHeld;
    [SerializeField] public ScriptableItems Boat;
    [SerializeField] public TMP_Text BoatAmountHeld;
    [SerializeField] public ScriptableItems SackPile;
    [SerializeField] public TMP_Text SackPileAmountHeld;
    [SerializeField] public ScriptableItems StackOBarrels;
    [SerializeField] public TMP_Text StackOBarrelsAmountHeld;
    [SerializeField] public ScriptableItems Statue;
    [SerializeField] public TMP_Text StatueAmountHeld;
    [SerializeField] public ScriptableItems StoneWall;
    [SerializeField] public TMP_Text StoneWallAmountHeld;
    [SerializeField] public ScriptableItems Torch;
    [SerializeField] public TMP_Text TorchAmountHeld;
    [SerializeField] public ScriptableItems WaterWell;
    [SerializeField] public TMP_Text WaterWellAmountHeld;



    //people
    [SerializeField] public GameObject peopleTypeSelector;
    //shop owners, residents, pets, special
    [SerializeField] public GameObject shopOwners;
    [SerializeField] public GameObject residents;
    [SerializeField] public GameObject pets;

    [SerializeField] public GameObject special;
    [SerializeField] public ScriptableItems EmptyCart;
    [SerializeField] public TMP_Text EmptyCartAmountHeld;
    [SerializeField] public ScriptableItems HayCart;
    [SerializeField] public TMP_Text HayCartAmountHeld;
    [SerializeField] public ScriptableItems FarmerCart;
    [SerializeField] public TMP_Text FarmerCartAmountHeld;
    [SerializeField] public ScriptableItems ProduceCart;
    [SerializeField] public TMP_Text ProduceCartAmountHeld;

    //geological
    [SerializeField] public GameObject geologicalTypeSelector;
    //rivers, hills, ditches, special
    [SerializeField] public GameObject rivers;
    [SerializeField] public GameObject hills;
    [SerializeField] public GameObject ditches;


    [SerializeField] public GameObject geoSpecial;
    [SerializeField] public ScriptableItems SmallRock;
    [SerializeField] public TMP_Text SmallRockAmountHeld;
    [SerializeField] public ScriptableItems MediumRock;
    [SerializeField] public TMP_Text MediumRockAmountHeld;
    [SerializeField] public ScriptableItems LargeRock;
    [SerializeField] public TMP_Text LargeRockAmountHeld;
    [SerializeField] public ScriptableItems LogPile;
    [SerializeField] public TMP_Text LogPileAmountHeld;


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
        if (scene == 1) //if eterius
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
        reticle.SetActive(false); //turn off reticle
        //Time.timeScale = 0; //reset time passed to zero
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
            SpiralFloorSelector.SetActive(false);
        }
        else
        {
            activeMenu.SetActive(isPaused); //what ever menu active off
            activeMenu = null; //active menu emptied
        }
        reticle.SetActive(true); //turn off reticle
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
                //playerEye.chosenObject = null;
                Debug.Log("Off");
            }
        }
    }
    void ChangeView()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (buildON)
            {
                buildON = false; //build flag off
                activeMenu = null; //active emptied
                buildMenu.SetActive(false); //build off
                playerEye.RemovePreview();
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
        SmallBirchAmountHeld.text = SmallBirch.amountHeld.ToString(); //Item Counts
        MediumBirchAmountHeld.text = MediumBirch.amountHeld.ToString(); //Item Counts
        TallBirchAmountHeld.text = TallBirch.amountHeld.ToString(); //Item Counts
        LargeBirchAmountHeld.text = LargeBirch.amountHeld.ToString(); //Item Counts
        SmallWillowAmountHeld.text = SmallWillow.amountHeld.ToString(); //Item Counts
        MediumWillowAmountHeld.text = MediumWillow.amountHeld.ToString(); //Item Counts
        LargeWillowAmountHeld.text = LargeWillow.amountHeld.ToString(); //Item Counts


        //turn on this selection menu
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        trees.SetActive(true);
        Debug.Log("Trees Selected");
    }
    public void SmallBirchSelected()
    {
        if (SmallBirch.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = SmallBirch.eshesBuildObject;
        }
    }
    public void MediumBirchSelected()
    {
        if (MediumBirch.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumBirch.eshesBuildObject;
        }
    }
    public void TallBirchSelected()
    {
        if (TallBirch.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = TallBirch.eshesBuildObject;
        }
    }
    public void LargeBirchSelected()
    {
        if (LargeBirch.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeBirch.eshesBuildObject;
        }
    }
    public void SmallWillowSelected()
    {
        if (SmallWillow.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = SmallWillow.eshesBuildObject;
        }
    }
    public void MediumWillowSelected()
    {
        if (MediumWillow.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumWillow.eshesBuildObject;
        }
    }
    public void LargeWillowSelected()
    {
        if (LargeWillow.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeWillow.eshesBuildObject;
        }
    }


    public void FoliageFlowers()
    {
        SetAllInactive();//turn off all build menus
        PurpleFlowerAmountHeld.text = PurpleFlower.amountHeld.ToString(); //Item Counts
        SmallFernAmountHeld.text = SmallFern.amountHeld.ToString(); //Item Counts
        LargeFernAmountHeld.text = LargeFern.amountHeld.ToString(); //Item Counts

        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        flowers.SetActive(true);
        Debug.Log("Flowers Selected");
    }
    public void PurpleFlowerSelected()
    {
        if (PurpleFlower.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = PurpleFlower.eshesBuildObject;
        }
    }
    public void SmallFernSelected()
    {
        if (SmallFern.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = SmallFern.eshesBuildObject;
        }
    }
    public void LargeFernSelected()
    {
        if (LargeFern.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeFern.eshesBuildObject;
        }
    }


    public void FoliageBushes()
    {
        //turn everything off to make sure only the following is open
        SetAllInactive();//turn off all build menus
        //Item Counts
        SmallBushAmountHeld.text = SmallBush.amountHeld.ToString();
        MediumBushAmountHeld.text = MediumBush.amountHeld.ToString();
        LargeBushAmountHeld.text = LargeBush.amountHeld.ToString();
        //turn on this selection
        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        bushes.SetActive(true);
        Debug.Log("Bushes Selected");
    }
    public void SmallBushSelected()
    {
        if (SmallBush.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = SmallBush.eshesBuildObject;
        }
    }
    public void MediumBushSelected()
    {
        if (MediumBush.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumBush.eshesBuildObject;
        }
    }
    public void LargeBushSelected()
    {
        if (LargeBush.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeBush.eshesBuildObject;
        }
    }


    public void FoliageGrass()
    {
        SetAllInactive();//turn off all build menus
        SmallGrassAmountHeld.text = SmallGrass.amountHeld.ToString(); //Item Counts
        MediumGrassAmountHeld.text = MediumGrass.amountHeld.ToString(); //Item Counts
        LargeGrassAmountHeld.text = LargeGrass.amountHeld.ToString(); //Item Counts
        UnderGrowthAmountHeld.text = UnderGrowth.amountHeld.ToString(); //Item Counts

        activeBuildSelection = foliageTypeSelector;
        foliageTypeSelector.SetActive(true);
        grass.SetActive(true);
        Debug.Log("Grass Selected");

    }
    public void SmallGrassSelected()
    {
        if (SmallGrass.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = SmallGrass.eshesBuildObject;
        }
    }
    public void MediumGrassSelected()
    {
        if (MediumGrass.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumGrass.eshesBuildObject;
        }
    }
    public void LargeGrassSelected()
    {
        if (LargeGrass.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeGrass.eshesBuildObject;
        }
    }
    public void UnderGrowthSelected()
    {
        if (UnderGrowth.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = UnderGrowth.eshesBuildObject;
        }
    }
    #endregion

    #region Buildings
    public void BuildingsTop()
    {
        //activate Building Type Selector
        if (activeBuildSelection == null || activeBuildSelection != null)
        {
            SetAllInactive();//turn off all build menus


            ShedAmountHeld.text = Shed.amountHeld.ToString(); //Item Counts
            RedStallAmountHeld.text = RedStall.amountHeld.ToString(); //Item Counts
            GreenStallAmountHeld.text = GreenStall.amountHeld.ToString(); //Item Counts
            OpenStallAmountHeld.text = OpenStall.amountHeld.ToString(); //Item Counts
            YellowStallAmountHeld.text = YellowStall.amountHeld.ToString(); //Item Counts

            BaseRedHouseAmountHeld.text = BaseRedHouse.amountHeld.ToString(); //Item Counts
            BaseDarkRedHouseAmountHeld.text = BaseDarkRedHouse.amountHeld.ToString(); //Item Counts
            BaseGreenHouseAmountHeld.text = BaseGreenHouse.amountHeld.ToString(); //Item Counts
            BaseBlueHouseAmountHeld.text = BaseBlueHouse.amountHeld.ToString(); //Item Counts
            MediumRedHouseAmountHeld.text = MediumRedHouse.amountHeld.ToString(); //Item Counts
            MediumDarkRedHouseAmountHeld.text = MediumDarkRedHouse.amountHeld.ToString(); //Item Counts
            MediumGreenHouseAmountHeld.text = MediumGreenHouse.amountHeld.ToString(); //Item Counts
            MediumBlueHouseAmountHeld.text = MediumBlueHouse.amountHeld.ToString(); //Item Counts
            LargeRedHouseAmountHeld.text = LargeRedHouse.amountHeld.ToString(); //Item Counts
            LargeDarkRedHouseAmountHeld.text = LargeDarkRedHouse.amountHeld.ToString(); //Item Counts
            LargeGreenHouseAmountHeld.text = LargeGreenHouse.amountHeld.ToString(); //Item Counts
            LargeBlueHouseAmountHeld.text = LargeBlueHouse.amountHeld.ToString(); //Item Counts
            LiftedRedHouseAmountHeld.text = LiftedRedHouse.amountHeld.ToString(); //Item Counts
            LiftedDarkRedHouseAmountHeld.text = LiftedDarkRedHouse.amountHeld.ToString(); //Item Counts
            LiftedGreenHouseAmountHeld.text = LiftedGreenHouse.amountHeld.ToString(); //Item Counts
            LiftedBlueHouseAmountHeld.text = LiftedBlueHouse.amountHeld.ToString(); //Item Counts

            BrazierAmountHeld.text = Brazier.amountHeld.ToString(); //Item Counts
            BridgeAmountHeld.text = Bridge.amountHeld.ToString(); //Item Counts
            BrokenWallAmountHeld.text = BrokenWall.amountHeld.ToString(); //Item Counts
            CampfireAmountHeld.text = Campfire.amountHeld.ToString(); //Item Counts
            CampfireWithPotAmountHeld.text = CampfireWithPot.amountHeld.ToString(); //Item Counts
            CurvedChestAmountHeld.text = CurvedChest.amountHeld.ToString(); //Item Counts
            DoubleFenceAmountHeld.text = DoubleFence.amountHeld.ToString(); //Item Counts
            EmptyCartAmountHeld.text = EmptyCart.amountHeld.ToString(); //Item Counts
            FarmerCartAmountHeld.text = FarmerCart.amountHeld.ToString(); //Item Counts
            FlatChestAmountHeld.text = FlatChest.amountHeld.ToString(); //Item Counts
            HayCartAmountHeld.text = HayCart.amountHeld.ToString(); //Item Counts
            LampPostAmountHeld.text = LampPost.amountHeld.ToString(); //Item Counts
            LogPileAmountHeld.text = LogPile.amountHeld.ToString(); //Item Counts
            MetalCrateAmountHeld.text = MetalCrate.amountHeld.ToString(); //Item Counts
            OpenCrateAmountHeld.text = OpenCrate.amountHeld.ToString(); //Item Counts
            ProduceCartAmountHeld.text = ProduceCart.amountHeld.ToString(); //Item Counts
            RoadSign1AmountHeld.text = RoadSign1.amountHeld.ToString(); //Item Counts
            RoadSign2AmountHeld.text = RoadSign2.amountHeld.ToString(); //Item Counts
            BoatAmountHeld.text = Boat.amountHeld.ToString(); //Item Counts
            SackPileAmountHeld.text = SackPile.amountHeld.ToString(); //Item Counts
            StackOBarrelsAmountHeld.text = StackOBarrels.amountHeld.ToString(); //Item Counts
            StatueAmountHeld.text = Statue.amountHeld.ToString(); //Item Counts
            StoneWallAmountHeld.text = StoneWall.amountHeld.ToString(); //Item Counts
            TorchAmountHeld.text = Torch.amountHeld.ToString(); //Item Counts
            WaterWellAmountHeld.text = WaterWell.amountHeld.ToString(); //Item Counts
            SmallRockAmountHeld.text = SmallRock.amountHeld.ToString(); //Item Counts
            MediumRockAmountHeld.text = MediumRock.amountHeld.ToString(); //Item Counts
            LargeRockAmountHeld.text = LargeRock.amountHeld.ToString(); //Item Counts



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
    public void ShedSelected()
    {
        if (Shed.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Shed.eshesBuildObject;
        }
    }
    public void RedStallSelected()
    {
        if (RedStall.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = RedStall.eshesBuildObject;
        }
    }
    public void GreenStallSelected()
    {
        if (GreenStall.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = GreenStall.eshesBuildObject;
        }
    }
    public void OpenStallSelected()
    {
        if (OpenStall.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = OpenStall.eshesBuildObject;
        }
    }
    public void YellowStallSelected()
    {
        if (YellowStall.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = YellowStall.eshesBuildObject;
        }
    }


    public void BuildingsShops()
    {
        SetAllInactive();//turn off all build menus
        ApothecaryAmountHeld.text = Apothecary.amountHeld.ToString(); //Item Counts
        BakeryAmountHeld.text = Bakery.amountHeld.ToString(); //Item Counts
        BlackSmithAmountHeld.text = BlackSmith.amountHeld.ToString(); //Item Counts
        ChurchAmountHeld.text = Church.amountHeld.ToString(); //Item Counts
        LeonsInnAmountHeld.text = LeonsInn.amountHeld.ToString(); //Item Counts
        MayorsAmountHeld.text = Mayors.amountHeld.ToString(); //Item Counts

        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        shops.SetActive(true);
        Debug.Log("Shops Selected");
    }
    public void ApothecarySelected()
    {
        if (Apothecary.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Apothecary.eshesBuildObject;
        }
    }
    public void BakerySelected()
    {
        if (Bakery.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Bakery.eshesBuildObject;
        }
    }
    public void BlackSmithSelected()
    {
        if (BlackSmith.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = BlackSmith.eshesBuildObject;
        }
    }
    public void ChurchSelected()
    {
        if (Church.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Church.eshesBuildObject;
        }
    }
    public void LeonsInnSelected()
    {
        if (LeonsInn.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LeonsInn.eshesBuildObject;
        }
    }
    public void MayorsSelected()
    {
        if (Mayors.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Mayors.eshesBuildObject;
        }
    }



    public void BuildingsHouses()
    {
        SetAllInactive();//turn off all build menus
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        houses.SetActive(true);
        Debug.Log("Houses Selected");
    }
    public void BaseRedHouseSelected()
    {
        if (BaseRedHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = BaseRedHouse.eshesBuildObject;
        }
    }
    public void BaseDarkRedHouseSelected()
    {
        if (BaseDarkRedHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = BaseDarkRedHouse.eshesBuildObject;
        }
    }
    public void BaseGreenHouseSelected()
    {
        if (BaseGreenHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = BaseGreenHouse.eshesBuildObject;
        }
    }
    public void BaseBlueHouseSelected()
    {
        if (BaseBlueHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = BaseBlueHouse.eshesBuildObject;
        }
    }
    public void MediumRedHouseSelected()
    {
        if (MediumRedHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumRedHouse.eshesBuildObject;
        }
    }
    public void MediumDarkRedHouseSelected()
    {
        if (MediumDarkRedHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumDarkRedHouse.eshesBuildObject;
        }
    }
    public void MediumGreenHouseSelected()
    {
        if (MediumGreenHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumGreenHouse.eshesBuildObject;
        }
    }
    public void MediumBlueHouseSelected()
    {
        if (MediumBlueHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumBlueHouse.eshesBuildObject;
        }
    }
    public void LargeRedHouseSelected()
    {
        if (LargeRedHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeRedHouse.eshesBuildObject;
        }
    }
    public void LargeDarkRedHouseSelected()
    {
        if (LargeDarkRedHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeDarkRedHouse.eshesBuildObject;
        }
    }
    public void LargeGreenHouseSelected()
    {
        if (LargeGreenHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeGreenHouse.eshesBuildObject;
        }
    }
    public void LargeBlueHouseSelected()
    {
        if (LargeBlueHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeBlueHouse.eshesBuildObject;
        }
    }
    public void LiftedRedHouseSelected()
    {
        if (LiftedRedHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LiftedRedHouse.eshesBuildObject;
        }
    }
    public void LiftedDarkRedHouseSelected()
    {
        if (LiftedDarkRedHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LiftedDarkRedHouse.eshesBuildObject;
        }
    }
    public void LiftedGreenHouseSelected()
    {
        if (LiftedGreenHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LiftedGreenHouse.eshesBuildObject;
        }
    }
    public void LiftedBlueHouseSelected()
    {
        if (LiftedBlueHouse.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LiftedBlueHouse.eshesBuildObject;
        }
    }



    public void BuildingsMisc()
    {
        SetAllInactive();//turn off all build menus
        activeBuildSelection = buildingsTypeSelector;
        buildingsTypeSelector.SetActive(true);
        misc.SetActive(true);
        Debug.Log("Roads Selected");
    }

    public void BrazierSelected()
    {
        if (Brazier.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Brazier.eshesBuildObject;
        }
    }
    public void BridgeSelected()
    {
        if (Bridge.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Bridge.eshesBuildObject;
        }
    }
    public void BrokenWallSelected()
    {
        if (BrokenWall.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = BrokenWall.eshesBuildObject;
        }
    }
    public void CampfireSelected()
    {
        if (Campfire.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Campfire.eshesBuildObject;
        }
    }
    public void CampfireWithPotSelected()
    {
        if (CampfireWithPot.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = CampfireWithPot.eshesBuildObject;
        }
    }
    public void CurvedChestSelected()
    {
        if (CurvedChest.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = CurvedChest.eshesBuildObject;
        }
    }
    public void DoubleFenceSelected()
    {
        if (DoubleFence.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = DoubleFence.eshesBuildObject;
        }
    }
    public void EmptyCartSelected()
    {
        if (EmptyCart.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = EmptyCart.eshesBuildObject;
        }
    }
    public void FarmerCartSelected()
    {
        if (FarmerCart.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = FarmerCart.eshesBuildObject;
        }
    }
    public void FlatChestSelected()
    {
        if (FlatChest.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = FlatChest.eshesBuildObject;
        }
    }
    public void HayCartSelected()
    {
        if (HayCart.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = HayCart.eshesBuildObject;
        }
    }
    public void LampPostSelected()
    {
        if (LampPost.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LampPost.eshesBuildObject;
        }
    }
    public void LogPileSelected()
    {
        if (LogPile.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LogPile.eshesBuildObject;
        }
    }
    public void MetalCrateSelected()
    {
        if (MetalCrate.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MetalCrate.eshesBuildObject;
        }
    }
    public void OpenCrateSelected()
    {
        if (OpenCrate.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = OpenCrate.eshesBuildObject;
        }
    }
    public void ProduceCartSelected()
    {
        if (ProduceCart.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = ProduceCart.eshesBuildObject;
        }
    }
    public void RoadSignSelected()
    {
        if (RoadSign1.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = RoadSign1.eshesBuildObject;
        }
    }
    public void RoadSign2Selected()
    {
        if (RoadSign2.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = RoadSign2.eshesBuildObject;
        }
    }
    public void BoatSelected()
    {
        if (Boat.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Boat.eshesBuildObject;
        }
    }
    public void SackPileSelected()
    {
        if (SackPile.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = SackPile.eshesBuildObject;
        }
    }
    public void StackOBarrelsSelected()
    {
        if (StackOBarrels.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = StackOBarrels.eshesBuildObject;
        }
    }
    public void StatueSelected()
    {
        if (Statue.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Statue.eshesBuildObject;
        }
    }
    public void StoneWallSelected()
    {
        if (StoneWall.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = StoneWall.eshesBuildObject;
        }
    }
    public void TorchSelected()
    {
        if (Torch.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = Torch.eshesBuildObject;
        }
    }
    public void WaterWellSelected()
    {
        if (WaterWell.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = WaterWell.eshesBuildObject;
        }
    }
    public void SmallRockSelected()
    {
        if (SmallRock.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = SmallRock.eshesBuildObject;
        }
    }
    public void MediumRockSelected()
    {
        if (MediumRock.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = MediumRock.eshesBuildObject;
        }
    }
    public void LargeRockSelected()
    {
        if (LargeRock.amountHeld == 0)
        {
            //say inventory is empty
        }
        else
        {
            playerEye.chosenObject = null;
            playerEye.chosenObject = LargeRock.eshesBuildObject;
        }
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
    #endregion

    public void UpdateItemCounts()
    {
        if (scene == 1)
        {
            SmallBirchAmountHeld.text = SmallBirch.amountHeld.ToString();
            MediumBirchAmountHeld.text = MediumBirch.amountHeld.ToString(); //Item Counts
            TallBirchAmountHeld.text = TallBirch.amountHeld.ToString(); //Item Counts
            LargeBirchAmountHeld.text = LargeBirch.amountHeld.ToString(); //Item Counts
            SmallWillowAmountHeld.text = SmallWillow.amountHeld.ToString(); //Item Counts
            MediumWillowAmountHeld.text = MediumWillow.amountHeld.ToString(); //Item Counts
            LargeWillowAmountHeld.text = LargeWillow.amountHeld.ToString(); //Item Counts

            PurpleFlowerAmountHeld.text = PurpleFlower.amountHeld.ToString(); //Item Counts
            SmallFernAmountHeld.text = SmallFern.amountHeld.ToString(); //Item Counts
            LargeFernAmountHeld.text = LargeFern.amountHeld.ToString(); //Item Counts

            SmallBushAmountHeld.text = SmallBush.amountHeld.ToString();
            MediumBushAmountHeld.text = MediumBush.amountHeld.ToString();
            LargeBushAmountHeld.text = LargeBush.amountHeld.ToString();

            SmallGrassAmountHeld.text = SmallGrass.amountHeld.ToString(); //Item Counts
            MediumGrassAmountHeld.text = MediumGrass.amountHeld.ToString(); //Item Counts
            LargeGrassAmountHeld.text = LargeGrass.amountHeld.ToString(); //Item Counts
            UnderGrowthAmountHeld.text = UnderGrowth.amountHeld.ToString(); //Item Counts

            ShedAmountHeld.text = Shed.amountHeld.ToString(); //Item Counts
            RedStallAmountHeld.text = RedStall.amountHeld.ToString(); //Item Counts
            GreenStallAmountHeld.text = GreenStall.amountHeld.ToString(); //Item Counts
            OpenStallAmountHeld.text = OpenStall.amountHeld.ToString(); //Item Counts
            YellowStallAmountHeld.text = YellowStall.amountHeld.ToString(); //Item Counts

            ApothecaryAmountHeld.text = Apothecary.amountHeld.ToString(); //Item Counts
            BakeryAmountHeld.text = Bakery.amountHeld.ToString(); //Item Counts
            BlackSmithAmountHeld.text = BlackSmith.amountHeld.ToString(); //Item Counts
            ChurchAmountHeld.text = Church.amountHeld.ToString(); //Item Counts
            LeonsInnAmountHeld.text = LeonsInn.amountHeld.ToString(); //Item Counts
            MayorsAmountHeld.text = Mayors.amountHeld.ToString(); //Item Counts

            BaseRedHouseAmountHeld.text = BaseRedHouse.amountHeld.ToString(); //Item Counts
            BaseDarkRedHouseAmountHeld.text = BaseDarkRedHouse.amountHeld.ToString(); //Item Counts
            BaseGreenHouseAmountHeld.text = BaseGreenHouse.amountHeld.ToString(); //Item Counts
            BaseBlueHouseAmountHeld.text = BaseBlueHouse.amountHeld.ToString(); //Item Counts
            MediumRedHouseAmountHeld.text = MediumRedHouse.amountHeld.ToString(); //Item Counts
            MediumDarkRedHouseAmountHeld.text = MediumDarkRedHouse.amountHeld.ToString(); //Item Counts
            MediumGreenHouseAmountHeld.text = MediumGreenHouse.amountHeld.ToString(); //Item Counts
            MediumBlueHouseAmountHeld.text = MediumBlueHouse.amountHeld.ToString(); //Item Counts
            LargeRedHouseAmountHeld.text = LargeRedHouse.amountHeld.ToString(); //Item Counts
            LargeDarkRedHouseAmountHeld.text = LargeDarkRedHouse.amountHeld.ToString(); //Item Counts
            LargeGreenHouseAmountHeld.text = LargeGreenHouse.amountHeld.ToString(); //Item Counts
            LargeBlueHouseAmountHeld.text = LargeBlueHouse.amountHeld.ToString(); //Item Counts
            LiftedRedHouseAmountHeld.text = LiftedRedHouse.amountHeld.ToString(); //Item Counts
            LiftedDarkRedHouseAmountHeld.text = LiftedDarkRedHouse.amountHeld.ToString(); //Item Counts
            LiftedGreenHouseAmountHeld.text = LiftedGreenHouse.amountHeld.ToString(); //Item Counts
            LiftedBlueHouseAmountHeld.text = LiftedBlueHouse.amountHeld.ToString(); //Item Counts

            BrazierAmountHeld.text = Brazier.amountHeld.ToString(); //Item Counts
            BridgeAmountHeld.text = Bridge.amountHeld.ToString(); //Item Counts
            BrokenWallAmountHeld.text = BrokenWall.amountHeld.ToString(); //Item Counts
            CampfireAmountHeld.text = Campfire.amountHeld.ToString(); //Item Counts
            CampfireWithPotAmountHeld.text = CampfireWithPot.amountHeld.ToString(); //Item Counts
            CurvedChestAmountHeld.text = CurvedChest.amountHeld.ToString(); //Item Counts
            DoubleFenceAmountHeld.text = DoubleFence.amountHeld.ToString(); //Item Counts
            EmptyCartAmountHeld.text = EmptyCart.amountHeld.ToString(); //Item Counts
            FarmerCartAmountHeld.text = FarmerCart.amountHeld.ToString(); //Item Counts
            FlatChestAmountHeld.text = FlatChest.amountHeld.ToString(); //Item Counts
            HayCartAmountHeld.text = HayCart.amountHeld.ToString(); //Item Counts
            LampPostAmountHeld.text = LampPost.amountHeld.ToString(); //Item Counts
            LogPileAmountHeld.text = LogPile.amountHeld.ToString(); //Item Counts
            MetalCrateAmountHeld.text = MetalCrate.amountHeld.ToString(); //Item Counts
            OpenCrateAmountHeld.text = OpenCrate.amountHeld.ToString(); //Item Counts
            ProduceCartAmountHeld.text = ProduceCart.amountHeld.ToString(); //Item Counts
            RoadSign1AmountHeld.text = RoadSign1.amountHeld.ToString(); //Item Counts
            RoadSign2AmountHeld.text = RoadSign2.amountHeld.ToString(); //Item Counts
            BoatAmountHeld.text = Boat.amountHeld.ToString(); //Item Counts
            SackPileAmountHeld.text = SackPile.amountHeld.ToString(); //Item Counts
            StackOBarrelsAmountHeld.text = StackOBarrels.amountHeld.ToString(); //Item Counts
            StatueAmountHeld.text = Statue.amountHeld.ToString(); //Item Counts
            StoneWallAmountHeld.text = StoneWall.amountHeld.ToString(); //Item Counts
            TorchAmountHeld.text = Torch.amountHeld.ToString(); //Item Counts
            WaterWellAmountHeld.text = WaterWell.amountHeld.ToString(); //Item Counts
            SmallRockAmountHeld.text = SmallRock.amountHeld.ToString(); //Item Counts
            MediumRockAmountHeld.text = MediumRock.amountHeld.ToString(); //Item Counts
            LargeRockAmountHeld.text = LargeRock.amountHeld.ToString(); //Item Counts


            coinPurseText.text = coinPurse.amountHeld.ToString();
        }
    }
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
        misc.SetActive(false);

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
        Cursor.visible = true; //visible cursor
        Cursor.lockState = CursorLockMode.Confined; //keep cursor in the window
        Time.timeScale = 1; //allow time to pass again
        activeMenu = SpiralFloorSelector; //active menu emptied
        activeMenu.SetActive(true); //what ever menu active off

    }
    public void SpiralMenuCancel()
    {
        Cursor.visible = true; //visible cursor
        Cursor.lockState = CursorLockMode.Confined; //keep cursor in the window
        Time.timeScale = 1; //allow time to pass again
        activeMenu = SpiralFloorSelector; //active menu emptied
        activeMenu.SetActive(false); //what ever menu active off
        activeMenu = pauseMenu;
        activeMenu.SetActive(true);

    }
    public void ResetScriptables()
    {
        foreach (var scriptable in scriptableList)
        {
            scriptable.amountHeld = 0;
        }
        foreach (var scriptable in scriptableSkillList)
        {
            scriptable.SkillLevel = 0;
        }
    }
    public void ResetCompleteFloors()
    {
        HighestLevel.highestLevelComplete = 0;
    }
    public void SpiralFloorSelection()
    {
        statePaused();
        SpiralFloorSelector.SetActive(true);
        activeMenu = SpiralFloorSelector;
    }
    public void SpiralFloorButtonClicked(string buttonName)
    {
        int floorNumber;
        floorNumber = int.Parse(buttonName);
        Debug.Log("Floor Number: " + floorNumber);
        if (HighestLevel.highestLevelComplete >= floorNumber - 1)
        {
            FloorConfirmPort.SetActive(true);
            activeMenu = FloorConfirmPort;
        }
    }
}

