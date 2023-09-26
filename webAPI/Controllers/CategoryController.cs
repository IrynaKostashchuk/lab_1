using Microsoft.AspNetCore.Mvc;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;

        private IUnitOfWork _unitofWork;
        public CategoryController(ILogger<CategoryController> logger, IUnitOfWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        // GET: api/Category/GetAllCategories
        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategoriesAsync()
        {
            try
            {
                var results = await _unitofWork._categoryRepository.GetAllAsync();
                _unitofWork.Commit();
                _logger.LogInformation($"Returned all categories from database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetCategoryAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }      
        }
        
        // GET: api/Category/GetById/id
        [HttpGet("GetById/{id}", Name = "GetCategoryById")]
        public async Task<ActionResult<Category>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitofWork._categoryRepository.GetAsync(id);
                _unitofWork.Commit();
                if (result == null)
                {
                    _logger.LogError($"Category with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned category with id: {id}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        
        // GET: api/Category/TopFiveCategories
        [HttpGet("TopFiveCategories")]
        public async Task<ActionResult<IEnumerable<Category>>> GetTopFiveAsync()
        {
            try
            {
                var results = await _unitofWork._categoryRepository.TopFiveCategoryAsync();
                _unitofWork.Commit();
                _logger.LogInformation($"Returned top five categories from database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetTopFiveAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("PostCategory")]
        public async Task<ActionResult> PostCategoryAsync([FromBody] Category newCategory)
        {
            try
            {
                if (newCategory == null)
                {
                    _logger.LogError("Category object sent from client is null.");
                    return BadRequest("Category object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Category object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var created_id = await _unitofWork._categoryRepository.AddAsync(newCategory);
                var CreatedCategory = await _unitofWork._categoryRepository.GetAsync(created_id);
                _unitofWork.Commit();
                return CreatedAtRoute("GetCategoryById", new { id = created_id}, CreatedCategory);
                //return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostCategoryAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<ActionResult> PutAsync (int id, [FromBody] Category updateProduct)
        {
            try
            {
                if (updateProduct == null)
                {
                    _logger.LogError("Category object sent from client is null.");
                    return BadRequest("Category object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid category object sent from client.");
                    return BadRequest("Invalid category object");
                }
                var CategoryEntity = await _unitofWork._categoryRepository.GetAsync(id);
                if (CategoryEntity == null)
                {
                    _logger.LogError($"category with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._categoryRepository.ReplaceAsync(updateProduct);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PutAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteAsync (int id)
        {
            try
            {
                var CategoryEntity = await _unitofWork._categoryRepository.GetAsync(id);
                if (CategoryEntity == null)
                {
                    _logger.LogError($"Category with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._categoryRepository.DeleteAsync(id);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Catalog action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
