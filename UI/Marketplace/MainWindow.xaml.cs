namespace CryoFall.Marketplace.UI
{
  using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
  using CryoFall.Marketplace.Managers;
  using CryoFall.Marketplace.UI.Data;

  public partial class MainWindow : BaseWindowMenu
  {
    private ViewModelMainWindow viewModel;

    protected override void OnLoaded()
    {
      base.OnLoaded();
    }

    protected override void WindowOpening()
    {
      base.WindowOpening();

      if (EntityViewModelManager.CurrentView.BaseCollection.Count == 0)
        EntityViewModelManager.Init();

      viewModel.RefreshPVE();
    }

    protected override void WindowClosing()
    {
      base.WindowClosing();
    }

    protected override void DisposeMenu()
    {
      base.DisposeMenu();
      DataContext = null;
      viewModel.Dispose();
      viewModel = null;
    }

    protected override void InitMenu()
    {
      DataContext = viewModel = new ViewModelMainWindow();
    }

  }
}