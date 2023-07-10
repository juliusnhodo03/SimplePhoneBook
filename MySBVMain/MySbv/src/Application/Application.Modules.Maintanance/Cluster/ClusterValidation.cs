using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto.Cluster;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.Cluster
{
    public class ClusterValidation : IClusterValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _accountValidation;

        #endregion

        #region Constructor

        public ClusterValidation(IMapper mapper, ILookup iLookup, IUserAccountValidation accountValidation, IRepository repository)
        {
            _mapper = mapper;
            _lookup = iLookup;
            _repository = repository;
            _accountValidation = accountValidation;
        }

        #endregion

        #region ICluster Validation

        public IEnumerable<ClusterDto> All()
        {
            var clusters = _repository.All<Domain.Data.Model.Cluster>();

            var listOfCluster = new List<ClusterDto>();

            foreach (var cluster in clusters)
            {
                var item = _mapper.Map<Domain.Data.Model.Cluster, ClusterDto>(cluster);
                listOfCluster.Add(item);
            }
            return listOfCluster;
        }

        public MethodResult<bool> Add(ClusterDto clusterDto, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;

            var cluster = _mapper.Map<ClusterDto, Domain.Data.Model.Cluster>(clusterDto);
            cluster.CreatedById = userId;
            cluster.CreateDate = DateTime.Now;
            cluster.LastChangedById = userId;
            cluster.LastChangedDate = DateTime.Now;
            cluster.RegionManagerId = 1;

            return _repository.Add(cluster) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Cluster Not Added");
        }

        public MethodResult<bool> Edit(ClusterDto clusterDto, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;
            var tempCluster =
                _repository.Query<Domain.Data.Model.Cluster>(a => a.ClusterId == clusterDto.ClusterId).FirstOrDefault();

            var mappedCluster = _mapper.Map<ClusterDto, Domain.Data.Model.Cluster>(clusterDto);
            mappedCluster.EntityState = State.Modified;
            mappedCluster.RegionManagerId = 1;
            mappedCluster.IsNotDeleted = true;
            mappedCluster.LastChangedById = userId;
            if (tempCluster != null)
            {
                mappedCluster.CreateDate = tempCluster.CreateDate;
                mappedCluster.CreatedById = tempCluster.CreatedById;
            }

            return _repository.Update(mappedCluster) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Cluster Not Updated");
        }

        public MethodResult<bool> Delete(int clusterId, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;
            var result = IsInUse(clusterId);
            if (result.Status == MethodStatus.Successful)
            {
                var _result = _repository.Delete<Domain.Data.Model.Cluster>(clusterId, userId) > 0;
                return _result ? new MethodResult<bool>(MethodStatus.Successful, true) :
                    new MethodResult<bool>(MethodStatus.Error, false, "Cluster Not Found");
            }
            return new MethodResult<bool>(MethodStatus.Error, false, result.Message);
        }

        public MethodResult<ClusterDto> Find(int id)
        {
            var cluster = _repository.Find<Domain.Data.Model.Cluster>(id);
            var mappedCluster = _mapper.Map<Domain.Data.Model.Cluster, ClusterDto>(cluster);

            return new MethodResult<ClusterDto>(MethodStatus.Successful, mappedCluster);
        }

        #endregion

        #region Helper

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<Domain.Data.Model.CashCenter>(a => a.ClusterId == id && a.IsNotDeleted) ?
                new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a Cluster that is linked to and active Cash Center.") :
                new MethodResult<bool>(MethodStatus.Successful);
        }

        public bool IsClusterNameInUse(string name)
        {
            return _repository.Any<Domain.Data.Model.Cluster>(a => a.Name.ToLower() == name.ToLower());
        }

        public bool NameUsedByAnotherCluster(string name, int id)
        {
            return _repository.Any<Domain.Data.Model.Cluster>(a => a.Name.ToLower() == name.ToLower() && a.ClusterId != id);
        }
        
        #endregion

    }
}