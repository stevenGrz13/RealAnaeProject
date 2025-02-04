﻿using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using AnaeLogiciel.Controllers;
using AnaeLogiciel.Data;
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

    public static void Action(int idprojet, ApplicationDbContext _context)
    {
        Console.WriteLine("MANDALO ATO AMLE FONCTION");
        Projet projet = _context.Projet
            .First(a => a.Id == idprojet);
        List<OccurenceActivite> listeactivite = new List<OccurenceActivite>();

        List<OccurenceResultat> listeresultat = _context.OccurenceResultat
            .Where(a => a.IdProjet == idprojet)
            .ToList();

        List<OccurenceSousActivite> listesousactivite = new List<OccurenceSousActivite>();

        foreach (var v in listeresultat)
        {
            listeactivite.AddRange(_context.OccurenceActivite
                .Where(a => a.IdOccurenceResultat == v.Id));
        }

        foreach (var v in listeactivite)
        {
            List<OccurenceSousActivite> liste = _context.OccurenceSousActivite
                .Where(a => a.IdOccurenceActivite == v.Id)
                .ToList();
            listesousactivite.AddRange(liste);
        }
        
    foreach (var v in listesousactivite)
        {
            List<OccurenceSousActiviteIndicateur> listeindicateur = _context.OccurenceSousActiviteIndicateur
                .Where(a => a.IdOccurenceSousActivite == v.Id)
                .ToList();
            double[] moyenneparindicateur = new double[listeindicateur.Count];
            int x = 0;
            foreach (var z in listeindicateur)
            {
                double somme = 0;
                List<RapportIndicateurSousActivite> listerapport = _context.RapportIndicateurSousActivite
                    .Where(a => a.IdIndicateurSousActivite == z.Id)
                    .ToList();
                foreach (var u in listerapport)
                {
                    somme += u.QuantiteEffectue;
                }

                if (somme == 0)
                {
                    moyenneparindicateur[x] = somme;
                }

                if (somme > 0)
                {
                    moyenneparindicateur[x] = (somme * 100) / z.Target;
                }

                x++;
            }

            double newmoyenne = 0;
            for (int i = 0; i < moyenneparindicateur.Length; i++)
            {
                newmoyenne += moyenneparindicateur[i];
            }

            if (Double.IsNaN(newmoyenne / moyenneparindicateur.Length))
            {
                v.Avancement = 0;
            }
            else
            {
                v.Avancement = newmoyenne / moyenneparindicateur.Length;
            }
        }

        _context.SaveChanges();
        
        foreach (var v in listeactivite)
        {
            double finalmoyenne = 0;
            List<VLienActiviteSousActivite> liste = _context.VLienActiviteSousActivite
                .Where(a => a.IdOccurenceActivite == v.Id)
                .ToList();
            if (liste.Count > 0)
            {
                foreach (var z in liste)
                {
                    finalmoyenne += z.Avancement;
                }
        
                if (Double.IsNaN(finalmoyenne / liste.Count()))
                {
                    v.Avancement = 0;
                }
                else
                {
                    v.Avancement = (finalmoyenne / liste.Count());
                }
            }
            else
            {
                v.Avancement = v.Avancement;
            }
        }

        _context.SaveChanges();

        //
        
        double moyenneparresultat = 0;
        foreach (var v in listeresultat)
        {
            OccurenceResultat or = _context.OccurenceResultat
                .First(a => a.Id == v.Id);
            List<OccurenceActivite> liste = _context.OccurenceActivite
                .Where(a => a.IdOccurenceResultat == v.Id)
                .ToList();
            Console.WriteLine("NOMBRE ACTIVITE ="+liste.Count());
            foreach (var z in liste)
            {
                Console.WriteLine("AVANCEMENT ACTIVITE ="+z.Avancement);
                moyenneparresultat += z.Avancement;
            }

            moyenneparresultat = moyenneparresultat / liste.Count;
            Console.WriteLine("MOYENNE RESULTAT = "+moyenneparresultat);
            if (Double.IsNaN(moyenneparresultat))
            {
                or.Avancement = 0;
            }
            else
            {
                or.Avancement = moyenneparresultat;
            }

            _context.SaveChanges();
        }
        
        double moyenneduprojet = 0;
        foreach (var v in listeresultat)
        {
            moyenneduprojet += v.Avancement;
        }

        moyenneduprojet = moyenneduprojet / listeresultat.Count;
        if (Double.IsNaN(moyenneduprojet))
        {
            projet.Avancement = 0;
        }
        else
        {
            projet.Avancement = moyenneduprojet;
        }

        _context.SaveChanges();
    }
}