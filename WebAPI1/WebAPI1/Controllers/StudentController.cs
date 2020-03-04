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
    public class StudentController : ApiController
    {
        // GET LIST OF ALL STUDENTS
        public IEnumerable<StudentView> Get()
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Student
                .ToList()
                .Select(p => new StudentView
                {
                    Student_id = p.Student_id,
                    Group_id = p.Group_id,
                    Student_email = p.Student_email,
                    Student_name = p.Student_name
                })
                .AsEnumerable();
                //return "All student details";
            }
        }

        // GET STUDENT DETAIL BY STUDENT ID
        [Route("api/student/Get/{test}")]
        public IEnumerable<StudentView> Get(string test)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                return entities.Student
                .Where(p => p.Student_id.Equals(test))
                .ToList()
                .Select(p => new StudentView
                {
                    Student_id = p.Student_id,
                    Group_id = p.Group_id,
                    Student_email = p.Student_email,
                    Student_name = p.Student_name
                })
                .AsEnumerable();
            }
        }        

        // GET GROUP DETAIL BY STUDENT ID
        [Route("api/student/GetByGroup/{test}")]
        public IEnumerable<GroupView> GetByGroup(string test)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {                
                var group = entities.Student.FirstOrDefault(p => p.Student_id.Equals(test));
                int g = group.Group_id;
                var list = (from stud in entities.Student
                            join grp in entities.Group on stud.Group_id equals grp.Group_id
                            where grp.Group_id == g
                            select new
                            {
                                grp.Supervisor_name,
                                grp.Cosupervisor_name,
                                grp.Project_title,
                                grp.Leader_id,
                                grp.Group_id,
                                stud.Student_id,
                                stud.Student_name,
                                stud.Student_email
                }).ToList();
                List<GroupView> glist = new List<GroupView>();
                GroupView obj = new GroupView();
                obj.Supervisor_name = list[0].Supervisor_name;
                obj.Cosupervisor_name = list[0].Cosupervisor_name;
                obj.Project_title = list[0].Project_title;
                obj.Leader_id = list[0].Leader_id;
                obj.Group_id = list[0].Group_id;
                foreach (var item in list)
                {
                    string s1 = item.Leader_id;
                    string s2 = item.Student_id;
                    if (s1.Equals(s2))
                    {
                        obj.Leader_name = item.Student_name;
                        break;
                    }
                }
                for (int i = 0; i < list.Count; i++)
                {
                    StudentView st = new StudentView();
                    st.Student_id = list[i].Student_id;
                    st.Student_name = list[i].Student_name;
                    st.Student_email = list[i].Student_email;
                    st.Group_id = obj.Group_id;
                    obj.Student_list.Add(st);
                }
                glist.Add(obj);
                return glist;
            }
        }

        // GET STUDENT MARKS
        /* QUERY(s):
            SELECT Tf.Form_id, Tf.Form_title, Tf.Project_no, Tf.Form_weightage FROM Template_form Tf
            JOIN Evaluated_form Ev ON Ev.Template_id = Tf.Form_id

            SELECT Form_template_id, SUM(Total_marks) Total, SUM(Obtained_marks) Obtained FROM Criteria_template Ct
            JOIN Criteria_marks Cm ON Ct.Criteria_id = Cm.Criteria_template_id
            WHERE Cm.Student_id = 'K163807' GROUP BY Form_template_id
        */
        [Route("api/student/GetMarks/{test}")]
        public IEnumerable<StudentReport> GetMarks(string test)
        {
            List<StudentReport> report_list = new List<StudentReport>();

            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                var form_list = (from Tf in entities.Template_form
                                 join Ev in entities.Evaluated_form on Tf.Form_id equals Ev.Template_id
                                 select new
                                 {
                                     Tf.Form_id,
                                     Tf.Form_title,
                                     Tf.Project_no,
                                     Tf.Form_weightage
                                 }).ToList();
                var marks_list = (from Ct in entities.Criteria_template
                                  join Cm in entities.Criteria_marks on Ct.Criteria_id equals Cm.Criteria_template_id
                                  where Cm.Student_id.Equals(test)
                                  //group Ct by Ct.Form_template_id into g
                                  select new
                                  {
                                      Ct.Form_template_id,
                                      Ct.Total_marks,
                                      Cm.Obtained_marks
                                  }).ToList();
                var results = (from p in marks_list
                               group p by p.Form_template_id into g
                               select new
                               {
                                   g.Key,
                                   Total_marks = g.Sum(x => x.Total_marks),
                                   Obtained_marks = g.Sum(x => x.Obtained_marks)
                               }).ToList();                

                for (int i = 0; i < results.Count; i++)
                {
                    if (form_list[i].Form_id == results[i].Key)
                    {
                        StudentReport rep = new StudentReport();
                        rep.Evaluation_type = form_list[i].Form_title;
                        rep.Project_no = form_list[i].Project_no;
                        rep.Obtained_marks = (results[i].Obtained_marks / results[i].Total_marks) * form_list[i].Form_weightage;
                        rep.Total_marks = form_list[i].Form_weightage;
                        report_list.Add(rep);
                    }
                }                
            }
            return report_list;
        }

        // POST STUDENT LOGIN AUTHENTICATION
        [HttpPost]
        public IEnumerable<StudentView> Login(StudentViewModel obj)
        {
            if (!ModelState.IsValid)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
                return null;
            }
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                var user = entities.Student
                .Where(p => p.Student_id == obj.Student_id && p.Password == obj.Password)
                .ToList()
                .Select(p => new StudentView
                {
                    Student_id = p.Student_id,
                    Student_name = p.Student_name,
                    Student_email = p.Student_email,
                })
                .AsEnumerable();
                if (user != null)
                {
                    return user;
                    //return Request.CreateResponse(HttpStatusCode.OK, "Login successful");                    
                }
                else
                {
                    return null;
                    //return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid credentials");
                }
            }
        }
        [HttpPost]
        public HttpResponseMessage Add(List<Group> obj)
        {
            if (!ModelState.IsValid)
            {
                //return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
                return null;
            }
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {                
                for (int i = 0; i < obj.Count; i++)
                {
                    AdminDataAccess.Group o2 = new AdminDataAccess.Group();
                    o2.Supervisor_name = obj[i].Supervisor_name;
                    if (String.IsNullOrEmpty(obj[i].Cosupervisor_name))
                        o2.Cosupervisor_name = null;
                    else
                        o2.Cosupervisor_name = obj[i].Cosupervisor_name;

                    o2.Project_title = obj[i].Project_title;
                    o2.Leader_id = obj[i].Student_list[0].Student_id;

                    entities.Group.Add(o2);
                    entities.SaveChanges();

                    for (int j = 0; j < obj[i].Student_list.Count; j++)
                    {
                        AdminDataAccess.Student o1 = new AdminDataAccess.Student();
                        o1.Student_email = obj[i].Student_list[j].Student_email;
                        o1.Student_id = obj[i].Student_list[j].Student_id;
                        o1.Student_name = obj[i].Student_list[j].Student_name;
                        o1.Password = obj[i].Student_list[j].Password;
                        o1.Group_id = entities.Group.Select(p => p.Group_id).Max();

                        entities.Student.Add(o1);
                        entities.SaveChanges();
                    }                    
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Students Added");
        }
    }
}
public class StudentView
{
    public string Student_id { get; set; }
    public string Student_email { get; set; }
    public string Student_name { get; set; }
    public int Group_id { get; set; }
}

public class StudentViewModel
{
    public string Student_id { get; set; }
    public string Password { get; set; }
}

public class GroupView
{
    public string Supervisor_name { get; set; }
    public string Cosupervisor_name { get; set; }
    public string Project_title { get; set; }
    public int Group_id { get; set; }
    public string Leader_id { get; set; }
    public string Leader_name { get; set; }
    public List<StudentView> Student_list { get; set; }
    public GroupView()
    {
        Student_list = new List<StudentView>();
    }
}
public class Group
{
    public string Supervisor_name { get; set; }
    public string Cosupervisor_name { get; set; }
    public string Project_title { get; set; }
    public List<Student> Student_list { get; set; }

    public Group()
    {
        Student_list = new List<Student>();
    }
    public Group(string s, string c, string t, List<Student> stud)
    {
        Supervisor_name = s;
        Cosupervisor_name = c;
        Project_title = t;
        Student_list = stud;
    }
}
public class Student
{
    public string Student_id { get; set; }
    public string Student_email { get; set; }
    public string Student_name { get; set; }
    public string Password { get; set; }
    public Student(string id, string email, string name, string password)
    {
        Student_id = id;
        Student_email = email;
        Student_name = name;
        Password = password;
    }
}
public class StudentReport
{
    public string Evaluation_type { get; set; }
    public double Obtained_marks { get; set; }
    public double Total_marks { get; set; }
    public int Project_no { get; set; }
}
 