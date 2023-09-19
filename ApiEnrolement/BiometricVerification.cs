using SixLabors.ImageSharp;
using SourceAFIS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing.Imaging;
using Image = System.Drawing.Image;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing;

namespace ApiEnrolement
{
    public static class BiometricVerification
    {
        public static Task<(bool, double)> Verify(Image fingerprint1)
        {
            int nbrTentative = 0;
            try
            {
                LabelDepart:
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "FolderImage"); // Remplacez par le chemin de votre répertoire
                var verificationOptions = new FingerprintImageOptions
                {
                    Dpi = 500
                };

                // Vérifier si le répertoire existe
                if (Directory.Exists(directoryPath))
                {
                    // Récupérer tous les fichiers .bmp du répertoire
                    string[] bmpFiles = Directory.GetFiles(directoryPath, "*.bmp");

                    // Parcourir les fichiers .bmp
                    foreach (string bmpFile in bmpFiles)
                    {
                        Image fingerprint2 = Image.FromFile(bmpFile);
                        //var probe = new FingerprintTemplate(new FingerprintImage(fingerprint1.Width, fingerprint1.Height,
                        //    new byte[8]));
                        var probe = new FingerprintTemplate(File.ReadAllBytes(@"C:\\Users\\sacha.ogou\\source\\repos\\FutronicFingerPrintOKFUTRONIC\\ApiEnrolement\\uploads\\index.bmp"));

                        var candidate = new FingerprintTemplate(new FingerprintImage(fingerprint2.Width, fingerprint2.Height,
                            new byte[8]));

                        double score = new FingerprintMatcher(probe).Match(candidate);

                        double threshold = 40;
                        bool match = score >= threshold;
                        if(match)
                            return Task.FromResult((match, score));

                    }
                    return Task.FromResult((false, 0.0));

                }
                else
                {
                   DirectoryInfo ret=  Directory.CreateDirectory(directoryPath);
                    if(ret.Exists)
                    {
                        goto LabelDepart;
                    }
                    return Task.FromResult((false, 0.0));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult((false, 0.0));
            }
        }
    }
}
