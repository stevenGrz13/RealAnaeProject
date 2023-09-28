using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using AnaeLogiciel.Controllers;
using AnaeLogiciel.Models;
using SQLitePCL;

namespace AnaeLogiciel.Fonction;

public class Fonction
{
    public static DateTime Make(DateTime dateTime)
    {
        dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
        dateTime = dateTime.ToUniversalTime();
        return dateTime;
    }
    
    public static string GenerateToken(int length = 32)
    {
        if (length <= 0)
            throw new ArgumentException("La longueur du token doit être supérieure à zéro.", nameof(length));

        byte[] randomBytes = new byte[length / 2]; // Chaque byte est représenté par deux caractères hexadécimaux
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }

        return BitConverter.ToString(randomBytes).Replace("-", "").ToLower();
    }
    
    public static void EnvoyerEmail(SmtpConfig smtpConfig, string expediteur, string destinataire, string sujet, string corps)
    {
        using (SmtpClient smtpClient = new SmtpClient(smtpConfig.Server, smtpConfig.Port))
        {
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(smtpConfig.Username, smtpConfig.Password);
            smtpClient.EnableSsl = true;
            MailMessage message = new MailMessage
            {
                From = new MailAddress(expediteur),
                Subject = sujet,
                Body = corps,
                IsBodyHtml = true
            };
            message.To.Add(destinataire);
            smtpClient.Send(message);
        }
    }
    
    public static bool ValidateDates(DateOnly datedebutprevision, DateOnly datefinprevision, DateOnly datedebutrealisation, DateOnly datefinrealisation)
    {
        // Vérifier si la date de début de réalisation est inférieure ou égale à la date de fin de prévision
        if (datedebutrealisation > datefinprevision)
        {
            // Gérer le cas où la date de début de réalisation est postérieure à la date de fin de prévision
            return false;
        }

        // Vérifier si les dates de début et de fin sont valides
        if (datedebutprevision > datefinprevision || datedebutrealisation > datefinrealisation)
        {
            // Gérer le cas où une date de début est supérieure à la date de fin
            return false;
        }
    
        // Vérifier si les périodes se chevauchent partiellement
        bool chevauchementPartiel = (datedebutrealisation <= datefinprevision && datefinrealisation >= datedebutprevision);

        // Vérifier si la période de réalisation est contenue dans la période prévue
        bool contenuDansPeriodePrevue = (datedebutrealisation >= datedebutprevision && datefinrealisation <= datefinprevision);

        // La période de réalisation est valide si elle est contenue dans la période prévue
        // ou si elle se chevauche partiellement avec la période prévue
        return contenuDansPeriodePrevue || chevauchementPartiel;
    }

    
    public static bool IsNumeric(string input)
    {
        try
        {
            double value = double.Parse(input);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    
    public static string ImportPhoto(IFormFile imageFile, string path, string nomfichier, IWebHostEnvironment webHostEnvironment)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            string extension = Path.GetExtension(imageFile.FileName);
            string imagePath = Path.Combine(webHostEnvironment.WebRootPath, path, nomfichier + extension);

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                imageFile.CopyTo(stream);
            }

            return nomfichier + extension;
        }
        return "";
    }    
    public static string ImportFileTxt(IFormFile fileUpload, string path, string nomdufichier, IWebHostEnvironment webHostEnvironment)
    {
        if (fileUpload != null && fileUpload.Length > 0)
        {
            string filePath = Path.Combine(webHostEnvironment.WebRootPath, path, nomdufichier);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileUpload.CopyTo(fileStream);
            }

            return nomdufichier;
        }
        return "";
    }
    public static void CreerDossier(IWebHostEnvironment _hostingEnvironment,int occurenceId,string nomsourcedeverification)
    {
        Console.WriteLine("miditra izy fa tsy mcreer anle dossier");
        string wwwrootPath = _hostingEnvironment.WebRootPath;
        string dossierOccurencePath = Path.Combine(wwwrootPath, "occurence"+occurenceId, nomsourcedeverification);

        if (!Directory.Exists(dossierOccurencePath))
        {
            Console.WriteLine("tafiditra ary cree ilay dossier");
            Directory.CreateDirectory(dossierOccurencePath);
        }
    }

    public static DateOnly getDateNow()
    {
        DateTime currentDate = DateTime.Now;

        DateTime dateOnly = currentDate.Date;

        return DateOnly.FromDateTime(dateOnly);
    }

    public static bool SecureDate(DateOnly debut, DateOnly fin)
    {
        bool res = true;
        if (fin < debut)
        {
            res = false;
        }
        return res;
    }
    
    public static bool SecureDates(DateOnly debut1, DateOnly fin1, DateOnly debut2, DateOnly fin2)
    {
        bool res = true;
        
        if (debut2 >= debut1 && fin2 <= fin1)
        {
            res = true;
        }
        else
        {
            res = false;
        }
        
        res = SecureDate(debut1,fin2);
        
        return res;
    }
}