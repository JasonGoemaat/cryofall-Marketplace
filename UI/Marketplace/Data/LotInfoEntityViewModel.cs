namespace CryoFall.Marketplace.UI.Data
{
  using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
  using AtomicTorch.CBND.GameApi.Scripting;
  using System;
  using static AtomicTorch.CBND.CoreMod.Systems.TradingStations.TradingStationsMapMarksSystem;

  public class LotInfoEntityViewModel : BaseViewModel, IComparable
  {
    public string Coords
    {
      get { return this.worldX + ";" + this.worldY; }
    }
    public ushort WorldX
    {
      get { return this.worldX; }
    }
    private ushort worldX;

    public ushort WorldY
    {
      get { return this.worldY; }
    }
    private ushort worldY;

    public string Mode
    {
      get { return this.isBuying ? "Is Buying" : "Is Selling"; }
    }
    public bool IsSelling
    {
      get { return !this.isBuying; }
    }
    public bool IsBuying
    {
      get { return this.isBuying; }
    }
    private bool isBuying;

    public bool IsOwner
    {
      get { return this.isOwner; }
    }
    private bool isOwner;

    public string Name
    {
      get { return this.name; }
    }
    private string name;

    public ushort Qty
    {
      get { return this.qty; }
    }
    private ushort qty;

    public string CountString
    {
      get { return "x " + this.count; }
    }
    public uint Count
    {
      get { return this.count; }
    }
    private uint count;

    public ushort PriceCoinShiny
    {
      get { return this.priceCoinShiny; }
    }
    private ushort priceCoinShiny;

    public ushort PriceCoinPenny
    {
      get { return this.priceCoinPenny; }
    }
    private ushort priceCoinPenny;

    public double PriceCoinShinyRatio
    {
      get { return this.priceCoinShinyRatio; }
    }
    private double priceCoinShinyRatio;

    public double PriceCoinPennyRatio
    {
      get { return this.priceCoinPennyRatio; }
    }
    private double priceCoinPennyRatio;

    public static string SortProperty = "";
    public static string SortOrder = "Asc";

    public LotInfoEntityViewModel(TradingStationMark mark, TradingStationLotInfo lot, bool isBuying)
    {
      var worldBoundsOffset = Api.Client.World.WorldBounds.Offset;
      this.worldX = (ushort)(mark.TilePosition.X - worldBoundsOffset.X);
      this.worldY = (ushort)(mark.TilePosition.Y - worldBoundsOffset.Y);
      this.isBuying = isBuying;
      this.isOwner = mark.IsOwner;
      this.name = lot.ProtoItem.Name;
      this.qty = lot.LotQuantity;
      this.count = lot.CountAvailable;
      this.priceCoinShiny = lot.PriceCoinShiny;
      this.priceCoinPenny = lot.PriceCoinPenny;
      this.priceCoinShinyRatio = 0;
      this.priceCoinPennyRatio = 0;
      if (this.qty != 0)
      {
        this.priceCoinShinyRatio = Math.Round((double)lot.PriceCoinShiny / (double)this.qty, 3);
        this.priceCoinPennyRatio = Math.Round((double)lot.PriceCoinPenny / (double)this.qty, 3);
      }
    }

    public int CompareTo(object obj)
    {
      LotInfoEntityViewModel lot = (LotInfoEntityViewModel)obj;

      int ret = this.CompareToValue(lot);

      return ret;
    }

    private int CompareToValue(LotInfoEntityViewModel lot)
    {
      int compareProperty = CompareToValueProperty(lot);
      if (compareProperty != 0)
      {
        if (SortOrder.ToLower().StartsWith("d"))
          return -compareProperty;
        return compareProperty;
      }

      if (this.name != lot.name)
        return this.name.CompareTo(lot.name);

      if (this.qty != lot.qty)
        return this.qty.CompareTo(lot.qty);

      if (this.Mode != lot.Mode)
        return this.Mode.CompareTo(lot.Mode);

      if (this.priceCoinShiny != lot.priceCoinShiny)
        return this.priceCoinShiny.CompareTo(lot.priceCoinShiny);

      if (this.priceCoinPenny != lot.priceCoinPenny)
        return this.priceCoinPenny.CompareTo(lot.priceCoinPenny);

      if (this.count != lot.count)
        return this.count.CompareTo(lot.count);

      return 0;
    }

    private int CompareToValueProperty(LotInfoEntityViewModel lot)
    {
      double last = SortOrder.ToLower().StartsWith("d") ? double.MinValue : double.MaxValue;

      switch (SortProperty)
      {
        case "Coords":
          if (this.Coords != lot.Coords)
          {
            if (this.worldX != lot.worldX)
              return this.worldX.CompareTo(lot.worldX);
            if (this.worldY != lot.worldY)
              return this.worldY.CompareTo(lot.worldY);
          }
          break;
        case "Mode":
          if (this.Mode != lot.Mode)
            return this.Mode.CompareTo(lot.Mode);
          break;
        case "Name":
          if (this.name != lot.name)
            return this.name.CompareTo(lot.name);
          break;
        case "Qty":
          if (this.qty != lot.qty)
            return this.qty.CompareTo(lot.qty);
          break;
        case "PriceCoinShiny":
          {
            double x = this.priceCoinShiny == 0 ? last : this.priceCoinShiny;
            double l = lot.priceCoinShiny == 0 ? last : lot.priceCoinShiny;
            if (x != l)
              return x.CompareTo(l);

            if (this.priceCoinPenny != lot.priceCoinPenny)
              return this.priceCoinPenny.CompareTo(lot.priceCoinPenny);
          }
          break;
        case "PriceCoinPenny":
          {
            double x = this.priceCoinPenny == 0 ? last : this.priceCoinPenny;
            double l = lot.priceCoinPenny == 0 ? last : lot.priceCoinPenny;
            if (x != l)
              return x.CompareTo(l);

            if (this.priceCoinShiny != lot.priceCoinShiny)
              return this.priceCoinShiny.CompareTo(lot.priceCoinShiny);
          }
          break;
        case "PriceCoinShinyRatio":
          {
            double x = this.priceCoinShinyRatio == 0.0 ? last : this.priceCoinShinyRatio;
            double l = lot.priceCoinShinyRatio == 0.0 ? last : lot.priceCoinShinyRatio;
            if (x != l)
              return x.CompareTo(l);

            if (this.priceCoinPennyRatio != lot.priceCoinPennyRatio)
              return this.priceCoinPennyRatio.CompareTo(lot.priceCoinPennyRatio);
          }
          break;
        case "PriceCoinPennyRatio":
          {
            double x = this.priceCoinPennyRatio == 0.0 ? last : this.priceCoinPennyRatio;
            double l = lot.priceCoinPennyRatio == 0.0 ? last : lot.priceCoinPennyRatio;
            if (x != l)
              return x.CompareTo(l);

            if (this.priceCoinShinyRatio != lot.priceCoinShinyRatio)
              return this.priceCoinShinyRatio.CompareTo(lot.priceCoinShinyRatio);
          }
          break;
        case "Count":
          if (this.count != lot.count)
            return this.count.CompareTo(lot.count);
          break;
      }

      return 0;
    }

    public override string ToString()
    {
      object[] s = new object[]
      {
         "Mark(" + this.Coords + ")",
         this.Mode,
         this.qty,
         this.name,
         "Shiny:", this.priceCoinShiny,
         "Penny:", this.PriceCoinPenny,
         BootstrapperClientMarketplace.IsPVE.HasValue && BootstrapperClientMarketplace.IsPVE.Value ? "Available:" + this.CountString : ""
      };
      return string.Join("  ", s);
    }
  }
}
