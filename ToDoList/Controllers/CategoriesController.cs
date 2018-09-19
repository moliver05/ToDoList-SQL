using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {

    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    [HttpGet("/categories/new")]
    public ActionResult CategoryForm()
    {
      return View();
    }

    [HttpPost("/categories")]
    public ActionResult Create(string categoryName)
    {
      Category newCategory = new Category(categoryName);
      List<Category> allCategories = Category.GetAll();
      return View("Index", allCategories);
    }

    [HttpPost("/items")]
    public ActionResult CreateItem(int categoryId, string itemDescription)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category foundCategory = Category.Find(categoryId);
      Item newItem = new Item(itemDescription);
      foundCategory.AddItem(newItem);
      List<Item> categoryItems = foundCategory.GetItems();
      model.Add("items", categoryItems);
      model.Add("category", foundCategory);
      return View("Details", model);
    }


    [HttpGet("/categories/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category selectedCategory = Category.Find(id);
      List<Item> categoryItems = selectedCategory.GetItems();
      model.Add("category", selectedCategory);
      model.Add("items", categoryItems);
      return View(model);
    }

    [HttpGet("/categories/{categoryId}/items{itemid}")]
    public ActionResult ItemByCategory(int categoryid, int itemid)
    {
      Item item = Item.Find(itemid);
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category category = Category.Find(categoryid);
      model.Add("category", category);
      model.Add("item", item);
      return View(item);
    }


    // [HttpGet("/categories/{id}/update")]
    // public ActionResult UpdateForm(int id)
    // {
    //   Category thisCategory = Category.Find(id);
    //   return View(thisCategory);
    // }
    //
    // [HttpPost("/items/{id}/update")]
    // public ActionResult Update(int id, string newDescription)
    // {
    //   Category thisCategory = Category.Find(id);
    //   thisCategory.Edit(newDescription);
    //   return RedirectToAction("Index");
    // }

    [HttpPost("/items/delete")]
    public ActionResult DeleteAll()
    {
      Category.DeleteAll();
      return View();
    }

  }
}
