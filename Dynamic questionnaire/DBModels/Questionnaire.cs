namespace Dynamic_questionnaire.DBModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Questionnaire")]
    public partial class Questionnaire
    {
        [Key]
        [Column(Order = 0)]
        public int QuestionnaireNumber { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(100)]
        public string QuestionnaireName { get; set; }

        [StringLength(100)]
        public string QuestionnaireDescribe { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string CreateAccount { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int State { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime StartTime { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime EndTime { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime CreateTime { get; set; }
    }
}
