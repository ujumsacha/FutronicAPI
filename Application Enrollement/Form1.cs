using Application_Enrollement.Models;
using Futronic.Scanners.FS26X80;
using Npgsql.Replication.PgOutput.Messages;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;

namespace Application_Enrollement
{
    public partial class Form1 : Form
    {
        private byte[] Thumb;
        private byte[] IndexFinger;
        private byte[] MiddleFinger;
        private byte[] RingFinger;
        private byte[] LittleFinger;
        public Form1()
        {
            InitializeComponent();
        }
        private byte[] GetImageBytes(Image image)
        {
            MemoryStream ms = new();
            image.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
        private void btnCaptureThumb_Click(object sender, EventArgs e)
        {
            var accessor = new DeviceAccessor();

            using (var device = accessor.AccessFingerprintDevice())
            {
                device.SwitchLedState(true, false);
                //device.StartFingerDetection();
                Bitmap ber = device.ReadFingerprint();
                this.pictureThumb.Image = ber;
                device.SwitchLedState(false, false);
            }
            //pictureThumb.Image = picturePreview.Image;
            this.Thumb = GetImageBytes(this.pictureThumb.Image);
        }
        private void btnCaptureIndex_Click(object sender, EventArgs e)
        {
            var accessor = new DeviceAccessor();

            using (var device = accessor.AccessFingerprintDevice())
            {
                device.SwitchLedState(true, false);
                //device.StartFingerDetection();
                Bitmap ber = device.ReadFingerprint();
                this.pictureIndex.Image = ber;
                device.SwitchLedState(false, false);
            }
            this.IndexFinger = GetImageBytes(this.pictureMiddle.Image);
        }
        private void btnCaptureMiddle_Click(object sender, EventArgs e)
        {
            var accessor = new DeviceAccessor();

            using (var device = accessor.AccessFingerprintDevice())
            {
                device.SwitchLedState(true, false);
                //device.StartFingerDetection();
                Bitmap ber = device.ReadFingerprint();
                this.pictureMiddle.Image = ber;
                device.SwitchLedState(false, false);
            }
            //pictureMiddle.Image = picturePreview.Image;
            this.MiddleFinger = GetImageBytes(this.pictureMiddle.Image);
        }
        private void btnCaptureRing_Click(object sender, EventArgs e)
        {
            var accessor = new DeviceAccessor();

            using (var device = accessor.AccessFingerprintDevice())
            {
                device.SwitchLedState(true, false);
                //device.StartFingerDetection();
                Bitmap ber = device.ReadFingerprint();
                this.pictureRing.Image = ber;
                device.SwitchLedState(false, false);
            }
            //pictureRing.Image = picturePreview.Image;
            this.RingFinger = GetImageBytes(this.pictureRing.Image);
        }
        private void btnCaptureLittle_Click(object sender, EventArgs e)
        {
            var accessor = new DeviceAccessor();

            using (var device = accessor.AccessFingerprintDevice())
            {
                device.SwitchLedState(true, false);
                //device.StartFingerDetection();
                Bitmap ber = device.ReadFingerprint();
                this.pictureLittle.Image = ber;
                device.SwitchLedState(false, false);
            }
            //pictureLittle.Image = picturePreview.Image;
            this.LittleFinger = GetImageBytes(this.pictureLittle.Image);
        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            if (txt_nom.Text == string.Empty || txt_numcni.Text == string.Empty || txt_date_emission.Text == string.Empty || txt_nom.Text == string.Empty || txt_nom.Text == string.Empty)
                MessageBox.Show("Veuilez renseigner tous les champs obligatoire");

            int res=0;
            TInfoPersonne Tinf = new TInfoPersonne
            {
                RCreatedBy = "Systeme",
                RCreatedOn = DateTime.Now,
                RDateDExpiration = Convert.ToDateTime(this.txt_date_exp_cni),
                RDateEmission = Convert.ToDateTime(this.txt_date_emission),
                RDateNaissance = Convert.ToDateTime(this.txt_date_naissance),
                RDescriptionLock = string.Empty,
                RId = new Guid().ToString(),
                RIsLock = false,
                RLieuDeNaissance = txt_lieu_naissance.Text,
                RLieuEmission = lieu_emission.Text,
                RNationnalite = txt_nationnalite.Text,
                RNni = txt_NNI.Text,
                RNom = txt_nom.Text,
                RNumCni = txt_numcni.Text,
                RNumUnique = txt_num_unique.Text,
                RPrenom = txt_prenom.Text,
                RProfession = txt_profession.Text,
                RSexe = cmb_sexe.Text,


                RTaille = int.Parse(txt_taille.Text),
                RUpdatedBy = "Systeme",
                RUpdatedOn = DateTime.Now,
            };
            //*******************************Ajoute Pouce **************************************
                TEmpreinte Tr = new TEmpreinte
                {
                     RIdPersonneFk= Tinf.RId,
                     RType= "POUCE", 
                     RValeur= RingFinger,
                     RCreatedBy="Systeme",
                     RCreatedOn=DateTime.Now.ToString(),
                     RLien = ""

                };
            //*******************************Ajoute Pouce **************************************

            //*******************************Ajoute index **************************************
            TEmpreinte Tr1 = new TEmpreinte
            {
                RIdPersonneFk = Tinf.RId,
                RType = "INDEX",
                RValeur = IndexFinger,
                RCreatedBy = "Systeme",
                RCreatedOn = DateTime.Now.ToString(),
                RLien = ""

            };
            //*******************************Ajoute index **************************************

            //*******************************Ajoute majeur **************************************
            TEmpreinte Tr2 = new TEmpreinte
            {
                RIdPersonneFk = Tinf.RId,
                RType = "MAJEUR",
                RValeur = MiddleFinger,
                RCreatedBy = "Systeme",
                RCreatedOn = DateTime.Now.ToString(),
                RLien = ""

            };
            //*******************************Ajoute majeur **************************************

            //*******************************Ajoute Annulaire **************************************
            TEmpreinte Tr3 = new TEmpreinte
            {
                RIdPersonneFk = Tinf.RId,
                RType = "ANNULAIRE",
                RValeur = RingFinger,
                RCreatedBy = "Systeme",
                RCreatedOn = DateTime.Now.ToString(),
                RLien = "",
            };
            //*******************************Ajoute Annulaire **************************************

            //*******************************Ajoute Annulaire **************************************
            TEmpreinte Tr4 = new TEmpreinte
            {
                RIdPersonneFk = Tinf.RId,
                RType = "AURICULAIRE",
                RValeur = LittleFinger,
                RCreatedBy = "Systeme",
                RCreatedOn = DateTime.Now.ToString(),
                RLien = ""

            };

            //*******************************Ajoute Annulaire **************************************
            Tinf.TEmpreintes.Add(Tr);
            Tinf.TEmpreintes.Add(Tr1);
            Tinf.TEmpreintes.Add(Tr2);
            Tinf.TEmpreintes.Add(Tr3);
            Tinf.TEmpreintes.Add(Tr4);

           
        }
    }
}