using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace WorkoutTelegramBot.Database
{
    public class WorkoutContextFactory : IDbContextFactory<WorkoutContext>, IDesignTimeDbContextFactory<WorkoutContext>
    {
        public WorkoutContextFactory() : this("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\JamesPC\\Documents\\WorkoutDatabase.mdf;Integrated Security=True;Connect Timeout=30")
        {

        }
        public WorkoutContextFactory(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; }


        public WorkoutContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<WorkoutContext>();
            builder.UseSqlServer(ConnectionString);
            return new WorkoutContext(builder.Options);
        }

        public WorkoutContext CreateDbContext(string[] args)
            => CreateDbContext();
    }
}
