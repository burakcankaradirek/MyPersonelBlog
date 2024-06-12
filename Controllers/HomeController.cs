using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyPersonelBlog.Models;
using MyPersonelBlog.ViewModels;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Extensions.Configuration;

namespace MyPersonelBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View();
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

                var service = new ExchangeService(ExchangeVersion.Exchange2010_SP2)
                {
                    Credentials = new WebCredentials(emailSettings.SenderEmail, emailSettings.SenderPassword)
                };
                service.Url = new Uri(emailSettings.EwsUrl);

                EmailMessage email = new EmailMessage(service)
                {
                    Subject = model.Subject,
                    Body = new MessageBody($"Name: {model.Name}<br/>Email: {model.Email}<br/>Message: {model.Message}"),
                    ToRecipients = { "burakcankaradirek@hotmail.com" }
                };

                email.Send();

                TempData["Message"] = "Mesajınız başarıyla gönderildi!";
                return RedirectToAction("Contact");
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}