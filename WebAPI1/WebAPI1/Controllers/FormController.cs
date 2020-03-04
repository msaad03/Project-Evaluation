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
    public class FormController : ApiController
    {
        // GET LIST OF ALL EVALUATED FORMS
        public IEnumerable<Evaluated_form> GetEvaluatedAll()
        {
            using (MyFirstDBEntities entity = new MyFirstDBEntities())
            {
                var eval_list = (from eval in entity.Evaluated_form

                                    join grp in entity.Group on eval.Group_id equals grp.Group_id
                                    join t in entity.Template_form on eval.Template_id equals t.Form_id
                                    join stud in entity.Student on grp.Leader_id equals stud.Student_id
                                    select new Evaluated_form
                                    {
                                        Leader_id = grp.Leader_id,
                                        Leader_name = stud.Student_name,
                                        Supervisor_name = grp.Supervisor_name,
                                        Cosupervisor_name = grp.Cosupervisor_name,
                                        Project_title = grp.Project_title,
                                        Form_status = eval.Form_status,
                                        Form_id = eval.Form_id,
                                        Form_template_id = eval.Template_id,
                                        Form_title = t.Form_title,
                                        Department = t.Department,
                                        Campus = t.Campus,
                                        Form_weightage = t.Form_weightage,
                                        Year = t.Year,
                                        Semester = t.Semester,
                                        Project_no = t.Project_no
                                    }).ToList();

                var criteria_list = (from crit in entity.Criteria_marks
                                        join temp in entity.Criteria_template
                                        on crit.Criteria_template_id equals temp.Criteria_id
                                        select new
                                        {
                                            crit.Form_id,
                                            temp.Criteria_title,
                                            temp.Total_marks,
                                            crit.Student_id,
                                            crit.Obtained_marks,
                                        }).ToList();

                var field_list = (from field in entity.Field_text
                                    join temp in entity.Field_template
                                    on field.Field_template_id equals temp.Field_id
                                    select new
                                    {
                                        field.Form_id,
                                        temp.Field_title,
                                        field.Field_text1
                                    }).ToList();
                foreach (var item in eval_list)

                {
                    foreach (var criteria in criteria_list)
                    {
                        if (criteria.Form_id == item.Form_id)
                        {
                            Evaluated_criteria_marks temp1 = new Evaluated_criteria_marks();
                            Evaluated_criteria_template temp2 = new Evaluated_criteria_template();
                            temp1.Student_id = criteria.Student_id;
                            temp1.Obtained_marks = criteria.Obtained_marks;
                            temp2.Criteria_title = criteria.Criteria_title;
                            temp2.Total_marks = criteria.Total_marks;
                            item.criteria_marks.Add(temp1);
                            item.criteria_template.Add(temp2);
                        }
                    }
                    foreach (var field in field_list)
                    {
                        if (field.Form_id == item.Form_id)
                        {
                            Evaluated_field_text temp1 = new Evaluated_field_text();
                            Evaluated_field_template temp2 = new Evaluated_field_template();
                            temp1.Field_text1 = field.Field_text1;
                            temp2.Field_title = field.Field_title;
                            item.field_text.Add(temp1);
                            item.field_template.Add(temp2);
                        }
                    }
                }
                return eval_list;
            }
        }

        // GET EVALUATED FORMS BY FORM ID
        public IEnumerable<Evaluated_form> GetEvaluatedByID(int id)
        {
            using (MyFirstDBEntities entity = new MyFirstDBEntities())
            {
                var eval_list = (from eval in entity.Evaluated_form
                                    join grp in entity.Group on eval.Group_id equals grp.Group_id
                                    join t in entity.Template_form on eval.Template_id equals t.Form_id
                                    join stud in entity.Student on grp.Leader_id equals stud.Student_id
                                    where eval.Form_id == id
                                    select new Evaluated_form
                                    {
                                        Leader_id = grp.Leader_id,
                                        Leader_name = stud.Student_name,
                                        Supervisor_name = grp.Supervisor_name,
                                        Cosupervisor_name = grp.Cosupervisor_name,
                                        Project_title = grp.Project_title,
                                        Form_status = eval.Form_status,
                                        Form_id = eval.Form_id,
                                        Form_template_id = eval.Template_id,
                                        Form_title = t.Form_title,
                                        Department = t.Department,
                                        Campus = t.Campus,
                                        Form_weightage = t.Form_weightage,
                                        Year = t.Year,
                                        Semester = t.Semester,
                                        Project_no = t.Project_no
                                    }).ToList();

                //var criteria_list = (from crit in entity.Criteria_marks
                //                     join temp in entity.Criteria_template
                //                     on crit.Criteria_template_id equals temp.Criteria_id
                //                     where crit.Form_id == id
                //                     select new
                //                     {
                //                         crit.Form_id,
                //                         temp.Criteria_title,
                //                         temp.Total_marks,
                //                         crit.Student_id,
                //                         crit.Obtained_marks,
                //                     }).ToList();
                var criteria_marks_list = entity.Criteria_marks.Where(p => p.Form_id == id).Select(p =>
                    new
                    {
                        p.Criteria_template_id,
                        p.Student_id,
                        p.Obtained_marks
                    }
                ).ToList();
                if (eval_list.Count != 0)
                {
                    var lanat = eval_list[0].Form_template_id;
                    var criteria_template_list = (from crit in entity.Criteria_template
                                                    where crit.Form_template_id == lanat
                                                    select new
                                                    {
                                                        crit.Criteria_title,
                                                        crit.Total_marks
                                                    }).ToList();
                    //var criteria_template_list = entity.Criteria_template
                    //    .Where(p => p.Form_template_id == eval_list[0].Form_template_id)
                    //    .Select(p=>
                    //    new
                    //    {
                    //        p.Criteria_title,
                    //        p.Total_marks
                    //    }
                    //).ToList();

                    var field_list = (from field in entity.Field_text
                                        join temp in entity.Field_template
                                        on field.Field_template_id equals temp.Field_id
                                        where field.Form_id == id
                                        select new
                                        {
                                            field.Form_id,
                                            temp.Field_title,
                                            field.Field_text1
                                        }).ToList();

                    foreach (var item in eval_list)
                    {
                        //foreach (var criteria in criteria_list)
                        //{
                        //    if (criteria.Form_id == item.Form_id)
                        //    {
                        //        Evaluated_criteria_marks temp1 = new Evaluated_criteria_marks();
                        //        Evaluated_criteria_template temp2 = new Evaluated_criteria_template();

                        //        temp1.Student_id = criteria.Student_id;
                        //        temp1.Obtained_marks = criteria.Obtained_marks;
                        //        temp2.Criteria_title = criteria.Criteria_title;
                        //        temp2.Total_marks = criteria.Total_marks;

                        //        item.criteria_marks.Add(temp1);
                        //        item.criteria_template.Add(temp2);
                        //    }
                        //}
                        foreach (var criteria in criteria_template_list)
                        {
                            Evaluated_criteria_template temp2 = new Evaluated_criteria_template();

                            temp2.Criteria_title = criteria.Criteria_title;
                            temp2.Total_marks = criteria.Total_marks;


                            item.criteria_template.Add(temp2);
                        }
                        foreach (var criteria in criteria_marks_list)
                        {
                            Evaluated_criteria_marks temp1 = new Evaluated_criteria_marks();

                            temp1.Student_id = criteria.Student_id;
                            temp1.Obtained_marks = criteria.Obtained_marks;

                            item.criteria_marks.Add(temp1);
                        }
                        foreach (var field in field_list)
                        {
                            if (field.Form_id == item.Form_id)
                            {
                                Evaluated_field_text temp1 = new Evaluated_field_text();
                                Evaluated_field_template temp2 = new Evaluated_field_template();
                                temp1.Field_text1 = field.Field_text1;
                                temp2.Field_title = field.Field_title;

                                item.field_text.Add(temp1);
                                item.field_template.Add(temp2);
                            }
                        }
                    }
                }

                /*
                foreach (var item in eval_list)
                {
                    //foreach (var criteria in criteria_list)
                    //{
                    //    if (criteria.Form_id == item.Form_id)
                    //    {
                    //        Evaluated_criteria_marks temp1 = new Evaluated_criteria_marks();
                    //        Evaluated_criteria_template temp2 = new Evaluated_criteria_template();

                    //        temp1.Student_id = criteria.Student_id;
                    //        temp1.Obtained_marks = criteria.Obtained_marks;
                    //        temp2.Criteria_title = criteria.Criteria_title;
                    //        temp2.Total_marks = criteria.Total_marks;

                    //        item.criteria_marks.Add(temp1);
                    //        item.criteria_template.Add(temp2);
                    //    }
                    //}
                    foreach (var criteria in criteria_template_list)
                    {
                        Evaluated_criteria_template temp2 = new Evaluated_criteria_template();

                        temp2.Criteria_title = criteria.Criteria_title;
                        temp2.Total_marks = criteria.Total_marks;


                        item.criteria_template.Add(temp2);
                    }
                    foreach (var criteria in criteria_marks_list)
                    {
                        Evaluated_criteria_marks temp1 = new Evaluated_criteria_marks();

                        temp1.Student_id = criteria.Student_id;
                        temp1.Obtained_marks = criteria.Obtained_marks;

                        item.criteria_marks.Add(temp1);
                    }
                    foreach (var field in field_list)
                    {
                        if (field.Form_id == item.Form_id)
                        {
                            Evaluated_field_text temp1 = new Evaluated_field_text();
                            Evaluated_field_template temp2 = new Evaluated_field_template();
                            temp1.Field_text1 = field.Field_text1;
                            temp2.Field_title = field.Field_title;

                            item.field_text.Add(temp1);
                            item.field_template.Add(temp2);
                        }
                    }
                }
                */
                return eval_list;
            }
        }

        // GET LIST OF ALL EVALUATED FORMS BY JURY ID
        public IEnumerable<Evaluated_form> GetEvaluatedByJuryID(int id)
            {
                using (MyFirstDBEntities entity = new MyFirstDBEntities())
                {
                    var eval_list = (from eval in entity.Evaluated_form
                                        join grp in entity.Group on eval.Group_id equals grp.Group_id
                                        join t in entity.Template_form on eval.Template_id equals t.Form_id
                                        join stud in entity.Student on grp.Leader_id equals stud.Student_id
                                        where eval.Jury_id1 == id
                                        select new Evaluated_form
                                        {
                                            Leader_id = grp.Leader_id,
                                            Leader_name = stud.Student_name,
                                            Supervisor_name = grp.Supervisor_name,
                                            Cosupervisor_name = grp.Cosupervisor_name,
                                            Project_title = grp.Project_title,
                                            Form_status = eval.Form_status,
                                            Form_id = eval.Form_id,
                                            Form_template_id = eval.Template_id,
                                            Form_title = t.Form_title,
                                            Department = t.Department,
                                            Campus = t.Campus,
                                            Form_weightage = t.Form_weightage,
                                            Year = t.Year,
                                            Semester = t.Semester,
                                            Project_no = t.Project_no
                                        }).ToList();

                    var criteria_list = (from crit in entity.Criteria_marks
                                            join temp in entity.Criteria_template
                                            on crit.Criteria_template_id equals temp.Criteria_id
                                            select new
                                            {
                                                crit.Form_id,
                                                temp.Criteria_title,
                                                temp.Total_marks,
                                                crit.Student_id,
                                                crit.Obtained_marks,
                                            }).ToList();

                    var field_list = (from field in entity.Field_text
                                        join temp in entity.Field_template
                                        on field.Field_template_id equals temp.Field_id
                                        select new
                                        {
                                            field.Form_id,
                                            temp.Field_title,
                                            field.Field_text1
                                        }).ToList();

                    foreach (var item in eval_list)
                    {
                        foreach (var criteria in criteria_list)
                        {
                            if (criteria.Form_id == item.Form_id)
                            {
                                Evaluated_criteria_marks temp1 = new Evaluated_criteria_marks();
                                Evaluated_criteria_template temp2 = new Evaluated_criteria_template();

                                temp1.Student_id = criteria.Student_id;
                                temp1.Obtained_marks = criteria.Obtained_marks;
                                temp2.Criteria_title = criteria.Criteria_title;
                                temp2.Total_marks = criteria.Total_marks;

                                item.criteria_marks.Add(temp1);
                                item.criteria_template.Add(temp2);
                            }
                        }
                        foreach (var field in field_list)
                        {
                            if (field.Form_id == item.Form_id)
                            {
                                Evaluated_field_text temp1 = new Evaluated_field_text();
                                Evaluated_field_template temp2 = new Evaluated_field_template();
                                temp1.Field_text1 = field.Field_text1;
                                temp2.Field_title = field.Field_title;

                                item.field_text.Add(temp1);
                                item.field_template.Add(temp2);
                            }
                        }
                    }
                    return eval_list;
                }
            }

        // GET LIST OF ALL EVALUATED FORMS BY FORM STATUS
        [Route("api/form/GetEvaluatedByStatus/{test}")]
        public IEnumerable<Evaluated_form> GetEvaluatedByStatus(string test)
        {
            using (MyFirstDBEntities entity = new MyFirstDBEntities())
            {
                var eval_list = (from eval in entity.Evaluated_form
                                    join grp in entity.Group on eval.Group_id equals grp.Group_id
                                    join t in entity.Template_form on eval.Template_id equals t.Form_id
                                    join stud in entity.Student on grp.Leader_id equals stud.Student_id
                                    where eval.Form_status.Equals(test)
                                    select new Evaluated_form
                                    {
                                        Leader_id = grp.Leader_id,
                                        Leader_name = stud.Student_name,
                                        Supervisor_name = grp.Supervisor_name,
                                        Cosupervisor_name = grp.Cosupervisor_name,
                                        Project_title = grp.Project_title,
                                        Form_status = eval.Form_status,
                                        Form_id = eval.Form_id,
                                        Form_template_id = eval.Template_id,
                                        Form_title = t.Form_title,
                                        Department = t.Department,
                                        Campus = t.Campus,
                                        Form_weightage = t.Form_weightage,
                                        Year = t.Year,
                                        Semester = t.Semester,
                                        Project_no = t.Project_no
                                    }).ToList();

                var criteria_list = (from crit in entity.Criteria_marks
                                        join temp in entity.Criteria_template
                                        on crit.Criteria_template_id equals temp.Criteria_id
                                        select new
                                        {
                                            crit.Form_id,
                                            temp.Criteria_title,
                                            temp.Total_marks,
                                            crit.Student_id,
                                            crit.Obtained_marks,
                                        }).ToList();

                var field_list = (from field in entity.Field_text
                                    join temp in entity.Field_template
                                    on field.Field_template_id equals temp.Field_id
                                    select new
                                    {
                                        field.Form_id,
                                        temp.Field_title,
                                        field.Field_text1
                                    }).ToList();

                foreach (var item in eval_list)
                {
                    foreach (var criteria in criteria_list)
                    {
                        if (criteria.Form_id == item.Form_id)
                        {
                            Evaluated_criteria_marks temp1 = new Evaluated_criteria_marks();
                            Evaluated_criteria_template temp2 = new Evaluated_criteria_template();

                            temp1.Student_id = criteria.Student_id;
                            temp1.Obtained_marks = criteria.Obtained_marks;
                            temp2.Criteria_title = criteria.Criteria_title;
                            temp2.Total_marks = criteria.Total_marks;

                            item.criteria_marks.Add(temp1);
                            item.criteria_template.Add(temp2);
                        }
                    }
                    foreach (var field in field_list)
                    {
                        if (field.Form_id == item.Form_id)
                        {
                            Evaluated_field_text temp1 = new Evaluated_field_text();
                            Evaluated_field_template temp2 = new Evaluated_field_template();
                            temp1.Field_text1 = field.Field_text1;
                            temp2.Field_title = field.Field_title;

                            item.field_text.Add(temp1);
                            item.field_template.Add(temp2);
                        }
                    }
                }
                return eval_list;
            }
        }

        // GET EVALUATED FORM BY GROUP ID
        public IEnumerable<Evaluated_form> GetEvaluatedByGroupID(int id)
        {
            using (MyFirstDBEntities entity = new MyFirstDBEntities())
            {
                var eval_list = (from eval in entity.Evaluated_form
                                    join grp in entity.Group on eval.Group_id equals grp.Group_id
                                    join t in entity.Template_form on eval.Template_id equals t.Form_id
                                    join stud in entity.Student on grp.Leader_id equals stud.Student_id
                                    where eval.Group_id == id
                                    select new Evaluated_form
                                    {
                                        Leader_id = grp.Leader_id,
                                        Leader_name = stud.Student_name,
                                        Supervisor_name = grp.Supervisor_name,
                                        Cosupervisor_name = grp.Cosupervisor_name,
                                        Project_title = grp.Project_title,
                                        Form_status = eval.Form_status,
                                        Form_id = eval.Form_id,
                                        Form_template_id = eval.Template_id,
                                        Form_title = t.Form_title,
                                        Department = t.Department,
                                        Campus = t.Campus,
                                        Form_weightage = t.Form_weightage,
                                        Year = t.Year,
                                        Semester = t.Semester,
                                        Project_no = t.Project_no
                                    }).ToList();

                var criteria_list = (from crit in entity.Criteria_marks
                                        join temp in entity.Criteria_template
                                        on crit.Criteria_template_id equals temp.Criteria_id
                                        select new
                                        {
                                            crit.Form_id,
                                            temp.Criteria_title,
                                            temp.Total_marks,
                                            crit.Student_id,
                                            crit.Obtained_marks,
                                        }).ToList();

                var field_list = (from field in entity.Field_text
                                    join temp in entity.Field_template
                                    on field.Field_template_id equals temp.Field_id
                                    select new
                                    {
                                        field.Form_id,
                                        temp.Field_title,
                                        field.Field_text1
                                    }).ToList();

                foreach (var item in eval_list)
                {
                    foreach (var criteria in criteria_list)
                    {
                        if (criteria.Form_id == item.Form_id)
                        {
                            Evaluated_criteria_marks temp1 = new Evaluated_criteria_marks();
                            Evaluated_criteria_template temp2 = new Evaluated_criteria_template();

                            temp1.Student_id = criteria.Student_id;
                            temp1.Obtained_marks = criteria.Obtained_marks;
                            temp2.Criteria_title = criteria.Criteria_title;
                            temp2.Total_marks = criteria.Total_marks;

                            item.criteria_marks.Add(temp1);
                            item.criteria_template.Add(temp2);
                        }
                    }
                    foreach (var field in field_list)
                    {
                        if (field.Form_id == item.Form_id)
                        {
                            Evaluated_field_text temp1 = new Evaluated_field_text();
                            Evaluated_field_template temp2 = new Evaluated_field_template();
                            temp1.Field_text1 = field.Field_text1;
                            temp2.Field_title = field.Field_title;

                            item.field_text.Add(temp1);
                            item.field_template.Add(temp2);
                        }
                    }
                }
                return eval_list;
            }
        }


        // GET LIST OF ALL EXISTING TEMPLATE FORMS
        public IEnumerable<Existing_form> GetExistingAll()
        {
            using (MyFirstDBEntities entity = new MyFirstDBEntities())
            {
                var form_list = (from temp in entity.Template_form
                                    where temp.Enabled == true
                                    select new Existing_form
                                    {
                                        Form_id = temp.Form_id,
                                        Form_weightage = temp.Form_weightage,
                                        Form_title = temp.Form_title,
                                        Department = temp.Department,
                                        Campus = temp.Campus,
                                        Year = temp.Year,
                                        Semester = temp.Semester,
                                        Project_no = temp.Project_no
                                    }).ToList();
                var field_list = entity.Field_template;
                var crit_list = entity.Criteria_template;

                foreach (var item in form_list)
                {
                    foreach (var field in field_list)
                    {
                        if (field.Form_template_id == item.Form_id)
                        {
                            Existing_field_template obj = new Existing_field_template();
                            obj.Field_template_id = field.Field_id;
                            obj.Field_title = field.Field_title;

                            item.field_template.Add(obj);
                        }
                    }
                    foreach (var crit in crit_list)
                    {
                        if (crit.Form_template_id == item.Form_id)
                        {
                            Existing_criteria_template obj = new Existing_criteria_template();
                            obj.Criteria_template_id = crit.Criteria_id;
                            obj.Criteria_title = crit.Criteria_title;
                            obj.Total_marks = crit.Total_marks;

                            item.criteria_template.Add(obj);
                        }
                    }
                }
                return form_list;
            }
        }


        // GET LIST OF ALL EXISTING TEMPLATE FORMS BY FORM ID
        public IEnumerable<Existing_form> GetExistingByID(int id)
        {
            using (MyFirstDBEntities entity = new MyFirstDBEntities())
            {
                var form_list = (from temp in entity.Template_form
                                    where temp.Form_id == id && temp.Enabled == true
                                    select new Existing_form
                                    {
                                        Form_id = temp.Form_id,
                                        Form_weightage = temp.Form_weightage,
                                        Form_title = temp.Form_title,
                                        Department = temp.Department,
                                        Campus = temp.Campus,
                                        Year = temp.Year,
                                        Semester = temp.Semester,
                                        Project_no = temp.Project_no
                                    }).ToList();
                var field_list = entity.Field_template.Where(i => i.Form_template_id == id);
                var crit_list = entity.Criteria_template.Where(i => i.Form_template_id == id);

                foreach (var item in form_list)
                {
                    foreach (var field in field_list)
                    {
                        Existing_field_template obj = new Existing_field_template();
                        obj.Field_template_id = field.Field_id;
                        obj.Field_title = field.Field_title;

                        item.field_template.Add(obj);
                    }
                    foreach (var crit in crit_list)
                    {
                        Existing_criteria_template obj = new Existing_criteria_template();
                        obj.Criteria_template_id = crit.Criteria_id;
                        obj.Criteria_title = crit.Criteria_title;
                        obj.Total_marks = crit.Total_marks;

                        item.criteria_template.Add(obj);
                    }
                }
                return form_list;
            }
        }


        // POST CREATE NEW FORM TEMPLATE
        [HttpPost]
        public HttpResponseMessage AddForm(Add_Template_Form obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
            }

            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                AdminDataAccess.Template_form o1 = new AdminDataAccess.Template_form();
                //o1.Form_id = obj.Form_id;
                o1.Form_weightage = obj.Form_weightage;
                o1.Form_title = obj.Form_title;
                o1.Department = obj.Department;
                o1.Campus = obj.Campus;
                o1.Year = obj.Year;
                o1.Semester = obj.Semester;
                o1.Project_no = obj.Project_no;
                o1.Enabled = true;

                entities.Template_form.Add(o1);
                entities.SaveChanges();

                for (int i = 0; i < obj.field_template.Count; i++)
                {
                    AdminDataAccess.Field_template o2 = new AdminDataAccess.Field_template();
                    o2.Form_template_id = entities.Template_form.Select(x => x.Form_id).Max();
                    //o2.Field_id = obj.field_template[i].Field_id;
                    o2.Field_title = obj.field_template[i].Field_title;
                    entities.Field_template.Add(o2);
                    entities.SaveChanges();
                }

                for (int i = 0; i < obj.criteria_template.Count; i++)
                {
                    AdminDataAccess.Criteria_template o3 = new AdminDataAccess.Criteria_template();
                    o3.Form_template_id = entities.Template_form.Select(x => x.Form_id).Max();
                    //o3.Criteria_id = obj.criteria_template[i].Criteria_id;
                    o3.Criteria_title = obj.criteria_template[i].Criteria_title;
                    o3.Total_marks = obj.criteria_template[i].Total_marks;

                    entities.Criteria_template.Add(o3);
                    entities.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Form Added");
            }
        }


        // POST EVALUATE FORM
        [HttpPost]
        public HttpResponseMessage EvaluateForm(Add_Evaluated_Form obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
            }
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                AdminDataAccess.Evaluated_form o1 = new AdminDataAccess.Evaluated_form();
                //o1.Form_id = 5;
                o1.Template_id = obj.Template_id;
                o1.Jury_id1 = obj.Jury_id1;
                //o1.Jury_id2 = obj.Jury_id2;
                o1.Group_id = obj.Group_id;
                o1.Form_status = obj.Form_status;

                entities.Evaluated_form.Add(o1);
                entities.SaveChanges();

                for (int i = 0; i < obj.criteria_marks.Count; i++)
                {
                    AdminDataAccess.Criteria_marks o2 = new AdminDataAccess.Criteria_marks();
                    //o2.Form_id = 5;
                    o2.Form_id = entities.Evaluated_form.Select(x => x.Form_id).Max();
                    o2.Criteria_template_id = obj.criteria_marks[i].Criteria_template_id;
                    o2.Student_id = obj.criteria_marks[i].Student_id;
                    o2.Obtained_marks = obj.criteria_marks[i].Obtained_marks;

                    entities.Criteria_marks.Add(o2);
                    entities.SaveChanges();
                }
                for (int i = 0; i < obj.field_text.Count; i++)
                {
                    AdminDataAccess.Field_text o3 = new AdminDataAccess.Field_text();
                    //o3.Form_id = 5;
                    o3.Form_id = entities.Evaluated_form.Select(x => x.Form_id).Max();
                    o3.Field_template_id = obj.field_text[i].Field_template_id;
                    o3.Field_text1 = obj.field_text[i].Field_text;

                    entities.Field_text.Add(o3);
                    entities.SaveChanges();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Form Added");
        }

        // DELETE EXISTING FORM
        [HttpGet]
        public HttpResponseMessage Delete(int id)
        {
            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                var record = entities.Template_form.FirstOrDefault(p => p.Form_id == id);
                if (record != null)
                {
                    record.Enabled = false;
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, "Form Deleted");
                }
                else
                {
                    return Request.CreateResponse("Record not found");
                }
            }
        }
        // EDIT EXISTING FORM
        [HttpPost]
        public HttpResponseMessage EditForm(Add_Template_Form_1 obj)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid data.");
            }

            using (MyFirstDBEntities entities = new MyFirstDBEntities())
            {
                var record = entities.Template_form.FirstOrDefault(p => p.Form_id == obj.Form_id);
                if (record != null)
                {
                    record.Enabled = false;
                    entities.SaveChanges();

                    AdminDataAccess.Template_form o1 = new AdminDataAccess.Template_form();
                    //o1.Form_id = obj.Form_id;
                    o1.Form_weightage = obj.Form_weightage;
                    o1.Form_title = obj.Form_title;
                    o1.Department = obj.Department;
                    o1.Campus = obj.Campus;
                    o1.Year = obj.Year;
                    o1.Semester = obj.Semester;
                    o1.Project_no = obj.Project_no;
                    o1.Enabled = true;

                    entities.Template_form.Add(o1);
                    entities.SaveChanges();

                    for (int i = 0; i < obj.field_template.Count; i++)
                    {
                        AdminDataAccess.Field_template o2 = new AdminDataAccess.Field_template();
                        o2.Form_template_id = entities.Template_form.Select(x => x.Form_id).Max();
                        //o2.Field_id = obj.field_template[i].Field_id;
                        o2.Field_title = obj.field_template[i].Field_title;
                        entities.Field_template.Add(o2);
                        entities.SaveChanges();
                    }

                    for (int i = 0; i < obj.criteria_template.Count; i++)
                    {
                        AdminDataAccess.Criteria_template o3 = new AdminDataAccess.Criteria_template();
                        o3.Form_template_id = entities.Template_form.Select(x => x.Form_id).Max();
                        //o3.Criteria_id = obj.criteria_template[i].Criteria_id;
                        o3.Criteria_title = obj.criteria_template[i].Criteria_title;
                        o3.Total_marks = obj.criteria_template[i].Total_marks;

                        entities.Criteria_template.Add(o3);
                        entities.SaveChanges();
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, "Form Edited");                    
                }
                else
                {
                    return Request.CreateResponse("Record not found");
                }                
            }
        }
    }
}

// MODEL CLASSES FOR GET EVALUATE FORMS
public class Evaluated_form
{
    public string Leader_id { get; set; }
    public string Leader_name { get; set; }
    public string Supervisor_name { get; set; }
    public string Cosupervisor_name { get; set; }
    public string Project_title { get; set; }
    public string Form_status { get; set; }
    public int Form_id { get; set; }
    public int Form_template_id { get; set; }
    public string Form_title { get; set; }
    public string Department { get; set; }
    public string Campus { get; set; }
    public double Form_weightage { get; set; }
    public int Year { get; set; }
    public string Semester { get; set; }
    public int Project_no { get; set; }

    public List<Evaluated_criteria_template> criteria_template { get; set; }
    public List<Evaluated_criteria_marks> criteria_marks { get; set; }
    public List<Evaluated_field_template> field_template { get; set; }
    public List<Evaluated_field_text> field_text { get; set; }
    public Evaluated_form()
    {
        criteria_template = new List<Evaluated_criteria_template>();
        field_template = new List<Evaluated_field_template>();
        criteria_marks = new List<Evaluated_criteria_marks>();
        field_text = new List<Evaluated_field_text>();
    }
}
public class Evaluated_criteria_marks
{
    public string Student_id { get; set; }
    public double Obtained_marks { get; set; }
}
public class Evaluated_field_text
{
    public string Field_text1 { get; set; }
}
public class Evaluated_criteria_template
{
    public string Criteria_title { get; set; }
    public double Total_marks { get; set; }
}
public class Evaluated_field_template
{
    public string Field_title { get; set; }
}

// MODEL CLASSES FOR GET EXISTING FORMS
public class Existing_form
{
    public int Form_id { get; set; }
    public double Form_weightage { get; set; }
    public string Form_title { get; set; }
    public string Department { get; set; }
    public string Campus { get; set; }
    public int Year { get; set; }
    public string Semester { get; set; }
    public int Project_no { get; set; }
    public List<Existing_criteria_template> criteria_template { get; set; }
    public List<Existing_field_template> field_template { get; set; }
    public Existing_form()
    {
        criteria_template = new List<Existing_criteria_template>();
        field_template = new List<Existing_field_template>();
    }
}
public class Existing_criteria_template
{
    public int Criteria_template_id { get; set; }
    public string Criteria_title { get; set; }
    public double Total_marks { get; set; }
}
public class Existing_field_template
{
    public int Field_template_id { get; set; }
    public string Field_title { get; set; }
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

// MODEL CLASSES FOR POST EDIT FORM
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
