using CurencyHire.Core.Interface;
using CurencyHire.Model.Entites;
using Microsoft.AspNetCore.Mvc;
namespace CurencyHire.Controllers
{
    [ApiController]
    public class CurencyController : ControllerBase
    {
        private readonly ICurrencyConverter _currency;
        public CurencyController(ICurrencyConverter currency)
        {
            _currency = currency;
        }
        [HttpDelete]
        [Route("api/[controller]/clear-configuration")]
        public async Task<IActionResult> clearconfiguration()
        {
            try
            {
                await _currency.ClearConfiguration();
                return StatusCode(StatusCodes.Status200OK, "Clear Configuration Done");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Exception raised: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("api/[controller]/convert")]
        public async Task<IActionResult> Post([FromBody] CurVM curChan)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Model State Is Not Valid");
            }
            try
            {
                double resultAmaount = await _currency.Convert(curChan.fromCurrency, curChan.toCurrency, curChan.Amount);
                return StatusCode(StatusCodes.Status200OK, resultAmaount.ToString());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Exception raised: " + ex.Message);
            }
        }
        [HttpPut]
        [Route("api/[controller]/update-configuration")]
        public async Task<ActionResult> UpdateConfiguration(List<DbTuple> lstTuples)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(StatusCodes.Status400BadRequest, "Model State Is Not Valid");
            }
            try
            {
                if (lstTuples != null && lstTuples.Count > 0)
                { 
                    var lstExchangeList = lstTuples.Select(arr => Tuple.Create(arr.from, arr.to, arr.rate));
                    await _currency.UpdateConfiguration(lstExchangeList);
                    return StatusCode(StatusCodes.Status200OK, "Update Done");
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, "No Data in parameters");
                }
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Exception raised: "+ex.Message);
            }
        }
    }
}
