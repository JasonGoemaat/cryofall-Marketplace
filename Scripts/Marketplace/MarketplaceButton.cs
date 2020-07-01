namespace CryoFall.Marketplace
{
  using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
  using AtomicTorch.CBND.GameApi;
  using AtomicTorch.CBND.GameApi.ServicesClient;
  using System.ComponentModel;

  [NotPersistent]
  public enum MarketplaceButton
  {
    [Description("Open Marketplace")]
    [ButtonInfo(InputKey.F9, Category = "Marketplace")]
    MenuOpen
  }
}