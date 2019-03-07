using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

using PersonalFinances.Services;

namespace PersonalFinances.Controllers.Api
{
    public class MovementsController : ApiController
    {
        private MovementService _service = new MovementService();

        /// <summary>
        /// This endpoint is responsible to update the MovementStatus field of all pending movements
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> UpdateStatus ()
        {
            try
            {
                await _service.UpdateMovementsStatusOfPendingMovements();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                return InternalServerError(e);
            }
        }
    }
}
