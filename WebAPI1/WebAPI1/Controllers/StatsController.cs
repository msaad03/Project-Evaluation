using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using AdminDataAccess;

namespace WebAPI1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StatsController : ApiController
    {
        // GET ALL STATISTICS
        public IEnumerable<StatsView> Get()
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Evaluated_form
                .ToList()
                .GroupBy(p => p.Form_status)
                .Select(p => new StatsView
                {
                    status = String.Copy(p.Key),
                    count = p.Count()
                })
                .AsEnumerable();
                //return "All evaluated form stats";
            }
        }

        // GET ALL STATISTICS BY JURY ID
        public IEnumerable<StatsView> Get(int id)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Evaluated_form
                .Where(p => p.Jury_id1 == id)
                .ToList()
                .GroupBy(p => p.Form_status)
                .Select(p => new StatsView
                {
                    status = String.Copy(p.Key),
                    count = p.Count()
                })
                .AsEnumerable();
                //return "All evaluated form stats w.r.t jury ID: ";
            }
        }
    }
}
public class StatsView
{
    public string status { get; set; }
    public int count { get; set; }
}
