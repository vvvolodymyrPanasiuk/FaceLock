using Microsoft.EntityFrameworkCore;


namespace FaceLock.EF.Tests.FaceLockDBTests
{
    public class FaceLockDBTestBase : IDisposable
    {
        private readonly DbContextOptions<FaceLockDbContext> _options;
        protected readonly FaceLockDbContext _context;


        public FaceLockDBTestBase()
        {
            _options = new DbContextOptionsBuilder<FaceLockDbContext>()
                .UseInMemoryDatabase(databaseName: "FaceLockTestDB")
                .Options;

            _context = new FaceLockDbContext(_options);
            _context.Database.EnsureCreated();

            FaceLockDBInitializer.SeedData(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
