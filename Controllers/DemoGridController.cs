using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExampleGrid.Models;
using System.Linq.Dynamic;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExampleGrid.Controllers
{
    public class DemoGridController : Controller
    {

        public DemoGridController( )
        {
        }
        // GET: /<controller>/
        public IActionResult ShowGrid()
        {
            return View();
        }

        public IActionResult LoadData()
        {
            try
            {
                var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
                // Skiping number of Rows count
                var start = Request.Form["start"].FirstOrDefault();
                // Paging Length 10,20
                var length = Request.Form["length"].FirstOrDefault();
                // Sort Column Name
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                // Sort Column Direction ( asc ,desc)
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data
                var customerData = (from tempcustomer in DemoData()
                                    select tempcustomer);

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.Name == searchValue || m.Phoneno == searchValue || m.City == searchValue);
                }

                //total number of rows count 
                recordsTotal = customerData.Count();
                //Paging 
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });

            }
            catch (Exception)
            {
                throw;
            }

        }

        //demo data yükleyelim, test için
        private List<CustomerTB> DemoData()
        {
            List<CustomerTB> employees = new List<CustomerTB>();
            for (int i = 0; i <= 300; i++)
            {
                var employe = new CustomerTB() { CustomerID = 1, Name = "Ali" + i, Address = "code.replyfeed@deneme" + i, Country = "Turkey" + i, City = "Ankara" + i, Phoneno = "0555123" + i };
                employees.Add(employe);
            }
            return employees;
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            try
            {
                //burada editleme işlemlerini yaptığınızı varsayalım
                return RedirectToAction("ShowGrid", "DemoGrid");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                //burada silme işlemlerini yaptığınızı varsayalım
                return RedirectToAction("ShowGrid", "DemoGrid");
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
