using LioProject.Domain.Entities.Helpers;
using LioProject.Domain.Users;
using LioProject.MVC.Helpers;
using LioProject.Persistence.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LioProject.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenericController<TEntity> : ControllerBase where TEntity : class, ISoftDeletable
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly SysHelper _sysHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _currentUser;
        private readonly ILogger<SysHelper> _logger;

        public GenericController(IGenericRepository<TEntity> repository, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> currentUser, ILogger<SysHelper> logger)
        {
            _repository = repository;

            _httpContextAccessor = httpContextAccessor;
            _currentUser = currentUser;
            _logger = logger;
            _sysHelper = new SysHelper(currentUser, httpContextAccessor, _logger);
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repository.GetWithDetails(id);
            if (entity == null || !entity.EstActif)
                return NotFound();

            return Ok(entity);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _repository.GetAllActif();
            return Ok(entities);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TEntity entity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var currentuser = await _sysHelper.GetCurrentUser();
            entity = await _repository.Add(entity, currentuser);

            return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TEntity entity)
        {
            var currentuser = await _sysHelper.GetCurrentUser();
            if (id != entity.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEntity = await _repository.Get(id);
            if (existingEntity == null || !existingEntity.EstActif)
                return NotFound();
           
            await _repository.Update(entity, currentuser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repository.Get(id);
            if (entity == null || !entity.EstActif)
                return NotFound();
            var currentuser = await _sysHelper.GetCurrentUser();
            await _repository.SoftDelete(entity, currentuser);

            return NoContent();
        }
        //public async Task<IActionResult> Search([FromQuery] string searchString)
        //{
        //    // Define the search expression based on the conditions specified by your junior developers.
        //    Expression<Func<TEntity, bool>> searchExpression = entity =>
        //    {
        //        // Replace the following with the conditions specified by your junior developers.
        //        // For example, they might use different properties and conditions.
        //        // Example: return entity.Name.Contains(searchString) || entity.Description.Contains(searchString);
        //        return true; // Replace this with the actual search expression.
        //    };

        //    var entities = await _repository.FindByCondition(searchExpression);
        //    return Ok(entities);
        //}

    }
}
