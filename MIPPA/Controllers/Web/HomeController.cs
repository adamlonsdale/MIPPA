using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mippa.Models;
using MIPPA.Models;
using MIPPA.ViewModels.Membership;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MIPPA.Controllers.Web
{
    public class HomeController : Controller
    {
        private readonly MippaContext _context;
        private readonly EmailSettings _emailSettings;
        private IRepository _repo;

        public HomeController(MippaContext context, IOptions<EmailSettings> emailSettings, IRepository repo)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
            _repo = repo;
        }

        public IActionResult Index()
        {
            var leagueInfo = _context.LeagueInformation.FirstOrDefault();

            if (leagueInfo == null)
            {
                leagueInfo = new Models.LeagueInformation();
            }

            return View(leagueInfo);
        }

        public IActionResult Leagues()
        {
            var managers = _context.Managers.Include(x => x.Sessions);

            return View(managers);
        }

        public IActionResult Tournaments()
        {
            return View();
        }

        public IActionResult Membership()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Qualifications()
        {
            var plays = _repo.GetPlaysForSeason();

            return View(plays);
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
