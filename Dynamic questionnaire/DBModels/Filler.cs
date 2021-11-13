namespace Dynamic_questionnaire.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Filler")]
    public partial class Filler
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string Name { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Phone { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string Email { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(3)]
        public string Ages { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string QuestionnaireTitle { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(100)]
        public string ProblemTitle { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime CreateTime { get; set; }

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
