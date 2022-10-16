using Digital_Signatues.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digital_Signatues.Services
{
    public interface IKySo
    {
        Task<int> AddKySoTest (KySoTest kySotest);
    }
    public class KySoSvc:IKySo 
    {
        private readonly DataContext _context;
        public KySoSvc(DataContext context)
        {
            _context = context;
        }
        public async Task<int> AddKySoTest(KySoTest kySotest)
        {
            int ret = 0;
            try
            {
                await _context.KySoTests.AddAsync(kySotest);
                await _context.SaveChangesAsync();
                ret = kySotest.Id_KySoTest;
            }
            catch
            {
                ret = 0;
            }
            return ret;
        }
    }
}
