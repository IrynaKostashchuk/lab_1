using Microsoft.AspNetCore.Mvc;
using webAPI.dal.Entities;
using webAPI.dal.Repositories.Interfaces;

namespace webAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        private IUnitOfWork _unitofWork;
        public ProductController(ILogger<ProductController> logger, IUnitOfWork unitofWork)
        {
            _logger = logger;
            _unitofWork = unitofWork;
        }

        // GET: api/Product/GetAllProducts
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProductsAsync()
        {
            try
            {
                var results = await _unitofWork._productRepository.GetAllAsync();
                _unitofWork.Commit();
                _logger.LogInformation($"Returned all products from database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside GetAllProductsAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Product/GetById/id
        [HttpGet("GetById/{id}", Name = "GetProductById")]
        public async Task<ActionResult<Product>> GetByIdAsync(int id)
        {
            try
            {
                var result = await _unitofWork._productRepository.GetAsync(id);
                _unitofWork.Commit();
                if (result == null)
                {
                    _logger.LogError($"Product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInformation($"Returned product with id: {id}");
                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetByIdAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Product/ProductsByCategory
        [HttpGet("ProductByCategory")]
        public async Task<ActionResult<IEnumerable<Product>>> ProductByCategoryAsync(int categoryId)
        {
            try
            {
                var results = await _unitofWork._productRepository.ProductByCategoryAsync(categoryId);
                _unitofWork.Commit();
                _logger.LogInformation($"Returned products by categoryId from database.");
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Transaction Failed! Something went wrong inside ProductByCategoryAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Product
        [HttpPost("PostProduct")]
        public async Task<ActionResult> PostProductAsync([FromBody] Product newProduct)
        {
            try
            {
                if (newProduct == null)
                {
                    _logger.LogError("Product object sent from client is null.");
                    return BadRequest("Product object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Product object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var created_id = await _unitofWork._productRepository.AddAsync(newProduct);
                var CreatedProduct = await _unitofWork._productRepository.GetAsync(created_id);
                _unitofWork.Commit();
                return CreatedAtRoute("GetProductById", new { id = created_id }, CreatedProduct);
                // можна просто, но не самый лучший подход
                // return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside PostProductAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("Put/{id}")]
        public async Task<ActionResult> PutAsync(int id, [FromBody] Product updateProduct)
        {
            try
            {
                if (updateProduct == null)
                {
                    _logger.LogError("Product object sent from client is null.");
                    return BadRequest("Product object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid product object sent from client.");
                    return BadRequest("Invalid product object");
                }
                var ProductEntity = await _unitofWork._productRepository.GetAsync(id);
                if (ProductEntity == null)
                {
                    _logger.LogError($"Product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._productRepository.ReplaceAsync(updateProduct);
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
                var ProductEntity = await _unitofWork._productRepository.GetAsync(id);
                if (ProductEntity == null)
                {
                    _logger.LogError($"Product with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _unitofWork._productRepository.DeleteAsync(id);
                _unitofWork.Commit();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Product action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}

