using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Threading;

namespace Pearson.RallyCrawler
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer objTimer = new System.Timers.Timer();
        
        public Service1()
        {
            InitializeComponent();
        }
        
        protected override void OnStart(string[] args)
        {
            try
            {                
                string strValue = ConfigurationManager.AppSettings["PROCESSTIMER"] != null ?
                    ConfigurationManager.AppSettings["PROCESSTIMER"] : "";

                double dTimer;
                try
                {
                    dTimer = Convert.ToDouble(strValue);
                }
                catch (Exception)
                {
                    dTimer = 60000 * 24 * 60;
                }
               
                objTimer.Interval = dTimer;
                objTimer.Elapsed += new System.Timers.ElapsedEventHandler(ObjTimerElapsed);
                objTimer.Start();
            }
            catch (Exception)
            {                
            }            
        }

        protected void ObjTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception)
            {                
            }            
        }
     
        protected override void OnStop()
        {
            try
            {
                objTimer.Stop();
                objTimer.Dispose();

                objTimer = null;               
            }
            catch (Exception)
            {                
            }
        } 
    }
}
