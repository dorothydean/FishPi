using FishPi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FishPi.Controllers
{
	[Authorize]
	public class PiCalculatorController : Controller
	{
		static WebsiteDB db = new WebsiteDB();

		// GET: Calculator
		public ActionResult FishPiCalculation()
		{
			return View();
		}

		//public void CalculateFishPi() {
		//	//TODO- Levenshtein's distanceMeet with Bruce
		//}

		[HttpPost]
		public ActionResult FishPiCalculation (FishPiCalculation p, HttpPostedFileBase textFile)
		{
			string extension = @"C:\Users\Sam\Documents\Test.txt"; //ask Joe if this is right- how do I get the file I uploaded?
			string e = Path.GetExtension(extension);

			//checks to make sure a file is being uploaded 
			if (textFile == null)
			{
				ModelState.AddModelError("FileURL", "Please upload a .txt file");
			}

			//validates file type
			if (e != ".txt")
			{
				ModelState.AddModelError("FileURL", "Please upload a .txt file");
			}

			if (ModelState.IsValid)
			{
				if (textFile != null &&
					textFile.ContentLength > 0)
				{
					string fileName =
						Guid.NewGuid().ToString() +
						Path.GetExtension(textFile.FileName);

					//generate a path with the file name appended
					string path = Path.Combine(
							Server.MapPath("~/Files"),
							fileName);

					textFile.SaveAs(path);
					p.FilePath = fileName;
				}

				DisplayCalculation.AddCalculation(p);
				return RedirectToAction("FishPiCalculation", "PiCalculator"); 
			}
			return View(p);
		}

		//move to profile page
		[HttpGet]
		public ActionResult CalcsList(int? id)
		{
			int currPage = (id.HasValue) ? id.Value : 1;
			const int CalcsPerPage = 10;

			List<FishPiCalculation> calcs =
				db.FishPiCalcs.OrderBy(p => p.Date)
				  .Skip((currPage - 1) * CalcsPerPage)
				  .Take(CalcsPerPage)
				  .ToList();


			ViewBag.MaxPage = Math.Ceiling(
				db.FishPiCalcs.Count() / (double)CalcsPerPage);
			ViewBag.CurrentPage = currPage;

			return View(calcs);
		}

		[HttpGet]
		public ActionResult Create()
		{
			return RedirectToAction("Index", "FishPiCalculation");
		}

		//[HttpPost]
		//public ActionResult Create(FishPiCalculation p)
		//{
		//	db.AddProduct(p);
		//	return RedirectToAction("Index", "FishPiCalculation");
		//}

		//	[HttpGet]
		//	public ActionResult Edit(int? id)
		//	{
		//		redirect
		//		if (id == null)
		//		{
		//			return new HttpStatusCodeResult(HttpStatusCode.NotFound);
		//		}

		//		FishPiCalculation p = db.GetProductById(id.Value);

		//		if (p == null)
		//			return HttpNotFound();

		//		return View(p);
		//	}

		//	[HttpPost]
		//	public ActionResult Edit(FishPiCalculation p)
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			db.UpdateProduct(p);
		//			return RedirectToAction("Index");
		//		}

		//		return View(p);
		//	}

		//	public ActionResult Delete(int? id)
		//	{
		//		db.DeleteProductById(id.Value);

		//		return RedirectToAction("Index");
		//	}

		//	public PartialViewResult GetProds(int id)
		//	{
		//		int currPage = id;
		//		const int ProdsPerPage = 2;

		//		List<FishPiCalculation> prods =
		//			db.FishPiCalculation
		//			.OrderBy(p => p.Name)
		//			.Skip((currPage - 1) * ProdsPerPage)
		//			.Take(ProdsPerPage)
		//			.ToList();

		//		return PartialView("_PartialProdList", prods);
		//	}

		//}
	}
}