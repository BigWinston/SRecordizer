using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Media;

namespace SRecordizer
{
    public static class ExceptionTrap
    {
        #region _CONSTANTS_
        #endregion
        #region _DATA_TYPES_
        #endregion
        #region _PUBLIC_PROPERTIES_
        #endregion
        #region _PRIVATE_MEMBERS_
        #endregion
        #region _CONSTRUCTORS_
        #endregion
        #region _PUBLIC_METHODS_
        /*********************************************************************/
        /// <summary>
        /// Traps an exception and handles it.
        /// </summary>
        /// <param name="e">The exception being trapped</param>
        public static void Trap(Exception e)
        {
            //SRecordizer.LogIt(LogView.LogType.Error, e.Message);
            MessageBox.Show(e.Message, "Trapped Application Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }

        /*********************************************************************/
        /// <summary>
        /// Method for handling an error programatically - end result appears
        /// same to user as the Exception Trap above.
        /// </summary>
        /// <param name="m">Message to display in error output</param>
        public static void Trap(string m)
        {
            //SRecordizer.LogIt(LogView.LogType.Error, m);
            MessageBox.Show(m, "Trapped Application Exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);            
        }
        #endregion
        #region _PRIVATE_METHODS_
        #endregion
        #region _GUI_CALLBACKS_
        #endregion
    }
}
