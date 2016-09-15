using Email5.Models;
using ExcelParser;
using Mailer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Email5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendEmail(EmailViewModel vm)
        {

            HttpPostedFileBase To = null;
            HttpPostedFileBase Attchmnts = null;
            ICollection<string> AttchmntFilePaths = new List<string>();

            if (Request.Files.Count > 0)
            {
                To = Request.Files["ToAddr"];
                Attchmnts = Request.Files["Attachment"];
            }

            if (string.IsNullOrEmpty(vm.ToAddress) && To.ContentLength <= 0)
                ModelState.AddModelError("ToAddress", "To Address field is required.");

            if (ModelState.IsValid)
            {
                EmailContent ec = new EmailContent();

                if (Attchmnts.ContentLength > 0)
                {
                    string filePath = Request.MapPath(ConfigurationManager.AppSettings["ExcelFilePath"] + DateTime.Now.ToString("ddMMMyyyyhhmmss") + Attchmnts.FileName);
                    Attchmnts.SaveAs(filePath);
                    AttchmntFilePaths.Add(filePath);
                }

                if (To != null && To.ContentLength > 0)
                {
                    string fileName = Request.MapPath(ConfigurationManager.AppSettings["ExcelFilePath"] + DateTime.Now.ToString("ddMMMyyyyhhmmss") + To.FileName);
                    To.SaveAs(fileName);

                    IEnumerable<string> ToAddresses = ParseExcel.GetUserNames(fileName);

                    foreach (var item in ToAddresses)
                    {
                        ec.To = item;
                        ec.Subject = vm.Subject;
                        ec.Body = vm.Body;
                        ec.Attachments = AttchmntFilePaths;
                        try
                        {
                            await MailNotifier.SendEmail(ec);
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                }

                else
                {
                    ec.To = vm.ToAddress;
                    ec.Subject = vm.Subject;
                    ec.Body = vm.Body;
                    ec.Attachments = AttchmntFilePaths;
                    try
                    {
                       await  MailNotifier.SendEmail(ec);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

               return RedirectToAction("SendEmail");
            }
            return View(vm);
        }
    }
}