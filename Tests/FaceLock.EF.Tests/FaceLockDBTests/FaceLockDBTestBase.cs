using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FaceLock.EF.Tests.FaceLockDBTests
{
    public class FaceLockDBTestBase
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
