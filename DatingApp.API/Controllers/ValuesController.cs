using System.Threading.Tasks;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


//FrontEndApp proxies all incoming requests into the calls to BackEndApp using this controller:
namespace DatingApp.API.Controllers
{
    // Class Attribute
//http:localhost:5000/api/values
    [Route("api/[controller]")]
    [ApiController]    //Aoutomatically validates our request
//*ControllerBase Sınıfından Inherit ediliyor olması gerekir.
    public class ValuesController : ControllerBase  //When we create new controller                             |
                                                     //it inherites from this base class. And these gives       |
                                                     //us access like HTTP responses and actions then we can    |
                                                     // use this inside our controller
    {//veritabanına bağlanılacak kod:
        private readonly DataContext _context;

        public ValuesController(DataContext context)
        {
            _context = context; 

        }

       [Authorize(Roles = "Admin, Moderator")]
        [HttpGet]
         //Burada api/value ya  request geldiğinde Getvalue klasına girer ve dataya ulaşıp;
         // oradaki Values datasetinden verileri alıp liste halinde values değişkenine atar. 
        public async Task<IActionResult> GetValues()
        {
            var values = await _context.Values.ToListAsync(); //veritabanındaki verilere erişebilmemi sağlar.
          
            return Ok(values);

        }

        //Get api/value/5
        [Authorize(Roles = "Member")]
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
        [HttpPut("{id}")] //Genelde güncelleneler için kullanılır.
        public void Put(int id, [FromBody] string value)
        {
        }

        //Delete api/value/5
        [HttpDelete("{id}")] //silmek için kullanılır.
        public void Delete(int id)
        {
        }
    }
}