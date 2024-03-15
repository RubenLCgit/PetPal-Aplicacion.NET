using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using PetPalApp.Data;
using PetPalApp.Domain;

namespace PetPalApp.Business;

public class ProductService : IProductService
{

  private IRepositoryGeneric<Product> Prepository;
  private IRepositoryGeneric<User> Urepository;

  public ProductService(IRepositoryGeneric<Product> _prepository, IRepositoryGeneric<User> _urepository)
  {
    Prepository = _prepository;
    Urepository = _urepository;
  }
  public void DeleteProduct(int userId, int productId)
  {
    var product = Prepository.GetByIdEntity(productId);
    User user = Urepository.GetByIdEntity(userId);
    if (user.ListProducts.ContainsKey(productId))
    {
      Prepository.DeleteEntity(product);
    }
    else Console.WriteLine("The product you want to delete does not exist or belongs to another user.");
  }

  public Dictionary<int, Product> GetAllProducts()
  {
    var allProducts = Prepository.GetAllEntities();
    
    return allProducts;
  }

  public string PrintProduct(Dictionary<int, Product> products)
  {
    String allDataProducts = "";
    foreach (var item in products)
    {
      string online;
      if (item.Value.ProductOnline) online = "Yes";
      else online = "No";
      String addProduct = @$"

    ====================================================================================
    
    - Type:                         {item.Value.ProductType}
    - Name:                         {item.Value.ProductName}
    - Desciption:                   {item.Value.ProductDescription}
    - Date of availabilility:       {item.Value.ProductAvailability}
    - Home delivery service:        {online}
    - Score:                        {item.Value.ProductRating}
    - Stock:                        {item.Value.ProductStock} units
    - Price:                        {item.Value.ProductPrice} €
    
    ====================================================================================";

      allDataProducts += addProduct;
    }
    return allDataProducts;
  }

  public void RegisterProduct(int idUser, string nameUser, string type, string nameProduct, string description, decimal price, bool online, int stock)
  {
    Product product = new(type, nameProduct, description, price, online, stock);
    AssignId(product);
    product.UserId = idUser;
    Prepository.AddEntity(product);
    var user = Urepository.GetByIdEntity(idUser);
    user.ListProducts.Add(product.ProductId, product);
    Urepository.UpdateEntity(idUser, user);
  }

  private void AssignId(Product product)
  {
    var allProducts = Prepository.GetAllEntities();
    int nextId = 0;
    if (allProducts == null || allProducts.Count == 0)
    {
      product.ProductId = 1;
    }
    else
    {
      foreach (var item in allProducts)
      {
        if (item.Value.ProductId > nextId)
        {
        nextId = item.Value.ProductId;
        }
      }
      product.ProductId = nextId + 1;
    }
  }

  public Dictionary<int, Product> SearchProduct(string productType)
  {
    var allProducts = Prepository.GetAllEntities();
    Dictionary<int, Product> typeProducts = new();
    foreach (var item in allProducts)
    {
      if (item.Value.ProductType.IndexOf(productType,StringComparison.OrdinalIgnoreCase) >= 0 || item.Value.ProductDescription.IndexOf(productType, StringComparison.OrdinalIgnoreCase) >= 0 || item.Value.ProductName.IndexOf(productType, StringComparison.OrdinalIgnoreCase) >= 0)
      {
        typeProducts.Add(item.Value.ProductId, item.Value);
      }
    }
    return typeProducts;
  }

  public Dictionary<int, Product> ShowMyProducts(int idUser)
  {
    Dictionary<int, Product> userProducts = Urepository.GetByIdEntity(idUser).ListProducts;
    return userProducts;
  }
}