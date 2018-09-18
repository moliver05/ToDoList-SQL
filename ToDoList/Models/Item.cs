using System.Collections.Generic;
using System.Collections;

namespace ToDoList.Models
{
  public class Item
  {
    private string _description;
    private int _id;


    public Item (string description, int Id = 0)
    {
      _description = description;
      _id = Id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public int GetId()
    {
      return _id;
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
           Item newItem = new Item(itemDescription, itemId);
           allItems.Add(newItem);
         }
         conn.Close();
         if (conn != null)
         {
             conn.Dispose();
         }
         return allItems;
    }
   }
  }
  // public class Program
  // {
  //   public static void Main ()
  //   {
  //     Console.WriteLine("Would You like to add an item to your list or view your list? (Add/View)");
  //     string response = Console.ReadLine().ToLower();
  //
  //     if (response == "add")
  //     {
  //       Console.WriteLine ("Please Add To Do:");
  //       string wordEntered = Console.ReadLine();
  //       Item newItem = new Item(wordEntered);
  //       newItem.Save();
  //
  //       Console.WriteLine (wordEntered +  " has been added to your list.");
  //       Main();
  //     }
  //     else if (response == "view")
  //     {
  //       List<Item> instances = Item.GetAll();
  //       for (int i = 0; i < instances.Count; i++)
  //       {
  //         string instanceDescription = instances[i].GetDescription();
  //         Console.WriteLine((i + 1) + ". " + instanceDescription);
  //       }
  //       Main();
  //     }
  //     else
  //     {
  //       Console.WriteLine("Would you like to quit the program? Type 'yes' to quit, otherwise hit enter to continue.");
  //       if (Console.ReadLine() != "yes")
  //       {
  //         Main();
  //       }
  //     }
  //   }
// }
