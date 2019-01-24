using ContactS.BLL.DTO;
using ContactS.BLL.DTO.Filtres;
using ContactS.BLL.Infrastructure;
using ContactS.DAL.Entities;
using ContactS.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactS.BLL.Queries
{
    public class FriendListQuery : QueryBase<FriendshipDTO>
    {
        UnitOfWork Database;
        public FriendListQuery(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public FriendshipFilter Filter { get; set; }

        protected override IQueryable<FriendshipDTO> GetQueryable()
        {
            IQueryable<Friendship> query = Database.Context.Friendships;
            Database.Context.ClientProfiles.Load();
            if ((Filter.Account != null) && (Filter.Account2 == null))
                query = query.Where(f => (f.User1.Id == Filter.Account.Id) || (f.User2.Id == Filter.Account.Id));
            else if ((Filter.Account != null) && (Filter.Account2 != null))
                query = query.Where(f =>
                    (f.User1.Id.Equals(Filter.Account.Id) && f.User2.Id.Equals(Filter.Account2.Id))
                    || (f.User2.Id.Equals(Filter.Account.Id) && f.User1.Id.Equals(Filter.Account2.Id)));

            List<FriendshipDTO> result = new List<FriendshipDTO>();
            foreach (var frndshp in query)
            {
                FriendshipDTO friendship = new FriendshipDTO
                {
                    Id = frndshp.Id,
                    User1Id = frndshp.User1.Id,
                    User2Id = frndshp.User2.Id
                };
                result.Add(friendship);
            }

            return result.AsQueryable();
        }
    }
}
