using CoreSampleWebAPI.Data;
using CoreSampleWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreSampleWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public readonly TestAPISampleContext _context;

        public BooksController(TestAPISampleContext context)
        {
            _context = context;
        }
        // GET: api/<BooksController>
        [HttpGet]
        public ActionResult<IEnumerable<Book>> GetBooks()
        {
            var booklist = _context.Books.ToList();
           
            return booklist;
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public ActionResult<Book> GetBook(int id)
        {
            var book = _context.Books.Find(id);
            if (book == null)
            {
                return NotFound();
            }
            return book;
        }

        // POST api/<BooksController>
        [HttpPost]
        public ActionResult<Book> PostBook()
        {
            string apiUrl = "https://fakerapi.it/api/v1/books?_quantity=1";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync(apiUrl).Result;
            //if (response.IsSuccessStatusCode)
            //{
                var o = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
                var books = o.Value<JArray>("data")
                    .ToObject<List<Book>>();
                foreach (var book in books)
                {
                    book.Id = 0;
                    book.published = book.published + DateTime.Now.ToString();
                    _context.Books.Add(book);
                    _context.SaveChanges();
                    
                }
            //}

            return CreatedAtAction(nameof(GetBook), new { id = books[0].Id }, books[0]);

        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
