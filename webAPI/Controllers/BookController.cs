using Microsoft.AspNetCore.Mvc;

using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private IUnitOfWork _unitofWork;

        public BookController(ILogger<BookController> logger, IUnitOfWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        // GET: api/Book/GetAllBooks
        [HttpGet("GetAllBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBooksAsync()
        {
            try
            {
                var results = await _unitofWork._bookRepository.GetAllAsync();
                _unitofWork.Commit();
                _logger.LogInformation($"Returned all books from the database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetAllBooksAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Book/GetById/id
        [HttpGet("GetById/{id}", Name = "GetBookById")]
        public async Task<ActionResult<Book>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitofWork._bookRepository.GetAsync(id);
                _unitofWork.Commit();
                if (result == null)
                {
                    _logger.LogError($"Book with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned book with id: {id}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetByIdAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Book
        [HttpPost("PostBook")]
        public async Task<ActionResult> PostBookAsync([FromBody] Book newBook)
        {
            try
            {
                if (newBook == null)
                {
                    _logger.LogError("Book object sent from client is null.");
                    return BadRequest("Book object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Book object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var createdId = await _unitofWork._bookRepository.AddAsync(newBook);
                var createdBook = await _unitofWork._bookRepository.GetAsync(createdId);
                _unitofWork.Commit();
                return CreatedAtRoute("GetBookById", new { id = createdId }, createdBook);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostBookAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Book updateBook)
        {
            try
            {
                if (updateBook == null)
                {
                    _logger.LogError("Book object sent from client is null.");
                    return BadRequest("Book object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid book object sent from client.");
                    return BadRequest("Invalid book object");
                }
                var bookEntity = await _unitofWork._bookRepository.GetAsync(id);
                if (bookEntity == null)
                {
                    _logger.LogError($"Book with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._bookRepository.ReplaceAsync(updateBook);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PutAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                var bookEntity = await _unitofWork._bookRepository.GetAsync(id);
                if (bookEntity == null)
                {
                    _logger.LogError($"Book with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._bookRepository.DeleteAsync(id);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Book/TopThreeBooks
        [HttpGet("TopThreeBooks")]
        public async Task<ActionResult<IEnumerable<Book>>> TopThreeBooksAsync()
        {
            try
            {
                var results = await _unitofWork._bookRepository.TopThreeBookAsync();
                _unitofWork.Commit();
                _logger.LogInformation("Returned top three books from the database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside TopThreeBooksAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}