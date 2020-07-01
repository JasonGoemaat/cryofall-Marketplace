namespace CryoFall.Marketplace.UI.Data
{
  using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
  using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Menu;
  using CryoFall.Marketplace.UI.Controls;

  public class ViewModelHUDButton : BaseViewModel
  {
    public Menu MenuMarketplace { get; }

    public IButtonReference ButtonReference { get; set; }

    public ViewModelHUDButton()
    {
      MenuMarketplace = Menu.Register<MainWindow>();
      ButtonReference = new MarketplaceButtonReference() { Button = MarketplaceButton.MenuOpen };
    }

    protected override void DisposeViewModel()
    {
      base.DisposeViewModel();

      MenuMarketplace?.Dispose();
      ButtonReference = null;
    }
  }
}