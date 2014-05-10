using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiSamples.Samples
{
    class HRESULT
    {
        private int _hr;

        private HRESULT(int hr)
        {
            _hr = hr;
        }

        public static implicit operator int(HRESULT hr)
        {
            return hr._hr;
        }

        public static implicit operator HRESULT(int hr)
        {
            return new HRESULT(hr);
        }

        #region winerror.h

        public const int MK_E_UNAVAILABLE = unchecked((int)0x800401E3);

        #endregion
    }
}
