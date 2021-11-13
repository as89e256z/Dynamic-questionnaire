namespace Dynamic_questionnaire.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Question")]
    public partial class Question
    {
        [StringLength(10)]
        public string QuestionID { get; set; }

        [Required]
        [StringLength(100)]
        public string QuestionnaireTitle { get; set; }

        public int ProblemID { get; set; }

        [Required]
        [StringLength(100)]
        public string ProblemTitle { get; set; }

        public int TypeOfProblem { get; set; }

        public bool Required { get; set; }

        public bool FrequentlyAsked { get; set; }


        [StringLength(50)]
        public string Ans1 { get; set; }

        [StringLength(50)]
        public string Ans2 { get; set; }

        [StringLength(50)]
        public string Ans3 { get; set; }

        [StringLength(50)]
        public string Ans4 { get; set; }

        [StringLength(50)]
        public string Ans5 { get; set; }

        [StringLength(50)]
        public string Ans6 { get; set; }

        [StringLength(50)]
        public string Ans7 { get; set; }

        [StringLength(50)]
        public string Ans8 { get; set; }

        [StringLength(50)]
        public string Ans9 { get; set; }
    }
}
