using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CurdwithImg.Models
{
    public class Student
    {
        public enum Gender { Male,Female }
        [Key]
        public int StudentId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age is required")]
        [Range(18,60, ErrorMessage =("Age must be between 18 to 60"))]
        [Display(Name = "Age")]
        public int Age { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Gender")]
        public Gender Genders { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase File { get; set; }

    }
}