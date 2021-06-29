using QuotesDotnetAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace QuotesDotnetAPI.Services
{
    public static class QuotesService
    {
        static List<Quote> Quotes { get; }
        static int nextId = 1;

        static QuotesService()
        {
            Quotes = new List<Quote>
            {
                new Quote { Id = 0, Author = "Socrates", Category = "Philosophy", QuoteText = "I know that I know nothing"}
            };
        }

        public static List<Quote> GetAll() => Quotes;

        public static Quote Get(int id) => Quotes.FirstOrDefault(p => p.Id == id);

        public static void Add(Quote quote)
        {
            quote.Id = nextId++;
            Quotes.Add(quote);
        }

        public static void Update(Quote quote)
        {
            int index = Quotes.FindIndex(p => p.Id == quote.Id);
            if (index == -1)
                return;
            
            Quotes[index] = quote;
        }

        public static void Delete(int Id)
        {
            var quote = Get(Id);
            if (quote is null)
                return;

            Quotes.Remove(quote);
        }
    }
}