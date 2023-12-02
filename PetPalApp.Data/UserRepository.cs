using System.Text.Json;
using PetPalApp.Domain;

namespace PetPalApp.Data;

public class UserRepository : IRepositoryGeneric<User>
{

  public Dictionary<string, User> EntityDictionary = new Dictionary<string, User>();
  private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserRepository", "UsersRepository.json");

  public void AddEntity(User entity)
  {
    Dictionary<String, User> listUsers;
    try
    {
      if (File.Exists(_filePath))
      {
        listUsers = GetAllEntities();
        EntityDictionary = listUsers;
      }
      EntityDictionary.Add(entity.UserName, entity);
      SaveChanges();
    }
    catch (Exception ex)
    {
      throw new Exception("No se ha podido realizar el registro", ex);
    }
  }

  public void DeleteEntity(User entity)
  {
    throw new NotImplementedException();
  }

  public Dictionary<string, User> GetAllEntities()
  {
    Dictionary <String, User> dictionaryUsers = new Dictionary<string, User>();;
    String jsonString;
    if (File.Exists(_filePath))
    {
      jsonString = File.ReadAllText(_filePath);
      dictionaryUsers = JsonSerializer.Deserialize<Dictionary<string, User>>(jsonString);
    }
    else
    {
      dictionaryUsers = EntityDictionary;
    }
    return dictionaryUsers;
  }

  public User GetByNameEntity(string name)
  {
    var dictionaryCurrentUser = GetAllEntities();
    User user = new();
    foreach (var item in dictionaryCurrentUser)
    {
      if (item.Value.UserName.Equals(name, StringComparison.OrdinalIgnoreCase))
      {
        user = item.Value;
        break;
      }
    }
    return user;
  }

  public void UpdateEntity(User entity)
  {
    throw new NotImplementedException();
  }

  public void SaveChanges()
  {
    string directoryPath = Path.GetDirectoryName(_filePath);
    if (!Directory.Exists(directoryPath))
    {
      Directory.CreateDirectory(directoryPath);
    }
    var serializeOptions = new JsonSerializerOptions { WriteIndented = true };
    string jsonString = JsonSerializer.Serialize(EntityDictionary, serializeOptions);
    File.WriteAllText(_filePath, jsonString);
  }
}