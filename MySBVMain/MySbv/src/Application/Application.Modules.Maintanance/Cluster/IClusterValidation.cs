using System.Collections.Generic;
using Application.Dto.Cluster;
using Utility.Core;


namespace Application.Modules.Maintanance.Cluster
{
    public interface IClusterValidation
    {

        /// <summary>
        /// Return a list of all Cluster
        /// </summary>
        /// <returns></returns>
        IEnumerable<ClusterDto> All();

        /// <summary>
        /// Add A New Cluster
        /// </summary>
        /// <param name="clusterDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(ClusterDto clusterDto, string username);

        /// <summary>
        /// Update a new Cluster
        /// </summary>
        /// <param name="clusterDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(ClusterDto clusterDto, string username);

        /// <summary>
        /// Delete a Cluster
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int Id, string username);
        
        /// <summary>
        /// Find a Cluster by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<ClusterDto> Find(int id);
        
        /// <summary>
        /// Checks if the Cluster is in use by another Cluster
        /// </summary>
        /// /// <param name="name"></param>
        /// <returns></returns>
        bool IsClusterNameInUse(string name);

        /// <summary>
        /// Check if the Cluster Name is in use by another Cluster (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="name"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool NameUsedByAnotherCluster(string name, int id);

        /// <summary>
        /// Check if Cluster is in use
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<bool> IsInUse(int id);

    }
}
