using QuotesDotnetAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System;


namespace QuotesDotnetAPI.Services
{
    public static class QuotesService
    {
        static List<Quote> Quotes { get; }
        static int nextId = 3;

        static QuotesService()
        {
            Quotes = new List<Quote>
            {
                new Quote 
                { 
                    Id = 0,
                    Author = "Socrates",
                    Category = "Philosophy", 
                    QuoteText = "I know that I know nothing",
                    CreatedAt = DateTime.Now
                },
                new Quote
                {
                    Id = 1,
                    Author = "Friedrich Nietzsche",
                    Category = "Life", 
                    QuoteText = "That which does not kill us makes us stronger.",
                    CreatedAt = DateTime.Now
                },
                new Quote
                {
                    Id = 2,
                    Author = "W. H. Auden",
                    Category = "Life", 
                    QuoteText = "We are all here on earth to help others; what on earth the others are here for I don't know.",
                    CreatedAt = DateTime.Now
                },
            };
        }

        public static List<Quote> GetAll() => Quotes;

        public static Quote Get(int id) => Quotes.FirstOrDefault(p => p.Id == id);

        public static List<Quote> GetByCategory(string category)
        {
            return Quotes.FindAll(q => q.Category == category);
        }

        public static List<Quote> GetOld()
        {
            return Quotes.FindAll(q => (DateTime.Now - q.CreatedAt).Hours > 24);
        }

        public static Quote GetRandom()
        {
            return Quotes[new Random().Next(Quotes.Count)];
        }

        public static void Add(Quote quote)
        {
            if (quote.Id == 0) quote.Id = nextId++;
            quote.CreatedAt = DateTime.Now;
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