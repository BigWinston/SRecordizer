using System;
using System.Diagnostics;

namespace HabelaLabs.Utility
{
    class InstantTimer
    {
        #region _CONSTANTS_
        #endregion
        #region _DATA_TYPES_
        #endregion
        #region _PUBLIC_PROPERTIES_
        #endregion
        #region _PRIVATE_MEMBERS_
        Stopwatch _Stopwatch;                
        #endregion
        #region _CONSTRUCTORS_
        public InstantTimer()
        {
            _Stopwatch = new Stopwatch();
            _Stopwatch.Start();
        }
        #endregion
        #region _PUBLIC_METHODS_
        public ulong Stop()
        {
            _Stopwatch.Stop();
            return (ulong)_Stopwatch.ElapsedMilliseconds;
        }
        #endregion
        #region _PRIVATE_METHODS_
        #endregion
        #region _GUI_CALLBACKS_
        #endregion
    }
}
