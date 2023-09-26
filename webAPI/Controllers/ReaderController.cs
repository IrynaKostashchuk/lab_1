using Microsoft.AspNetCore.Mvc;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly ILogger<ReaderController> _logger;
        private IUnitOfWork _unitofWork;

        public ReaderController(ILogger<ReaderController> logger, IUnitOfWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        // GET: api/Reader/GetAllReaders
        [HttpGet("GetAllReaders")]
        public async Task<ActionResult<IEnumerable<Reader>>> GetAllReadersAsync()
        {
            try
            {
                var results = await _unitofWork._readerRepository.GetAllAsync();
                _unitofWork.Commit();
                _logger.LogInformation($"Returned all readers from the database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetAllReadersAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Reader/GetById/id
        [HttpGet("GetById/{id}", Name = "GetReaderById")]
        public async Task<ActionResult<Reader>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitofWork._readerRepository.GetAsync(id);
                _unitofWork.Commit();
                if (result == null)
                {
                    _logger.LogError($"Reader with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned reader with id: {id}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetByIdAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Reader
        [HttpPost("PostReader")]
        public async Task<ActionResult> PostReaderAsync([FromBody] Reader newReader)
        {
            try
            {
                if (newReader == null)
                {
                    _logger.LogError("Reader object sent from client is null.");
                    return BadRequest("Reader object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Reader object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var createdId = await _unitofWork._readerRepository.AddAsync(newReader);
                var createdReader = await _unitofWork._readerRepository.GetAsync(createdId);
                _unitofWork.Commit();
                return CreatedAtRoute("GetReaderById", new { id = createdId }, createdReader);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostReaderAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Reader updateReader)
        {
            try
            {
                if (updateReader == null)
                {
                    _logger.LogError("Reader object sent from client is null.");
                    return BadRequest("Reader object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid reader object sent from client.");
                    return BadRequest("Invalid reader object");
                }
                var readerEntity = await _unitofWork._readerRepository.GetAsync(id);
                if (readerEntity == null)
                {
                    _logger.LogError($"Reader with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._readerRepository.ReplaceAsync(updateReader);
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
                var readerEntity = await _unitofWork._readerRepository.GetAsync(id);
                if (readerEntity == null)
                {
                    _logger.LogError($"Reader with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._readerRepository.DeleteAsync(id);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Reader/GetByLastName/lastName
        [HttpGet("GetByLastName/{lastName}")]
        public async Task<ActionResult<IEnumerable<Reader>>> GetByLastNameAsync(string lastName)
        {
            try
            {
                var results = await _unitofWork._readerRepository.GetByLastNameAsync(lastName);
                _unitofWork.Commit();
                _logger.LogInformation($"Returned readers by last name '{lastName}' from the database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetByLastNameAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}