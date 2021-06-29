using Microsoft.AspNetCore.Mvc;
using QuotesDotnetAPI.Services;
using QuotesDotnetAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using System;
using Microsoft.Extensions.Logging;

namespace QuotesDotnetAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuotesController : ControllerBase
    {
        private readonly Timer _cleanUpTimer;
        private readonly Timer _dailyQuoteTimer;
        private readonly ILogger<QuotesController> _logger;

        public QuotesController(ILogger<QuotesController> logger)
        {
            _logger = logger;

            _cleanUpTimer = new Timer
            {
                AutoReset = true, Enabled = true, Interval = TimeSpan.FromMinutes(5).TotalMilliseconds 
            };
            _cleanUpTimer.Elapsed += CleanUpOldQuotes;

            _dailyQuoteTimer = new Timer
            {
                AutoReset = true, Enabled = true, Interval = TimeSpan.FromDays(1).TotalMilliseconds
            };
            _dailyQuoteTimer.Elapsed += SendDailyQuote;
        }

        private void CleanUpOldQuotes(object sender, ElapsedEventArgs e)
        {
            var quotes = QuotesService.GetOld();
            foreach(var quote in quotes)
            {
                QuotesService.Delete(quote.Id);
            }
        }

        private void SendDailyQuote(object sender, ElapsedEventArgs e)
        {
            var emails = SubscribersService.GetAllEmails();
            var phones = SubscribersService.GetAllPhones();
            var randomQuote = QuotesService.GetRandom();
            foreach(var email in emails)
            {                
                // TODO: send the randomQuote to email
                _logger.Log(LogLevel.Information, "\n\tSent daily quote to " + email, null);
            }
            foreach(var phone in phones)
            {
                // TODO: send the randomQuote to phone
                _logger.Log(LogLevel.Information, "\n\tSent daily quote to " + phone, null);
            }
        }

        /// <summary>Retrieves a list of all quotes </summary>
        /// <returns>A list of all quotes </returns>
        [HttpGet]
        [Produces("application/json")]
        public ActionResult<List<Quote>> GetAll() => QuotesService.GetAll();

        /// <summary>Retrieves a quote based on id</summary>
        /// <param name="Id"></param>
        /// <response code="200">Success</response>
        /// <response code="404">A quote matching the provided id parameter doesn't exist in the in-memory.</response>
        [HttpGet("{Id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ActionResult<Quote> Get(int Id)
        {
            var quote = QuotesService.Get(Id);
            if (quote is null)
                return NotFound();
            
            return quote;
        } 

        /// <summary>Retrieves all quotes in a given category</summary>
        /// <param name="category"></param>        
        [HttpGet("cat/{category}")]
        [Produces("application/json")]
        public ActionResult<List<Quote>> GetByCategory(string category)
        {
            var quote = QuotesService.GetByCategory(category); 

            return quote;
        }

        /// <summary>Retrieves a random quote</summary>
        [HttpGet("rand")]
        [Produces("application/json")]
        public ActionResult<Quote> GetRandom()
        {
            return QuotesService.GetRandom();
        }

        /// <summary>Creates a new Quote. Id and CreatedAt can be ommited as they are autogenerated</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Quotes
        ///     {
        ///         "Author":"Dalai Lama",
        ///         "Category":"Life",
        ///         "QuoteText":"The purpose of our lives is to be happy."
        ///     }
        /// </remarks>
        /// <param name="quote"></param>
        /// <response code="201">Quote successfully creaeted</response>
        /// <response code="400">Quote with the specified id already exists</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Create(Quote quote)
        {
            if (QuotesService.GetAll().Exists(q => q.Id == quote.Id) || quote.Id < 0)
                return BadRequest();

            QuotesService.Add(quote);
            return CreatedAtAction(nameof(Create), new { id = quote.Id }, quote);
        }

        /// <summary>Adds an email to the daily quote subscribers list</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST subscribe/email
        ///     {
        ///         "email":"someone@mail.com"
        ///     }
        /// </remarks>
        /// <param name="sub"></param>
        /// <response code="400">Request body is null</response>
        /// <response code="201">Email successfully subscribed</response>
        /// <response code="409">The specified email is already subscribed</response>
        [HttpPost("subscribe/email")]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(201)]
        public IActionResult SubscribeEmail(Subscriber sub)
        {
            if (sub is null)
                return BadRequest();

            if (SubscribersService.GetAllEmails().Exists(e => e == sub.Email))
                return Conflict();

            SubscribersService.AddEmail(sub.Email);
            return CreatedAtAction(nameof(SubscribeEmail), null, sub.Email);
        }

        /// <summary>Adds a phone number to the daily quote subscribers list</summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST subscribe/email
        ///     {
        ///         "phone":"+9998887777"
        ///     }
        /// </remarks>
        /// <param name="sub"></param>
        /// <response code="400">Request body is null</response>
        /// <response code="409">The specified phone number is already subscribed</response>
        /// <response code="201">Phone number successfully subscribed</response>
        [HttpPost("subscribe/phone")]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(201)]
        public IActionResult SubscribePhone(Subscriber sub)
        {
            if (sub is null)
                return BadRequest();

            if (SubscribersService.GetAllPhones().Exists(ph => ph == sub.Phone))
                return Conflict();

            SubscribersService.AddPhone(sub.Phone);
            return CreatedAtAction(nameof(SubscribePhone), null, sub.Phone);
        }

        /// <summary>Modifies the quote </summary>
        /// <param name="Id"></param>
        /// <param name="quote"></param>
        /// <response code="204">The quote was successully updated</response>
        /// <response code="400">The parameeter Id doesn't match with Id in the request body</response>
        /// <response code="404">A quote matching the provided id parameter doesn't exist in the in-memory.</response>
        [HttpPut("{Id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
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

        /// <summary>Deletes a quote</summary>
        /// <param name="Id"></param>
        /// <response code="404">A quote matching the provided id parameter doesn't exist in the in-memory.</response>
        /// <response code="204">The quote was successully deleted</response>
        [HttpDelete("{Id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
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