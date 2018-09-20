using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ToDoList;
using System;

namespace ToDoList.Models
{
  public class Item
  {

    public int id {get; set; }
    public string description {get; set; }
    public string dueDate {get; set; }
    public int categoryId {get; set;}

    public Item(string Description, string newDueDate, int newCategoryId, int Id = 0)
    {
      id = Id;
      description = Description;
      dueDate = newDueDate;
      categoryId = newCategoryId;
    }

    public void Edit(string newDescription, int newCategoryId)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE items SET description = @newDescription, category_id = @newCategoryId WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      MySqlParameter itemDescription = new MySqlParameter();
      itemDescription.ParameterName = "@newDescription";
      itemDescription.Value = newDescription;
      cmd.Parameters.Add(itemDescription);

      MySqlParameter itemCategoryId = new MySqlParameter();
      itemCategoryId.ParameterName = "@newCategoryId";
      itemCategoryId.Value = newCategoryId;
      cmd.Parameters.Add(itemCategoryId);

      cmd.ExecuteNonQuery();
      description = newDescription;
      categoryId = newCategoryId;

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public void Delete(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items WHERE id = @thisId";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@thisId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        string itemDueDate = rdr.GetString(2);
        int itemCategoryId = rdr.GetInt32(3);
        Item newItem = new Item(itemDescription, itemDueDate, itemCategoryId, itemId);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allItems;
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO items (description, dueDate, category_id) VALUES (@itemDescription, @itemDueDate, @itemCategoryId);";

      MySqlParameter Description = new MySqlParameter();
      Description.ParameterName = "@itemDescription";
      Description.Value = this.description;
      cmd.Parameters.Add(Description);

      MySqlParameter newDueDate = new MySqlParameter();
      newDueDate.ParameterName = "@itemDueDate";
      newDueDate.Value = this.dueDate;
      cmd.Parameters.Add(newDueDate);

      MySqlParameter newCategoryId = new MySqlParameter();
      newCategoryId.ParameterName = "@itemCategoryId";
      newCategoryId.Value = this.categoryId;
      cmd.Parameters.Add(newCategoryId);

      cmd.ExecuteNonQuery();
      id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }
    }

    public static List<Item> Filter(string sortOrder)
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items ORDER BY dueDate " + sortOrder + ";";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while (rdr.Read())
      {
        int id = rdr.GetInt32(0);
        string description = rdr.GetString(1);
        string dueDate = rdr.GetString(2);
        int categoryId = rdr.GetInt32(3);
        Item newItem = new Item(description, dueDate, categoryId, id);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn !=null)
      {
        conn.Dispose();
      }
      return allItems;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {

        Item newItem = (Item) otherItem;
        bool idEquality = (this.id == newItem.id);

        bool descriptionEquality = (this.description == newItem.description);
        return (descriptionEquality && idEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.description.GetHashCode();
    }

    public static Item Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM `items` WHERE id = @thisId;";

      MySqlParameter thisId = new MySqlParameter();
      thisId.ParameterName = "@thisId";
      thisId.Value = id;
      cmd.Parameters.Add(thisId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      int itemId = 0;
      string itemDescription = "";
      string itemDueDate = "";
      int itemCategoryId = 0;

      while (rdr.Read())
      {
          itemId = rdr.GetInt32(0);
          itemDescription = rdr.GetString(1);
          itemDueDate = rdr.GetString(2);
          itemCategoryId = rdr.GetInt32(3);
      }

      Item foundItem= new Item(itemDescription, itemDueDate, itemCategoryId, itemId);  // This line is new!

       conn.Close();
       if (conn != null)
       {
           conn.Dispose();
       }

      return foundItem;  // This line is new!
    }
  }
}
