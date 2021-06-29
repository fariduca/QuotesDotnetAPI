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

        [HttpPost]
        public IActionResult Create(Quote quote)
        {
            QuotesService.Add(quote);
            return CreatedAtAction(nameof(Create), new { id = quote.Id }, quote);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int Id, Quote quote)
        {
            if (Id != quote.Id)
                return BadRequest();
            
            var existingQuote = QuotesService.Get(Id);
            if(existingQuote is null)
                return NotFound();

            QuotesService.Update(quote);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int Id)
        {
            var quote = QuotesService.Get(Id);
            if (quote is null)
                return NotFound();
            
            QuotesService.Delete(Id);

            return NoContent();
        }
    }
}