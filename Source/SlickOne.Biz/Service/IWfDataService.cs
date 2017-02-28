using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlickOne.Biz.Entity;

namespace SlickOne.Biz.Service
{
    /// <summary>
    /// workflow data service interface
    /// </summary>
    public interface IWfDataService
    {
        IList<ProcessEntity> GetProcessListSimple();
        IList<ProcessInstanceEntity> GetProcessInstanceList();
        IList<ActivityInstanceEntity> GetActivityInstanceList();
        IList<ActivityInstanceEntity> GetActivityInstanceList(int processInstanceID);
        IList<TaskEntity> GetTaskList();
        IList<FormEntity> GetFormListSimple();
        IList<LogEntity> GetLogList();
    }
}
