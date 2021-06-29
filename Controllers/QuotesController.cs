using Microsoft.AspNetCore.Mvc;
using QuotesDotnetAPI.Services;
using QuotesDotnetAPI.Models;
using System.Collections.Generic;


namespace QuotesDotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotesController : ControllerBase
    {
        public QuotesController()
        {

        }

        [HttpGet]
        public ActionResult<List<Quote>> GetAll() => QuotesService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<Quote> Get(int Id)
        {
            var quote = QuotesService.Get(Id);
            if (quote is null)
                return NotFound();
            
            return quote;
        } 

        // POST action

        // PUT action

        // DELETE action
    }
}