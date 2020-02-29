using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

//FrontEndApp proxies all incoming requests into the calls to BackEndApp using this controller:
namespace DatingApp.API.Controllers
{
//http:localhost:5000/api/values
    [Route("api/[controller]")]
    [ApiController]

    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context; 

        }

        //Get api/values
        [HttpGet]
         //Burada api/value ya  request geldiğinde Getvalue klasına girer ve dataya ulaşıp;
         // oradaki Values datasetinden verileri alıp liste halinde values değişkenine atar. 
        public async Task<IActionResult> GetValues()
        {
            var values = await _context.Values.ToListAsync();
          
            return Ok(values);

        }

        //Get api/value/5
       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(value);
        } 
        //Post/api/value
        [HttpPost]
        public void post([FromBody] string value)
        {
        }
        //Put/api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        //Delete api/value/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}