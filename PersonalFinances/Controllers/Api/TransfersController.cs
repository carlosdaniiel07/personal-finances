using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity.Infrastructure;

using PersonalFinances.Services;

namespace PersonalFinances.Controllers.Api
{
    public class TransfersController : ApiController
    {
        TransferService _service = new TransferService();

        /// <summary>
        /// Update the 'TransferStatus' field and launch all pending transfers
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> UpdateStatus ()
        {
            try
            {
                await _service.LaunchPendingTransfers();
                return Ok();
            }
            catch (DbUpdateException e)
            {
                return InternalServerError(e);
            }
        }
    }
}
