using System.Threading.Tasks;
using Business.Extractors;
using Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
   [ApiController]
   [Route("api/HsaTransactionReconciler")]
   public class HsaTransactionReconcilerController : ControllerBase
   {
      private readonly IImageReader _imageReader;

      public HsaTransactionReconcilerController(IImageReader imageReader)
      {
         _imageReader = imageReader;
      }

      [Route("FromImage")]
      [HttpPost]
      public async Task<ActionResult> ReconcileFromImage(IFormFile file)
      {
         var json = await _imageReader.ReadTextFromImageToJsonAsync(file);

         var transactions = TransactionExtractor.BuildTransactionsFromJson(json);

         return Ok(transactions);
      }
   }
}