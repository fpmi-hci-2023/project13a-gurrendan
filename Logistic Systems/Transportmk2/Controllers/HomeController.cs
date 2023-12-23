using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Transportmk2.Models;
using Microsoft.Data.SqlClient;

namespace Transportmk2.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.Storages.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Task() {
            string answer = "";
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=Transport;Trusted_Connection=True;";
            List<string> demandName = new List<string>();
            List<int> demandNeed = new List<int>();
            List<string> shareName = new List<string>();
            List<int> shareCapacity = new List<int>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string demandNameQuery = "SELECT Storage_Name from Storages Where Needed != 0";
                string demandNeedQuery = "SELECT Needed from Storages Where Needed != 0";
                string shareNameQuery = "SELECT Storage_Name from Storages Where Needed = 0";
                string shareCapacityQuery = "SELECT Capacity from Storages Where Needed = 0";
                SqlCommand cmd1 = new SqlCommand(demandNameQuery, conn);
                SqlCommand cmd2 = new SqlCommand(demandNeedQuery, conn);
                SqlCommand cmd3 = new SqlCommand(shareNameQuery, conn);
                SqlCommand cmd4 = new SqlCommand(shareCapacityQuery, conn);


                conn.Open();
                SqlDataReader reader1 = cmd1.ExecuteReader();
                if (reader1.HasRows)
                {
                    while (reader1.Read())
                        demandName.Add(reader1.GetString(0));   
                }
                conn.Close();

                conn.Open();
                SqlDataReader reader2 = cmd2.ExecuteReader();
                if (reader2.HasRows)
                {
                    while (reader2.Read())
                        demandNeed.Add(reader2.GetInt32(0));
                }
                conn.Close();

                conn.Open();
                SqlDataReader reader3 = cmd3.ExecuteReader();
                if (reader3.HasRows)
                {
                    while (reader3.Read())
                        shareName.Add(reader3.GetString(0));
                }
                conn.Close();

                conn.Open();
                SqlDataReader reader4 = cmd4.ExecuteReader();
                if (reader4.HasRows)
                {
                    while (reader4.Read())
                        shareCapacity.Add(reader4.GetInt32(0));
                }
                conn.Close();

                for (int i = 0; i < shareCapacity.Count; i++)
                    for (int j = 0; j < demandNeed.Count; j++) 
                    {
                        if (demandNeed[j] != 0)
                        {
                            if (shareCapacity[i] != 0) 
                            {  
                            if (shareCapacity[i] < demandNeed[j])
                            {
                                int a = shareCapacity[i];
                                demandNeed[j] -= a;
                                shareCapacity[i] = 0;
                                answer += $"{shareName[i]} [{a}]-> {demandName[j]}</br></br>";
                            }
                            else
                            {
                                int a = shareCapacity[i] - (shareCapacity[i] - demandNeed[j]);
                                shareCapacity[i] -= a;
                                demandNeed[j] -= 0;
                                answer += $"{shareName[i]} [{a}]-> {demandName[j]}</br></br>";
                            }
                            }
                            else
                                j = demandNeed.Count;
                        }

                    }
                ViewBag.Message = answer;
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Storage storage)
        {
            db.Storages.Add(storage);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Storage storage = await db.Storages.FirstOrDefaultAsync(p => p.Storage_id == id);
                if (storage != null)
                    return View(storage);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Storage storage)
        {
            db.Storages.Update(storage);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Storage storage = await db.Storages.FirstOrDefaultAsync(p => p.Storage_id == id);
                if (storage != null)
                    return View(storage);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Storage storage = new Storage { Storage_id = id.Value };
                db.Entry(storage).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }

    }
}
