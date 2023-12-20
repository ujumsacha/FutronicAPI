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
using Microsoft.EntityFrameworkCore;

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
            try
            {
                if (rseach.r_key != "CNI" && rseach.r_key != "UNIQUE")
                {
                    return StatusCode(400, new SortiDTO { Code = "Err001", ErrDescri = "veuillez renseigner le code adequat", data = null });
                }
                if (rseach.r_key == "CNI")
                {
                    var res = _context.TInfoPersonnes.Where(p => p.RNumCni == rseach.r_value).Cast<TInfoPersonne>();
                    return StatusCode(200, new SortiDTO { Code = "OK001", ErrDescri = (res==null?"Aucune empreinte correspondante": ""), data = res });
                }
                var res1 = _context.TInfoPersonnes.Where(p => p.RNumUnique == rseach.r_value).Cast<TInfoPersonne>();
                return StatusCode(200, new SortiDTO { Code = "OK001", ErrDescri = (res1 == null ? "Aucune empreinte correspondante" : ""), data = res1 });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new SortiDTO { Code = "Err002", ErrDescri = ex.Message, data = null });
            }

        }
        [HttpPost("upload")]
        public async Task<IActionResult> upload(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new SortiDTO { Code = "Err002", ErrDescri = "Aucun fichier chargé ", data = null });
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
                Image img = Image.FromFile(filePath);
                //**********************************************BD GET *************************************************
                 List<TEmpreinte> malisteimageDb = _context.TEmpreintes.ToList();
                //**********************************************BD GET *************************************************


                (bool, double, string) retourverification = BiometricVerification.Verify(img).Result;
                if (!retourverification.Item1)
                {
                    return Ok(new SortiDTO { Code = "Err005", ErrDescri = "Empreinte non trouvé", data = null });
                }

                //string transit = retourverification.ToString().Replace("/\\/", "\\");
                ////string req = $@"SELECT pers.* FROM sc_enrollement.t_empreinte as emp inner join sc_enrollement. t_info_personne as pers on emp.r_id_personne_fk=pers.r_id where emp.r_lien='{retourverification.Item3}";
                //var tre = _context.TInfoPersonnes.FromSql($"SELECT pers.* FROM sc_enrollement.t_empreinte as emp inner join sc_enrollement. t_info_personne as pers on emp.r_id_personne_fk=pers.r_id where emp.r_lien='{transit}'").FirstOrDefault();
                return Ok(new SortiDTO { Code = "OK001", ErrDescri = "Empreinte trouvé", data = retourverification.Item3 });
            }
            catch (Exception ex)
            {
                return BadRequest(new SortiDTO { Code = "Err014", data = null, ErrDescri = ex.Message});
            }
        }
        [HttpGet("repert")]
        public string GetCurrent ()
        {
           return Directory.GetCurrentDirectory();
        }
    }
}
