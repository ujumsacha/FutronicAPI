//using Futronic.Models;
using ApiEnrolement.DTO;
using ApiEnrolement.Models;
using Futronic.SDKHelper;
using Microsoft.Extensions.FileSystemGlobbing;
//using SixLabors.ImageSharp;
using SourceAFIS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace ApiEnrolement
{
   public static class BiometricVerification
   {
        private static readonly IConfiguration _configuration;
        private readonly static BdEnrollementContext _Context;
        private static List<RetResultat> malisteimage = new List<RetResultat>();
        private static List<TEmpreinte> malisteimageDb = new List<TEmpreinte>();

        public static Task<(bool, double,string)> Verify(Image fingerprint1)
        {
            ParcourirDossierEtRecupererImages(_configuration.GetValue<string>("pathOfimage"));
            var verificationOptions = new FingerprintImageOptions
            {
                Dpi = 500
            };
            ////////////**************************************************IMAGE 1 *******************************************************
            MemoryStream stream = new MemoryStream();
            fingerprint1.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp); 
            byte[] imageBytesreceive = stream.ToArray();
            ////////////**************************************************IMAGE 1 *******************************************************
            foreach(var image in malisteimage)
            {
                var probe = new FingerprintTemplate(new FingerprintImage(imageBytesreceive));
                var candidate = new FingerprintTemplate(new FingerprintImage(image.imageByte));
                double score = new FingerprintMatcher(probe).Match(candidate);
                double threshold = 40;
                bool match = score >= threshold;
                if(match)
                    return Task.FromResult((match, score, image.imagelien));
            }
            return Task.FromResult((false, 0.0,""));
            
        }


        public static Task<(bool, double,TInfoPersonne)> VerifyIntoDb(Image fingerprint1,List<TEmpreinte> _infoPersonnes)
        {
            //ParcourirDossierEtRecupererImages(@"E:\Futronic\FutronicFingerPrint\bin\Debug\net5.0-windows\empreintePicture");
            //GetALLdatabaseempreinte();
            var verificationOptions = new FingerprintImageOptions
            {
                Dpi = 500
            };
            ////////////**************************************************IMAGE 1 *******************************************************
            MemoryStream stream = new MemoryStream();
            fingerprint1.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] imageBytesreceive = stream.ToArray();
            ////////////**************************************************IMAGE 1 *******************************************************
            foreach (var image in _infoPersonnes)
            {
                var probe = new FingerprintTemplate(new FingerprintImage(imageBytesreceive));
                byte[] intermediaire = image.RValeur;
                var candidate = new FingerprintTemplate(new FingerprintImage(intermediaire));
                double score = new FingerprintMatcher(probe).Match(candidate);
                double threshold = 40;
                bool match = score >= threshold;
                if (match)
                    return Task.FromResult((match, score,_Context.TInfoPersonnes.Where(x=>x.RId==image.RIdPersonneFk).FirstOrDefault()));
            }
            return Task.FromResult((false, 0.0, new TInfoPersonne()));

        }
        static void ParcourirDossierEtRecupererImages(string dossier)
        {
            try
            {
                // Obtenez la liste des fichiers dans le dossier actuel
                string[] fichiers = Directory.GetFiles(dossier);

                // Parcourez les fichiers pour trouver les images
                foreach (string fichier in fichiers)
                {
                    // Vérifiez si le fichier est une image (vous pouvez ajouter d'autres extensions si nécessaire)
                    if (EstImage(fichier))
                    {
                        RetResultat rt = new RetResultat { imageByte = ConvertirImageEnByteArray(fichier), imagelien = fichier };
                        malisteimage.Add(rt);
                    }
                }
                // Obtenez la liste des sous-dossiers dans le dossier actuel
                string[] sousDossiers = Directory.GetDirectories(dossier);

                // Parcourez les sous-dossiers de manière récursive
                foreach (string sousDossier in sousDossiers)
                {
                    ParcourirDossierEtRecupererImages(sousDossier);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
            }
        }

        static void GetALLdatabaseempreinte()
        {
            malisteimageDb= _Context.TEmpreintes.ToList();
        }
        static List<TEmpreinte> _empreintes = new List<TEmpreinte>();

        static bool EstImage(string cheminFichier)
        {
            string extension = Path.GetExtension(cheminFichier).ToLower();
            return extension == ".bmp";
        }
        static byte[] ConvertirImageEnByteArray(string cheminImage)
        {
            try
            {
                // Chargez l'image à partir du fichier
                using (Image image = Image.FromFile(cheminImage))
                {
                    // Créez un flux mémoire pour stocker les octets de l'image
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Enregistrez l'image dans le flux mémoire au format souhaité (par exemple, JPEG)
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                        // Obtenez le tableau d'octets à partir du flux mémoire
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
                return null; // En cas d'erreur, retournez null ou gérez l'erreur selon vos besoins
            }
        }
    }
}
