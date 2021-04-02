namespace kindergarden
{
    using kindergarden.Models;
    using System.Data.Entity;

    public class KinderModelContext : DbContext
    {
        public KinderModelContext() : base("name=KinderString") { }

        public virtual DbSet<activities> activities { get; set; }
        public virtual DbSet<ActiveUser> ActiveUser { get; set; }
        public virtual DbSet<LogRecord> LogRecords { get; set; }
        public virtual DbSet<activitiesMessage> activitiesMessage { get; set; }
        public virtual DbSet<answerActivityMessage> AnswerActivityMessages { get; set; }
        public virtual DbSet<activitiesPicture> activitiesPicture { get; set; }
        public virtual DbSet<advertisement> advertisement { get; set; }
        public virtual DbSet<School> School { get; set; }
        public virtual DbSet<news> news { get; set; }
        public virtual DbSet<Person> person { get; set; }
        public virtual DbSet<personMessage> personMessage { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<gallery> gallery { get; set; }
        public virtual DbSet<galleryImage> galleryimage { get; set; }
        public virtual DbSet<CalendarActivity> CalendarActivities { get; set; }
        //public virtual DbSet<Address> Addresses { get; set; }

    }
}
