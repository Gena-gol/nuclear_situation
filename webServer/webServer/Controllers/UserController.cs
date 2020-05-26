using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqliteLib.Model;

namespace webServer.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private NoteContext NoteContext;
        public UserController()
        {
            NoteContext = new NoteContext();
        }

        [HttpGet("{login}/{password}")]
        public ActionResult<bool> CheckUser(string login, string password)
        {
            return NoteContext.Users.Any(n => n.Login == login && n.Password == password);
        }

        [HttpPost("{login}/{password}")]
        public ActionResult<bool> Add([FromBody] string login, string password)
        {
            if (NoteContext.Users.Any(n => n.Login == login))
                return false;
            var user = new User();
            user.Login = login;
            user.Password = password;
            NoteContext.Users.Add(user);
            NoteContext.SaveChanges();
            return true;
        }
    }
}