using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Dynamic_questionnaire.DBModels
{
    public partial class ContextModel : DbContext
    {
        public ContextModel()
            : base("name=DefaultConnectionString")
        {
        }

        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Filler> Fillers { get; set; }
        public virtual DbSet<Questionnaire> Questionnaires { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>()
                .Property(e => e.QuestionID)
                .IsFixedLength();

            modelBuilder.Entity<Admin>()
                .Property(e => e.Account)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .Property(e => e.PassWord)
                .IsUnicode(false);

            modelBuilder.Entity<Filler>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Filler>()
                .Property(e => e.Ages)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Questionnaire>()
                .Property(e => e.CreateAccount)
                .IsUnicode(false);
        }
    }
}
