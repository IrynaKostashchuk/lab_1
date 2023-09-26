using Microsoft.AspNetCore.Mvc;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private IUnitOfWork _unitofWork;

        public AuthorController(ILogger<AuthorController> logger, IUnitOfWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        // GET: api/Author/GetAllAuthors
        [HttpGet("GetAllAuthors")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAllAuthorsAsync()
        {
            try
            {
                var results = await _unitofWork._authorRepository.GetAllAsync();
                _unitofWork.Commit();
                _logger.LogInformation($"Returned all authors from the database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetAllAuthorsAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Author/GetById/id
        [HttpGet("GetById/{id}", Name = "GetAuthorById")]
        public async Task<ActionResult<Author>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitofWork._authorRepository.GetAsync(id);
                _unitofWork.Commit();
                if (result == null)
                {
                    _logger.LogError($"Author with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned author with id: {id}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetByIdAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Author
        [HttpPost("PostAuthor")]
        public async Task<ActionResult> PostAuthorAsync([FromBody] Author newAuthor)
        {
            try
            {
                if (newAuthor == null)
                {
                    _logger.LogError("Author object sent from client is null.");
                    return BadRequest("Author object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Author object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var createdId = await _unitofWork._authorRepository.AddAsync(newAuthor);
                var createdAuthor = await _unitofWork._authorRepository.GetAsync(createdId);
                _unitofWork.Commit();
                return CreatedAtRoute("GetAuthorById", new { id = createdId }, createdAuthor);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostAuthorAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Author updateAuthor)
        {
            try
            {
                if (updateAuthor == null)
                {
                    _logger.LogError("Author object sent from client is null.");
                    return BadRequest("Author object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid author object sent from client.");
                    return BadRequest("Invalid author object");
                }
                var authorEntity = await _unitofWork._authorRepository.GetAsync(id);
                if (authorEntity == null)
                {
                    _logger.LogError($"Author with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._authorRepository.ReplaceAsync(updateAuthor);
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
                var authorEntity = await _unitofWork._authorRepository.GetAsync(id);
                if (authorEntity == null)
                {
                    _logger.LogError($"Author with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._authorRepository.DeleteAsync(id);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Author/TopFiveAuthors
        [HttpGet("TopFiveAuthors")]
        public async Task<ActionResult<IEnumerable<Author>>> TopFiveAuthorsAsync()
        {
            try
            {
                var results = await _unitofWork._authorRepository.TopFiveAuthorAsync();
                _unitofWork.Commit();
                _logger.LogInformation("Returned top five authors from the database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside TopFiveAuthorsAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}