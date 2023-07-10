using System.Collections.Generic;
using System.Linq;
using Application.Dto.Task;
using Application.Mapper;
using Application.Modules.Common;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.Approval
{
    public class ApprovalValidation : IApprovalValidation
    {

        #region Fields

        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly ILookup _lookup;

        #endregion

        #region Constructor

        public ApprovalValidation(IMapper mapper, IRepository repository, ILookup lookup)
        {
            _mapper = mapper;
            _repository = repository;
            _lookup = lookup;
        }

        #endregion

        #region IApproval Validation

        public IEnumerable<ListTaskDto> All()
        {
            var tasks = _repository.All<Task>(a => a.Merchant, b => b.Site, c => c.User, d => d.Status, e => e.Account)
                .Where(a => a.StatusId == _lookup.GetStatusId("PENDING") || a.StatusId == _lookup.GetStatusId("DECLINED"));
            
            List<ListTaskDto> list = tasks.Select(task => _mapper.Map<Task, ListTaskDto>(task)).ToList();
            
            return list;
        }


        public MethodResult<TaskDto> Find(int id)
        {
            var approval = _repository.Find<Task>(id);

            var mappedApproval = _mapper.Map<Task, TaskDto>(approval);

            return new MethodResult<TaskDto>(MethodStatus.Successful, mappedApproval);
        }


        #endregion

    }
}
