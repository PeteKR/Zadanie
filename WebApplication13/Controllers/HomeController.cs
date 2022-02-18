using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebApplication13.Models;

namespace WebApplication13.Controllers
{
    public class HomeController : Controller
    {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = new HttpResponseMessage();

        public async Task<ActionResult> Index()
        {
            var get = await GetUsersAsync();
            return View(get);
        }

        public async Task<ActionResult> Todos()
        {
            var get = await GetTodos();
            return View(get);
        }

        public async Task<ActionResult> TodosAndUsers(string sortOrder)
        {
            var get = await GetUsersAsync();
            var get2 = await GetTodos();
            List<UsersTodo> UsersTodoList = new List<UsersTodo>();
            for (int i = 0; i < get.Count; i++)
            {
                for (int k = 0; k < get2.Count; k++)
                {
                    if (get[i].Id == get2[k].UserId)
                    {
                        UsersTodoList.Add(new UsersTodo()
                        {
                            Id=get2[k].Id,
                            UserId=get2[i].Id,
                            Name=get[i].Name,
                            Title=get2[k].Title,
                            Completed=get2[k].Completed,
                        });
                    }
                }
            }
            ViewBag.CompletedSortParam = sortOrder == "Completed?" ? "Not_Completed" : "Completed";
            switch (sortOrder)
            {
                case "Completed":
                    UsersTodoList.RemoveAll(s => s.Completed == true);
                    break;
                default:
                    UsersTodoList.RemoveAll(s => s.Completed == false);
                    break;
            }
            return View(UsersTodoList);
        }

        private async Task<List<User>> GetUsersAsync()
        {
            List<User> listaUsers = new List<User>();
            response = await client.GetAsync($"https://jsonplaceholder.typicode.com/users/");
            string responseBody = await response.Content.ReadAsStringAsync();
            JArray jArray = JArray.Parse(responseBody);
            for (int i = 0; i < jArray.Count; i++)
            {
                listaUsers.Add(new User()
                {
                    Id = int.Parse(jArray[i]["id"].ToString()),
                    Name = jArray[i]["name"].ToString(),
                    Email = jArray[i]["email"].ToString()
                });
            }
            return listaUsers;
        }
        
        private async Task<List<Todo>> GetTodos()
        {
            List<Todo> listaTodos = new List<Todo>();
            response = await client.GetAsync($"https://jsonplaceholder.typicode.com/todos/");
            string responseBody = await response.Content.ReadAsStringAsync();
            JArray jArray = JArray.Parse(responseBody);
            for (int i = 0; i < jArray.Count; i++)
            {
                listaTodos.Add(new Todo()
                {
                    UserId = int.Parse(jArray[i]["userId"].ToString()),
                    Id = int.Parse(jArray[i]["id"].ToString()),
                    Title = jArray[i]["title"].ToString(),
                    Completed = bool.Parse(jArray[i]["completed"].ToString())
                });
            }
            return listaTodos;
        }
    }
}