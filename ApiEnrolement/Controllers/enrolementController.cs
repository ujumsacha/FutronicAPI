using ApiEnrolement.DTO;
using ApiEnrolement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using System.IO.Compression;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using Image = System.Drawing.Image;

namespace ApiEnrolement.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class enrolementController : ControllerBase
    {
        private readonly BdEnrollementContext _context;
        public enrolementController(BdEnrollementContext bd)
        {
            _context = bd;
        }
        [HttpPost("Getbytype")]
        public ObjectResult Getbytype([FromBody] RechercheDto rseach)
        {
            if(rseach.r_key !="CNI" && rseach.r_key!="UNIQUE")
            {
                return StatusCode(400, new SortiDTO {  Code = "Err001", ErrDescri = "veuillez renseigner le code adequat" , data=null});
            }
            if (rseach.r_key=="CNI")
            {
                var res= _context.TInfoPersonnes.Where(p => p.RNumCni == rseach.r_value).Cast<TInfoPersonne>();
                return StatusCode(200, new SortiDTO { Code = "OK001", ErrDescri = "", data = res });
            }
            var res1 = _context.TInfoPersonnes.Where(p => p.RNumUnique == rseach.r_value).Cast<TInfoPersonne>();
            return StatusCode(200, new SortiDTO { Code = "OK001", ErrDescri = "", data = res1 });
        }
        [HttpPost("upload")]
        public async Task<IActionResult> upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new SortiDTO { Code="Err002", ErrDescri="Aucun fichier chargé ", data=null});
            }
            string TempsFolder = Path.Combine(Directory.GetCurrentDirectory(), "uploads");
            
            if (!Directory.Exists(TempsFolder))
            {
                Directory.CreateDirectory(TempsFolder);
            }

            string filePath = Path.Combine(TempsFolder, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Image img=Image.FromFile(filePath);

            (bool,double) retourverification =  BiometricVerification.Verify(img).Result;
            if(!retourverification.Item1)
            {
                return Ok(new SortiDTO { Code = "Err005", ErrDescri = "Empreinte non trouvé", data = null });
            }
            return Ok(new SortiDTO { Code = "OK001", ErrDescri = "Empreinte trouvé", data = null });
        }
        [HttpGet("repert")]
        public string GetCurrent ()
        {
           return Directory.GetCurrentDirectory();
        }
    }
}
