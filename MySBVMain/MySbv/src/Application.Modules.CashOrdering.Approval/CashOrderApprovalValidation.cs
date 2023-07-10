using System.Collections.Generic;
using System.Linq;
using Application.Dto.CashOrderTask;
using Application.Mapper;
using Application.Modules.Common;
using Domain.Data.Model;
using Domain.Repository;

namespace Application.Modules.CashOrdering.Approval
{
    public class CashOrderApprovalValidation : ICashOrderApprovalValidation
    {

        #region Fields

        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly ILookup _lookup;

        #endregion

        #region Constructor

        public CashOrderApprovalValidation(IMapper mapper, IRepository repository, ILookup lookup)
        {
            _mapper = mapper;
            _repository = repository;
            _lookup = lookup;
        }

        #endregion

        #region ICashOrderApprovalValidation Validation

        public IEnumerable<ListCashOrderTaskDto> All()
        {
            var tasks = _repository.All<CashOrderTask>(b => b.Site, c => c.User, a => a.User.Merchant, d => d.Status)
                .Where(a => a.StatusId == _lookup.GetStatusId("PENDING") || a.StatusId == _lookup.GetStatusId("DECLINED"))
                .OrderByDescending(a => a.CashOrderTaskId);
            var list = new List<ListCashOrderTaskDto>();

            foreach (var task in tasks)
            {
                var item = _mapper.Map<CashOrderTask, ListCashOrderTaskDto>(task);
                var cashOrder = _repository.Query<Domain.Data.Model.CashOrder>(a => a.CashOrderId == task.CashOrderId).FirstOrDefault();

                item.SiteName = task.Site.Name;
                item.CitCode = task.Site.CitCode;
                item.UserName = task.User.UserName;
                item.StatusName = task.Status.Name;
                if (cashOrder != null) item.CashOrderAmount = string.Format("{0:C}", cashOrder.CashOrderAmount);

                list.Add(item);
            }

            return list;
        }

   
        #endregion


        #region Helper
            
            

        #endregion

    }
}
