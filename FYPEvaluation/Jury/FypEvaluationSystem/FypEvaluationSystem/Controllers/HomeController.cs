using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FypEvaluationSystem.ViewModel;
using FypEvaluationSystem.Models;
using System.Net;
using Newtonsoft.Json;
using System.Web.Security;

namespace FypEvaluationSystem.Controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("form/getExistingAll");
                    var list = JsonConvert.DeserializeObject<List<Existing_form>>(json);
                    ViewBag.forms = list;
                    return View();
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public ActionResult SubmittedForms()
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
            
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("form/GetEvaluatedByJuryID/"+ticket.Name);
                    var list = JsonConvert.DeserializeObject<List<Evaluated_form>>(json);
                    ViewBag.forms = list;
                    return View();
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public ActionResult Statistics()
        {
            try
            {
                string juryId = User.Identity.Name;

                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("stats/get/" + juryId);
                    var list = JsonConvert.DeserializeObject<List<Status>>(json);
                    ViewBag.Status = list;
                    return View();
                }
            }
            catch(WebException ex)
            {
                throw ex;
            }
            
        }

     

        [HttpPost]
        public ActionResult StudentForm(string StudentID, string FormID)
        {
            GroupView list = new GroupView();
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("student/getbygroup/"+StudentID);
                    var list2 = JsonConvert.DeserializeObject<List<GroupView>>(json);
                    list = list2[0];
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            Group group = new Group();

            group.GroupId = list.Group_id;
            group.ProjectTitle = list.Project_title;
            group.LeaderId = list.Leader_id;
            group.Supervisor = list.Supervisor_name;
            if (list.Cosupervisor_name != null)
                group.CoSupervisor = list.Cosupervisor_name;
            else
                group.CoSupervisor = "-";
            List<Student> stdList = new List<Student>();
            foreach(var item in list.Student_list)
            {
                Student std = new Student(item.Student_id, item.Student_name);
                stdList.Add(std);
            }
            group.members = stdList;

            Existing_form exist_obj = new Existing_form();
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("form/getexistingbyid/" + FormID);
                    var list2 = JsonConvert.DeserializeObject<List<Existing_form>>(json);

                    // Check for already evaluated form
                    string juryId = User.Identity.Name;
                    var json2 = client.DownloadString("form/GetEvaluatedByJuryID/" + juryId);
                    var list3 = JsonConvert.DeserializeObject<List<Evaluated_form>>(json2);
                    foreach (var item in list3)
                    {
                        if (list2[0].Form_id == item.Form_template_id && group.LeaderId.Equals(item.Leader_id))
                        {
                            // GROUP ALREADY EVALUATED
                            /*
                             * Q: why TempData instead of ViewBag?
                             * A: https://stackoverflow.com/questions/19294975/why-isnt-viewbag-value-passing-back-to-the-view
                             */
                            TempData["ExistMessage"] = "Group is already evaluated";
                            return RedirectToAction("FormData", "Home", new { Form_id = item.Form_id.ToString()});
                        }
                    }

                    exist_obj = list2[0];
                    ViewBag.form = exist_obj;
                    TempData["Ex_Form"] = exist_obj;
                    TempData["grp"] = group;
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            return View(group);
        }
        [HttpPost]
        public ActionResult EvaluateForm(string optradio, ICollection<double> marks, ICollection<string> Fields, int? MemberCount)
        {

            int? fieldCount = marks.Count / MemberCount;

            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);

            Existing_form exForm = (Existing_form)TempData["Ex_Form"];
            Group grp = (Group)TempData["grp"];

            Add_Evaluated_Form obj = new Add_Evaluated_Form();

            obj.Template_id = exForm.Form_id;
            obj.Form_status = optradio;
            obj.Group_id = grp.GroupId;
            obj.Jury_id1 = int.Parse(ticket.Name);

            List<Add_field_text> ft = new List<Add_field_text>();
            List<Add_Criteria_marks> ct = new List<Add_Criteria_marks>();

            int i = 0;
            foreach(var item in Fields)
            {
                Add_field_text x = new Add_field_text();
                x.Field_template_id = exForm.field_template[i].Field_template_id;
                x.Field_text = item;
                ft.Add(x);
                i++;
            }


            var memMarks = marks.ToArray();
            
            for( var j = 0; j< MemberCount; j++)
            {
                int l = 0;

                for (var k = j; k< memMarks.Length; k=k+(int)MemberCount)
                {

                    Add_Criteria_marks x = new Add_Criteria_marks();
                    x.Criteria_template_id = exForm.criteria_template[l].Criteria_template_id;
                    x.Student_id = grp.members[j].ID;
                    x.Obtained_marks = memMarks[k];
                    ct.Add(x);

                    l++;
                }
            }


            obj.criteria_marks = ct;
            obj.field_text = ft;

            using (WebClient client = new WebClient())
            {
                client.BaseAddress = "http://localhost:59782/api/";
                var url = "form/EvaluateForm";
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                string data = JsonConvert.SerializeObject(obj);
                var response = client.UploadString(url, data);
            }

            return RedirectToAction("Index", "Home");
        }
        public ActionResult FormData(string Form_id)
        {
            var x = Form_id;
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("form/getevaluatedbyid/" + Form_id);
                    var list = JsonConvert.DeserializeObject<List<Evaluated_form>>(json);

                    ViewBag.forms = list[0];
                    return View();
                }

            }
            catch (WebException ex)
            {
                throw ex;
            }

        }
        public ActionResult FormDesign()
        {

            return View();
        }

        public class GroupView
        {
            public string Supervisor_name { get; set; }
            public string Cosupervisor_name { get; set; }
            public string Project_title { get; set; }
            public int Group_id { get; set; }
            public string Leader_id { get; set; }
            public List<StudentView> Student_list { get; set; }
            public GroupView()
            {
                Student_list = new List<StudentView>();
            }
        }

        public class StudentView
        {
            public string Student_id { get; set; }
            public string Student_email { get; set; }
            public string Student_name { get; set; }
            public int Group_id { get; set; }
        }
        // MODEL CLASSES FOR POST EVALUATE FORM
        public class Add_Evaluated_Form
        {
            //public int Form_id { get; set; }
            public int Template_id { get; set; }
            //public string Leader_id { get; set; }
            //public string Supervisor_name { get; set; }
            //public string Cosupervisor_name { get; set; }
            //public string Project_title { get; set; }
            public int Jury_id1 { get; set; }
            //public int Jury_id2 { get; set; }
            public string Form_status { get; set; }
            public int Group_id { get; set; }
            public List<Add_field_text> field_text { get; set; }
            public List<Add_Criteria_marks> criteria_marks { get; set; }
            public Add_Evaluated_Form()
            {
                field_text = new List<Add_field_text>();
                criteria_marks = new List<Add_Criteria_marks>();
            }
        }
        public class Add_field_text
        {
            //public int Form_id { get; set; }
            public int Field_template_id { get; set; }
            public string Field_text { get; set; }
        }
        public class Add_Criteria_marks
        {
            //public int Form_id { get; set; }
            public int Criteria_template_id { get; set; }
            public double Obtained_marks { get; set; }
            public string Student_id { get; set; }
        }

    }
}