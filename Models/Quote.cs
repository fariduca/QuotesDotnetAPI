using System;
using System.ComponentModel.DataAnnotations;

namespace QuotesDotnetAPI.Models 
{
    public class Quote
    {
        
        public int Id {get; set;}
        
        [Required]
        public string Author {get; set;}

        [Required]
        public string QuoteText {get; set;}

        [Required]
        public string Category {get; set;}

        public DateTime CreatedAt {get; set;}
    }
}