using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxor.Persist.Middleware
{
    public class PersistMiddlewareOptions
    {
        private static readonly string mandatoryBlackList = "@routing,PersistMiddleware";
        private string stateBlackList = mandatoryBlackList;
        private string stateWhiteList = "";

        public bool UseWhiteList
        {
            get
            {
                return StateWhiteList != "";
            }
        }

        public bool ShouldPersistState(string stateName)
        {
            if (UseWhiteList)
                return StateWhiteList.Split(new char[] { ',' }).Contains(stateName); // TODO:Optimize
            else
                return !StateBlackList.Split(new char[] { ',' }).Contains(stateName); // TODO:Optimize
        }

        public string StateBlackList { 
            get
            {
                return stateBlackList;
            } 
            set
            {
                stateBlackList = value;
                if (! stateBlackList.Contains(mandatoryBlackList))
                {
                    if (stateBlackList == "") stateBlackList = mandatoryBlackList;
                    else stateBlackList += "," + mandatoryBlackList;
                }
            }
        }
        public string StateWhiteList
        {
            get
            {
                return stateWhiteList;
            }
            set
            {
                stateWhiteList = value;
                if (stateWhiteList.Contains(mandatoryBlackList))
                {
                    stateWhiteList = stateWhiteList.Replace(mandatoryBlackList,""); // TODO: smarten this up
                }
            }
        }

    }
}
