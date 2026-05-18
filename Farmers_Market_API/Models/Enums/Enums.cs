namespace FarmerMarketAPI.Models;

public enum Category { Vegetables = 0, Fruit = 1, Grain = 2, Dairy = 3, Other = 4 }
public enum OrderStatus { Pending = 0, Confirmed = 1, Collected = 2, Cancelled = 3 }
public enum BuyerType { Individual = 0, SpazaShop = 1, Restaurant = 2, School = 3, Other = 4 }
public enum Province { Gauteng = 0, WesternCape = 1, KwaZuluNatal = 2, Limpopo = 3, Mpumalanga = 4, NorthWest = 5, FreeState = 6, NorthernCape = 7, EasternCape = 8 }