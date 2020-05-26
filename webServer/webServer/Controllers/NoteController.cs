using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SqliteLib.Model;
using SqliteLib;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors;

namespace webServer.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
        public class NoteController : ControllerBase
        {
            private NoteContext NoteContext;
            public NoteController()
            {
                NoteContext = new NoteContext();
            }

            [HttpGet]
            public IEnumerable<Note> Get()
            {
                return NoteContext.Notes.ToList();
            }

            [HttpGet("{ID}")]
            public ActionResult<Note> GetByID(int ID)
            {
                if (NoteContext.Notes.Any(n => n.ID == ID))
                    return new ActionResult<Note>(NoteContext.Notes.First(n => n.ID == ID));
                return NotFound();
            }
      
        
            [HttpPost]
            public ActionResult<bool> AddSeveral([FromBody] object notes_json)
            {
            if (notes_json is null)
                return false;
            Note[] notes = JsonConvert.DeserializeObject<Note[]>(notes_json.ToString());
                foreach (var n in notes)
                    NoteContext.Notes.Add(n);
                NoteContext.SaveChanges();
                return true;
            }

            [HttpGet("DataFilter/{DateTime}")]
            public ActionResult<List<Note>> GetByDateTime(DateTime DateTime)
            {
                return new ActionResult<List<Note>>(NoteContext.Notes.Where(n => n.DateTime == DateTime).ToList());
            }

            //[HttpGet("PlaceFilter/{Lat}/{Long}/{Rad}")]
            //public ActionResult<List<Note>> GetByPlace(double Lat, double Long, double Rad)
            //{  
            //}
        }
    }

