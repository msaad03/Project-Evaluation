using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FypEvaluationSystem.Models;
using FypEvaluationSystem.ViewModel;
using Newtonsoft.Json;

namespace FypEvaluationSystem.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        public string Type = "0"; // for add new form and 1 for edit form
        // GET: Admin Stats
        public ActionResult Index()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("stats/get");
                    var list = JsonConvert.DeserializeObject<List<Statistics>>(json);
                    ViewBag.stats = list;
                    return View();
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Import(string staff, HttpPostedFileBase UploadFile)
        {
            string ext = Path.GetExtension(UploadFile.FileName);

            if (!ext.Equals(".csv"))
            {
                ViewBag.Error = "Please select .csv file only";
                return View();
            }

            if (staff.Equals("Teacher"))
            {

                List<Teacher> obj = new List<Teacher>();
                using (var reader = new StreamReader(UploadFile.InputStream))
                {
                    bool flag = false;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!flag)
                        {
                            flag = true;
                            continue;
                        }
                        var values = line.Split(',');
                        obj.Add(new Teacher(values[0], values[1], values[2]));
                    }
                }
                try
                {
                    string result;
                    using (WebClient client = new WebClient())
                    {
                        client.BaseAddress = "http://localhost:59782/api/";
                        var url = "teacher/add";
                        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        string data = JsonConvert.SerializeObject(obj);
                        var response = client.UploadString(url, data);

                        result = JsonConvert.DeserializeObject<string>(response);
                    }
                    if (result.Equals("Teachers Added"))
                    {
                        ViewBag.message = "Teachers Added";
                    }
                }
                catch (WebException e)
                {
                    throw e;
                }

            }
            else
            {
                List<Group2> obj1 = new List<Group2>();
                using (var reader = new StreamReader(UploadFile.InputStream))
                {
                    bool flag = false;
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (!flag)
                        {
                            flag = true;
                            continue;
                        }
                        var values = line.Split(',');
                        var title = values[0];
                        var supervisor = values[1];
                        string cosupervisor;
                        if (values[2].Equals("-"))
                            cosupervisor = null;
                        else
                            cosupervisor = values[2];
                        List<Student2> stu = new List<Student2>();
                        for (int i = 3; i < values.Length; i += 4)
                        {
                            if (!values[i].Equals("-"))
                                stu.Add(new Student2(values[i], values[i + 1], values[i + 2], values[i + 3]));
                            else
                                continue;
                        }
                        obj1.Add(new Group2(supervisor, cosupervisor, title, stu));
                    }
                }
                try
                {
                    string result;
                    using (WebClient client = new WebClient())
                    {
                        client.BaseAddress = "http://localhost:59782/api/";
                        var url = "student/add";
                        client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";
                        string data = JsonConvert.SerializeObject(obj1);
                        var response = client.UploadString(url, data);

                        result = JsonConvert.DeserializeObject<string>(response);
                    }
                    if (result.Equals("Students Added"))
                    {
                        ViewBag.message = "Students Added";
                    }
                }
                catch (WebException e)
                {
                    throw e;
                }

            }

            ViewBag.SuccessMessage = "Successfully uploaded";
            return View();
        }

        

        public ActionResult ViewStudents()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("student/get");
                    var list = JsonConvert.DeserializeObject<List<StudentInfo>>(json);
                    ViewBag.StuList = list;
                    return View();
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
        public ActionResult ViewStudentInfo(string Student_id)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("student/getbygroup/" + Student_id);
                    var list = JsonConvert.DeserializeObject<List<GroupInfo>>(json);
                    
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json2 = client.DownloadString("student/getmarks/" + Student_id);
                    var list2 = JsonConvert.DeserializeObject<List<StudentReport>>(json2);

                    List<StudentReport> fyp1 = new List<StudentReport>();
                    List<StudentReport> fyp2 = new List<StudentReport>();

                    foreach(var elem in list2)
                    {
                        if(elem.Project_no == 1)
                        {
                            fyp1.Add(elem);
                        }
                        else
                        {
                            fyp2.Add(elem);
                        }
                    }


                    ViewBag.FYP1 = fyp1;
                    ViewBag.FYP2 = fyp2;


                    return View(list[0]);
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }

        }


        public ActionResult AddForm()
        {
            return View();
        }
        
        public ActionResult ViewForms()
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

        public ActionResult FormData(string Form_id)
        {
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

        public ActionResult SubmittedForms()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("form/getevaluatedall");
                    var list = JsonConvert.DeserializeObject<List<Evaluated_form>>(json);
                    ViewBag.forms = list;
                    return View("~/Views/Admin/DisplayForm.cshtml");
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        public ActionResult DisplayForm(string status)
        {
            ViewBag.Status = status;
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("form/GetEvaluatedByStatus/" + status);
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

        [HttpPost]
        public ActionResult FormInfo(PreFormInfo info)
        {
            if (ModelState.IsValid)
            {
                AddFields field = (AddFields) TempData["EditFields"];
                TempData["PreFormInfo"] = info;
                TempData.Keep();
                ViewBag.Pre = info;
                
                return View(field);
            }
            return View("AddForm");
        }

        [HttpPost]
        public ActionResult ViewForms(AddFields fields)
        {
            if (ModelState.IsValid)
            {
                fields.preFormInfo = (PreFormInfo)TempData["PreFormInfo"];
                var assesmentString = fields.Hidden;
                var fieldString = fields.Hidden2;

                string test = Type;

                fields.AssesmentCriteria = new List<AssesmentCriteria>();
                fields.Fields = new List<string>();

                string[] lines = assesmentString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                foreach (string item in lines)
                {
                    if (item.Equals(""))
                        continue;
                    string[] res = item.Split(',');

                    AssesmentCriteria criteria = new AssesmentCriteria();
                    criteria.Field = res[0];
                    criteria.Marks = int.Parse(res[1]);

                    fields.AssesmentCriteria.Add(criteria);
                }
                string[] res2 = fieldString.Split(',');

                foreach (string item in res2)
                {
                    if (item.Equals(""))
                        continue;
                    fields.Fields.Add(item);
                }

                Add_Template_Form finalFields = new Add_Template_Form();
                finalFields.Campus = fields.preFormInfo.Campus;
                finalFields.Department = fields.preFormInfo.Department;
                finalFields.Form_title = fields.FormTitle;
                finalFields.Form_weightage = fields.preFormInfo.TotalMarks;
                finalFields.Year = int.Parse(fields.preFormInfo.Year);
                finalFields.Semester = fields.preFormInfo.Semester;
                finalFields.Project_no = int.Parse(fields.preFormInfo.Project);

                foreach (var item in fields.AssesmentCriteria)
                {
                    Criteria_template ct = new Criteria_template();
                    ct.Criteria_title = item.Field;
                    ct.Total_marks = item.Marks;
                    finalFields.criteria_template.Add(ct);

                }
                foreach (var item in fields.Fields)
                {
                    Field_template ft = new Field_template();
                    ft.Field_title = item;
                    finalFields.field_template.Add(ft);
                }

                if (TempData["override"] != null && (bool)TempData["override"] == false)
                {
                    AddFields obj2 = (AddFields)TempData["EditFields"];
                    int fid = obj2.FormId;
                    Add_Template_Form obj_edit = new Add_Template_Form();
                    
                    obj_edit.Form_title = fields.FormTitle;
                    obj_edit.Department = fields.preFormInfo.Department;
                    obj_edit.Campus = fields.preFormInfo.Campus;
                    obj_edit.Form_weightage = fields.preFormInfo.TotalMarks;
                    obj_edit.Year = int.Parse(fields.preFormInfo.Year);
                    obj_edit.Semester = fields.preFormInfo.Semester;
                    obj_edit.Project_no = int.Parse(fields.preFormInfo.Project);

                    foreach (var item in fields.Fields)
                    {
                        Field_template ft = new Field_template();
                        ft.Field_title = item;
                        obj_edit.field_template.Add(ft);
                    }

                    foreach (var item in fields.AssesmentCriteria)
                    {
                        Criteria_template ct = new Criteria_template();
                        ct.Criteria_title = item.Field;
                        ct.Total_marks = item.Marks;
                        obj_edit.criteria_template.Add(ct);
                    }

                    try
                    {
                        string result;
                        using (WebClient client = new WebClient())
                        {
                            client.BaseAddress = "http://localhost:59782/api/";
                            var url = "form/addform";
                            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                            client.Headers[HttpRequestHeader.ContentType] = "application/json";
                            string data = JsonConvert.SerializeObject(obj_edit);
                            var response = client.UploadString(url, data);

                            result = JsonConvert.DeserializeObject<string>(response);
                        }
                        if (result.Equals("Form Added"))
                        {
                            ViewBag.message = "Form Added";
                        }
                    }
                    catch (WebException ex)
                    {
                        throw ex;
                    }

                    Type = "0";
                }
                else if(TempData["override"] != null && (bool)TempData["override"] == true)
                {
                    AddFields obj2 = (AddFields)TempData["EditFields"];
                    int fid = obj2.FormId;
                    Add_Template_Form_1 obj_edit = new Add_Template_Form_1();

                    obj_edit.Form_id = fid;
                    obj_edit.Form_title = fields.FormTitle;
                    obj_edit.Department = fields.preFormInfo.Department;
                    obj_edit.Campus = fields.preFormInfo.Campus;
                    obj_edit.Form_weightage = fields.preFormInfo.TotalMarks;
                    obj_edit.Year = int.Parse(fields.preFormInfo.Year);
                    obj_edit.Semester = fields.preFormInfo.Semester;
                    obj_edit.Project_no = int.Parse(fields.preFormInfo.Project);

                    foreach (var item in fields.Fields)
                    {
                        Field_template ft = new Field_template();
                        ft.Field_title = item;
                        obj_edit.field_template.Add(ft);
                    }

                    foreach (var item in fields.AssesmentCriteria)
                    {
                        Criteria_template ct = new Criteria_template();
                        ct.Criteria_title = item.Field;
                        ct.Total_marks = item.Marks;
                        obj_edit.criteria_template.Add(ct);
                    }

                    try
                    {
                        string result;
                        using (WebClient client = new WebClient())
                        {
                            client.BaseAddress = "http://localhost:59782/api/";
                            var url = "form/EditForm";
                            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                            client.Headers[HttpRequestHeader.ContentType] = "application/json";
                            string data = JsonConvert.SerializeObject(obj_edit);
                            var response = client.UploadString(url, data);

                            result = JsonConvert.DeserializeObject<string>(response);
                        }
                        if (result.Equals("Form Edited"))
                        {
                            ViewBag.message = "Form Edited";
                        }
                    }
                    catch (WebException ex)
                    {
                        throw ex;
                    }

                    Type = "0";
                }
                else
                {
                    try
                    {
                        string result;
                        using (WebClient client = new WebClient())
                        {
                            client.BaseAddress = "http://localhost:59782/api/";
                            var url = "form/addform";
                            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                            client.Headers[HttpRequestHeader.ContentType] = "application/json";
                            string data = JsonConvert.SerializeObject(finalFields);
                            var response = client.UploadString(url, data);

                            result = JsonConvert.DeserializeObject<string>(response);
                        }
                        if (result.Equals("Form Added"))
                        {
                            ViewBag.message = "Form Added";
                        }
                    }
                    catch (WebException ex)
                    {
                        throw ex;
                    }

                    Type = "0";
                }



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
            return View("FormInfo");
        }
        [HttpGet]
        public ActionResult FormDesignEval(int? Form_id, int? Form_template_id)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("form/getExistingByID/"+Form_template_id);
                    var list = JsonConvert.DeserializeObject<List<Existing_form>>(json);
                    ViewBag.formDetails = list[0];
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            //ViewBag.form = form;
            ViewBag.Disabled = true;
            return View("FormDesign");
        }

        public ActionResult FormDesign(string Campus, string Department, float? Form_weightage, int? Form_id, string Form_title, string criteria, string ass_fields, bool Disabled, string Semester, int Year, int Project_no)
        {
            Existing_form obj = new Existing_form();
            obj.Campus = Campus;
            obj.Department = Department;
            obj.Form_weightage = (double)Form_weightage;
            obj.Form_id = (int)Form_id;
            obj.Form_title = Form_title;
            obj.Semester = Semester;
            obj.Year = Year;
            obj.Project_no = Project_no;

            var assesmentString = criteria;
            var fieldString = ass_fields;

            string[] lines = assesmentString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string item in lines)
            {
                if (item.Equals(""))
                    continue;
                string[] res = item.Split(',');

                Existing_criteria_template criteria2 = new Existing_criteria_template();
                criteria2.Criteria_title = res[0];
                criteria2.Total_marks = int.Parse(res[1]);
                obj.criteria_template.Add(criteria2);
            }

            string[] res2 = fieldString.Split(',');

            foreach (string item in res2)
            {
                if (item.Equals(""))
                    continue;
                Existing_field_template fields = new Existing_field_template();
                fields.Field_title = item;
                obj.field_template.Add(fields);
            }
            
            ViewBag.FormInfo = obj;
            ViewBag.Disabled = Disabled;
            return View();
        }

        public ActionResult FormControl(string id)
        {
            ViewBag.ID = id;
            return View("~/Views/Admin/FormDesign.cshtml");
        }

        public ActionResult Edit(string Campus, string Department, float? Form_weightage, int? Form_id, string Form_title, string criteria, string ass_fields, bool Disabled, bool OverRide, string Year, string Semester, string Project_no)
        {
            Type = "1";
            string test = Type;
            var assesmentString = criteria;
            var fieldString = ass_fields;

            List<AssesmentCriteria> AssesmentCriteria = new List<AssesmentCriteria>();
            List<string> fields = new List<string>();

            PreFormInfo preFormInfo = new PreFormInfo()
            {
                Campus = Campus,
                Department = Department,
                Project = Project_no,
                Semester = Semester,
                Year = Year,
                TotalMarks = (double)Form_weightage
            };


            string[] lines = assesmentString.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string item in lines)
            {
                if (item.Equals(""))
                    continue;
                string[] res = item.Split(',');

                AssesmentCriteria criteria2 = new AssesmentCriteria();
                criteria2.Field = res[0];
                criteria2.Marks = int.Parse(res[1]);

                AssesmentCriteria.Add(criteria2);
            }
            string[] res2 = fieldString.Split(',');

            foreach (string item in res2)
            {
                if (item.Equals(""))
                    continue;
                fields.Add(item);
            }

            AddFields field = new AddFields()
            {
                FormId = (int)Form_id,
                FormTitle = Form_title,
                Hidden = criteria,
                Hidden2 = ass_fields,
                AssesmentCriteria = AssesmentCriteria,
                Fields = fields,
                preFormInfo = preFormInfo
            };
            ViewBag.AddField = field;
            TempData["EditFields"] = field;
            if (OverRide == false)
            {
                TempData["override"] = false;
            }
            else
            {
                TempData["override"] = true;
            }
            return View("AddForm", preFormInfo);
        }

        [HttpPost]
        public ActionResult DeleteForm(string FormID)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.BaseAddress = "http://localhost:59782/api/";
                    var json = client.DownloadString("form/delete/" + FormID);
                    var response = JsonConvert.DeserializeObject<string>(json);
                    if(response.Equals("Form Deleted"))
                    {
                        return RedirectToAction("ViewForms", "Admin");
                    }
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            return View("ViewForms");
        }


        // temp models
        public class Add_Template_Form_1
        {
            public int Form_id { get; set; }
            public string Form_title { get; set; }
            public string Department { get; set; }
            public string Campus { get; set; }
            public double Form_weightage { get; set; }
            public int Year { get; set; }
            public string Semester { get; set; }
            public int Project_no { get; set; }

            public List<Field_template> field_template { get; set; }

            public List<Criteria_template> criteria_template { get; set; }

            public Add_Template_Form_1()
            {
                field_template = new List<Field_template>();
                criteria_template = new List<Criteria_template>();
            }
        }
        // MODEL CLASSES FOR POST ADD FORM
        public class Add_Template_Form
        {
            //public int Form_id { get; set; }
            public string Form_title { get; set; }
            public string Department { get; set; }
            public string Campus { get; set; }
            public double Form_weightage { get; set; }
            public int Year { get; set; }
            public string Semester { get; set; }
            public int Project_no { get; set; }

            public List<Field_template> field_template { get; set; }

            public List<Criteria_template> criteria_template { get; set; }

            public Add_Template_Form()
            {
                field_template = new List<Field_template>();
                criteria_template = new List<Criteria_template>();
            }
        }
        public class Field_template
        {
            //public int Field_template_id { get; set; }
            public int Field_id { get; set; }
            public string Field_title { get; set; }
        }
        public class Criteria_template
        {
            //public int Criteria_template_id { get; set; }
            public int Criteria_id { get; set; }
            public string Criteria_title { get; set; }
            public double Total_marks { get; set; }
        }

    }
}