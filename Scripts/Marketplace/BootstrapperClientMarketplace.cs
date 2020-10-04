namespace CryoFall.Marketplace
{
  using AtomicTorch.CBND.CoreMod.Bootstrappers;
  using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
  using AtomicTorch.CBND.CoreMod.Systems.PvE;
  using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
  using AtomicTorch.CBND.CoreMod.UI.Controls.Game.HUD;
  using AtomicTorch.CBND.GameApi.Data;
  using AtomicTorch.CBND.GameApi.Data.Characters;
  using AtomicTorch.CBND.GameApi.Scripting;
  using CryoFall.Marketplace.Managers;
  using CryoFall.Marketplace.UI;
  using CryoFall.Marketplace.UI.Data;
  using CryoFall.Marketplace.UI.Helpers;

  [PrepareOrder(afterType: typeof(BootstrapperClientOptions))]
  public class BootstrapperClientMarketplace : BaseBootstrapper
  {
    private static ClientInputContext gameplayInputContext;

    private static ViewModelHUDButton hudButton;

    private static HUDLayoutControl hudLayoutControl;

    public static bool? IsPVE = null;

    public override void ClientInitialize()
    {
      ClientInputManager.RegisterButtonsEnum<MarketplaceButton>();

      BootstrapperClientGame.InitEndCallback += GameInitHandler;

      BootstrapperClientGame.ResetCallback += ResetHandler;
    }
    private async static void InitPVE()
    {
      await PveSystem.ClientAwaitPvEModeFromServer();
      IsPVE = PveSystem.ClientIsPve(false);
    }
    private static void GameInitHandler(ICharacter currentCharacter)
    {
      InitPVE();

      hudButton = new ViewModelHUDButton();

      foreach (var child in Api.Client.UI.LayoutRootChildren)
      {
        if (child is HUDLayoutControl layoutControl)
        {
          hudLayoutControl = layoutControl;
        }
      }

      if (hudLayoutControl != null)
      {
        hudLayoutControl.Loaded += LayoutControl_Loaded;
      }
      else
      {
        //Api.Logger.Error("Marketplace: HUDLayoutControl not found.");
      }

      gameplayInputContext = ClientInputContext
                             .Start("Marketplace menu")
                             .HandleButtonDown(MarketplaceButton.MenuOpen, Menu.Toggle<MainWindow>);
    }

    private static void LayoutControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
      HUDButtonHelper.AddHUDButton(hudButton.MenuMarketplace,
                                   "/Content/Textures/Items/Generic/ItemCoinShiny.png",
                                   "/Content/Textures/Items/Generic/ItemCoinShiny.png",
                                   "Marketplace",
                                   hudButton.ButtonReference);

    }

    private static void ResetHandler()
    {
      EntityViewModelManager.Reset();
      IsPVE = null;

      hudButton?.Dispose();
      hudButton = null;

      if (hudLayoutControl != null)
      {
        hudLayoutControl.Loaded -= LayoutControl_Loaded;
        hudLayoutControl = null;
      }

      gameplayInputContext?.Stop();
      gameplayInputContext = null;
    }

  }
}