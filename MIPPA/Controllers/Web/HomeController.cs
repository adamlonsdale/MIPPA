using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mippa.Models;
using Microsoft.EntityFrameworkCore;
using MIPPA.ViewModels.Membership;
using System.Net.Http;
using MIPPA.Models;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Net;

namespace MIPPA.Controllers.Web
{
    public class HomeController : Controller
    {
        private readonly MippaContext _context;
        private readonly EmailSettings _emailSettings;

        public HomeController(MippaContext context, IOptions<EmailSettings> emailSettings)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Leagues()
        {
            var managers = _context.Managers.Include(x => x.Sessions);

            return View(managers);
        }

        public IActionResult Membership()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task<IActionResult> SubmitPlayerMemberInfo([FromForm] PlayerMemberInfo playerMemberInfo)
        {
            StringBuilder emailBody = new StringBuilder();

            emailBody.AppendLine("The following information was entered into Player Member Registration: ");
            emailBody.AppendLine("");
            emailBody.AppendLine("Name: " + playerMemberInfo.Name);
            emailBody.AppendLine("Email: " + playerMemberInfo.Email);
            emailBody.AppendLine("Address: " + playerMemberInfo.Address);
            emailBody.AppendLine("City: " + playerMemberInfo.City);
            emailBody.AppendLine("Prior League Information: ");
            emailBody.AppendLine("");
            emailBody.AppendLine(string.IsNullOrWhiteSpace(playerMemberInfo.Message) ? "No Information Provided" : playerMemberInfo.Message);

            await SendEmailAsync(
                "qhustler9@yahoo.com",
                string.Format("New Player Member Request for {0}", playerMemberInfo.Name),
                emailBody.ToString());

            return RedirectToAction("Index", new { });
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(_emailSettings.BaseUri) })
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                    Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(_emailSettings.ApiKey)));

                var content = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string, string>("from", _emailSettings.From),
                        new KeyValuePair<string, string>("to", email),
                        new KeyValuePair<string, string>("subject", subject),
                        new KeyValuePair<string, string>("text", message)
                    });

                var response = await client.PostAsync(_emailSettings.RequestUri, content).ConfigureAwait(false);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Debug.WriteLine("Success");
                    return true;
                }
                else
                {
                    Debug.WriteLine("StatusCode: " + response.StatusCode);
                    Debug.WriteLine("ReasonPhrase: " + response.ReasonPhrase);
                    return false;
                }


            }
        }
    }
}
