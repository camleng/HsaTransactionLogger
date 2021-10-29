using Business.Extractors;
using Business.Parsers;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Controller]
[Route("api/HsaTransactions")]
public class HsaTransactionsController : ControllerBase
{
   private readonly IImageReader _imageReader;

   public HsaTransactionsController(IImageReader imageReader)
   {
      _imageReader = imageReader;
   }

   [HttpGet("FromImage")]
   public async Task<ActionResult> GetFromImage(IFormFile file)
   {
      var json = await _imageReader.ReadTextFromImageToJson(file);

      var transactions = TransactionExtractor.BuildTransactionsFromJson(json);
      
      return Ok(transactions);
   }
}