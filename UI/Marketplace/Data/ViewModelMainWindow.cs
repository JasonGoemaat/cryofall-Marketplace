namespace CryoFall.Marketplace.UI.Data
{
  using AtomicTorch.CBND.CoreMod.Systems.Notifications;
  using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
  using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
  using CryoFall.Marketplace.Managers;

  public class ViewModelMainWindow : BaseViewModel
  {
    public bool ModeBuying
    {
      get => this.modeBuying;
      set
      {
        if (this.modeBuying == value)
          return;

        this.modeBuying = value;
        NotifyThisPropertyChanged();
        EntityViewModelCollection.Refresh();
      }
    }
    private bool modeBuying = true;

    public bool ModeSelling
    {
      get => this.modeSelling;
      set
      {
        if (this.ModeSelling == value)
          return;

        this.modeSelling = value;
        NotifyThisPropertyChanged();
        EntityViewModelCollection.Refresh();
      }
    }
    private bool modeSelling = true;

    public string SearchText
    {
      get => searchText;
      set
      {
        value = value?.TrimStart() ?? string.Empty;
        if (searchText == value)
        {
          return;
        }
        searchText = value;
        NotifyThisPropertyChanged();
        EntityViewModelCollection.Refresh();
      }
    }
    private string searchText = string.Empty;

    public string IsCountVisible => BootstrapperClientMarketplace.IsPVE.HasValue && BootstrapperClientMarketplace.IsPVE.Value ? "Visible" : "Hidden";

    public int NameWidth => BootstrapperClientMarketplace.IsPVE.HasValue && BootstrapperClientMarketplace.IsPVE.Value ? 205 : 270;

    public FilteredObservableWithPaging<LotInfoEntityViewModel> EntityViewModelCollection { get; set; }

    public int PageCapacity = 27;

    public BaseCommand NextPage { get; }

    public BaseCommand PrevPage { get; }

    public BaseCommand Refresh { get; }

    public BaseCommand Notify { get; }

    public BaseCommand Sort { get; }

    private bool SearchFilter(LotInfoEntityViewModel model)
    {
      return
        model.Name.ToLower().Contains(searchText.ToLower()) ||
        model.ProtoItemType.ToLower().Contains((searchText.ToLower()));
    }

    private bool ModeFilter(LotInfoEntityViewModel model)
    {
      if (this.modeBuying && model.IsBuying)
        return true;

      if (this.ModeSelling && model.IsSelling)
        return true;

      return false;
    }

    public ViewModelMainWindow()
    {
      LotInfoEntityViewModel.SortProperty = "PriceCoinPennyRatio";
      LotInfoEntityViewModel.SortOrder = "Asc";

      EntityViewModelCollection = EntityViewModelManager.CurrentView;
      EntityViewModelCollection.AddFilter(SearchFilter);
      EntityViewModelCollection.AddFilter(ModeFilter);
      EntityViewModelCollection.SetPageCapacity(PageCapacity);

      NextPage = new ActionCommand(() => EntityViewModelCollection.NextPage());
      PrevPage = new ActionCommand(() => EntityViewModelCollection.PrevPage());

      Refresh = new ActionCommand(() => EntityViewModelManager.Init());

      Notify = new ActionCommandWithParameter(lot =>
      {
        NotificationSystem.ClientShowNotification("Marketplace", lot.ToString(), NotificationColor.Neutral, null, null, false, true, false);
      });

      Sort = new ActionCommandWithParameter(param =>
      {
        string property = param.ToString();

        if (property.ToString() == LotInfoEntityViewModel.SortProperty)
        {
          LotInfoEntityViewModel.SortOrder = LotInfoEntityViewModel.SortOrder == "Asc" ? "Desc" : "Asc";
        }
        else
        {
          LotInfoEntityViewModel.SortOrder = "Asc";
          LotInfoEntityViewModel.SortProperty = property.ToString();
        }
        EntityViewModelCollection.Refresh();
      });
    }

    public void RefreshPVE()
    {
      NotifyThisPropertyChanged("IsCountVisible");
      NotifyThisPropertyChanged("NameWidth");
    }

  }
}