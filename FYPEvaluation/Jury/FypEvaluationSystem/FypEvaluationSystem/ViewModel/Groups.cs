using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FypEvaluationSystem.Models;
namespace FypEvaluationSystem.ViewModel
{
    public class Groups
    {

        List<Group> groups;

        public Groups()
        {
            Group grp = new Group();
            Group grp2 = new Group();

            grp.GroupId = 1;
            grp.ProjectTitle = "Social Searcher";
            grp.Supervisor = "Dr. Rauf";
            grp.CoSupervisor = "Arsalan";
            grp.members = new List<Student>
            {
                new Student("k163807", "Shehmeer"),
                new Student("k163782", "Talha"),
                new Student("k163788", "Krinza")
            };

            grp2.GroupId = 2;
            grp2.ProjectTitle = "Power Grid";
            grp2.Supervisor = "Dr. Sufiyan";
            grp2.CoSupervisor = "";
            grp2.members = new List<Student>
            {
                new Student("k163808", "Saad"),
                new Student("k1637801", "Anas"),
                new Student("k163757", "Haris")
            };


            groups = new List<Group>
            {
                grp,
                grp2
                
            };


        }

        public Group GetGroups(string ID)
        {
            foreach(var item in groups)
            {

                if (item.members[0].ID.Equals(ID))
                {

                    return item;
                }
            }
            return null;
        }

    }
}