using Microsoft.AspNetCore.Mvc;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoanController : ControllerBase
{
    private readonly ILogger<LoanController> _logger;
    private IUnitOfWork _unitofWork;

    public LoanController(ILogger<LoanController> logger, IUnitOfWork unitofWork)
    {
        _logger = logger;
        _unitofWork = unitofWork;
    }

    // GET: api/Loan/GetAllLoans
    [HttpGet("GetAllLoans")]
    public async Task<ActionResult<IEnumerable<Loan>>> GetAllLoansAsync()
    {
        try
        {
            var results = await _unitofWork._loanRepository.GetAllAsync(); // Assuming you have a LoanRepository
            _unitofWork.Commit();
            _logger.LogInformation("Returned all loans from the database.");
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Transaction Failed! Something went wrong inside GetAllLoansAsync() action: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    // GET: api/Loan/GetById/id
    [HttpGet("GetById/{id}", Name = "GetLoanById")]
    public async Task<ActionResult<Loan>> GetByIdAsync(int id)
    {
        try
        {
            var result = await _unitofWork._loanRepository.GetAsync(id); // Assuming you have a LoanRepository
            _unitofWork.Commit();
            if (result == null)
            {
                _logger.LogError($"Loan with id: {id}, hasn't been found in the database.");
                return NotFound();
            }
            else
            {
                _logger.LogInformation($"Returned loan with id: {id}");
                return Ok(result);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside GetByIdAsync() action: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    // POST: api/Loan
    [HttpPost("PostLoan")]
    public async Task<ActionResult> PostLoanAsync([FromBody] Loan newLoan)
    {
        try
        {
            if (newLoan == null)
            {
                _logger.LogError("Loan object sent from the client is null.");
                return BadRequest("Loan object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid Loan object sent from the client.");
                return BadRequest("Invalid model object");
            }
            var createdId = await _unitofWork._loanRepository.AddAsync(newLoan); // Assuming you have a LoanRepository
            var createdLoan = await _unitofWork._loanRepository.GetAsync(createdId); // Assuming you have a LoanRepository
            _unitofWork.Commit();
            return CreatedAtRoute("GetLoanById", new { id = createdId }, createdLoan);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside PostLoanAsync action: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    // PUT: api/Loan/Put/{id}
    [HttpPut("Put/{id}")]
    public async Task<ActionResult> PutAsync(int id, [FromBody] Loan updateLoan)
    {
        try
        {
            if (updateLoan == null)
            {
                _logger.LogError("Loan object sent from the client is null.");
                return BadRequest("Loan object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid loan object sent from the client.");
                return BadRequest("Invalid loan object");
            }
            var loanEntity = await _unitofWork._loanRepository.GetAsync(id); // Assuming you have a LoanRepository
            if (loanEntity == null)
            {
                _logger.LogError($"Loan with id: {id}, hasn't been found in the database.");
                return NotFound();
            }
            await _unitofWork._loanRepository.ReplaceAsync(updateLoan); // Assuming you have a LoanRepository
            _unitofWork.Commit();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside PutAsync() action: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }

    // DELETE: api/Loan/Delete/{id}
    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> DeleteAsync(int id)
    {
        try
        {
            var loanEntity = await _unitofWork._loanRepository.GetAsync(id); // Assuming you have a LoanRepository
            if (loanEntity == null)
            {
                _logger.LogError($"Loan with id: {id}, hasn't been found in the database.");
                return NotFound();
            }
            await _unitofWork._loanRepository.DeleteAsync(id); // Assuming you have a LoanRepository
            _unitofWork.Commit();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
    
    // GET: api/Loan/LoanByReaderName
    [HttpGet("LoanByReaderName")]
    public async Task<ActionResult<IEnumerable<Loan>>> LoanByReaderNameAsync(string readerName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(readerName))
            {
                _logger.LogError("Reader name parameter is null or empty.");
                return BadRequest("Reader name parameter is required.");
            }

            var results = await _unitofWork._loanRepository.LoanByReaderName(readerName); // Assuming you have a LoanRepository
            _unitofWork.Commit();
            _logger.LogInformation($"Returned loans for reader with name: {readerName}");
            return Ok(results);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong inside LoanByReaderNameAsync() action: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
        }
    }
}