using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentRecordMVC.Models
{
    public class StudentRecordModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Date Of Birth")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public System.DateTime? DOB { get; set; }
        [Required]
        public int? Age { get; set; }
        public string Email { get; set; }
        public int? Class { get; set; }
        [Required]
        public string Result { get; set; }
        [Required]
        public System.DateTime? DOA { get; set; }
        [Required]
        public string Branch { get; set; }
      
        public bool? IsJava { get; set; }
       
        public bool? IsPython { get; set; }
       
        public bool? IsCpp { get; set; }
    }
    public enum Branch
    {
        
        CSE,
        IT,
        ME,
        ECE

    }
}