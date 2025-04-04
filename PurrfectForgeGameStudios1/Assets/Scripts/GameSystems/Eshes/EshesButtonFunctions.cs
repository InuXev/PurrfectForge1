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
    public void HousesPageNext()
    {
        gameManager.housesPageTwo.SetActive(true);
        gameManager.houses.SetActive(false);
    }
    public void HousesPageBack()
    {
        gameManager.housesPageTwo.SetActive(false);
        gameManager.houses.SetActive(true);
    }
    public void BuildingMiscNext()
    {
        gameManager.miscPageTwo.SetActive(true);
        gameManager.misc.SetActive(false);
    }
    public void BuildingMiscBack()
    {
        gameManager.miscPageTwo.SetActive(false);
        gameManager.misc.SetActive(true);
    }
    public void ToBackPack()
    {
        gameManager.activeMenu.SetActive(false);
        gameManager.activeMenu = gameManager.BackPackMenu;
        gameManager.BackPackMenu.SetActive(true);
    }
    public void FromBackPack()
    {
        gameManager.BackPackMenu.SetActive(false);
        gameManager.activeMenu = gameManager.pauseMenu;
        gameManager.activeMenu.SetActive(true);

    }
    public void ToJournal()
    {
        gameManager.activeMenu.SetActive(false);
        gameManager.activeMenu = gameManager.BackPackMenu;
        gameManager.JournalMenu.SetActive(true);
    }
    public void FromJournal()
    {
        gameManager.JournalMenu.SetActive(false);
        gameManager.activeMenu = gameManager.pauseMenu;
        gameManager.activeMenu.SetActive(true);
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
        saveLoadManager.ClearInventory();
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
    public void SmallBirch()
    {
        gameManager.buildON = true;
        gameManager.SmallBirchSelected();
    }
    public void MediumBirch()
    {
        gameManager.buildON = true;
        gameManager.MediumBirchSelected();
    }
    public void TallBirch()
    {
        gameManager.buildON = true;
        gameManager.TallBirchSelected();
    }
    public void LargeBirch()
    {
        gameManager.buildON = true;
        gameManager.LargeBirchSelected();
    }
    public void SmallWillow()
    {
        gameManager.buildON = true;
        gameManager.SmallWillowSelected();
    }
    public void MediumWillow()
    {
        gameManager.buildON = true;
        gameManager.MediumWillowSelected();
    }
    public void LargeWillow()
    {
        gameManager.buildON = true;
        gameManager.LargeWillowSelected();
    }


    //flowers
    public void FoliageFlowers()
    {
        gameManager.FoliageFlowers();
    }

    public void PurpleFlower()
    {
        gameManager.buildON = true;
        gameManager.PurpleFlowerSelected();
    }
    public void SmallFern()
    {
        gameManager.buildON = true;
        gameManager.SmallFernSelected();
    }
    public void LargeFern()
    {
        gameManager.buildON = true;
        gameManager.LargeFernSelected();
    }



    //Bushes
    public void FoliageBushes()
    {
        gameManager.FoliageBushes();
    }
    public void SmallBush()
    {
        gameManager.buildON = true;
        gameManager.SmallBushSelected();
    }
    public void MediumBush()
    {
        gameManager.buildON = true;
        gameManager.MediumBushSelected();
    }
    public void LargeBush()
    {
        gameManager.buildON = true;
        gameManager.LargeBushSelected();
    }


    //Grass
    public void FoliageGrass()
    {
        gameManager.FoliageGrass();
    }
    public void SmallGrass()
    {
        gameManager.buildON = true;
        gameManager.SmallGrassSelected();
    }
    public void MediumGrass()
    {
        gameManager.buildON = true;
        gameManager.MediumGrassSelected();
    }
    public void LargeGrass()
    {
        gameManager.buildON = true;
        gameManager.LargeGrassSelected();
    }
    public void UnderGrowth()
    {
        gameManager.buildON = true;
        gameManager.UnderGrowthSelected();
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
    public void Shed()
    {
        gameManager.buildON = true;
        gameManager.ShedSelected();
    }
    public void RedStall()
    {
        gameManager.buildON = true;
        gameManager.RedStallSelected();
    }
    public void GreenStall()
    {
        gameManager.buildON = true;
        gameManager.GreenStallSelected();
    }
    public void OpenStall()
    {
        gameManager.buildON = true;
        gameManager.OpenStallSelected();
    }
    public void yellowStall()
    {
        gameManager.buildON = true;
        gameManager.YellowStallSelected();
    }


    public void BuildingsShops()
    {
        //activate Building Type Selector
        gameManager.BuildingsShops();
    }
    public void Apothecary()
    {
        gameManager.buildON = true;
        gameManager.ApothecarySelected();
    }
    public void Bakery()
    {
        gameManager.buildON = true;
        gameManager.BakerySelected();
    }
    public void BlackSmith()
    {
        gameManager.buildON = true;
        gameManager.BlackSmithSelected();
    }
    public void Church()
    {
        gameManager.buildON = true;
        gameManager.ChurchSelected();
    }
    public void LeonsInn()
    {
        gameManager.buildON = true;
        gameManager.LeonsInnSelected();
    }
    public void Mayors()
    {
        gameManager.buildON = true;
        gameManager.MayorsSelected();
    }


    public void BuildingsHouses()
    {
        //activate Building Type Selector
        gameManager.BuildingsHouses();
    }

    public void BaseRedHouse()
    {
        gameManager.buildON = true;
        gameManager.BaseRedHouseSelected();
    }
    public void BaseDarkRedHouse()
    {
        gameManager.buildON = true;
        gameManager.BaseDarkRedHouseSelected();
    }
    public void BaseGreenHouse()
    {
        gameManager.buildON = true;
        gameManager.BaseGreenHouseSelected();
    }
    public void BaseBlueHouse()
    {
        gameManager.buildON = true;
        gameManager.BaseBlueHouseSelected();
    }
    public void MediumRedHouse()
    {
        gameManager.buildON = true;
        gameManager.MediumRedHouseSelected();
    }
    public void MediumDarkRedHouse()
    {
        gameManager.buildON = true;
        gameManager.MediumDarkRedHouseSelected();
    }
    public void MediumGreenHouse()
    {
        gameManager.buildON = true;
        gameManager.MediumGreenHouseSelected();
    }
    public void MediumBlueHouse()
    {
        gameManager.buildON = true;
        gameManager.MediumBlueHouseSelected();
    }
    public void LargeRedHouse()
    {
        gameManager.buildON = true;
        gameManager.LargeRedHouseSelected();
    }
    public void LargeDarkRedHouse()
    {
        gameManager.buildON = true;
        gameManager.LargeDarkRedHouseSelected();
    }
    public void LargeGreenHouse()
    {
        gameManager.buildON = true;
        gameManager.LargeGreenHouseSelected();
    }
    public void LargeBlueHouse()
    {
        gameManager.buildON = true;
        gameManager.LargeBlueHouseSelected();
    }
    public void LiftedRedHouse()
    {
        gameManager.buildON = true;
        gameManager.LiftedRedHouseSelected();
    }
    public void LiftedDarkRedHouse()
    {
        gameManager.buildON = true;
        gameManager.LiftedDarkRedHouseSelected();
    }
    public void LiftedGreenHouse()
    {
        gameManager.buildON = true;
        gameManager.LiftedGreenHouseSelected();
    }
    public void LiftedBlueHouse()
    {
        gameManager.buildON = true;
        gameManager.LiftedBlueHouseSelected();
    }




    public void BuildingsMisc()
    {
        //activate Building Type Selector
        gameManager.BuildingsMisc();
    }
    public void Brazier()
    {
        gameManager.buildON = true;
        gameManager.BrazierSelected();
    }
    public void Bridge()
    {
        gameManager.buildON = true;
        gameManager.BridgeSelected();
    }
    public void BrokenWall()
    {
        gameManager.buildON = true;
        gameManager.BrokenWallSelected();
    }
    public void Campfire()
    {
        gameManager.buildON = true;
        gameManager.CampfireSelected();
    }
    public void CampfireWithPot()
    {
        gameManager.buildON = true;
        gameManager.CampfireWithPotSelected();
    }
    public void CurvedChest()
    {
        gameManager.buildON = true;
        gameManager.CurvedChestSelected();
    }
    public void DoubleFence()
    {
        gameManager.buildON = true;
        gameManager.DoubleFenceSelected();
    }
    public void EmptyCart()
    {
        gameManager.buildON = true;
        gameManager.EmptyCartSelected();
    }
    public void FarmerCart()
    {
        gameManager.buildON = true;
        gameManager.FarmerCartSelected();
    }
    public void FlatChest()
    {
        gameManager.buildON = true;
        gameManager.FlatChestSelected();
    }
    public void HayCart()
    {
        gameManager.buildON = true;
        gameManager.HayCartSelected();
    }
    public void LampPost()
    {
        gameManager.buildON = true;
        gameManager.LampPostSelected();
    }
    public void LogPile()
    {
        gameManager.buildON = true;
        gameManager.LogPileSelected();
    }
    public void MetalCrate()
    {
        gameManager.buildON = true;
        gameManager.MetalCrateSelected();
    }
    public void OpenCrate()
    {
        gameManager.buildON = true;
        gameManager.OpenCrateSelected();
    }
    public void ProduceCart()
    {
        gameManager.buildON = true;
        gameManager.ProduceCartSelected();
    }
    public void RoadSign1()
    {
        gameManager.buildON = true;
        gameManager.RoadSignSelected();
    }
    public void RoadSign2()
    {
        gameManager.buildON = true;
        gameManager.RoadSign2Selected();
    }
    public void Boat()
    {
        gameManager.buildON = true;
        gameManager.BoatSelected();
    }
    public void SackPile()
    {
        gameManager.buildON = true;
        gameManager.SackPileSelected();
    }
    public void StackOBarrels()
    {
        gameManager.buildON = true;
        gameManager.StackOBarrelsSelected();
    }
    public void Statue()
    {
        gameManager.buildON = true;
        gameManager.StatueSelected();
    }
    public void StoneWall()
    {
        gameManager.buildON = true;
        gameManager.StoneWallSelected();
    }
    public void Torch()
    {
        gameManager.buildON = true;
        gameManager.TorchSelected();
    }
    public void WaterWell()
    {
        gameManager.buildON = true;
        gameManager.WaterWellSelected();
    }
    public void SmallRock()
    {
        gameManager.buildON = true;
        gameManager.SmallRockSelected();
    }
    public void MediumRock()
    {
        gameManager.buildON = true;
        gameManager.MediumRockSelected();
    }
    public void LargeRock()
    {
        gameManager.buildON = true;
        gameManager.LargeRockSelected();
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
