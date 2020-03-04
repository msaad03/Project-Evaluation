using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdminDataAccess;

namespace WebAPI1.Controllers
{
    public class FormController : ApiController
    {
        // GET LIST OF ALL EVALUATED FORMS
        public IEnumerable<Evaluated_form> GetEvaluatedAll()
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Evaluated_form.ToList();
                //return "All evaluated forms";
            }
        }

        // GET EVALUATED FORMS BY FORM ID
        public IEnumerable<Evaluated_form> GetEvaluatedByID(int id)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Evaluated_form
                .Where(p => p.Form_id == id)
                .ToList();
                //return "All evaluated form with ID: " + id.ToString();
            }
        }

        // GET LIST OF ALL EVALUATED FORMS BY JURY ID
        public IEnumerable<Evaluated_form> GetEvaluatedByJuryID(int id)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Evaluated_form
                .Where(p => p.Jury_id1 == id || p.Jury_id2 == id)
                .ToList();
                //return "All evaluated form with Jury ID: " + id.ToString();
            }
        }

        // GET LIST OF ALL EVALUATED FORMS BY FORM STATUS
        [Route("api/form/GetEvaluatedByStatus/{test}")]
        public IEnumerable<Evaluated_form> GetEvaluatedByStatus(string test)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Evaluated_form
                .Where(p => p.Form_status.Equals(test))
                .ToList();
                //return "All evaluated form with Status: " + test;
            }
        }

        // GET EVALUATED FORM BY GROUP ID
        public IEnumerable<Evaluated_form> GetEvaluatedByGroupID(int id)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Evaluated_form
                .Where(p => p.Group_id == id)
                .ToList();
                //return "All evaluated form with Group ID: " + id.ToString();
            }
        }

        // GET LIST OF ALL EXISTING TEMPLATE FORMS
        public IEnumerable<Template_form> GetExistingAll()
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Template_form.ToList();
                //return "All existing forms";
            }
        }

        // GET LIST OF ALL EXISTING TEMPLATE FORMS BY FORM ID
        public IEnumerable<Template_form> GetExistingByID(int id)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Template_form
                .Where(p => p.Form_id == id)
                .ToList();
                //return "All existing form with ID: " + id.ToString();
            }
        }
    }
}
