using System;
using System.Windows.Input;

namespace AssociaValoriQuotaNew.Resources.Classes
{   

    public class WpfWaitCursor : IDisposable
    {
        private Cursor m_oldCursor = null;

        public WpfWaitCursor()
        {
            m_oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = m_oldCursor;
        }
    }
}
