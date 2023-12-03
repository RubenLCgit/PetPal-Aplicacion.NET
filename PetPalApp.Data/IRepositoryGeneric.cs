﻿using PetPalApp.Domain;

namespace PetPalApp.Data;

public interface IRepositoryGeneric<T> where T : class
{
  void AddEntity(T entity);
  T GetByNameEntity(string name);
  void UpdateEntity(string key, T entity);
  void DeleteEntity(T entity);
  void SaveChanges();
  Dictionary<string, T> GetAllEntities();
}
