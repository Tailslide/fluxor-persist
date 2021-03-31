using Fluxor.Persist.Storage;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Fluxor.Persist.Middleware
{
    public class PersistMiddlewareOptions
    {
        private static readonly Dictionary<string, bool> _mandatoryBlackList = new Dictionary<string, bool>
        { 
            ["@routing"] = false,
            [nameof(PersistMiddleware)] = false
        };

        private Dictionary<string, bool> _stateBlackList = _mandatoryBlackList;
        private Dictionary<string, bool> _stateWhiteList;
        private bool _useInclusionApproach;
        

        public bool ShouldPersistState(IFeature feature)
        {
            if (_useInclusionApproach)
                return feature.GetStateType().IsDefined(typeof(PersistState), false)
                    || _stateWhiteList.ContainsKey(feature.GetName());
            else
                return !feature.GetStateType().IsDefined(typeof(SkipPersistState), false)
                    && !_stateBlackList.ContainsKey(feature.GetName());
        }

        public Dictionary<string, bool> StateBlackList 
        {
            get => _stateBlackList;            
            private set
            {                
                _stateBlackList = _mandatoryBlackList.Union(value, new ListComparer())
                    .ToDictionary(k => k.Key, v => v.Value);
                _useInclusionApproach = false;
                _stateWhiteList = null;
            }
        }

        public Dictionary<string, bool> StateWhiteList
        {
            get => _stateWhiteList;
            private set
            {
                _stateWhiteList = value.Except( _mandatoryBlackList, new ListComparer())
                    .ToDictionary(k => k.Key, v => v.Value);
                _useInclusionApproach = true;
                _stateBlackList = null;
            }
        }

        public void SetBlackList(params string[] stateNames) => StateBlackList = stateNames.ToDictionary(k => k, v => true);
        public void SetWhiteList(params string[] stateNames) => StateWhiteList = stateNames.ToDictionary(k => k, v => true);

        public void UseInclusionApproach()
        {
            _useInclusionApproach = true;
            _stateWhiteList ??= new();
            _stateBlackList = null;
        }

        private class ListComparer : IEqualityComparer<KeyValuePair<string, bool>>
        {
            public bool Equals(KeyValuePair<string, bool> x, KeyValuePair<string, bool> y) => x.Key.Equals(y.Key);

            public int GetHashCode([DisallowNull] KeyValuePair<string, bool> obj) => obj.Key.GetHashCode();
        }
    }
}
