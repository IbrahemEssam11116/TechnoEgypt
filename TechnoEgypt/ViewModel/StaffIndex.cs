using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TechnoEgypt.Models;

namespace TechnoEgypt.ViewModel
{
    public class Staffindex
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        //public Course course { get; set; }
        public string Email { get; set; }

        public string Phone { get; set; }


        [DisplayName("Branch Name")]
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public List<Stage> Branchs { get; set; }
        public SelectList BranchList { get; set; }

    }
}
