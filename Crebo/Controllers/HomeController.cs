using Crebo.Base.Interfaces;
using Crebo.Business.Queries.Student;
using Crebo.Business.Views.Student;
using Crebo.Models;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Crebo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQueryHandler<GetStudentListQuery, IEnumerable<StudentListView>> _getStudentListQuery;

        public HomeController(IQueryHandler<GetStudentListQuery, IEnumerable<StudentListView>> getStudentListQuery)
        {
            _getStudentListQuery = getStudentListQuery;
        }

        public ActionResult Index()
        {
            return View(new ResultContentModel());
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase csvFile)
        {
            var model = new ResultContentModel();

            if (ModelState.IsValid)
            {
                if (csvFile == null)
                {
                    model.ErrorMessage = "U heeft nog geen bestand geselecteerd";
                }
                else
                {
                    if (Path.GetExtension(csvFile.FileName).ToLower() == ".csv")
                    {
                        model.FileName = csvFile.FileName;
                        model.Data = _getStudentListQuery.Handle(new GetStudentListQuery() { csv = csvFile.InputStream, length = csvFile.ContentLength });
                    }
                    else
                    {
                        model.ErrorMessage = "Alleen bestanden met CSV extensie kunnen worden verwerkt";
                    }
                }
            }

            return PartialView("Results", model);
        }
    }
}