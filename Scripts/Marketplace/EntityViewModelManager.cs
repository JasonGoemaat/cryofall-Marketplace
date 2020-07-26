namespace CryoFall.Marketplace.Managers
{
  using AtomicTorch.CBND.CoreMod.Systems.TradingStations;
  using AtomicTorch.CBND.CoreMod.UI.Controls.Core.Data;
  using CryoFall.Marketplace.UI.Data;
  using System;
  using static AtomicTorch.CBND.CoreMod.Systems.TradingStations.TradingStationsMapMarksSystem;

  public static class EntityViewModelManager
  {
    public static FilteredObservableWithPaging<LotInfoEntityViewModel> CurrentView =
        new FilteredObservableWithPaging<LotInfoEntityViewModel>();

    public async static void Init()
    {
      if (CurrentView.Count() == 0)
        CurrentView.IsReady = true;

      if (!CurrentView.IsReady)
        return;

      try
      {
        CurrentView.IsReady = false;

        Reset();

        SuperObservableCollection<TradingStationsMapMarksSystem.TradingStationMark> marksProvider =
          TradingStationsMapMarksSystem.ClientTradingStationMarksList;

        foreach (var mark in marksProvider)
        {
          var result = await TradingStationsMapMarksSystem.ClientRequestTradingStationInfo(mark.TradingStationId);

          foreach (TradingStationLotInfo lot in result.ActiveLots)
          {
            LotInfoEntityViewModel lotViewModel = new LotInfoEntityViewModel(mark, lot, result.IsBuying);
            CurrentView.Add(lotViewModel);
          }
        }

        CurrentView.Date = DateTime.Now;
      }
      finally
      {
        CurrentView.IsReady = true;
        CurrentView.Refresh();
      }
    }

    public static void Reset()
    {
      CurrentView.Date = DateTime.MinValue;
      CurrentView.Clear();
      CurrentView.Sort = true;
    }

  }
}