using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAMSS.Process
{
    public interface IProcess
    {
        //string ProcessName;
        //string InstanceName;
        //string Status;
        //DateTime LivingConfirmationTime;
        void Start();
        void Pause();
        void Exit();
    }

    public abstract class TimerProcess : IProcess
    {
        public string _name;
        public string _status;
        public DateTime _livingConfirmationTime;

        public TimerProcess()
        {
 
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }

        protected abstract void CycleProcess();

        private async void ProcessRoop()
        {
            await Task.Run(() => 
            {
                while (true)
                {
                    this.CycleProcess();
                }
            });
        }
    }
}
